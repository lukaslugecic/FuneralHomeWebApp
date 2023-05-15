import { ComponentFixture, TestBed } from '@angular/core/testing';

import { AddInsuranceDialogComponent } from './add-insurance-dialog.component';

describe('AddInsuranceDialogComponent', () => {
  let component: AddInsuranceDialogComponent;
  let fixture: ComponentFixture<AddInsuranceDialogComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ AddInsuranceDialogComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(AddInsuranceDialogComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
