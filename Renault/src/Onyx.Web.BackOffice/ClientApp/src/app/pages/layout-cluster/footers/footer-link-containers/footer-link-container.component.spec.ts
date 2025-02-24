import { ComponentFixture, TestBed } from '@angular/core/testing';

import { FooterLinkContainerComponent } from './footer-link-container.component';

describe('FooterLinkContainerComponent', () => {
  let component: FooterLinkContainerComponent;
  let fixture: ComponentFixture<FooterLinkContainerComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ FooterLinkContainerComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(FooterLinkContainerComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
