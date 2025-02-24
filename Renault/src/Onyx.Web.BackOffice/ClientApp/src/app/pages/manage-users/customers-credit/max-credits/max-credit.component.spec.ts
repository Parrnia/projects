import { ComponentFixture, TestBed } from '@angular/core/testing';
import { MaxCreditComponent } from './max-credit.component';


describe('MaxCreditComponent', () => {
  let component: MaxCreditComponent;
  let fixture: ComponentFixture<MaxCreditComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ MaxCreditComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(MaxCreditComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
