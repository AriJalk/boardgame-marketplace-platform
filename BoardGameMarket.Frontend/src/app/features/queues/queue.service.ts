import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Observable, of } from 'rxjs';
import { Inject, Injectable } from '@angular/core';
import { QueuePageResponse as ListingQueueResponse, QueueEntry } from './models/quque-entry.model';
import { API_CONFIG_SERVICE } from '../../core/api-config/api-config.service';
import { ApiConfig } from '../../core/api-config/api-config.interface';
import { AuthStubService } from '../../core/auth/auth-stub.service';

@Injectable({
  providedIn: 'root',
})
export class QueueService {
  private headers: HttpHeaders;
  constructor(
    private httpClient: HttpClient,
    private authService: AuthStubService,
    @Inject(API_CONFIG_SERVICE) private apiConfig: ApiConfig,
  ) {
    this.headers = new HttpHeaders({ Authorization: this.authService.loggedInUser?.userId ?? '' });
  }

  getQueues(listingId: string): Observable<ListingQueueResponse> {
    const response = this.httpClient.get<ListingQueueResponse>(
      `${this.apiConfig.apiEndpoint}/listings/${listingId}/queue`,
      { headers: this.headers },
    );
    // console.log(headers);
    // response.subscribe(data => console.log(data));
    return response;
  }

  /**
   * Update priority of item.
   *
   * @param a Requested item Id.
   * @param b Requested new index.
   * @returns Priority result.
   */
  updatePriority(
    listingId: string,
    queueEntryId: string,
    requestedIndex: number | null,
  ): Observable<void> {
    return this.httpClient.patch<void>(
      `${this.apiConfig.apiEndpoint}/listings/${listingId}/queue/subscription/${queueEntryId}/priority`,
      { requestedIndex },
      { headers: this.headers },
    );
  }

  addPriority(listingId: string, queueEntryId: string): Observable<QueueEntry> {
    return this.httpClient.post<QueueEntry>(
      `${this.apiConfig.apiEndpoint}/listings/${listingId}/queue/subscription/${queueEntryId}/priority`,
      null,
      { headers: this.headers },
    );
  }

  removePriority(listingId: string, queueEntryId: string): Observable<QueueEntry> {
    return this.httpClient.delete<QueueEntry>(
      `${this.apiConfig.apiEndpoint}/listings/${listingId}/queue/subscription/${queueEntryId}/priority`,
      { headers: this.headers },
    );
  }

  // TODO: batch fetching
  getSubscription(listingId: string): Observable<QueueEntry> {
    const response = this.httpClient.get<QueueEntry>(
      `${this.apiConfig.apiEndpoint}/listings/${listingId}/queue/subscription`,
      { headers: this.headers },
    );
    // response.subscribe(data => console.log(data));
    return response;
  }

  subscribeToListing(listingId: string): Observable<QueueEntry> {
    const response = this.httpClient.put<QueueEntry>(
      `${this.apiConfig.apiEndpoint}/listings/${listingId}/queue/subscription`,
      null,
      { headers: this.headers },
    );
    // response.subscribe(data => console.log(data));
    return response;
  }

  unsubscribeFromListing(listingId: string, subscriptionId: string): Observable<void> {
    const response = this.httpClient.delete<void>(
      `${this.apiConfig.apiEndpoint}/listings/${listingId}/queue/subscription/${subscriptionId}`,
      { headers: this.headers },
    );
    return response;
  }
}
