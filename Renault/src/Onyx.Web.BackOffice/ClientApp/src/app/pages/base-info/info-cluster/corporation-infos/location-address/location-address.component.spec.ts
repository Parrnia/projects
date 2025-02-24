import { ComponentFixture, TestBed } from '@angular/core/testing';

import { LocationAddressComponent } from './location-address.component';

describe('LocationAddressesComponent', () => {
  let component: LocationAddressComponent;
  let fixture: ComponentFixture<LocationAddressComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ LocationAddressComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(LocationAddressComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
