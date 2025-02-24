import { Component, EventEmitter, HostBinding, Input, Output, SimpleChanges } from '@angular/core';
import { NestedLink } from '../../../../interfaces/link';

@Component({
    selector: 'app-menu',
    templateUrl: './menu.component.html',
    styleUrls: ['./menu.component.scss'],
})
export class MenuComponent {
    @Input() items: any[] = [];

    @Output() itemClick: EventEmitter<any> = new EventEmitter<any>();

    @HostBinding('class.menu') classMenu = true;

    constructor() { }

    ngOnChanges(changes: SimpleChanges) {
    
     console.log(' app menu items' ,  changes['items'].currentValue)

    }
}
