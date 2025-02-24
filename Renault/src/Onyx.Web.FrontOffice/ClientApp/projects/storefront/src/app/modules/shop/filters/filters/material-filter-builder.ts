
import { Observable, of } from 'rxjs';
import { AbstractFilterBuilder } from './abstract-filter-builder';
import { ProductOptionMaterialsClient, ProductOptionValueMaterialDto } from 'projects/storefront/src/app/web-api-client';
import { BaseFilterItem, MaterialFilter } from 'projects/storefront/src/app/interfaces/filter';


export class MaterialFilterBuilder extends AbstractFilterBuilder {

    public override value: string[] = [];
    private materials : ProductOptionValueMaterialDto[] = [];

    constructor(
        slug: string,
        name: string,
        private productOptionMaterialsClient : ProductOptionMaterialsClient
    ) {
        super(slug,name);
    }
    
    
    
    makeItems(value: string, isLoaded : boolean): Observable<void> {
            this.productOptionMaterialsClient.getAllProductOptionValueMaterials().subscribe({
                next : (res) => {
         
                  
                    this.materials = res;
                    this.items = this.extractItems();
                },
                error : (err) => console.log(err)
            });
        
        this.value = this.parseValue(value);
        return of(undefined);
    }


    build(): MaterialFilter {
        return {
            type: 'material',
            slug: this.slug,
            name: this.name,
            items: this.items,
            value: this.value,
        };
    }

    // noinspection JSMethodCanBeStatic
    private parseValue(value: string): string[] {
        return value ? value.split(',') : [];
    }

    private extractItems(): BaseFilterItem[] {
      
        return this.materials.map(value => ({
            slug: value.slug ?? '',
            name: value.name ?? '',
            count: 0,
        }));
    }

}
