import { TestBed } from '@angular/core/testing';

import { AnonGuard } from './anon.guard';

describe('AnonGuard', () => {
  let guard: AnonGuard;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    guard = TestBed.inject(AnonGuard);
  });

  it('should be created', () => {
    expect(guard).toBeTruthy();
  });
});
