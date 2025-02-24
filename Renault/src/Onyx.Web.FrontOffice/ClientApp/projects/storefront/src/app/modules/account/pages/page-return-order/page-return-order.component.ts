import { MainProductDto, MainReturnOrderDto, ReturnOrderItemDto, ReturnOrderItemGroupDto, ReturnOrderTotalDocumentDto } from './../../../../web-api-client';
import { AuthService } from 'projects/storefront/src/app/services/authService/auth.service';
import { ImageService } from 'projects/storefront/src/app/mapServieces/image.service';
import { Component, OnDestroy, OnInit, ViewChild } from '@angular/core';
import { forkJoin, Observable, Subject, filter, from, of } from 'rxjs';
import { ActivatedRoute, Router } from '@angular/router';
import { map, mergeMap, takeUntil, toArray } from 'rxjs/operators';
import { Order, OrderForReturn, OrderItem, OrderItemForReturn } from '../../../../interfaces/order';
import { TranslateService } from '@ngx-translate/core';
import {
  CreateItemCommandForItemGroup, CreateItemGroupCommandForReturnOrder, CreateReasonCommandForItem, CreateReturnOrderCommand, DocumentCommandForItem,
  DocumentCommandForTotal, FileUploadMetadataDto, OrdersClient, ReturnOrderByIdDto, ReturnOrderByOrderIdDto, ReturnOrderCustomerReasonType,
  ReturnOrderOrganizationReasonType, ReturnOrderReasonType, ReturnOrdersClient, ReturnOrderStatus, ReturnOrderTransportationType, SendReturnOrderCommand,
} from 'projects/storefront/src/app/web-api-client';
import { ModalDirective } from 'ngx-bootstrap/modal';
import { OrdermapperService } from 'projects/storefront/src/app/mapServieces/ordersCluster/ordermapper.service';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { CreateItemCommandForItemGroupModel, CreateItemGroupCommandForReturnOrderModel, CreateReasonCommandForItemModel, DocumentCommandForItemModel, ReturnOrderItemGroupModel, ReturnOrderModel } from './return-order-item-group-model';
import { ToastrService } from 'ngx-toastr';
import * as momentJalaali from "moment-jalaali";

@Component({
  selector: 'app-page-return-order',
  templateUrl: './page-return-order.component.html',
  styleUrls: ['./page-return-order.component.scss'],
})
export class PageReturnOrderComponent implements OnInit, OnDestroy {
  private destroy$: Subject<void> = new Subject<void>();
  returnOrderItemGroupModels: Set<ReturnOrderItemGroupModel> = new Set<ReturnOrderItemGroupModel>();
  temporaryReturnOrderItemGroupModels: Set<ReturnOrderItemGroupModel> = new Set<ReturnOrderItemGroupModel>();
  imageUrl: string | undefined = undefined;
  documentListItems: DocumentCommandForItemModel[] = [];
  documentListItems1: DocumentCommandForItemModel[] = [];
  returnOrderItemForm!: FormGroup;
  customerAccountInfoForm!: FormGroup;
  documentFrom!: FormGroup;
  documentFrom1!: FormGroup;
  inProgress = false;
  productAttributeOptionId!: number;
  itemGroupId = 1;
  itemId = 1;
  itemDocId = 1;
  selectedGroupId = 0;
  selectedItemId = 0;
  selectedReturnOrderItem: CreateItemCommandForItemGroupModel | undefined;
  selectedOrderItem: OrderItemForReturn | undefined;
  selectedOrderItemLimit: number = 0;
  @ViewChild('selectItemModal') selectItemModal!: ModalDirective;
  @ViewChild('returnFormItemModal') returnFormItemModal!: ModalDirective;
  @ViewChild('addDocumentModal') addDocumentModal!: ModalDirective;
  ReturnOrderReasonType = ReturnOrderReasonType;
  ReturnOrderCustomerReasonType = ReturnOrderCustomerReasonType;
  ReturnOrderOrganizationReasonType = ReturnOrderOrganizationReasonType;
  returnOrderReasonTypes = [0, 1, 2, 3, 4];

  order!: OrderForReturn;
  orderId!: number;
  returnOrderSubtotal: number = 0;
  returnOrderTotal: number = 0;
  returnOrderQuantity: number = 0;
  addedItems: string[] = []
  i: number=0;
  returnOrders: ReturnOrderModel[] = [];

