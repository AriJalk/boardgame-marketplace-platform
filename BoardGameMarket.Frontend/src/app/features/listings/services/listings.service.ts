import { HttpClient } from '@angular/common/http';
import { Inject, Injectable } from '@angular/core';
import { ApiConfig } from '../../../core/api-config/api-config.interface';
import { API_CONFIG_SERVICE } from '../../../core/api-config/api-config.service';
import { Observable } from 'rxjs';
import { ListingDetail } from '../models/listing.model';

@Injectable({
  providedIn: 'root',
})
export class ListingsService {
  constructor(
    private httpClient: HttpClient,
    @Inject(API_CONFIG_SERVICE) private apiConfig: ApiConfig,
  ) {}

  getListing(listingId: string): Observable<ListingDetail> {
    return this.httpClient.get<ListingDetail>(
      `${this.apiConfig.apiEndpoint}/listings/${listingId}`,
    );
  }
}
