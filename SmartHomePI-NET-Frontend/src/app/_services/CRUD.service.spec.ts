/* tslint:disable:no-unused-variable */

import { TestBed, async, inject } from '@angular/core/testing';
import { CrudService } from './CRUD.service';

describe('Service: Crud', () => {
  beforeEach(() => {
    TestBed.configureTestingModule({
      providers: [CrudService]
    });
  });

  it('should ...', inject([CrudService], (service: CrudService) => {
    expect(service).toBeTruthy();
  }));
});
