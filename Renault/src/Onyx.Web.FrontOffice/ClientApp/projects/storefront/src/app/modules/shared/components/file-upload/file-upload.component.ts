import { Component, OnInit, OnDestroy, Input, Output, ViewChild, ElementRef, Renderer2, EventEmitter } from "@angular/core";
import { ToastrService } from "ngx-toastr";
import { ImageService } from "projects/storefront/src/app/mapServieces/image.service";
import { FileUploadMetadataDto, FilesClient, FileParameter } from "projects/storefront/src/app/web-api-client";
import { BehaviorSubject, of } from "rxjs";


@Component({
  selector: "app-file-upload",
  templateUrl: "./file-upload.component.html",
  styleUrls: ["./file-upload.component.scss"],
})
export class FileUploadComponent implements OnInit, OnDestroy {
  @Input() title: string = "تصویر";
  @Input() savedFileId: string | undefined = undefined;
  @Input() savedFileUrl: string | undefined = undefined;
  @Input() isRequired: boolean = false;

  id: string = "تصویر";
  @Output() uploadResponseEvent =
    new EventEmitter<FileUploadMetadataDto | null>();

  @ViewChild("fileInput", { static: false }) fileInput!: ElementRef;

  public _uploadedImageUrl$ = new BehaviorSubject<string | ArrayBuffer | null>(
    null
  );

  selectedFile: File | null = null;
  uploadResponse?: FileUploadMetadataDto;
  isUpdate: boolean = false;
  maxSizeMB: number = 5;
  fileName: string | undefined;

  constructor(
    private filesClient: FilesClient,
    private toast: ToastrService,
    private imageService: ImageService,
    private renderer: Renderer2
  ) { }

  ngOnInit(): void {
    debugger;
    this.id = this.title.replace(" ", "-");
    if (this.savedFileUrl !== undefined) {
      this._uploadedImageUrl$.next(this.savedFileUrl);
      this.isUpdate = true;
    }
  }

  ngAfterViewInit() {
    if (this.fileInput !== undefined) {
      this.renderer.setAttribute(this.fileInput.nativeElement, "id", this.id);
    }
  }

  get uploadedImageUrl$() {
    return this.savedFileId != undefined
      ? this._uploadedImageUrl$.asObservable()
      : of(null);
  }

  onFileSelected(event: any) {
    this.selectedFile = event.target.files[0];
  }

  onUploadTemp() {
    debugger;
    if (this.selectedFile) {
      const fileParameter: FileParameter = {
        data: this.selectedFile,
        fileName: this.selectedFile.name,
      };
      let fileSizeIsValid = this.isFileSizeValid(this.selectedFile);
      debugger;
      if (
        this.selectedFile.type !== "image/jpeg" &&
        this.selectedFile.type !== "image/svg" &&
        this.selectedFile.type !== "image/webp" &&
        this.selectedFile.type !== "image/jpg" &&
        this.selectedFile.type !== "image/png"
      ) {
        this.toast.error("فرمت تصویر نامعتبر است");
      } else if (!fileSizeIsValid) {
        this.toast.error(
          "حداکثر حجم تصویر باید " + this.maxSizeMB + " مگابایت باشد"
        );
      } else {
        this.filesClient.uploadTemp(fileParameter).subscribe(
          (response) => {
            this.uploadResponse = response;
            this.uploadResponseEvent.emit(response); // Emit the upload response
            if (this.selectedFile) {
              debugger;
              this.onGetTemp(response.fileId ?? "");
            }
            console.log("Upload response", response);
          },
          (error) => {
            console.error("Upload error", error);
          }
        );
      }
    }
  }

  onDownloadTemp(fileId: string) {
    this.filesClient.downloadTemp(fileId).subscribe(
      (response) => {
        const url = window.URL.createObjectURL(response.data);
        const a = document.createElement("a");
        a.href = url;
        a.download = response.fileName ?? "file.jpg";
        a.click();
        window.URL.revokeObjectURL(url);
      },
      (error) => {
        console.error("Download error", error);
      }
    );
  }

  onDeleteTemp(fileId: string) {
    this.filesClient.deleteTemp(fileId).subscribe(
      (response) => {
        this.uploadResponse = undefined;
        this._uploadedImageUrl$.next(null);
        this.uploadResponseEvent.emit(null); // Emit the upload response
      },
      (error) => {
        console.error("Delete error", error);
      }
    );
  }

  onGetTemp(fileId: string) {
    this._uploadedImageUrl$.next(this.imageService.makeImageUrlTemp(fileId ) ?? '');
  }

  onDownloadPersisted(fileId: string) {
    debugger;
    this.filesClient.downloadPersisted(fileId).subscribe(
      (response) => {
        const url = window.URL.createObjectURL(response.data);
        const a = document.createElement("a");
        a.href = url;
        a.download = response.fileName ?? "file.jpg";
        a.click();
        window.URL.revokeObjectURL(url);
      },
      (error) => {
        console.error("Download error", error);
      }
    );
  }

  onDeletePersisted() {
    this.uploadResponse = undefined;
    this._uploadedImageUrl$.next(null);
    this.savedFileId = undefined;
    this.uploadResponseEvent.emit(null); // Emit the upload response
  }

  ngOnDestroy(): void {
    debugger;
    this.savedFileId = undefined;
    this.selectedFile = null;
    this._uploadedImageUrl$.next(null);
    this.isUpdate = false;
  }

  isFileSizeValid(file: File): boolean {
    debugger;
    const fileSizeMB = file.size / 1024 / 1024; // Convert file size to MB
    return fileSizeMB <= this.maxSizeMB;
  }

  chooseFile(): void {
    const fileInput = document.getElementById(this.id) as HTMLInputElement;
    fileInput.click();
  }
}
