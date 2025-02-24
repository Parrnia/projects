import { Injectable } from '@angular/core';
import { AddressDto, CreateAddressCommand, UpdateAddressCommand } from '../../web-api-client';
import { Address } from '../../interfaces/address';
import { AuthService } from '../../services/authService/auth.service';
@Injectable({
  providedIn: 'root'
})
export class AddressMapperService {

  constructor(private authService : AuthService  ) { }

  //#region FrontToDatabase

  //#region Create
  mapAddressCommand(address: Address) {
    let addressCommand = new CreateAddressCommand();
    addressCommand.title = address.title!;
    addressCommand.company = address.company!;
    addressCommand.countryId = parseInt(address.country);
    addressCommand.addressDetails1 = address.address1;
    addressCommand.addressDetails2 = address.address2;
    addressCommand.city = address.city!;
    addressCommand.state = address.state!;
    addressCommand.postcode = address.postcode!;
    addressCommand.default = address.default;
    addressCommand.customerId = localStorage.getItem('userId') ?? "";
    return addressCommand;
  }

  mapAddressCommands(brandDtos: Address[]) {
    let addressCommands: CreateAddressCommand[] = [];
    brandDtos.forEach(c => {
      addressCommands.push(this.mapAddressCommand(c));
    })
    return addressCommands;
  }
  //#endregion
  //#region Update
  mapUpdateAddressCommands(addresses: Address[]) {
    let addressCommands: UpdateAddressCommand[] = [];
    addresses.forEach(c => {
      addressCommands.push(this.mapAddressCommand(c));
    })
    return addresses;
  }
  mapUpdateAddressCommand(address: Address) {
    let addressCommand = new UpdateAddressCommand();
    addressCommand.id = address.id!;
    addressCommand.default = address.default;
    addressCommand.title = address.title!;
    addressCommand.company = address.company!;
    addressCommand.countryId = parseInt(address.country);
    addressCommand.addressDetails1 = address.address1!;
    addressCommand.addressDetails2 = address.address2!;
    addressCommand.city = address.city!;
    addressCommand.state = address.state!;
    addressCommand.postcode = address.postcode!;
    addressCommand.customerId = localStorage.getItem('userId') ?? "";
    return addressCommand;
  }
  //#endregion
  //#endregion

  //#region DatabaseToFront
  mapAddresses(addressDtos: AddressDto[]) {
    let addresses: Address[] = [];
    addressDtos.forEach(c => {
      addresses.push(this.mapAddress(c));
    })
    return addresses;
  }
  mapAddress(addressDto: AddressDto) {
    let address = new Address();
    address.id = addressDto?.id ?? 0;
    address.default = addressDto.default!;
    address.title = addressDto.title!;
    address.company = addressDto.company!;
    address.country = addressDto.countryName!;
    address.countryId = addressDto.countryId!;
    address.address1 = addressDto.addressDetails1!;
    address.address2 = addressDto.addressDetails2!;
    address.city = addressDto.city!;
    address.state = addressDto.state!;
    address.postcode = addressDto.postcode!;
    return address;
  }
  //#endregion
  
}
