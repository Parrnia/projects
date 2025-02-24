import {
    ChildLinkDtoFirstLayer,
    ChildLinkDtoSecondLayer,
    FirstLayerLinkDto,
} from './../../../web-api-client';
import { map } from 'rxjs';
import { Megamenu, MegamenuColumn } from './../../../interfaces/menu';
import { DepartmentsLink } from './../../../interfaces/departments-link';
import { Injectable } from '@angular/core';
import { Brand } from '../../../interfaces/brand';
import { NestedLink } from '../../../interfaces/link';

@Injectable({
    providedIn: 'root',
})
export class LinkmapperService {
    constructor() {}

    //#region FirstLayerLinks
    mapFirstLayerLink(linkDto: FirstLayerLinkDto) {
        let departmentsLink = new DepartmentsLink();
        departmentsLink.title = linkDto.title ?? '';
        departmentsLink.url = linkDto.url;
        departmentsLink.submenu = this.mapMegamenu(linkDto);
        return departmentsLink;
    }

    mapMegamenu(linkDto: FirstLayerLinkDto) {
        let megaMenu = new Megamenu();
        megaMenu.image = linkDto.image;
        megaMenu.size = 'xl';
        megaMenu.type = 'megamenu';
        megaMenu.columns = this.mapMegaMenuColumns(linkDto.childLinks ?? []);
        return megaMenu;
    }

    mapMegaMenuColumns(childLinkDtos: ChildLinkDto[]) {
        let megamenuColumns: MegamenuColumn[] = [];
        megamenuColumns.push(this.mapMegaMenuColumn(childLinkDtos));
        return megamenuColumns;
    }

    mapMegaMenuColumn(childLinkDtos: ChildLinkDto[]) {
        let column = new MegamenuColumn();
        column.size = '1of5';
        column.links = this.mapLinks(childLinkDtos);
        return column;
    }

    mapLinks(childLinkDtos: ChildLinkDto[]) {
        let links: NestedLink[] = [];
        childLinkDtos.forEach((c) => {
            links.push(this.mapLink(c));
        });
        return links;
    }

    mapLink(childLinkDto: ChildLinkDto) {
        let nestedLink = new NestedLink();
        nestedLink.title = childLinkDto.title ?? '';
        nestedLink.url = childLinkDto.url;
        nestedLink.links = this.mapLinks(childLinkDto.childLinks ?? []);
        return nestedLink;
    }

    mapFirstLayerLinks(linkDtos: FirstLayerLinkDto[]) {
        let departmentsLinks: DepartmentsLink[] = [];
        linkDtos.forEach((c) => {
            departmentsLinks.push(this.mapFirstLayerLink(c));
        });
        return departmentsLinks;
    }
    //#endregion
}

export type ChildLinkDto = ChildLinkDtoFirstLayer | ChildLinkDtoSecondLayer;
