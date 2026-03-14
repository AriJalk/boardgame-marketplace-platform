import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Observable, of } from 'rxjs';
import { Inject, Injectable } from '@angular/core';
import { QueuePageResponse as ListingQueueResponse, QueueEntry } from './models/quque-entry.model';
import { API_CONFIG_SERVICE } from '../../core/api-config/api-config.service';
import { ApiConfig } from '../../core/api-config/api-config.interface';

@Injectable({
  providedIn: 'root',
})
export class QueueService {
  constructor(
    private httpClient: HttpClient,
    @Inject(API_CONFIG_SERVICE) private apiConfig: ApiConfig,
  ) {}

  getQueues(listingId: string): Observable<ListingQueueResponse> {
    const response = this.httpClient.get<ListingQueueResponse>(
      `${this.apiConfig.apiEndpoint}/listings/${listingId}/queue-entries`,
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
      `${this.apiConfig.apiEndpoint}/listings/${listingId}/queue-entries/${queueEntryId}/priority`,
      { requestedIndex },
    );
  }

  addPriority(listingId: string, queueEntryId: string): Observable<void> {
    return this.httpClient.post<void>(
      `${this.apiConfig.apiEndpoint}/listings/${listingId}/queue-entries/${queueEntryId}/priority`,
      null,
    );
  }

  removePriority(listingId: string, queueEntryId: string): Observable<void> {
    return this.httpClient.delete<void>(
      `${this.apiConfig.apiEndpoint}/listings/${listingId}/queue-entries/${queueEntryId}/priority`,
    );
  }

  // TODO: batch fetching
  getSubscription(listingId: string): Observable<QueueEntry> {
    const response = this.httpClient.get<QueueEntry>(
      `${this.apiConfig.apiEndpoint}/listings/${listingId}/queue-entries/my`,
    );
    // response.subscribe(data => console.log(data));
    return response;
  }

  subscribeToListing(listingId: string): Observable<QueueEntry> {
    const response = this.httpClient.put<QueueEntry>(
      `${this.apiConfig.apiEndpoint}/listings/${listingId}/queue-entries`,
      null,
    );
    // response.subscribe(data => console.log(data));
    return response;
  }

  unsubscribeFromListing(listingId: string, subscriptionId: string): Observable<void> {
    const response = this.httpClient.delete<void>(
      `${this.apiConfig.apiEndpoint}/listings/${listingId}/queue-entries/${subscriptionId}`,
    );
    return response;
  }

  reserveQueueEntry(listingId: string, subscriptionId: string): Observable<void> {
    const response = this.httpClient.post<void>(
      `${this.apiConfig.apiEndpoint}/listings/${listingId}/queue-entries/${subscriptionId}/reserve`,
      null,
    );
    return response;
  }
}
