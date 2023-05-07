import { ComponentFixture, TestBed } from '@angular/core/testing';

import { EquipmentCatalogComponent } from './equipment-catalog.component';

describe('EquipmentCatalogComponent', () => {
  let component: EquipmentCatalogComponent;
  let fixture: ComponentFixture<EquipmentCatalogComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ EquipmentCatalogComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(EquipmentCatalogComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