  ReturnOrderStatus = ReturnOrderStatus;
  ReturnOrderTransportationType = ReturnOrderTransportationType;
  showShippingForm = false;
  selectedOrder: MainReturnOrderDto | undefined | null;

  shippingDetails = { address: '', date: '' };
  selectedTab: number = 1;
  selectedTab1: number = 1;
  orderreturnid: number = 0;

  details: boolean = false;
  returnitem: boolean = false;
  documents: boolean = false;
  costdocuments: boolean = false;
  senddata: boolean = false;
  selectedItem: MainReturnOrderDto | undefined | null;

  selectedOrder1: MainReturnOrderDto | undefined | null; 
  allItemGroups: ReturnOrderItemGroupDto[] = [];

  profileIsLoaded = true;
  allItemDocImage$: Observable<string | undefined> = from([]);
  allItemcostDocImage$: Observable<string | undefined> = from([]);


  selectedReturnOrder!: ReturnOrderModel | undefined;
  selectedReturnOrderItemGroup!: ReturnOrderItemGroupDto | undefined;
  selectedReturnOrderItems !: ReturnOrderItemDto | undefined;
  selectedReturnOrderItemsDoc !: ReturnOrderTotalDocumentDto | undefined;
  isDataSent: boolean = false;

  constructor(
    private route: ActivatedRoute,
    private translate: TranslateService,
    private ordersClient: OrdersClient,
    private ordermapperService: OrdermapperService,
    public imageService: ImageService,
    private toastr: ToastrService,
    private returnOrdersClient: ReturnOrdersClient,
    private fb: FormBuilder,
    private router: Router
  ) {
    this.customerAccountInfoForm = this.fb.group({
      value: ['', Validators.required],
      details: ['', Validators.required]
    });
    this.returnOrderItemForm = this.fb.group({
      quantity: ['', Validators.required],
      returnOrderReasonDetails: ['', [Validators.required, Validators.maxLength(250)]],
      returnOrderReasonType: ['', [Validators.required]],
    });

    this.documentFrom = this.fb.group({
      id: 0,
      description: ['', Validators.required],
      imageContent: ['', Validators.required],
    });
    this.documentFrom1 = this.fb.group({
      id: 0,
      description: ['', Validators.required],
      imageContent: ['', Validators.required],
    });

    this.returnOrderItemGroupModels.clear();
  }


  ngOnInit(): void {

    this.route.params
      .pipe(
        map((x) =>
          x['id']
            ? parseFloat(x['id'])
            : this.route.snapshot.data['orderId']
        ),
        mergeMap((orderId) => {
          this.orderId = orderId;
          return this.ordersClient.getOrderForReturnById(orderId)
        }),
        takeUntil(this.destroy$)
      )
      .subscribe((order) => {

        this.order = this.ordermapperService.mapOrderForReturnById(order);
        this.returnOrderItemForm.patchValue({
          quantity: 0 
        });
        debugger;
      });
    this.fetchReturnOrders();
  }

  get orderNumber(): string {
    return this.translate.instant('FORMAT_ORDER_NUMBER', {
      number: this.order?.number,

    });
  }


  mapNumericKeyToString(key: number ): string {
    switch ((key)) {
      case 0:
        return "انصراف مشتری";
      case 1:
        return "کالای معیوب";
      case 2:
        return "کالای ناقص";
      case 3:
        return "سایر";
      case 4:
        return "کالای اشتباه";
      default:
        return "";
    }
  }

  modalsenddata(orderId: number): void {
    this.resetModalData(); 
    this.modalData.id = orderId;
    this.senddata = true;
  }
  modalData: SendReturnOrderCommand = new SendReturnOrderCommand();
  isPriceNegative = false;
  successMessage = ''; 
  errorMessage = ''; 
  loading = false; 

  closemodalsenddata(): void {
    this.senddata = false;
    this.resetModalData(); 
  }

  resetModalData(): void {
    this.modalData = new SendReturnOrderCommand(); 
    this.isPriceNegative = false;
    this.errorMessage = '';
    this.successMessage = '';
  }

 
  addDocument(): void {
    if (!this.modalData.documentCommandForTotals) {
      this.modalData.documentCommandForTotals = [];
    }

    this.modalData.documentCommandForTotals.push({
      description: '',
      image: undefined, 
      init: (_data?:DocumentCommandForTotal ) => { },  
      toJSON: (_data?: DocumentCommandForTotal) => { return null; }, 
    });
  }

