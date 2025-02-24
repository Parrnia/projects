import { AbstractControl, AsyncValidatorFn, ValidationErrors } from '@angular/forms';
import { Observable, map, of, switchMap } from 'rxjs';
import { BlockBannersClient } from 'src/app/web-api-client';

export class BlockBannerValidators {
    /**
     *
     */
    constructor() { }
    static validBlockBanner(blockBannersClient: BlockBannersClient, blockBannerId : number): AsyncValidatorFn {
        return (control: AbstractControl): Observable<ValidationErrors | null> => {
            debugger;
            const blockBanner = control.value;
            return blockBannersClient.isUniqueBlockBannerPosition(blockBanner,blockBannerId).pipe(
                map(isAvailable => (isAvailable ? null : { blockBannerTaken: true }))
            );
        }
    }
}
