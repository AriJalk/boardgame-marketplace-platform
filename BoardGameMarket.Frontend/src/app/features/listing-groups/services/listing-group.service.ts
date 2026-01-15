import { HttpClient } from '@angular/common/http';
import { Inject, Injectable } from '@angular/core';
import { API_CONFIG_SERVICE } from '../../../core/api-config/api-config.service';
import { ApiConfig } from '../../../core/api-config/api-config.interface';
import { ListingGroupDetails, ListingGroupSummary } from '../models/listing-group.model';
import { Observable } from 'rxjs';
import { PagedResult } from '../../../common/paged-result';

@Injectable({
  providedIn: 'root',
})
export class ListingGroupService {
  constructor(
    private httpClient: HttpClient,
    @Inject(API_CONFIG_SERVICE) private apiConfig: ApiConfig,
  ) {}
  getListingGroupDetails(listingGroupId: string) {
    return this.httpClient.get<ListingGroupDetails>(
      `${this.apiConfig.apiEndpoint}/listingGroups/${listingGroupId}/detail`,
    );
  }

  getListingGroupSummariesPage(): Observable<PagedResult<ListingGroupSummary>> {
    return this.httpClient.get<PagedResult<ListingGroupSummary>>(`${this.apiConfig.apiEndpoint}/listingGroups?pageSize=50`)
  }
}
