import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ReportDeathComponent } from './report-death.component';

describe('ReportDeathComponent', () => {
  let component: ReportDeathComponent;
  let fixture: ComponentFixture<ReportDeathComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ ReportDeathComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(ReportDeathComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
