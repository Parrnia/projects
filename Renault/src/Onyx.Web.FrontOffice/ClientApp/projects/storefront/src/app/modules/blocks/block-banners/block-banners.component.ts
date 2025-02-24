import { ImageService } from '../../../mapServieces/image.service';
import {
    BlockBannerDto,
    BlockBannerPosition,
    BlockBannersClient,
} from './../../../web-api-client';
import { Component, HostBinding } from '@angular/core';

@Component({
    selector: 'app-block-banners',
    templateUrl: './block-banners.component.html',
    styleUrls: ['./block-banners.component.scss'],
})
export class BlockBannersComponent {
    @HostBinding('class.block') classBlock = true;

    @HostBinding('class.block-banners') classBlockBanners = true;

    leftBlock!: BlockBannerDto | undefined;
    rightBlock!: BlockBannerDto | undefined;

    constructor(
        private blockBannersClient: BlockBannersClient,
        private imageService: ImageService
    ) {
        this.blockBannersClient.getAllBlockBanners().subscribe({
            next: (res) => {
                debugger;
                this.leftBlock = res.find(
                    (c) => c.blockBannerPosition == BlockBannerPosition.LeftSide
                );
                this.rightBlock = res.find(
                    (c) =>
                        c.blockBannerPosition == BlockBannerPosition.RightSide
                );
                if (this.leftBlock !== undefined) {
                    this.leftBlock.image = this.imageService.makeImageUrl(this.leftBlock.image);
                }
                if (this.rightBlock !== undefined) {
                    this.rightBlock.image = this.imageService.makeImageUrl(this.rightBlock.image);
                }
            },
        });
    }
}
