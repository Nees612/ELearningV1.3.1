import { TestBed } from '@angular/core/testing';

import { ModulesService } from './modules.service';

describe('ModuleService', () => {
  beforeEach(() => TestBed.configureTestingModule({}));

  it('should be created', () => {
    const service: ModulesService = TestBed.get(ModulesService);
    expect(service).toBeTruthy();
  });
});
