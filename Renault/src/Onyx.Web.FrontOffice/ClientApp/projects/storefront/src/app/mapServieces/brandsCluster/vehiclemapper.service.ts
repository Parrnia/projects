import { Injectable } from '@angular/core';
import { Vehicle } from '../../interfaces/vehicle';
import { VehicleByCustomerIdDto, VehicleByVinNumberDto, VehicleDto } from '../../web-api-client';

@Injectable({
  providedIn: 'root'
})
export class VehiclemapperService {

  constructor() { }


  //#region VehicleByUserId
  mapVehicleByUserId(vehicleByUserIdDto: VehicleByCustomerIdDto | null) {
    let vehicle = new Vehicle();
    vehicle.id = vehicleByUserIdDto?.id ?? 0;
    vehicle.year = vehicleByUserIdDto?.kind?.brandLocalizedName ?? '';
    vehicle.make = vehicleByUserIdDto?.kind?.familyLocalizedName ?? '';
    vehicle.model = vehicleByUserIdDto?.kind?.modelLocalizedName ?? '';
    vehicle.engine = vehicleByUserIdDto?.kind?.localizedName ?? '';
    vehicle.vinNumber = vehicleByUserIdDto?.vinNumber ?? '';
    vehicle.kindId = vehicleByUserIdDto?.kind?.id ?? -1;
    return vehicle;
  }

  mapVehiclesByUserId(vehicleByUserIdDtos: VehicleByCustomerIdDto[]) {
    let vehicles: Vehicle[] = [];
    vehicleByUserIdDtos.forEach(c => {
      vehicles.push(this.mapVehicleByUserId(c));
    })
    return vehicles;
  }
  //#endregion

  //#region VehicleByVinNumber
  mapVehicleByVinNumber(vehicleByVinNumberDto: VehicleByVinNumberDto | null) {
    let vehicle = new Vehicle();
    vehicle.id = vehicleByVinNumberDto?.id ?? 0;
    vehicle.year = vehicleByVinNumberDto?.kind?.brandLocalizedName ?? '';
    vehicle.make = vehicleByVinNumberDto?.kind?.familyLocalizedName ?? '';
    vehicle.model = vehicleByVinNumberDto?.kind?.modelLocalizedName ?? '';
    vehicle.engine = vehicleByVinNumberDto?.kind?.localizedName ?? '';
    vehicle.vinNumber = vehicleByVinNumberDto?.vinNumber ?? '';
    vehicle.kindId = vehicleByVinNumberDto?.kind?.id ?? -1;
    return vehicle;
  }

  mapVehicleByVinNumbers(vehicleByVinNumberDtos: VehicleByVinNumberDto[]) {
    let vehicles: Vehicle[] = [];
    vehicleByVinNumberDtos.forEach(c => {
      vehicles.push(this.mapVehicleByVinNumber(c));
    })
    return vehicles;
  }
  //#endregion

}
