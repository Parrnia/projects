import { Injectable } from '@angular/core';
import { Observable, Observer } from 'rxjs';
import { FilesClient } from '../web-api-client';
import { DomSanitizer, SafeResourceUrl } from '@angular/platform-browser';
import { AppConfig } from 'config';

@Injectable({
  providedIn: 'root',
})
export class ImageService {
  constructor(private filesClient : FilesClient,
    private sanitizer: DomSanitizer
  ) { }

  public makeImageUrl(id: string | undefined): string | undefined {
    if(id === undefined){
      return undefined;
    }
    return `${AppConfig.filesUrl}/${id}`
  }

  public makeImageUrlTemp(id: string | undefined): string | undefined {
    if(id === undefined){
      return undefined;
    }
    return `${AppConfig.tempsUrl}/${id}`
  }
  
  public convertBlobToSrc(blob: Blob | undefined): Observable<SafeResourceUrl | undefined> {
    return new Observable((observer: Observer<SafeResourceUrl | undefined>) => {
      if (!blob) {
        observer.next(undefined);
        observer.complete();
        return;
      }
  
      const reader = new FileReader();
      reader.onloadend = () => {
        observer.next(this.sanitizer.bypassSecurityTrustResourceUrl(reader.result as string));
        observer.complete();
      };
  
      reader.onerror = (error) => {
        observer.error(error);
      };
  
      reader.readAsDataURL(blob);
    });
  }

}
