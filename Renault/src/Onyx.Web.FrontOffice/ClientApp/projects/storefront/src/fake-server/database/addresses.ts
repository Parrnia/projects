import { Observable, of, switchMap } from 'rxjs';
import { Address } from '../../app/interfaces/address';
import { Customer, User } from '../../app/interfaces/user';
import { Vehicle } from '../../app/interfaces/vehicle';

let lastId = 0;

export function getId(): number {
    return ++lastId;
}

export const addresses: Address[] = [
    {
        id: getId(),
        title: '',
        company: 'Stroyka',
        country: 'RAND',
        address1: 'ul. Varshavskaya, 15-2-178',
        address2: '',
        city: 'Moscow',
        state: 'Moscow',
        postcode: '115302',
        default: true,
        countryId:1
    },
    {
        id: getId(),
        title: '',
        company: 'Stroyka',
        country: 'LAND',
        address1: 'Sun Orbit, 43.3241-85.239',
        address2: '',
        city: 'MarsGrad',
        state: 'MarsGrad',
        postcode: '4b4f53',
        default: false,
        countryId:1
    },
];
let vehicle = new Vehicle();
vehicle.year = "نیسان";
vehicle.make = "جوک";
vehicle.model = "سفید";
vehicle.engine = "V6";



export const currentVehicle : Vehicle = vehicle;
