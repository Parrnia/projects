import { OrderForReturn, OrderItem, OrderItemForReturn, OrderPayment, OrderStateBase, OrderTotal, ProductAttributeOptionForReturn } from './../../interfaces/order';
import { CheckoutItemData, CheckoutItemOptionData } from './../../api/base/shop.api';
import { Injectable } from '@angular/core';
import { OrderItemDto, OrderTotalType, PaymentType, OrderItemOptionDto, OrderTotalDto, OrderByIdDto, OrderByCustomerIdDto, OrderByCustomerIdWithPaginationDto, CreateOrderItemCommandForOrder, CreateOrderItemOptionCommandForOrderItem, OrderStateBaseDto, ProductAttributeOptionForOrderDto, OrderItemProductAttributeOptionValueDto, OrderByNumberDto, OrderAddressDto, OrderForReturnByIdDto, OrderItemForReturnByIdDto, ProductAttributeOptionForReturnByIdDto, OrderPaymentType, OrderStatus, OrderPaymentDto } from '../../web-api-client';
import { CartTotal } from '../../services/cart.service';
import { Order, OrderItemOption } from '../../interfaces/order';
import * as momentJalaali from "moment-jalaali";
import { ProductmapperService } from '../productsCluster/productmapper.service';
import { Address } from '../../interfaces/address';
import { Product, ProductAttributeOption, ProductAttributeOptionValue } from '../../interfaces/product';
import { ImageService } from '../image.service';

@Injectable({
  providedIn: 'root'
})
export class OrdermapperService {

  constructor(private imageService: ImageService
  ) { }

  //#region front to database

  //#region OrderItem
  mapOrderItemCommand(checkoutItemData: CheckoutItemData) {
    let orderItem = new CreateOrderItemCommandForOrder();
    orderItem.quantity = checkoutItemData.quantity!;
    orderItem.productId = checkoutItemData.productId!;
    orderItem.productAttributeOptionId = checkoutItemData.selectedProductAttributeOption.id;
    return orderItem;
  }
  mapOrderItemCommands(checkoutItemDatas: CheckoutItemData[]) {
    let orderItems: CreateOrderItemCommandForOrder[] = [];
    checkoutItemDatas.forEach(c => {
      orderItems.push(this.mapOrderItemCommand(c));
    })
    return orderItems;
  }
  //#endregion

  //#region OrderItemOption
  mapOrderItemOptionCommand(checkoutItemOptionData: CheckoutItemOptionData) {
    let orderItemOption = new CreateOrderItemOptionCommandForOrderItem();
    orderItemOption.name = checkoutItemOptionData.name;
    orderItemOption.value = checkoutItemOptionData.value;
    return orderItemOption;
  }
  mapOrderItemOptionCommands(checkoutItemOptionDatas: CheckoutItemOptionData[]) {
    let orderItemOptions: CreateOrderItemOptionCommandForOrderItem[] = [];
    checkoutItemOptionDatas.forEach(c => {
      orderItemOptions.push(this.mapOrderItemOptionCommand(c));
    })
    return orderItemOptions;
  }
  //#endregion


  //#endregion



  //#region datebase to front

  //#region OrderById

  mapOrderById(orderDto: OrderByIdDto) {
    let order = new Order();
    order.id = orderDto.id!;
    order.token = orderDto.token!;
    order.number = orderDto.number!.toString();
    momentJalaali.loadPersian(/*{ usePersianDigits: true }*/);
    order.createdAt = momentJalaali(orderDto.createdAt).format("jD jMMMM jYYYY");
    order.payment = this.mapPaymentType(orderDto.orderPaymentType);
    order.paymentHistory = this.mapOrderPayments(orderDto.paymentMethods ?? [])
    order.status = this.mapOrderStatus(orderDto.currentOrderStatus);
    order.orderStateHistory = this.mapOrderStates(orderDto.orderStateHistory!);
    order.statusDetails = orderDto.currentOrderStatusDetails ?? '';
    order.CurrentOrderStatus = orderDto.currentOrderStatus ?? OrderStatus.PendingForRegister;
    order.items = this.mapOrderItems(orderDto.items!);
    order.quantity = orderDto.quantity!;
    order.subtotal = this.modifyPrice(orderDto.subtotal!)!;
    order.totals = this.mapOrderTotals(orderDto.totals!);
    order.total = this.modifyPrice(orderDto.total!)!;
    order.orderAddress = this.mapAddress(orderDto.orderAddress!);
    return order;
  }

