import { ComponentFixture, TestBed } from '@angular/core/testing';

import { PendingForRegisterOrderComponent } from "./pending-for-register-order.component";

describe("PendingForRegisterOrderComponent", () => {
  let component: PendingForRegisterOrderComponent;
  let fixture: ComponentFixture<PendingForRegisterOrderComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [PendingForRegisterOrderComponent],
    }).compileComponents();

    fixture = TestBed.createComponent(PendingForRegisterOrderComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it("should create", () => {
    expect(component).toBeTruthy();
  });
});
