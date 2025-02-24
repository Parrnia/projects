import { LinkmapperService } from './../../../../mapServieces/layoutsCluster/headerCluster/link-mapper.service';
import { LinksClient } from './../../../../web-api-client';
import {
    Component,
    ElementRef,
    HostBinding,
    Inject,
    Input,
    NgZone,
    OnDestroy,
    OnInit,
    PLATFORM_ID,
} from '@angular/core';
import { DepartmentsLink } from '../../../../interfaces/departments-link';
import { fromOutsideClick } from '../../../../functions/rxjs/from-outside-click';
import { of, Subject } from 'rxjs';
import { filter, switchMap, takeUntil } from 'rxjs/operators';
import { isPlatformBrowser } from '@angular/common';
import { departments } from 'projects/storefront/src/data/header-departments';
import { ImageService } from 'projects/storefront/src/app/mapServieces/image.service';

@Component({
    selector: 'app-departments',
    templateUrl: './departments.component.html',
    styleUrls: ['./departments.component.scss'],
})
export class DepartmentsComponent implements OnInit, OnDestroy {
    private destroy$: Subject<void> = new Subject<void>();

    isOpen = false;

    items: DepartmentsLink[] = [];

    currentItem: DepartmentsLink|null = null;

    @Input() label: string = '';
    //@Input() items: any = [];

    @HostBinding('class.departments') classDepartments = true;

    @HostBinding('class.departments--open') get classDepartmentsOpen() {
        return this.isOpen;
    }

    constructor(
        @Inject(PLATFORM_ID) private platformId: any,
        private elementRef: ElementRef<HTMLElement>,
        private zone: NgZone,
        private linksClient: LinksClient,
        private linkmapperService: LinkmapperService,
        private imageService: ImageService
    ) { }

    ngOnInit(): void {
        if (!isPlatformBrowser(this.platformId)) {
            return;
        }

        this.zone.runOutsideAngular(() => {
            fromOutsideClick(this.elementRef.nativeElement).pipe(
                filter(() => this.isOpen),
                takeUntil(this.destroy$),
            ).subscribe(() => {
                this.zone.run(() => this.isOpen = false);
            });
        });
        this.linksClient.getFirstLayerLinks().pipe(switchMap(res => {
            this.items = this.linkmapperService.mapFirstLayerLinks(res);
            this.items.forEach(c => {
                if(c.submenu !== undefined){
                    c.submenu.image = this.imageService.makeImageUrl(c.submenu?.image);
                }
            });
            return of(res);
        }
    )).subscribe({
            next: (res) => {
            }
        })
    }

    ngOnDestroy(): void {
        this.destroy$.next();
        this.destroy$.complete();
    }

    onClick() {
        this.isOpen = !this.isOpen;
    }

    onMouseenter(item: any) {
        this.currentItem = item;
    }

    onMouseleave() {
        this.currentItem = null;
    }

    onItemClick(): void {
        this.isOpen = false;
        this.currentItem = null;
    }


}