  mapOrdersById(orderDtos: OrderByIdDto[]) {
    let orders: Order[] = [];
    orderDtos.forEach(c => {
      orders.push(this.mapOrderById(c));
    })
    return orders;
  }

  //#endregion

  //#region OrderByNumber

  mapOrderByNumber(orderDto: OrderByNumberDto) {
    let order = new Order();
    order.id = orderDto.id!;
    order.token = orderDto.token!;
    order.number = orderDto.number!.toString();
    momentJalaali.loadPersian(/*{ usePersianDigits: true }*/);
    order.createdAt = momentJalaali(orderDto.createdAt).format("jD jMMMM jYYYY");
    order.payment = this.mapPaymentType(orderDto.orderPaymentType);
    order.status = this.mapOrderStatus(orderDto.currentOrderStatus);
    order.orderStateHistory = this.mapOrderStates(orderDto.orderStateHistory!);
    order.statusDetails = orderDto.currentOrderStatusDetails ?? '';
    order.CurrentOrderStatus = orderDto.currentOrderStatus ?? OrderStatus.PendingForRegister;
    order.items = this.mapOrderItems(orderDto.items!);
    order.quantity = orderDto.quantity!;
    order.subtotal = this.modifyPrice(orderDto.subtotal!)!;
    order.totals = this.mapOrderTotals(orderDto.totals!);
    order.total = this.modifyPrice(orderDto.total!)!;
    order.orderAddress = this.mapAddress(orderDto.orderAddress!);
    order.phoneNumber = orderDto.phoneNumber;
    return order;
  }

  mapOrdersByNumber(orderDtos: OrderByNumberDto[]) {
    let orders: Order[] = [];
    orderDtos.forEach(c => {
      orders.push(this.mapOrderByNumber(c));
    })
    return orders;
  }

  //#endregion

  //#region OrderForReturnById

  mapOrderForReturnById(orderDto: OrderForReturnByIdDto) {
    let order = new OrderForReturn();
    order.id = orderDto.id!;
    order.token = orderDto.token!;
    order.number = orderDto.number!.toString();
    momentJalaali.loadPersian(/*{ usePersianDigits: true }*/);
    order.createdAt = momentJalaali(orderDto.createdAt).format("jD jMMMM jYYYY");
    order.payment = this.mapPaymentType(orderDto.orderPaymentType);
    order.status = this.mapOrderStatus(orderDto.currentOrderStatus);
    order.orderStateHistory = this.mapOrderStates(orderDto.orderStateHistory!);
    order.statusDetails = orderDto.currentOrderStatusDetails ?? '';
    order.items = this.mapOrderItemsForReturn(orderDto.itemsForReturn!);
    order.quantity = orderDto.quantity!;
    order.subtotal = this.modifyPrice(orderDto.subtotal!)!;
    order.totals = this.mapOrderTotals(orderDto.totals!);
    order.total = this.modifyPrice(orderDto.total!)!;
    order.orderAddress = this.mapAddress(orderDto.orderAddress!);
    return order;
  }

  mapOrdersForReturnById(orderDtos: OrderForReturnByIdDto[]) {
    let orders: OrderForReturn[] = [];
    orderDtos.forEach(c => {
      orders.push(this.mapOrderForReturnById(c));
    })
    return orders;
  }

