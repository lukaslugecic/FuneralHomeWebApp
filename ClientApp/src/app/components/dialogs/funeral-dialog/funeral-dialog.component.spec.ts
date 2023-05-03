import { ComponentFixture, TestBed } from '@angular/core/testing';

import { FuneralDialogComponent } from './funeral-dialog.component';

describe('FuneralDialogComponent', () => {
  let component: FuneralDialogComponent;
  let fixture: ComponentFixture<FuneralDialogComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ FuneralDialogComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(FuneralDialogComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
