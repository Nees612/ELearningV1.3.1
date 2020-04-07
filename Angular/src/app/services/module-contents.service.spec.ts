import { TestBed } from '@angular/core/testing';

import { ModuleContentsService } from './module-contents.service';

describe('ModuleContentsService', () => {
  beforeEach(() => TestBed.configureTestingModule({}));

  it('should be created', () => {
    const service: ModuleContentsService = TestBed.get(ModuleContentsService);
    expect(service).toBeTruthy();
  });
});