  mapOrderItemForReturn(orderItemDto: OrderItemForReturnByIdDto) {
    let orderItem = new OrderItemForReturn();
    let product = new Product();
    product.name = orderItemDto.productLocalizedName!;
    orderItem.price = this.modifyPrice(orderItemDto.price!)!;
    orderItem.quantity = orderItemDto.quantity!;
    orderItem.selectedProductAttributeOption = this.mapProductAttributeOptionForOrderForReturn(orderItemDto.productAttributeOption!);
    orderItem.options = this.mapOrderItemOptions(orderItemDto.options!);
    orderItem.total = this.modifyPrice(orderItemDto.total!)!;
    orderItem.optionValues = this.mapProductAttributeOptionValuesForOrder(orderItemDto.optionValues ?? []);
    orderItem.totalDiscountPercent = orderItemDto.totalDiscountPercent ?? 0;
    orderItem.taxPercent = orderItemDto.taxPercent?? 0;
    orderItem.selectedProductAttributeOptionId = orderItemDto.productAttributeOptionId;
    return orderItem;
  }
  mapOrderItemsForReturn(orderItemDtos: OrderItemForReturnByIdDto[]) {
    let orderItems: OrderItemForReturn[] = [];
    orderItemDtos.forEach(c => {
      orderItems.push(this.mapOrderItemForReturn(c));
    })
    return orderItems;
  }
  mapProductAttributeOptionForOrderForReturn(productAttributeOptionDto: ProductAttributeOptionForReturnByIdDto) {
    let child = new ProductAttributeOptionForReturn();
    child.id = productAttributeOptionDto?.id ?? 0;
    child.productName = productAttributeOptionDto.productName ?? '';
    child.productNo = productAttributeOptionDto.productNo ?? '';
    child.productBrandName = productAttributeOptionDto.productBrandName ?? '';
    child.productCategory = productAttributeOptionDto.productCategory ?? '';
    child.productImage = this.imageService.makeImageUrl(productAttributeOptionDto.productImage);
    return child;
  }

  //#endregion
  
  //#region AllOrderDto

  // mapAllOrder(orderDto: AllOrderDto) {
  //   let order = new Order();
  //   order.id = orderDto.id!;
  //   order.token = orderDto.token!;
  //   order.number = orderDto.number!.toString();
  //   momentJalaali.loadPersian(/*{ usePersianDigits: true }*/);
  //   order.createdAt = momentJalaali(orderDto.createdAt).format("jD jMMMM jYYYY");
  //   order.payment = this.mapPaymentType(orderDto.paymentType);
  //   order.status = this.mapOrderStatus(orderDto.currentOrderStatus);
  //   order.statusDetails = orderDto.currentOrderStatusDetails ?? '';
  //   order.items = this.mapOrderItems(orderDto.items!);
  //   order.quantity = orderDto.quantity!;
  //   order.subtotal = orderDto.subtotal!;
  //   order.totals = this.mapOrderTotals(orderDto.totals!);
  //   order.total = orderDto.total!;
  //   order.shippingAddress = this.mapAddress(orderDto.shippingAddress!);
  //   order.billingAddress = this.mapAddress(orderDto.shippingAddress!);
  //   return order;
  // }
  // mapAllOrders(orderDtos: AllOrderDto[]){
  //   let orders: Order[] = [];
  //   orderDtos.forEach(c => {
  //     orders.push(this.mapAllOrder(c));
  //   })
  //   return orders;
  // }
  //#endregion

  //#region OrderByCustomerIdDto

  mapOrderByCustomerId(orderDto: OrderByCustomerIdDto) {
    let order = new Order();
    order.id = orderDto.id!;
    order.token = orderDto.token!;
    order.number = orderDto.number!.toString();
    momentJalaali.loadPersian(/*{ usePersianDigits: true }*/);
    order.createdAt = momentJalaali(orderDto.createdAt).format("jD jMMMM jYYYY");
    order.payment = this.mapPaymentType(orderDto.orderPaymentType);
    order.status = this.mapOrderStatus(orderDto.currentOrderStatus);
    order.statusDetails = orderDto.currentOrderStatusDetails ?? '';
    order.CurrentOrderStatus = orderDto.currentOrderStatus ?? OrderStatus.PendingForRegister;
    order.items = this.mapOrderItems(orderDto.items!);
    order.quantity = orderDto.quantity!;
    order.subtotal = this.modifyPrice(orderDto.subtotal!)!;
    order.totals = this.mapOrderTotals(orderDto.totals!);
    order.total = this.modifyPrice(orderDto.total!)!;
    order.orderAddress = this.mapAddress(orderDto.orderAddress!);
    return order;
  }
  mapOrdersByCustomerId(orderDtos: OrderByCustomerIdDto[]) {
    let orders: Order[] = [];
    orderDtos.forEach(c => {
      orders.push(this.mapOrderByCustomerId(c));
    })
    return orders;
  }
  //#endregion