  removeDocument(index: number): void {
    if (this.modalData.documentCommandForTotals) {
      this.modalData.documentCommandForTotals.splice(index, 1);
    }
  }

  onFileSelected1(event: FileUploadMetadataDto | null, index: number): void {
    if (event) {
      if (!this.modalData.documentCommandForTotals) {
        this.modalData.documentCommandForTotals = [];
      }
        this.documentFrom1.controls['imageContent'].setValue(event.fileId);
      } else {
        this.documentFrom1.controls['imageContent'].setValue(null);
      }
   
    }
  


addToDocListItem1() {
  this.documentFrom1.markAllAsTouched();  
  debugger;
  if (this.documentFrom1.valid) {
    if (!this.modalData.documentCommandForTotals) {
      this.modalData.documentCommandForTotals = [];
    }

   if (this.modalData.documentCommandForTotals.length < 5) {
      this.modalData.documentCommandForTotals.push({
        description: this.documentFrom1.controls['description'].value,
        image: this.documentFrom1.controls['imageContent'].value, 
        init: (_data?: DocumentCommandForTotal) => {},
        toJSON: (_data?: DocumentCommandForTotal) => { return null; }
      });
      console.log(this.modalData.documentCommandForTotals)
      this.resetForm(this.documentFrom1);  
      this.imageUrl = '';  
    } else {
      this.toastr.error('تعداد مستندات برای یک آیتم نمی تواند بیشتر از 5 عدد باشد');
    }
  }
}


submitModalForm(): void {
  if (!this.modalData.id) {
    console.error('ID is required to send data.');
    this.errorMessage = 'خطا: شناسه ارسال داده الزامی است.';
    return;
  }

  if (this.isPriceNegative) {
    this.errorMessage = 'خطا: قیمت ارسال نباید منفی باشد.';
    return;
  }

  if (!this.modalData.documentCommandForTotals || this.modalData.documentCommandForTotals.some(doc => !doc.image)) {
    this.errorMessage = 'خطا: وارد کردن مدارک و تصاویر الزامی است.';
    return;
  }

  let cmd = new SendReturnOrderCommand();
  cmd.id = this.modalData.id;
  cmd.details = this.modalData.details;
  cmd.returnOrderTransportationType = this.modalData.returnOrderTransportationType;
  cmd.returnShippingPrice = this.modalData.returnShippingPrice !== undefined 
    ? parseFloat(this.modalData.returnShippingPrice.toString()) * 10 
    : 0;

  debugger;
  cmd.documentCommandForTotals = [];
  cmd.documentCommandForTotals = this.modalData.documentCommandForTotals.map(c => {
    let doc = new DocumentCommandForTotal();
    doc.description = c.description;
    doc.image = c.image;
    return doc;
  });

  this.loading = true;  
  this.returnOrdersClient.send(this.modalData.id, cmd).subscribe({
    next: (response) => {
      console.log('Data sent successfully:', response);
      this.successMessage = 'اطلاعات با موفقیت ارسال شد!';
      this.errorMessage = '';
      this.loading = false;

      this.isDataSent = true; 
      const updatedOrder = this.returnOrders.find(order => order.id === this.modalData.id);
      if (updatedOrder && updatedOrder.currentReturnOrderState) {
        updatedOrder.currentReturnOrderState.returnOrderStatus = ReturnOrderStatus.Sent;
      }

      this.closemodalsenddata();  


      setTimeout(() => {
        this.successMessage = ''; 
      }, 3000);
    },
    error: (err) => {
      console.error('Error sending data:', err);
      this.errorMessage = 'خطا در ارسال اطلاعات!';
      this.successMessage = '';
      this.loading = false;
    },
  });
}




  validateShippingPrice(): void {
    this.isPriceNegative = this.modalData.returnShippingPrice !== undefined && this.modalData.returnShippingPrice < 0;
  }


  detailsmodal(returnOrderId: number): void {
    this.selectedReturnOrder = this.returnOrders.filter(c => c.id == returnOrderId)[0];
    this.details = true;
  }
  openDetailsModal(returnOrderItemGroupId: number | undefined): void {
    this.selectedReturnOrderItemGroup = this.selectedReturnOrder!.itemGroups?.filter(c => c.id === returnOrderItemGroupId)[0];
    this.details = true;
  }
  
