import { Observable, of } from 'rxjs';
import { Country } from '../../app/interfaces/country';
import { clone } from '../utils';

const countries: Country[] = [
    { id: 1, code: 'RAND', name: 'Random Federation' },
    { id: 2, code: 'LAND', name: 'RandomLand' },

    { id: 3, code: 'AU', name: 'Australia' },
    { id: 4, code: 'DE', name: 'Germany' },
    { id: 5, code: 'FR', name: 'France' },
    { id: 6, code: 'IT', name: 'Italy' },
    { id: 7, code: 'RU', name: 'Russia' },
    { id: 8, code: 'UA', name: 'Ukraine' },
    { id: 9, code: 'US', name: 'United States' },
];

export function getCountries(): Observable<Country[]> {
    return of(clone(countries));
}