  //#region OrderByProductIdDto

  // mapOrderByProductId(orderDto: OrderByProductIdDto) {
  //   let order = new Order();
  //   order.id = orderDto.id!;
  //   order.token = orderDto.token!;
  //   order.number = orderDto.number!.toString();
  //   momentJalaali.loadPersian(/*{ usePersianDigits: true }*/);
  //   order.createdAt = momentJalaali(orderDto.createdAt).format("jD jMMMM jYYYY");
  //   order.payment = this.mapPaymentType(orderDto.paymentType);
  //   order.status = this.mapOrderStatus(orderDto.currentOrderStatus);
  //   order.statusDetails = orderDto.currentOrderStatusDetails ?? '';
  //   order.items = this.mapOrderItems(orderDto.items!);
  //   order.quantity = orderDto.quantity!;
  //   order.subtotal = orderDto.subtotal!;
  //   order.totals = this.mapOrderTotals(orderDto.totals!);
  //   order.total = orderDto.total!;
  //   order.shippingAddress = this.mapAddress(orderDto.shippingAddress!);
  //   order.billingAddress = this.mapAddress(orderDto.shippingAddress!);
  //   return order;
  // }
  // mapOrdersByProductId(orderDtos: OrderByProductIdDto[]){
  //   let orders: Order[] = [];
  //   orderDtos.forEach(c => {
  //     orders.push(this.mapOrderByProductId(c));
  //   })
  //   return orders;
  // }
  //#endregion

  //#region OrderByCustomerIdWithPaginationDto

  mapOrderByCustomerIdWithPagination(orderDto: OrderByCustomerIdWithPaginationDto) {
    let order = new Order();
    order.id = orderDto.id!;
    order.token = orderDto.token!;
    order.number = orderDto.number!.toString();
    momentJalaali.loadPersian(/*{ usePersianDigits: true }*/);
    order.createdAt = momentJalaali(orderDto.createdAt).format("jD jMMMM jYYYY");
    order.payment = this.mapPaymentType(orderDto.orderPaymentType);
    order.status = this.mapOrderStatus(orderDto.currentOrderStatus);
    order.statusDetails = orderDto.currentOrderStatusDetails ?? '';
    order.CurrentOrderStatus = orderDto.currentOrderStatus ?? OrderStatus.PendingForRegister;
    order.items = this.mapOrderItems(orderDto.items!);
    order.quantity = orderDto.quantity!;
    order.subtotal = this.modifyPrice(orderDto.subtotal!)!;
    order.totals = this.mapOrderTotals(orderDto.totals!);
    order.total = this.modifyPrice(orderDto.total!)!;
    order.orderAddress = this.mapAddress(orderDto.orderAddress!);
    return order;
  }
  mapOrdersByCustomerIdWithPagination(orderDtos: OrderByCustomerIdWithPaginationDto[]) {
    let orders: Order[] = [];
    orderDtos.forEach(c => {
      orders.push(this.mapOrderByCustomerIdWithPagination(c));
    })
    return orders;
  }
  //#endregion

  //#region shared

  //#region PaymentType
  mapPaymentType(type: number | undefined) {
    switch (type) {
      case 0:
        return OrderPaymentType.Unspecified;
      case 1:
        return OrderPaymentType.Cash;
      case 2:
        return OrderPaymentType.Credit;
      case 3:
        return OrderPaymentType.Online;
      case 4:
        return OrderPaymentType.CreditOnline;
      default:
        return OrderPaymentType.Unspecified;
    }
  }
  //#endregion