  returnitems(returnOrderItemGroupId: number | undefined): void {
    this.selectedReturnOrderItemGroup = this.selectedReturnOrder!.itemGroups?.filter(c => c.id === returnOrderItemGroupId)[0];
    this.returnitem = true;
  }
  
  documentmodal(returnOrderItemGroupId: number | undefined) {
    this.selectedReturnOrderItems = this.selectedReturnOrderItemGroup!.returnOrderItems?.filter(c => c.id == returnOrderItemGroupId)[0];
    this.allItemDocImage$ = from(
      (this.selectedReturnOrderItems?.returnOrderItemDocuments ?? []).map(itemDoc =>
        this.imageService.makeImageUrl(itemDoc?.image)
      )
    );
    this.documents = true;
  }

  costdocument(returnOrderItemGroupId: number | undefined) {
    this.selectedReturnOrderItemsDoc = this.selectedReturnOrder!.totals?.filter(c => c.id == returnOrderItemGroupId)[0];
    this.costdocuments = true;
    this.allItemcostDocImage$ = from(
      (this.safeReturnOrderItemsDoc ?? []).map(itemDoc =>
        this.imageService.makeImageUrl(itemDoc?.image)
      ));
  }
  get safeReturnOrderItemsDoc(): any[] {
    return Array.isArray(this.selectedReturnOrderItemsDoc) ? this.selectedReturnOrderItemsDoc : [];
  }

 
  closedetailsmodal() {
    this.details = false;
  }
  closedocumentmodal() {
    this.documents = false;
  }
  closecostdocument() {
    this.costdocuments = false;
  }

  selectTab(tabIndex: number) {
    this.selectedTab = tabIndex;
  }
  selectTab1(tabIndex: number) {
    this.selectedTab1 = tabIndex;
  }
  getStringPaymentType(value: number): string {
    switch (value) {
      case 0:
        return 'نقدی';
      case 1:
        return 'اعتباری';
      case 2:
        return 'آنلاین';
      default:
        return '';
    }
  }

  fetchReturnOrders(): void {
    debugger;
    this.returnOrdersClient.getReturnOrdersByOrderId(this.orderId).subscribe(
      (data: ReturnOrderByOrderIdDto[]) => {
        this.returnOrders = data;
        debugger;
        this.returnOrders.forEach(c => {
          momentJalaali.loadPersian(/*{ usePersianDigits: true }*/);
            c.createdAtText = momentJalaali(c.createdAt).format("jD jMMMM jYYYY");
        });
        this.allItemGroups = this.returnOrders.flatMap(order => order.itemGroups ?? []);
        debugger;
      },
      (error) => {
        console.error('Error fetching return orders:', error);
      }
    );
  }


  submitSendInfoForm(order: number): void {
    console.log('اطلاعات ارسال شده:', order);
    this.resetModal();
  }

  resetModal(): void {
    this.selectedOrder1 = null;
  }

  closereturnitems() {
    this.returnitem = false; 

  }

  getReturnOrderStatusText(status?: ReturnOrderStatus): string {
    switch (status) {
      case ReturnOrderStatus.PendingForRegister:
        return 'در انتظار ثبت';
      case ReturnOrderStatus.Registered:
        return 'ثبت شده';
      case ReturnOrderStatus.Rejected:
        return 'رد شده';
      case ReturnOrderStatus.Accepted:
        return 'پذیرفته شده';
      case ReturnOrderStatus.Sent:
        return 'ارسال شده';
      case ReturnOrderStatus.Received:
        return 'دریافت شده';
      case ReturnOrderStatus.AllConfirmed:
        return 'همه تایید شده‌اند';
      case ReturnOrderStatus.CostRefunded:
        return 'بازپرداخت هزینه';
      case ReturnOrderStatus.SomeConfirmed:
        return 'برخی تایید شده‌اند';
      case ReturnOrderStatus.Canceled:
        return 'لغو شده';
      case ReturnOrderStatus.Completed:
        return 'تکمیل شده';
      default:
        return 'نامشخص';
    }
  }
  getTransportationTypeText(type?: ReturnOrderTransportationType): string {
    switch (type) {
      case ReturnOrderTransportationType.NotDetermined:
        return 'مشخص نشده';
      case ReturnOrderTransportationType.CustomerReturn:
        return 'بازگشت توسط مشتری';
      case ReturnOrderTransportationType.OrganizationReturn:
        return 'بازگشت توسط سازمان';
      case ReturnOrderTransportationType.OnLocation:
        return 'تحویل در محل';
      default:
        return 'نامشخص';
    }
  }


