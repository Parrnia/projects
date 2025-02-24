import { Injectable } from '@angular/core';
import { BehaviorSubject } from 'rxjs';

@Injectable({
  providedIn: 'root',
})
export class LoadingService {
  private loadingSubject = new BehaviorSubject<boolean>(false);

  setLoading(loading: boolean): void {
    this.loadingSubject.next(loading);
  }

  getLoadingState(): BehaviorSubject<boolean> {
    return this.loadingSubject;
  }
}