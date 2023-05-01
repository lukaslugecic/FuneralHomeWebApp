import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ServiceDialogComponent } from './service-dialog.component';

describe('ServiceDialogComponent', () => {
  let component: ServiceDialogComponent;
  let fixture: ComponentFixture<ServiceDialogComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ ServiceDialogComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(ServiceDialogComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
