/* tslint:disable:no-unused-variable */

import { TestBed, async, inject } from '@angular/core/testing';
import { RoomService } from './CRUD.service';

describe('Service: Room', () => {
  beforeEach(() => {
    TestBed.configureTestingModule({
      providers: [RoomService]
    });
  });

  it('should ...', inject([RoomService], (service: RoomService) => {
    expect(service).toBeTruthy();
  }));
});
