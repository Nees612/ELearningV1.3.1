import { TestBed } from '@angular/core/testing';

import { AssigmentsService } from './assigments.service';

describe('AssigmentsService', () => {
  beforeEach(() => TestBed.configureTestingModule({}));

  it('should be created', () => {
    const service: AssigmentsService = TestBed.get(AssigmentsService);
    expect(service).toBeTruthy();
  });
});
