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
import { AuthStubService } from '../../../core/auth/auth-stub.service';
import { ListingGroupFilter, PagedListingGroupQuery } from '../models/listing-group-queries';

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
    const headers = new HttpHeaders({ Authorization: this.authService.loggedInUser?.userId ?? '' });
    return this.httpClient.get<ListingGroupDetails>(
      `${this.apiConfig.apiEndpoint}/listing-groups/${listingGroupId}/detail`,
      { headers: headers },
    );
  }

  // TODO: actual pagination support, psize + cursor
  getListingGroupSummariesPage(): Observable<PagedResult<ListingGroupSummary>> {
    const headers = new HttpHeaders({ Authorization: this.authService.loggedInUser?.userId ?? '' });
    const query: PagedListingGroupQuery = {
      pageSize: 10,
    };
    return this.httpClient.post<PagedResult<ListingGroupSummary>>(
      `${this.apiConfig.apiEndpoint}/listing-groups/page`,
      query,
      { headers: headers },
    );
  }

  getMyListingGroups(): Observable<ListingGroupSummary[]> {
    return this.httpClient.post<ListingGroupSummary[]>(
      `${this.apiConfig.apiEndpoint}/listing-groups/filtered`,
      { sellerUserId: this.authService.loggedInUser?.userId },
      { headers: this.authService.headers },
    );
  }

  getFilteredListingGroups(filter: ListingGroupFilter): Observable<ListingGroupSummary[]> {
    return this.httpClient.post<ListingGroupSummary[]>(
      `${this.apiConfig.apiEndpoint}/listing-groups/filtered`,
      filter,
      { headers: this.authService.headers },
    );
  }

  getMySubscribedListingGroups(): Observable<ListingGroupSummary[]> {
    return this.httpClient.get<ListingGroupSummary[]>(
      `${this.apiConfig.apiEndpoint}/subscriptions/listing-groups`,
      { headers: this.authService.headers },
    );
  }

  getMyListingGroupsWithSubscriptionOfUser(
    subscriberUserId: string,
  ): Observable<ListingGroupSummary[]> {
    return this.httpClient.get<ListingGroupSummary[]>(
      `${this.apiConfig.apiEndpoint}/subscriptions/my-listing-groups/subscriber/${subscriberUserId}`,
      { headers: this.authService.headers },
    );
  }

  postListingGroup(request: CreateListingGroupRequest): Observable<ListingGroupDetails> {
    return this.httpClient.post<ListingGroupDetails>(
      `${this.apiConfig.apiEndpoint}/listing-groups`,
      request,
      { headers: this.authService.headers },
    );
  }

  updateListingGroup(request: UpdateListingGroupRequest): Observable<ListingGroupDetails> {
    return this.httpClient.put<ListingGroupDetails>(
      `${this.apiConfig.apiEndpoint}/listing-groups/${request.id}`,
      request,
      { headers: this.authService.headers },
    );
  }
}