  //#region OrderStatus
  mapOrderStatus(type: number | undefined) {
    switch (type) {
      case 0:
        return 'در انتظار ثبت';
      case 1:
        return 'در انتظار تایید پرداخت';
      case 2:
        return 'در انتظار تایید سفارش';
      case 3:
        return 'در انتظار آماده سازی';
      case 4:
        return 'در انتظار ارسال';
      case 5:
        return 'ارسال شده';
      case 6:
        return 'پرداخت تایید نشده';
      case 7:
        return 'سفارش تایید نشده و در انتظار بازگشت وجه احتمالی';
      case 8:
        return 'لغو شده و در انتظار بازگشت وجه احتمالی';
      case 9:
        return 'مبلغ سفارش بازگشت داده شده';
      case 10:
        return 'فرایند سفارش کامل شده است';
      default:
        return 'در انتظار';
    }
  }
  //#endregion

  //#region OrderStatus
  mapOrderTotalTypeToDto(type: number | undefined) {
    switch (type) {
      case 0:
        return 'shipping';
      case 1:
        return 'fee';
      case 2:
        return 'tax';
      case 3:
        return 'other';
      case 4:
        return 'freeShipping';
      case 5:
        return 'discountOnCustomerType';
      case 6:
        return 'discountOnProduct';
      default:
        return 'other';

    }
  }
  //#endregion

  //#region Address
  mapAddress(addressDto: OrderAddressDto) {
    let address = new Address();
    address.id = addressDto.id!;
    address.title = addressDto.title!;
    address.company = addressDto.company!;
    address.country = addressDto.country!;
    address.address1 = addressDto.addressDetails1!;
    address.address2 = addressDto.addressDetails2!;
    address.city = addressDto.city!;
    address.state = addressDto.state!;
    address.postcode = addressDto.postcode!;
    return address;
  }
  //#endregion

  //#region OrderItem
  mapOrderItem(orderItemDto: OrderItemDto) {
    let orderItem = new OrderItem();
    let product = new Product();
    product.name = orderItemDto.productLocalizedName!;
    product.slug = orderItemDto.productSlug!;
    orderItem.price = this.modifyPrice(orderItemDto.price!)!;
    orderItem.quantity = orderItemDto.quantity!;
    orderItem.product = product;
    orderItem.selectedProductAttributeOption = this.mapProductAttributeOptionForOrder(orderItemDto.productAttributeOption!);
    orderItem.options = this.mapOrderItemOptions(orderItemDto.options!);
    orderItem.total = this.modifyPrice(orderItemDto.total!)!;
    orderItem.optionValues = this.mapProductAttributeOptionValuesForOrder(orderItemDto.optionValues ?? []);
    orderItem.discountPercentForProduct = orderItemDto.discountPercentForProduct ?? 0;
    return orderItem;
  }
  mapOrderItems(orderItemDtos: OrderItemDto[]) {
    let orderItems: OrderItem[] = [];
    orderItemDtos.forEach(c => {
      orderItems.push(this.mapOrderItem(c));
    })
    return orderItems;
  }
  //#endregion

