import { ComponentFixture, TestBed } from '@angular/core/testing';

import { DeathCustomerFormComponent } from './death-customer-form.component';

describe('DeathCustomerFormComponent', () => {
  let component: DeathCustomerFormComponent;
  let fixture: ComponentFixture<DeathCustomerFormComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ DeathCustomerFormComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(DeathCustomerFormComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