  showShippingFormForOrder(order: MainReturnOrderDto): boolean {
    return this.selectedOrder?.orderNumber === order.orderNumber;

  }

  toggleShippingForm(order:MainReturnOrderDto ): void {
    if (this.selectedOrder?.orderNumber === order.orderNumber) {
      this.selectedOrder = null;
    } else {
      this.selectedOrder = order;
    }
  }

  submitShippingDetails(order: MainReturnOrderDto): void {
    console.log('Shipping details submitted for order:', order);
    this.selectedOrder = null;
  }
  getReturnReasonType(type: number | undefined): string {
    return type === ReturnOrderReasonType.CustomerSide ? 'سمت مشتری' : 'سمت سازمان';
  }

  getCustomerReasonType(type: number | undefined): string {
    return type === ReturnOrderCustomerReasonType.CustomerCancelation ? 'لغو کردن مشتری' : 'نامعلوم';
  }

  getOrganizationReasonType(type: number | undefined): string {
    switch (type) {
      case ReturnOrderOrganizationReasonType.WrongProduct:
        return 'محصول اشتباه';
      case ReturnOrderOrganizationReasonType.DefectiveProduct:
        return 'محصول معیوب';
      case ReturnOrderOrganizationReasonType.IncompleteProduct:
        return 'محصول ناقص';
      case ReturnOrderOrganizationReasonType.Other:
        return 'سایر';
      default:
        return 'نامعلوم';
    }
  }
  //Return Item Group
  toggleCheckbox(item: OrderItemForReturn) {
    let cmd = new CreateItemGroupCommandForReturnOrderModel();
    cmd.productAttributeOptionId = item.selectedProductAttributeOption.id;

    let returnOrderItemGroupModel = new ReturnOrderItemGroupModel();
    returnOrderItemGroupModel.groupCommand = cmd;
    returnOrderItemGroupModel.productName = item.selectedProductAttributeOption.productName;
    returnOrderItemGroupModel.productNo = item.selectedProductAttributeOption.productNo;
    returnOrderItemGroupModel.productBrandName = item.selectedProductAttributeOption.productBrandName;
    returnOrderItemGroupModel.productCategory = item.selectedProductAttributeOption.productCategory;
    returnOrderItemGroupModel.productImage = item.selectedProductAttributeOption.productImage ?? '';
    returnOrderItemGroupModel.price = item.price;
    returnOrderItemGroupModel.totalDiscountPercent = item.totalDiscountPercent;
    returnOrderItemGroupModel.taxPercent = item.taxPercent;

    const existingItem = this.findInSet(this.temporaryReturnOrderItemGroupModels, returnOrderItemGroupModel)
      || this.findInSet(this.returnOrderItemGroupModels, returnOrderItemGroupModel);

    if (existingItem) {
      this.temporaryReturnOrderItemGroupModels.delete(existingItem);
    } else {
      this.temporaryReturnOrderItemGroupModels.add(returnOrderItemGroupModel);
    }
  }
  isChecked(orderItem: OrderItemForReturn): boolean {
    return Array.from(this.returnOrderItemGroupModels).some(
      (item) => item.groupCommand.productAttributeOptionId === orderItem.selectedProductAttributeOption.id
    ) || Array.from(this.temporaryReturnOrderItemGroupModels).some(
      (item) => item.groupCommand.productAttributeOptionId === orderItem.selectedProductAttributeOption.id
    );
  }
  private findInSet(set: Set<ReturnOrderItemGroupModel>, target: ReturnOrderItemGroupModel): ReturnOrderItemGroupModel | undefined {
    for (const item of set) {
      if (item.productNo === target.productNo) {
        return item;
      }
    }
    return undefined;
  }

 
  removeItemGroup(itemGroupId: number) {
    const itemToRemove = Array.from(this.returnOrderItemGroupModels).find(
      (item) => item.groupCommand.productAttributeOptionId === itemGroupId
    );

    if (itemToRemove) {
      this.returnOrderItemGroupModels.delete(itemToRemove);
    }
  }

