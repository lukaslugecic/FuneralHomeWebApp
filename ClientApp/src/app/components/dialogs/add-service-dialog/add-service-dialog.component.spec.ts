import { ComponentFixture, TestBed } from '@angular/core/testing';

import { AddServiceDialogComponent } from './add-service-dialog.component';

describe('AddServiceDialogComponent', () => {
  let component: AddServiceDialogComponent;
  let fixture: ComponentFixture<AddServiceDialogComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ AddServiceDialogComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(AddServiceDialogComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
