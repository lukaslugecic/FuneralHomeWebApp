import { ComponentFixture, TestBed } from '@angular/core/testing';

import { OrganizeFuneralComponent } from './organize-funeral.component';

describe('OrganizeFuneralComponent', () => {
  let component: OrganizeFuneralComponent;
  let fixture: ComponentFixture<OrganizeFuneralComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ OrganizeFuneralComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(OrganizeFuneralComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
