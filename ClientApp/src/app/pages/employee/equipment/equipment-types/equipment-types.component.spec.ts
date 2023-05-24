import { ComponentFixture, TestBed } from '@angular/core/testing';

import { EquipmentTypesComponent } from './equipment-types.component';

describe('EquipmentTypesComponent', () => {
  let component: EquipmentTypesComponent;
  let fixture: ComponentFixture<EquipmentTypesComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ EquipmentTypesComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(EquipmentTypesComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
