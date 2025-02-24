import { Injectable, TemplateRef } from '@angular/core';

@Injectable({ providedIn: 'root' })
export class ToastService {
  toasts: any[] = [];

  showSuccess(textOrTpl: string | TemplateRef<any>, options: any = {}) {
    this.showNotification(textOrTpl, { ...options, classname: 'bg-success text-light', delay: 3000 });
  }

  showError(textOrTpl: string | TemplateRef<any>, options: any = {}) {
    this.showNotification(textOrTpl, { ...options, classname: 'bg-danger text-light', delay: 3000 });
  }

  showWarning(textOrTpl: string | TemplateRef<any>, options: any = {}) {
    this.showNotification(textOrTpl, { ...options, classname: 'bg-warning text-dark', delay: 3000 });
  }

  showInfo(textOrTpl: string | TemplateRef<any>, options: any = {}) {
    this.showNotification(textOrTpl, { ...options, classname: 'bg-info text-light', delay: 3000 });
  }

  private showNotification(textOrTpl: string | TemplateRef<any>, options: any = {}) {
    this.toasts.push({ textOrTpl, ...options });
  }

  remove(toast: any) {
    this.toasts = this.toasts.filter(t => t !== toast);
  }
}

