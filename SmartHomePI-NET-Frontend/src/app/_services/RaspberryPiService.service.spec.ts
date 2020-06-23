/* tslint:disable:no-unused-variable */

import { TestBed, async, inject } from '@angular/core/testing';
import { RaspberryPiServiceService } from './RaspberryPiService.service';

describe('Service: RaspberryPiService', () => {
  beforeEach(() => {
    TestBed.configureTestingModule({
      providers: [RaspberryPiServiceService]
    });
  });

  it('should ...', inject([RaspberryPiServiceService], (service: RaspberryPiServiceService) => {
    expect(service).toBeTruthy();
  }));
});
