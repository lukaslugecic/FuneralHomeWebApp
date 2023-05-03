import { ComponentFixture, TestBed } from '@angular/core/testing';

import { DeathDialogComponent } from './death-dialog.component';

describe('DeathDialogComponent', () => {
  let component: DeathDialogComponent;
  let fixture: ComponentFixture<DeathDialogComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ DeathDialogComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(DeathDialogComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
