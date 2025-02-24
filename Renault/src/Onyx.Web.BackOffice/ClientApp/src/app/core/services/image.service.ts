import { Injectable } from '@angular/core';
import { Observable, from, Observer, forkJoin, of } from 'rxjs';
import { GlobalComponent } from 'src/app/global-component';
import { FileParameter, FilesClient } from 'src/app/web-api-client';

@Injectable({
  providedIn: 'root',
})
export class ImageService {
  constructor(private filesClient : FilesClient) { }


  public getImageSrcById(id: string | undefined): string | undefined {
    if(id === undefined){
      return undefined;
    }
    return `${GlobalComponent.FILES_URL}/${id}`
  }
  public getImageSrcByIdTemp(id: string | undefined): string | undefined {
    if(id === undefined){
      return undefined;
    }
    return `${GlobalComponent.TEMPS_URL}/${id}`
  }


  public convertListOfBlobsToSrcList(blobs: (Blob | undefined)[]): Observable<(string | undefined)[]> {
    const observables = blobs?.map((blob) => this.convertBlobToSrc(blob)) || [];
    
    return new Observable((observer: Observer<(string | undefined)[]>) => {
      forkJoin(observables).subscribe(
        (results) => {
          observer.next(results);
          observer.complete();
        },
        (error) => {
          observer.error(error);
        }
      );
    });
  }
  
  public convertBlobToSrc(blob: Blob | undefined): Observable<string | undefined> {
    return new Observable((observer: Observer<string | undefined>) => {
      if (!blob) {
        observer.next(undefined);
        observer.complete();
        return;
      }
  
      const reader = new FileReader();
      reader.onloadend = () => {
        observer.next(reader.result as string);
        observer.complete();
      };
  
      reader.onerror = (error) => {
        observer.error(error);
      };
  
      reader.readAsDataURL(blob);
    });
  }
}
