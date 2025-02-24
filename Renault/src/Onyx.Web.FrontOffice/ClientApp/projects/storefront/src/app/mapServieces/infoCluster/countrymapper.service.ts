import { Injectable } from '@angular/core';
import { Country } from '../../interfaces/country';
import { AllCountryDto, CountryDto, CountryWithPaginationDto } from '../../web-api-client';

@Injectable({
  providedIn: 'root'
})
export class CountrymapperService {

  constructor() { }

  //#region AllCountries
  mapAllCountry(countryDto: AllCountryDto) {
    let country = new Country();
    country.id = countryDto.id!;
    country.name = countryDto.localizedName!;
    return country;
  }

  mapAllCountries(countryDtos: AllCountryDto[]) {
    let countries: Country[] = [];
    countryDtos.forEach(c => {
      countries.push(this.mapAllCountry(c));
    })
    return countries;
  }
  //#endregion

  //#region CountriesWithPagination
  mapCountryWithPagination(countryDto: CountryWithPaginationDto) {
    let country = new Country();
    country.id = countryDto.id!;
    country.name = countryDto.localizedName!;
    return country;
  }

  mapCountriesWithPagination(countryDtos: CountryWithPaginationDto[]) {
    let countries: Country[] = [];
    countryDtos.forEach(c => {
      countries.push(this.mapCountryWithPagination(c));
    })
    return countries;
  }
  //#endregion

  //#region Country
  mapCountry(countryDto: CountryDto) {
    let country = new Country();
    country.id = countryDto.id!;
    country.name = countryDto.localizedName!;
    return country;
  }
  //#endregion
}
