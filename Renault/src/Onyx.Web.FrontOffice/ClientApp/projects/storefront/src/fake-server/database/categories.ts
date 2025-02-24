import { CategoryDef } from '../interfaces/category-def';
import { BaseCategory, BlogCategory, Category, ShopCategory } from '../../app/interfaces/category';

let lastId = 0;

function makeShopCategory(def: CategoryDef, parent: ShopCategory|null): ShopCategory {
    return {
        id: ++lastId,
        type: 'shop',
        name: def.name,
        slug: def.slug,
        image: def.image || null,
        items: def.items,
        parent: parent || undefined,
        children: [],
        layout: def.layout ? def.layout : 'products',
        customFields: {},
    };
}

function makeBlogCategory(def: CategoryDef, parent: BlogCategory|null): BlogCategory {
    return {
        id: ++lastId,
        type: 'blog',
        name: def.name,
        slug: def.slug,
        image: def.image || null,
        items: def.items,
        parent: parent || undefined,
        children: [],
        customFields: {},
    };
}

function makeCategories<T extends BaseCategory>(
    makeFn: (def: CategoryDef, parent: T|null) => T,
    defs: CategoryDef[],
    parent: T|null = null,
): T[] {
    const categories: T[] = [];

    defs.forEach(def => {
        const category: T = makeFn(def, parent);

        if (def.children) {
            category.children = makeCategories(makeFn, def.children, category);
        }

        categories.push(category);
    });

    return categories;
}

function flatTree<T extends Category>(categories: T[]): T[] {
    let result: T[] = [];

    categories.forEach(category => result = [...result, category, ...flatTree(category.children as T[])]);

    return result;
}

const shopCategoriesDef: CategoryDef[] = [
    {
        name: 'چراغ های جلو و روشنایی',
        slug: 'headlights-lighting',
        image: 'assets/images/categories/category-1.jpg',
        items: 131,
        children: [
            { name: 'چراغ های راهنما', slug: 'turn-signals' },
            { name: 'چراغهای مه شکن', slug: 'fog-lights' },
            { name: 'چراغ های جلو', slug: 'headlights' },
            { name: 'سوئیچ ها و رله ها', slug: 'switches-relays' },
            { name: 'چراغ های عقب', slug: 'tail-lights' },
            { name: 'چراغ های گوشه', slug: 'corner-lights' },
            { name: 'روشنایی خارج از جاده', slug: 'off-road-lighting' },
            { name: 'لوازم جانبی روشنایی', slug: 'lighting-accessories' },
        ],
    },
    {
        name: 'سیستم سوخت',
        slug: 'fuel-system',
        image: 'assets/images/categories/category-2.jpg',
        items: 356,
        children: [
            { name: 'پمپ های سوخت', slug: 'fuel-pumps' },
            { name: 'روغن موتور', slug: 'motor-oil' },
            { name: 'درپوش گاز', slug: 'gas-caps' },
            { name: 'تزریق کننده ی سوخت', slug: 'fuel-injector' },
            { name: 'موتور کنترل', slug: 'control-motor' },
        ],
    },
    {
        name: 'اجزای بدنه',
        slug: 'body-parts',
        image: 'assets/images/categories/category-3.jpg',
        items: 54,
        children: [
            { name: 'ضربه گیر', slug: 'bumpers' },
            { name: 'هود', slug: 'hoods' },
            { name: 'توری', slug: 'grilles' },
            { name: 'چراغهای مه شکن', slug: 'fog-lights' },
            { name: 'دستگیره در', slug: 'door-handles' },
        ],
    },
    {
        name: 'قطعات داخلی',
        slug: 'interior-parts',
        image: 'assets/images/categories/category-4.jpg',
        items: 274,
        children: [
            { name: 'داشبوردها', slug: 'dashboards' },
            { name: 'روکش صندلی', slug: 'seat-covers' },
            { name: 'تشک کف', slug: 'floor-mats' },
            { name: 'سایه های آفتاب', slug: 'sun-shades' },
            { name: 'ویسورها', slug: 'visors' },
            { name: 'روکش ماشین', slug: 'car-covers' },
            { name: 'تجهیزات جانبی', slug: 'interior-parts-accessories' },
        ],
    },
    {
        name: 'لاستیک و چرخ',
        slug: 'tires-wheels',
        image: 'assets/images/categories/category-5.jpg',
        items: 508,
        children: [
            { name: 'روکش چرخ', slug: 'wheel-covers' },
            { name: 'کیت ترمز', slug: 'brake-kits' },
            { name: 'زنجیر چرخ', slug: 'tire-chains' },
            { name: 'دیسک های چرخ', slug: 'wheel-disks' },
            { name: 'لاستیک', slug: 'tires' },
            { name: 'حسگرها', slug: 'sensors' },
            { name: 'تجهیزات جانبی', slug: 'tires-wheels-accessories' },
        ],
    },
    {
        name: 'موتور و پیشرانه',
        slug: 'engine-drivetrain',
        image: 'assets/images/categories/category-6.jpg',
        items: 95,
        children: [
            { name: 'تسمه تایم', slug: 'timing-belts' },
            { name: 'شمع ها', slug: 'spark-plugs' },
            { name: 'تابه های روغن', slug: 'oil-pans' },
            { name: 'واشر موتور', slug: 'engine-gaskets' },
            { name: 'فیلترهای روغن', slug: 'oil-filters' },
            { name: 'پایه های موتور', slug: 'engine-mounts' },
            { name: 'تجهیزات جانبی', slug: 'engine-drivetrain-accessories' },
        ],
    },
    {
        name: 'روغن ها و روان کننده ها',
        slug: 'oils-lubricants',
        image: 'assets/images/categories/category-7.jpg',
        items: 179,
    },
    {
        name: 'ابزار و گاراژ',
        slug: 'tools-garage',
        image: 'assets/images/categories/category-8.jpg',
        items: 106,
    },
];

const blogCategoriesDef: CategoryDef[] = [
    {
        name: 'آخرین خبرها',
        slug: 'latest-news',
    },
    {
        name: 'پیشنهادات ویژه',
        slug: 'special-offers',
        children: [
            {
                name: 'فروش بهاره',
                slug: 'spring-sales',
            },
            {
                name: 'فروش تابستانی',
                slug: 'summer-sales',
            },
            {
                name: 'فروش پاییزه',
                slug: 'autumn-sales',
            },
            {
                name: 'فروش عیدانه',
                slug: 'christmas-sales',
            },
            {
                name: 'سایر فروش ها',
                slug: 'other-sales',
            },
        ],
    },
    {
        name: 'تازه ها',
        slug: 'new-arrivals',
    },
    {
        name: 'بررسی',
        slug: 'reviews',
    },
    {
        name: 'چرخ و لاستیک',
        slug: 'wheels-tires',
    },
    {
        name: 'موتور و پیشرانه',
        slug: 'engine-drivetrain',
    },
    {
        name: 'سیستم انتقال',
        slug: 'transmission',
    },
    {
        name: 'کارایی',
        slug: 'performance',
    },
];

export const shopCategoriesTree: ShopCategory[] = makeCategories(makeShopCategory, shopCategoriesDef);

export const shopCategoriesList: ShopCategory[] = flatTree(shopCategoriesTree);

export const blogCategoriesTree: BlogCategory[] = makeCategories(makeBlogCategory, blogCategoriesDef);

export const blogCategoriesList: BlogCategory[] = flatTree(blogCategoriesTree);
