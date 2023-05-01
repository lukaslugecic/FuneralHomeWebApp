import { ComponentFixture, TestBed } from '@angular/core/testing';

import { AllEquipmentComponent } from './all-equipment.component';

describe('AllEquipmentComponent', () => {
  let component: AllEquipmentComponent;
  let fixture: ComponentFixture<AllEquipmentComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ AllEquipmentComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(AllEquipmentComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
