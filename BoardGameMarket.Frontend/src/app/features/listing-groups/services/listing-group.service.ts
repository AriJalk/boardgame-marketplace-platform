import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Inject, Injectable } from '@angular/core';
import { API_CONFIG_SERVICE } from '../../../core/api-config/api-config.service';
import { ApiConfig } from '../../../core/api-config/api-config.interface';
import {
  CreateListingGroupRequest,
  ListingGroupDetails,
  ListingGroupSummary,
  UpdateListingGroupRequest,
} from '../models/listing-group.model';
import { Observable } from 'rxjs';
import { PagedResult } from '../../../common/paged-result';
import { ListingGroupFilter, PagedListingGroupQuery } from '../models/listing-group-queries';
import { AuthService } from '../../../core/auth/auth.service';

@Injectable({
  providedIn: 'root',
})
export class ListingGroupService {
  constructor(
    private httpClient: HttpClient,
    @Inject(API_CONFIG_SERVICE) private apiConfig: ApiConfig,
    private authService: AuthService,
  ) {}

  getListingGroupDetails(listingGroupId: string) {
    return this.httpClient.get<ListingGroupDetails>(
      `${this.apiConfig.apiEndpoint}/listing-groups/${listingGroupId}/detail`,
    );
  }

  // TODO: actual pagination support, psize + cursor
  getListingGroupSummariesPage(
    pageSize: number,
    cursor: string | null = null,
  ): Observable<PagedResult<ListingGroupSummary>> {
    const query: PagedListingGroupQuery = {
      pageSize: pageSize,
      cursor: cursor,
    };
    return this.httpClient.post<PagedResult<ListingGroupSummary>>(
      `${this.apiConfig.apiEndpoint}/listing-groups/page`,
      query,
    );
  }

  getMyListingGroups(): Observable<ListingGroupSummary[]> {
    return this.httpClient.post<ListingGroupSummary[]>(
      `${this.apiConfig.apiEndpoint}/listing-groups/filtered`,
      { sellerUserId: this.authService.activeAccount?.localAccountId },
    );
  }

  getFilteredListingGroups(filter: ListingGroupFilter): Observable<ListingGroupSummary[]> {
    return this.httpClient.post<ListingGroupSummary[]>(
      `${this.apiConfig.apiEndpoint}/listing-groups/filtered`,
      filter,
    );
  }

  getMySubscribedListingGroups(): Observable<ListingGroupSummary[]> {
    return this.httpClient.get<ListingGroupSummary[]>(
      `${this.apiConfig.apiEndpoint}/listing-groups/subscriptions`,
    );
  }

  getMyListingGroupsWithSubscriptionOfUser(
    subscriberUserId: string,
  ): Observable<ListingGroupSummary[]> {
    return this.httpClient.get<ListingGroupSummary[]>(
      `${this.apiConfig.apiEndpoint}/listing-groups/owned/subscribers/${subscriberUserId}`,
    );
  }

  createListingGroup(request: CreateListingGroupRequest): Observable<string> {
    return this.httpClient.post<string>(`${this.apiConfig.apiEndpoint}/listing-groups`, request);
  }

  updateListingGroup(request: UpdateListingGroupRequest): Observable<string> {
    return this.httpClient.put<string>(
      `${this.apiConfig.apiEndpoint}/listing-groups/${request.id}`,
      request,
    );
  }
}
