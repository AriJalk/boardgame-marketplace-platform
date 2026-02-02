import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Inject, Injectable } from '@angular/core';
import { API_CONFIG_SERVICE } from '../../../core/api-config/api-config.service';
import { ApiConfig } from '../../../core/api-config/api-config.interface';
import { ListingGroupDetails, ListingGroupSummary } from '../models/listing-group.model';
import { Observable } from 'rxjs';
import { PagedResult } from '../../../common/paged-result';
import { AuthStubService } from '../../../core/auth/auth-stub.service';
import { PagedListingGroupQuery } from '../models/listing-group-queries';

@Injectable({
  providedIn: 'root',
})
export class ListingGroupService {
  constructor(
    private httpClient: HttpClient,
    @Inject(API_CONFIG_SERVICE) private apiConfig: ApiConfig,
    private authService: AuthStubService,
  ) {}
  getListingGroupDetails(listingGroupId: string) {
    return this.httpClient.get<ListingGroupDetails>(
      `${this.apiConfig.apiEndpoint}/listingGroups/${listingGroupId}/detail`,
    );
  }

  getListingGroupSummariesPage(): Observable<PagedResult<ListingGroupSummary>> {
    const query: PagedListingGroupQuery = {
      pageSize: 10,
    };
    return this.httpClient.post<PagedResult<ListingGroupSummary>>(
      `${this.apiConfig.apiEndpoint}/listingGroups/page`,
      query,
    );
  }

  getSubscribedListingGroups(): Observable<ListingGroupSummary[]> {
    const headers = new HttpHeaders({ Authorization: this.authService.loggedInUser?.userId ?? '' });
    return this.httpClient.get<ListingGroupSummary[]>(
      `${this.apiConfig.apiEndpoint}/subscriptions/listingGroups`,
      { headers: headers },
    );
  }
}
