import { Component, TemplateRef } from '@angular/core';
import { ToastService } from './toast-service';

@Component({
  selector: 'app-toasts',
  template: `
    <ngb-toast
      *ngFor="let toast of toastService.toasts"
      [class]="toast.classname"
      [autohide]="true"
      [delay]="toast.delay || 5000"
      (hidden)="toastService.remove(toast)"
      (mouseenter)="onMouseEnter()"
      (mouseleave)="onMouseLeave()"
      (click)="onClick(toast)"
    >
      <ng-template [ngIf]="isTemplate(toast)" [ngIfElse]="text">
        <ng-template [ngTemplateOutlet]="toast.textOrTpl"></ng-template>
      </ng-template>

      <ng-template #text>{{ toast.textOrTpl }}</ng-template>
    </ngb-toast>
  `,
  host: { 'class': 'toast-container position-fixed top-0 end-0 p-3', 'style': 'z-index: 1200' }
})
export class ToastsContainer {
  constructor(public toastService: ToastService) {
    debugger;
  }

  isTemplate(toast: { textOrTpl: any }) {
    return toast.textOrTpl instanceof TemplateRef;
  }

  onMouseEnter() {
    // Change the cursor to a pointer when hovering over a toast
    document.body.style.cursor = 'pointer';
  }

  onMouseLeave() {
    // Reset the cursor to its default state when not hovering over a toast
    document.body.style.cursor = 'auto';
  }

  onClick(toast: any) {
    // Remove the toast instantly when clicked
    this.toastService.remove(toast);
    document.body.style.cursor = 'auto';
  }
}