  addItems() {
    this.temporaryReturnOrderItemGroupModels.forEach((item) =>
      this.returnOrderItemGroupModels.add(item)
    );
    debugger;
    this.closeSelectItemModal();
    this.resetCheckboxes();
  }

  resetCheckboxes() {
    this.temporaryReturnOrderItemGroupModels.clear();
  }
  uniqueItems(items: Set<ReturnOrderItemGroupModel>): ReturnOrderItemGroupModel[] {
    const uniqueArray = Array.from(items);
    return uniqueArray.filter((item, index, self) =>
      index === self.findIndex((t) => (
        t.productNo === item.productNo
      ))
    );
  }


  trackByIndex(index: number, item: MainProductDto) {
    return item.productNo;
  }


  openSelectItemModal(): void {
    this.selectItemModal.show();

    console.log(this.selectItemModal);
  }
  closeSelectItemModal(): void {
    this.selectItemModal.hide();
  }

  //Return Item
  onSubmitReturnOrderItems() {
    if (this.returnOrderItemForm.valid) {
      let itemCommand = new CreateItemCommandForItemGroupModel();
      itemCommand.id = ++this.itemId;
      itemCommand.quantity = this.returnOrderItemForm.controls['quantity'].value;
      itemCommand.returnOrderReason = new CreateReasonCommandForItemModel();
      itemCommand.returnOrderReason.details = this.returnOrderItemForm.controls['returnOrderReasonDetails'].value;
      debugger;
      if (parseFloat(this.returnOrderItemForm.controls['returnOrderReasonType'].value) === 0) {
        itemCommand.returnOrderReason.customerType = this.returnOrderItemForm.controls['returnOrderReasonType'].value;
      } else if (parseFloat(this.returnOrderItemForm.controls['returnOrderReasonType'].value) <= 4) {
        itemCommand.returnOrderReason.organizationType = parseFloat(this.returnOrderItemForm.controls['returnOrderReasonType'].value);
      }
      debugger;
      let itemGroup = Array.from(this.returnOrderItemGroupModels).find(
        (item) => item.groupCommand.productAttributeOptionId === this.selectedGroupId
      )?.groupCommand;
      if (itemGroup) {
        if (!itemGroup.orderItems) {
          itemGroup.orderItems = [];
        }
        itemGroup?.orderItems?.push(itemCommand);
        debugger;
        this.returnOrderItemGroupModels;
        this.resetForm(this.returnOrderItemForm);
        this.closeReturnFormItemModal();
      }
    }
    this.UpdateTotalAndQuantity();
  }


  removeItem(itemGroupId: number, itemId: number) {
    const itemGroup = Array.from(this.returnOrderItemGroupModels).find(
      (item) => item.groupCommand.productAttributeOptionId === itemGroupId
    );

    if (itemGroup) {
      itemGroup.groupCommand.orderItems = itemGroup.groupCommand.orderItems?.filter(c => c.id !== itemId);
    }
    this.UpdateTotalAndQuantity();
  }

