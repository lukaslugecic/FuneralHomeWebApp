import { ComponentFixture, TestBed } from '@angular/core/testing';

import { FuneralCustomerFormComponent } from './funeral-customer-form.component';

describe('FuneralCustomerFormComponent', () => {
  let component: FuneralCustomerFormComponent;
  let fixture: ComponentFixture<FuneralCustomerFormComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ FuneralCustomerFormComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(FuneralCustomerFormComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