  //#region OrderItemOption
  mapOrderItemOption(orderItemOptionDto: OrderItemOptionDto) {
    let orderItemOption = new OrderItemOption();
    orderItemOptionDto.name = orderItemOptionDto.name;
    orderItemOptionDto.value = orderItemOptionDto.value;
    return orderItemOption;
  }
  mapOrderItemOptions(orderItemOptionDtos: OrderItemOptionDto[]) {
    let orderItemOptions: OrderItemOption[] = [];
    orderItemOptionDtos.forEach(c => {
      orderItemOptions.push(this.mapOrderItemOption(c));
    })
    return orderItemOptions;
  }
  //#endregion
//#region OrderPayment Mapper
mapOrderPayment(orderPaymentDto: OrderPaymentDto): OrderPayment {
  let orderPayment = new OrderPayment();
  orderPayment.id = orderPaymentDto.id;
  orderPayment.paymentType = orderPaymentDto.paymentType;
  orderPayment.paymentTypeName = orderPaymentDto.paymentTypeName;
  orderPayment.amount = orderPaymentDto.amount;
  orderPayment.paymentServiceType = orderPaymentDto.paymentServiceType;
  orderPayment.paymentServiceTypeName = orderPaymentDto.paymentServiceTypeName;
  orderPayment.status = orderPaymentDto.status;
  orderPayment.statusName = orderPaymentDto.statusName;
  orderPayment.authority = orderPaymentDto.authority;
  orderPayment.cardNumber = orderPaymentDto.cardNumber;
  orderPayment.rrn = orderPaymentDto.rrn;
  orderPayment.refId = orderPaymentDto.refId;
  orderPayment.payGateTranId = orderPaymentDto.payGateTranId;
  orderPayment.salesOrderId = orderPaymentDto.salesOrderId;
  orderPayment.serviceTypeId = orderPaymentDto.serviceTypeId;
  orderPayment.error = orderPaymentDto.error;
  return orderPayment;
}

mapOrderPayments(orderPaymentDtos: OrderPaymentDto[]): OrderPayment[] {
  let orderPayments: OrderPayment[] = [];
  orderPaymentDtos.forEach(dto => {
      orderPayments.push(this.mapOrderPayment(dto));
  });
  return orderPayments;
}
//#endregion

  //#region OrderTotal
  mapOrderTotal(orderTotalDto: OrderTotalDto) {
    let orderTotal = new OrderTotal();
    orderTotal.price = this.modifyPrice(orderTotalDto.price!)!;
    orderTotal.title = orderTotalDto.title!;
    orderTotal.type = this.mapOrderTotalTypeToDto(orderTotalDto.type)!;
    return orderTotal;
  }
  mapOrderTotals(orderTotalDtos: OrderTotalDto[]) {
    let orderTotals: OrderTotal[] = [];
    orderTotalDtos.forEach(c => {
      orderTotals.push(this.mapOrderTotal(c));
    })
    return orderTotals;
  }
  //#endregion

  //#region OrderStateBase
  mapOrderState(orderStateBaseDto: OrderStateBaseDto) {
    let orderStateBase = new OrderStateBase();
    orderStateBase.created = momentJalaali(orderStateBaseDto.created).format("jD jMMMM jYYYY");
    orderStateBase.details = orderStateBaseDto.details?.toString()!;
    orderStateBase.orderStatus = this.mapOrderStatus(orderStateBaseDto.orderStatus);
    return orderStateBase;
  }
  mapOrderStates(OrderStateBaseDtos: OrderStateBaseDto[]) {
    let orderStateBases: OrderStateBase[] = [];
    OrderStateBaseDtos.forEach(c => {
      orderStateBases.push(this.mapOrderState(c));
    })
    return orderStateBases;
  }
  //#endregion

  //#region ProductAttributeOption
  mapProductAttributeOptionForOrder(productAttributeOptionDto: ProductAttributeOptionForOrderDto) {
    let child: ProductAttributeOption = { id: 0, availability: 'out-of-stock', isDefault: false, optionValues: [], price: 0, maxOrderQuantityPerOrder: 1, minOrderQuantityPerOrder: 1, badges: [], compareAtPrice: 0 };
    child.id = productAttributeOptionDto?.id ?? 0;
    return child;
  }
  mapProductAttributeOptionValuesForOrder(productAttributeOptionValueDtos: OrderItemProductAttributeOptionValueDto[]) {
    let res: ProductAttributeOptionValue[] = [];
    productAttributeOptionValueDtos.forEach(l => {
      let child: ProductAttributeOptionValue = { id: 0, name: "", value: "" };
      child.id = l.id ?? 0;
      child.name = l.name ?? '';
      child.value = l.value ?? '';
      res.push(child);
    })
    return res;
  }
  //#endregion
  addTax(value: number | undefined) {
    if (value == undefined) {
      return undefined;
    }
    return value * 1.1;
  }
  convertToTooman(value: number | undefined) {
    if (value == undefined) {
      return undefined;
    }
    return value * 0.1;
  }
  modifyPrice(value: number){
    return this.convertToTooman(value);
  }
  //#endregion
  //#endregion

}