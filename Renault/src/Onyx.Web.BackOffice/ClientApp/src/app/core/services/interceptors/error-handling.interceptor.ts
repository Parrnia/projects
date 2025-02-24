import { Injectable } from '@angular/core';
import { HttpInterceptor, HttpRequest, HttpHandler, HttpEvent, HttpErrorResponse } from '@angular/common/http';
import { Observable, throwError } from 'rxjs';
import { catchError } from 'rxjs/operators';
import { ToastService } from '../toastService/toast-service';

@Injectable()
export class ErrorHandlingInterceptor implements HttpInterceptor {


  constructor(
    private toastr: ToastService,
  ) { }

  intercept(request: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
    return next.handle(request).pipe(
      catchError((error: HttpErrorResponse) => {

        let validationErrors: string[] = [];
        this.blobToText(error.error).subscribe({
          next: (res) => {
            debugger;
            let paresedResult = JSON.parse(res != '' ? res : "{}");
            if (error.status === 400) {
              if (res != '') {
                let mainValidationErrors = this.extractValidationErrors(paresedResult);
                debugger;
                mainValidationErrors.forEach(c => validationErrors.push(c));
              }
              if (error.error.message || error.error.Message || paresedResult.Message) {
                if (error.error.message) {
                  validationErrors.push(error.error.message);
                }
                if (error.error.Message) {
                  validationErrors.push(error.error.Message);
                }
                if (paresedResult.Message) {
                  validationErrors.push(paresedResult.Message);
                }
              }
            } else if (error.status === 401) {
              validationErrors.push('شما دسترسی لازم را ندارید');
            } else if (error.status === 500) {
              validationErrors.push('مشکلی در سرور رخ داده است');
            };

            if (validationErrors.length === 0) {
              validationErrors.push('مشکلی رخ داده است');
            }

            validationErrors.forEach(e => this.toastr.showError(e));
          },
          error: () => {

          }
        })


        return throwError(error);
      })
    );
  }

  private extractValidationErrors(response: any): string[] {
    const validationMessages: string[] = [];
    debugger;
    Object.keys(response.Errors).forEach(key => {

      validationMessages.push(...response.Errors[key]);
    });

    return validationMessages;
  }

  private blobToText(blob: any): Observable<string> {
    return new Observable<string>((observer: any) => {

      if (!blob) {
        observer.next("");
        observer.complete();
      } else {
        let reader = new FileReader();
        reader.onload = event => {
          observer.next((event.target as any).result);
          observer.complete();
        };
        if (blob instanceof Blob) {
          reader.readAsText(blob);
        } else {
          reader.readAsText(new Blob());
        }
      }
    });
  }
}
