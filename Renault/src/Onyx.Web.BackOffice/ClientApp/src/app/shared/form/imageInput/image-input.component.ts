import { FilesClient } from './../../../web-api-client';
import { Component, ElementRef, EventEmitter, Input, Output, ViewChild } from "@angular/core";

@Component({
  selector: "app-image-input",
  templateUrl: "./image-input.component.html",
  styleUrls: ["./image-input.component.scss"],
})
export class ImageInputComponent {
  @Input() selectedFileUrl: string | null | undefined;
  @Input() isRequired : boolean = false;
  @ViewChild('fileInput') fileInput!: any;
  @Input() title: string = 'تصویر';
  @Output() fileSelected = new EventEmitter<File>();
  @Output() fileCleared = new EventEmitter<void>();
  
  constructor(private FilesClient : FilesClient){
    
  }

  onFileSelected(event: Event): void {
    const input = event.target as HTMLInputElement;
    if (input.files && input.files.length > 0) {
      this.fileSelected.emit(input.files[0]);
      const file = input.files[0];
      const reader = new FileReader();
      reader.onload = (e: any) => {
        this.selectedFileUrl = e.target.result;
      };
      reader.readAsDataURL(file);
    }
  }

  clearFileForm(): void {
    this.selectedFileUrl = undefined;
    this.fileCleared.emit();
  }
}
