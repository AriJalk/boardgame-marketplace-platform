import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Inject, Injectable, OnInit } from '@angular/core';
import { API_CONFIG_SERVICE } from '../../../core/api-config/api-config.service';
import { ApiConfig } from '../../../core/api-config/api-config.interface';
import {
  CreateListingGroupRequest,
  ListingGroupDetails,
  ListingGroupSummary,
  UpdateListingGroupRequest,
} from '../models/listing-group.model';
import { firstValueFrom, Observable } from 'rxjs';
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

  getListingGroupDetails(listingGroupId: string, authHeader?: string) {
    const headers = authHeader ? { Authorization: authHeader } : undefined;
    return this.httpClient.get<ListingGroupDetails>(
      `${this.apiConfig.apiEndpoint}/listing-groups/${listingGroupId}/detail`,
      { headers: headers },
    );
  }

  getListingGroupSummariesPage(
    pageSize: number,
    cursor: string | null = null,
    authHeader?: string,
  ): Observable<PagedResult<ListingGroupSummary>> {
    const query: PagedListingGroupQuery = {
      pageSize: pageSize,
      cursor: cursor,
    };
    const headers = authHeader ? { Authorization: authHeader } : undefined;
    return this.httpClient.post<PagedResult<ListingGroupSummary>>(
      `${this.apiConfig.apiEndpoint}/listing-groups/page`,
      query,
    );
  }

  getOwnedListingGroups(): Observable<ListingGroupSummary[]> {
    return this.httpClient.post<ListingGroupSummary[]>(
      `${this.apiConfig.apiEndpoint}/listing-groups/filtered`,
      { sellerUserId: this.authService.activeAccountValue?.localAccountId },
    );
  }

  getFilteredListingGroups(
    filter: ListingGroupFilter,
    authHeader?: string,
  ): Observable<ListingGroupSummary[]> {
    const headers = authHeader ? { Authorization: authHeader } : undefined;
    return this.httpClient.post<ListingGroupSummary[]>(
      `${this.apiConfig.apiEndpoint}/listing-groups/filtered`,
      filter,
      { headers: headers },
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

  createListingGroup(
    request: CreateListingGroupRequest,
    authHeader: string,
    idempotencyKey: string,
  ): Observable<string> {
    const headers = authHeader
      ? { Authorization: authHeader, 'Idempotency-Key': idempotencyKey }
      : undefined;
    return this.httpClient.post<string>(`${this.apiConfig.apiEndpoint}/listing-groups`, request, {
      headers: headers,
    });
  }

  updateListingGroup(request: UpdateListingGroupRequest, authHeader: string): Observable<string> {
    const headers = authHeader ? { Authorization: authHeader } : undefined;
    return this.httpClient.put<string>(
      `${this.apiConfig.apiEndpoint}/listing-groups/${request.id}`,
      request,
      { headers: headers },
    );
  }
}
