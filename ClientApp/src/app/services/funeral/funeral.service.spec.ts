import { TestBed } from '@angular/core/testing';

import { FuneralService } from './funeral.service';

describe('FuneralService', () => {
  let service: FuneralService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(FuneralService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
