import { ComponentFixture, TestBed } from '@angular/core/testing';

import { FuneralDetailComponent } from './funeral-detail.component';

describe('FuneralDetailComponent', () => {
  let component: FuneralDetailComponent;
  let fixture: ComponentFixture<FuneralDetailComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ FuneralDetailComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(FuneralDetailComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
