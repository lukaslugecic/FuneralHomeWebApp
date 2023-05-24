import { ComponentFixture, TestBed } from '@angular/core/testing';

import { TypeDialogComponent } from './type-dialog.component';

describe('TypeDialogComponent', () => {
  let component: TypeDialogComponent;
  let fixture: ComponentFixture<TypeDialogComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ TypeDialogComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(TypeDialogComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
