import { ChangeDetectionStrategy, ChangeDetectorRef, Component, OnDestroy, OnInit, TemplateRef, ViewChild, ViewContainerRef, ViewEncapsulation } from '@angular/core';
import { Overlay, OverlayRef } from '@angular/cdk/overlay';
import { TemplatePortal } from '@angular/cdk/portal';
import { MatButton } from '@angular/material/button';
import { map, Observable, Subject, takeUntil } from 'rxjs';
import { Message } from 'app/layout/common/messages/messages.types';
import { MessagesService } from 'app/layout/common/messages/messages.service';
import { AuthorizeService } from 'api-authorization/authorize.service';

@Component({
    selector       : 'messages',
    templateUrl    : './messages.component.html',
    exportAs       : 'messages'
})
export class MessagesComponent implements OnInit
{
    public isAuthenticated?: Observable<boolean>;
    public userName?: Observable<string | null | undefined>;
  
    constructor(private authorizeService: AuthorizeService) { }
  
    ngOnInit() {
      this.isAuthenticated = this.authorizeService.isAuthenticated();
      this.userName = this.authorizeService.getUser().pipe(map(u => u && u.name));
    }
}
