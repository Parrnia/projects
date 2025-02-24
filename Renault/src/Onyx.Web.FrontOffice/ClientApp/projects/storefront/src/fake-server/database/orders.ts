import { OrderDef } from '../interfaces/order-def';
import { Order, OrderTotal } from '../../app/interfaces/order';
import { products } from './products';
import { addresses } from './addresses';
import { PaymentType } from '../../app/web-api-client';
import { Address } from '../../app/interfaces/address';

let lastId = 0;

export function getNextOrderId(): number {
    return ++lastId;
}

export function getOrderToken(orderId: number): string {
    const token = 'b84486c31644eac99f6909a6e8c19109';

    return token.slice(0, token.length - orderId.toString().length) + orderId.toString();
}

export function getNextOrderNumber(): string {
    return (orders.reduce((prev, curr) => Math.max(prev, parseFloat(curr.number)), 0) + 1).toString();
}

function makeOrders(defs: OrderDef[]): Order[] {
    return [];
}

const ordersDef: OrderDef[] = [
    {
        number: '9478',
        createdAt: '2020-10-19',
        payment: PaymentType.Cash,
        status: 'PENDING',
        items: [
            {
                product: 'brandix-spark-plug-kit-asr-400',
                options: [
                    { name: 'Color', value: 'True Red' },
                    { name: 'Material', value: 'Aluminium' },
                ],
                quantity: 2,
                optionValues:[]
            },
            {
                product: 'brandix-brake-kit-bdx-750z370-s',
                options: [],
                quantity: 1,
                optionValues:[]
            },
            {
                product: 'left-headlight-of-brandix-z54',
                options: [
                    { name: 'Color', value: 'Green' },
                ],
                quantity: 3,
                optionValues:[]
            },
        ],
    },
    {
        number: '7592',
        createdAt: '2019-03-28',
        payment: PaymentType.Cash,
        status: 'PENDING',
        items: [
            {
                product: 'brandix-drivers-seat',
                options: [
                    { name: 'Color', value: 'True Red' },
                    { name: 'Material', value: 'Aluminium' },
                ],
                quantity: 2,
                optionValues:[]
            },
            {
                product: 'set-of-four-19-inch-spiked-tires',
                options: [],
                quantity: 1,
                optionValues:[]
            },
        ],
    },
    {
        number: '7192',
        createdAt: '2019-03-15',
        payment: PaymentType.Cash,
        status: 'SHIPPED',
        items: [
            {
                product: 'wiper-blades-brandix-wl2',
                options: [],
                quantity: 5,
                optionValues:[]
            },
            {
                product: 'brandix-engine-block-z4',
                options: [],
                quantity: 1,
                optionValues:[]
            },
        ],
    },
    {
        number: '6321',
        createdAt: '2019-02-28',
        payment: PaymentType.Cash,
        status: 'COMPLETED',
        items: [
            {
                product: 'left-headlight-of-brandix-z54',
                options: [],
                quantity: 1,
                optionValues:[]
            },
        ],
    },
    {
        number: '6001',
        createdAt: '2019-02-21',
        payment: PaymentType.Cash,
        status: 'COMPLETED',
        items: [
            {
                product: 'taillights-brandix-z54',
                options: [],
                quantity: 7,
                optionValues:[]
            },
            {
                product: 'fantastic-12-stroke-engine-with-a-power-of-1991-hp',
                options: [],
                quantity: 1,
                optionValues:[]
            },
        ],
    },
    {
        number: '4120',
        createdAt: '2018-12-11',
        payment: PaymentType.Cash,
        status: 'COMPLETED',
        items: [
            {
                product: 'air-filter-from-ashs-chainsaw',
                options: [],
                quantity: 1,
                optionValues:[]
            },
        ],
    },
];

export const orders: Order[] = makeOrders(ordersDef);