  private UpdateTotalAndQuantity() {
    this.returnOrderSubtotal = Array.from(this.returnOrderItemGroupModels).reduce((total, group) => {
      const groupSum = (group.groupCommand.orderItems || []).reduce((groupTotal, item) => {
        const quantity = item.quantity || 0;
        return groupTotal + (quantity * group.price);
      }, 0);
      return total + groupSum;
    }, 0);
    this.returnOrderQuantity = Array.from(this.returnOrderItemGroupModels).flatMap(c => c.groupCommand.orderItems).reduce((total, item) => total + (item?.quantity ?? 0), 0);
    this.returnOrderTotal = Array.from(this.returnOrderItemGroupModels).reduce((total, group) => {
      const groupSum = (group.groupCommand.orderItems || []).reduce((groupTotal, item) => {
        const quantity = item.quantity || 0;
        return groupTotal + (quantity * group.price) * (1 - (group.totalDiscountPercent / 100)) * ((1 + (group.taxPercent / 100)));
      }, 0);
      return total + groupSum;
    }, 0);
  }
  openReturnFormItemModal(productAttributeOptionId: number): void {
    this.selectedGroupId = productAttributeOptionId;
    this.selectedOrderItem = this.order.items.find(c => c.selectedProductAttributeOptionId == productAttributeOptionId);

    //apply order limit
    this.selectedOrderItemLimit = this.selectedOrderItem?.quantity ?? 0;

    //apply database limit
    const totalQuantity = this.allItemGroups
      .filter(group => group.productAttributeOptionId === productAttributeOptionId)
      .map(group => group.totalQuantity ?? 0)
      .reduce((acc, curr) => acc + curr, 0);
    this.selectedOrderItemLimit = this.selectedOrderItemLimit - totalQuantity;

    //apply create local items limit
    let localQuantity = Array.from(this.returnOrderItemGroupModels)
    .filter(group => group.groupCommand.productAttributeOptionId === productAttributeOptionId)
    .map(group => group.groupCommand.orderItems?.reduce((sum, item) => sum + item.quantity!, 0) ?? 0)
    .reduce((acc, curr) => acc + curr, 0);
    this.selectedOrderItemLimit = this.selectedOrderItemLimit - localQuantity;

    debugger;
    this.returnFormItemModal.show();
  }
  closeReturnFormItemModal(): void {
    this.returnFormItemModal.hide();
  }

  openAddDocumentModal(itemGroupId: number, itemId: number): void {
    this.selectedGroupId = itemGroupId;
    this.selectedItemId = itemId;
    let itemGroup = Array.from(this.returnOrderItemGroupModels).find(
      (item) => item.groupCommand.productAttributeOptionId === this.selectedGroupId
    )?.groupCommand;
    if (itemGroup) {
      let orderItem = itemGroup.orderItems?.find(c => c.id === itemId);
      if (orderItem) {
        this.documentListItems = orderItem.returnOrderItemDocuments ?? [];
        this.selectedReturnOrderItem = orderItem;
      }
    }
    this.addDocumentModal.show();
  }

  //Return Item Doc
  addToDocListItem() {
    this.documentFrom.markAllAsTouched();

    if (this.documentFrom.valid) {
      if (this.documentListItems.length < 5) {
        this.documentListItems.push({
          id: ++this.itemDocId,
          description: this.documentFrom.controls['description'].value,
          image: this.documentFrom.controls['imageContent'].value,
        });
        this.resetForm(this.documentFrom);
        this.imageUrl = '';
      } else {
        this.toastr.error('تعداد مستندات برای یک آیتم نمی تواند بیشتر از 5 عدد باشد');
      }
    }
  }

  removeFromDocListItem(index: number) {
    this.documentListItems.splice(index, 1);
  }

  onFileSelected(event: FileUploadMetadataDto | null): void {
    if (event) {
      this.documentFrom.controls['imageContent'].setValue(event.fileId);
    } else {
      this.documentFrom.controls['imageContent'].setValue(null);
    }
  }

  AddDocsToItem() {
    debugger;
    if (this.selectedReturnOrderItem) {
      this.selectedReturnOrderItem.returnOrderItemDocuments = this.documentListItems;
    }
    this.closeAddDocumentModal();
    this.documentListItems = [];
  }

  closeAddDocumentModal(): void {
    this.resetForm(this.documentFrom);
    this.addDocumentModal.hide();
  }

