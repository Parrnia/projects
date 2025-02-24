import { Address } from './../../app/interfaces/address';
import { Observable, of } from 'rxjs';
import { Order, OrderItem, OrderTotal } from '../../app/interfaces/order';
import { CheckoutData } from '../../app/api/base';
import { delayResponse } from '../utils';
import { getNextOrderId, getNextOrderNumber, getOrderToken, orders } from '../database/orders';
import { Product } from '../../app/interfaces/product';
import { products } from '../database/products';
import { OrderPaymentType } from '../../app/web-api-client';

export function checkout(data: CheckoutData): Observable<Order> {
    const id = getNextOrderId();
    const items: OrderItem[] = data.items.map(x => {
        const product: Product|undefined = products.find(p => p.id === x.productId);

        if (!product) {
            throw new Error('Product not found');
        }

        return {
            product,
            options: x.options,
            price: product.selectedAttributeOption.price,
            quantity: x.quantity,
            selectedProductAttributeOption: x.selectedProductAttributeOption,
            total: product.selectedAttributeOption.price * x.quantity,
            optionValues: [],
            discountPercentForProduct: 0
        };
    });
    const quantity = items.reduce((acc, x) => acc + x.quantity, 0);
    const subtotal = items.reduce((acc, x) => acc + x.total, 0);
    const totals: OrderTotal[] = [
        {
            title: 'SHIPPING',
            price: 25,
            type: 'discountOnCustomerType'
        },
        {
            title: 'TAX',
            price: subtotal * 0.20,
            type: 'discountOnCustomerType'
        },
    ];
    const total = subtotal + totals.reduce((acc, x) => acc + x.price, 0);

    const date = new Date();
    const pad = (v: number) => ('00' + v).substr(-2);
    const createdAt = `${date.getFullYear()}-${pad(date.getMonth() + 1)}-${pad(date.getDate())}`;

    const order: Order = {
        id,
        token: getOrderToken(id),
        number: getNextOrderNumber(),
        createdAt,
        payment: OrderPaymentType.Unspecified,
        status: 'PENDING',
        items,
        quantity,
        subtotal,
        totals,
        total,
        orderAddress: new Address(),//data.billingAddress,
        statusDetails: '',
        CurrentOrderStatus: 0,
        paymentHistory: []
    };

    orders.unshift(order);

    return delayResponse(of(order));
}
