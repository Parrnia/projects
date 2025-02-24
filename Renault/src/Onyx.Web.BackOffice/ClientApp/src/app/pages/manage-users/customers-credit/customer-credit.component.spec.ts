import { ComponentFixture, TestBed } from '@angular/core/testing';

import { CustomerCreditComponent } from './customer-credit.component';

describe('CustomerCreditComponent', () => {
  let component: CustomerCreditComponent;
  let fixture: ComponentFixture<CustomerCreditComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ CustomerCreditComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(CustomerCreditComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