  OnSubmitCommand() {
    this.inProgress = true;

    const isUserOnline = navigator.onLine;

    if (isUserOnline) {
      this.customerAccountInfoForm.controls['value'].setValidators([Validators.required]);
    } else {
      this.customerAccountInfoForm.controls['value'].clearValidators();
    }

    this.customerAccountInfoForm.controls['value'].updateValueAndValidity();

    if (!this.isFormValid()) {
      this.toastr.error("لطفا تمام فیلدهای ضروری را پر کنید.");
      this.inProgress = false;
      return;
    }

    let createReturnOrderCommand = new CreateReturnOrderCommand();
    createReturnOrderCommand.customerAccountInfo = this.customerAccountInfoForm.controls['value'].value;
    createReturnOrderCommand.details = this.customerAccountInfoForm.controls['details'].value;

    createReturnOrderCommand.customerId = localStorage.getItem('userId') ?? '';
    createReturnOrderCommand.orderId = this.order.id;

    if (!this.returnOrderItemGroupModels || this.returnOrderItemGroupModels.size === 0) {
      this.toastr.error("هیچ آیتمی برای برگشت انتخاب نشده است.");
      this.inProgress = false;
      return;
    }

    createReturnOrderCommand.itemGroups = Array.from(this.returnOrderItemGroupModels).map(c => new CreateItemGroupCommandForReturnOrder({
      productAttributeOptionId: c.groupCommand.productAttributeOptionId,
      orderItems: c.groupCommand.orderItems?.map(e => new CreateItemCommandForItemGroup({
        quantity: e.quantity,
        returnOrderReason: new CreateReasonCommandForItem({
          details: e.returnOrderReason?.details,
          customerType: e.returnOrderReason?.customerType,
          organizationType: e.returnOrderReason?.organizationType
        }),
        returnOrderItemDocuments: e.returnOrderItemDocuments?.map(t => new DocumentCommandForItem({
          description: t.description,
          image: t.image
        }))
      }))
    }));

    createReturnOrderCommand.quantity = Array.from(this.returnOrderItemGroupModels).flatMap(c => c.groupCommand.orderItems).reduce((total, item) => total + (item?.quantity ?? 0), 0);

    createReturnOrderCommand.subtotal = Array.from(this.returnOrderItemGroupModels).reduce((total, group) => {
      const groupSum = (group.groupCommand.orderItems || []).reduce((groupTotal, item) => {
        const quantity = item.quantity || 0;
        return groupTotal + (quantity * group.price);
      }, 0);
      return total + groupSum;
    }, 0);

    createReturnOrderCommand.total = Array.from(this.returnOrderItemGroupModels).reduce((total, group) => {
      const groupSum = (group.groupCommand.orderItems || []).reduce((groupTotal, item) => {
        const quantity = item.quantity || 0;
        return groupTotal + (quantity * group.price) * (1 - (group.totalDiscountPercent / 100)) * (1 + (group.taxPercent / 100));
      }, 0);
      return total + groupSum;
    }, 0);

    console.log('Submit Command:', createReturnOrderCommand);

    this.returnOrdersClient.create(createReturnOrderCommand).subscribe(
      (result) => {
        if (result > 0) {
          this.toastr.success("برگشت سفارش با موفقیت ثبت شد.");
          this.router.navigateByUrl(this.router.url).then(() => {
            window.location.reload();
          });
          this.returnOrderItemGroupModels.clear();
        }
        else {
          this.toastr.error("خطا در ثبت برگشت سفارش.");
        }
      },
      (error) => {
        this.inProgress = false;
        console.error('Error:', error);
      }
    );
  }


  private isFormValid() {
    let result = true;
    if (!this.customerAccountInfoForm.valid) {
      result = false;
    }

    if (Array.from(this.returnOrderItemGroupModels).length === 0) {
      this.toastr.error('لطفا یک کالا را به لیست اضافه کنید');
      result = false;
    }

    Array.from(this.returnOrderItemGroupModels).forEach(c => {
      debugger;
      if (c.groupCommand?.orderItems?.length === 0 || c.groupCommand?.orderItems === undefined) {
        this.toastr.error('لطفا یک جزئیات بازگشت کالا به همه کالاهای انتخاب شده اضافه کنید');
        result = false;
      };
      c.groupCommand.orderItems?.forEach(e => {
        if (e.returnOrderItemDocuments?.length === 0 || e.returnOrderItemDocuments === undefined) {
          this.toastr.error('لطفا برای همه جزئیات بازگشت کالا حداقل یک مستند اضافه کنید');
          result = false;
        };
      });
    });
    return result;
  }


  resetForm(form: FormGroup): void {
    Object.keys(form.controls).forEach((controlName) => {
      const control = form.controls[controlName];
      if (control.enabled) {
        control.markAsPristine();
        control.markAsUntouched();
        control.reset();
      }
    });
    form.setErrors(null);
  }

  resetAllForms() {
    this.resetForm(this.customerAccountInfoForm);
    this.resetForm(this.returnOrderItemForm);
    this.resetForm(this.documentFrom);
    this.itemGroupId = 1;
    this.itemId = 1;
    this.itemDocId = 1;
    this.selectedGroupId = 0;
    this.selectedItemId = 0;
    this.documentListItems = [];
  }

  ngOnDestroy(): void {
    this.destroy$.next();
    this.destroy$.complete();
  }
}