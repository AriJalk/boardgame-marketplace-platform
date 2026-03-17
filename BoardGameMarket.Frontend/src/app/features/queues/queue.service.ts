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

  getQueues(listingId: string, authHeader: string): Observable<ListingQueueResponse> {
    const headers = authHeader ? { Authorization: authHeader } : undefined;
    const response = this.httpClient.get<ListingQueueResponse>(
      `${this.apiConfig.apiEndpoint}/listings/${listingId}/queue-entries`,
      { headers: headers },
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
    authHeader: string,
  ): Observable<void> {
    const headers = authHeader ? { Authorization: authHeader } : undefined;
    return this.httpClient.patch<void>(
      `${this.apiConfig.apiEndpoint}/listings/${listingId}/queue-entries/${queueEntryId}/priority`,
      { requestedIndex },
      { headers: headers },
    );
  }

  addPriority(listingId: string, queueEntryId: string, authHeader: string): Observable<void> {
    const headers = authHeader ? { Authorization: authHeader } : undefined;
    return this.httpClient.post<void>(
      `${this.apiConfig.apiEndpoint}/listings/${listingId}/queue-entries/${queueEntryId}/priority`,
      null,
      { headers: headers },
    );
  }

  removePriority(listingId: string, queueEntryId: string, authHeader: string): Observable<void> {
    const headers = authHeader ? { Authorization: authHeader } : undefined;
    return this.httpClient.delete<void>(
      `${this.apiConfig.apiEndpoint}/listings/${listingId}/queue-entries/${queueEntryId}/priority`,
      { headers: headers },
    );
  }

  // TODO: batch fetching
  getSubscription(listingId: string, authHeader: string): Observable<QueueEntry> {
    const headers = authHeader ? { Authorization: authHeader } : undefined;
    return this.httpClient.get<QueueEntry>(
      `${this.apiConfig.apiEndpoint}/listings/${listingId}/queue-entries/my`,
      { headers: headers },
    );
  }

  subscribeToListing(listingId: string, authHeader: string): Observable<QueueEntry> {
    const headers = authHeader ? { Authorization: authHeader } : undefined;
    return this.httpClient.put<QueueEntry>(
      `${this.apiConfig.apiEndpoint}/listings/${listingId}/queue-entries`,
      null,
      { headers: headers },
    );
  }

  unsubscribeFromListing(
    listingId: string,
    subscriptionId: string,
    authHeader: string,
  ): Observable<void> {
    const headers = authHeader ? { Authorization: authHeader } : undefined;
    return this.httpClient.delete<void>(
      `${this.apiConfig.apiEndpoint}/listings/${listingId}/queue-entries/${subscriptionId}`,
      { headers: headers },
    );
  }

  reserveQueueEntry(
    listingId: string,
    subscriptionId: string,
    authHeader: string,
  ): Observable<void> {
    const headers = authHeader ? { Authorization: authHeader } : undefined;
    return this.httpClient.post<void>(
      `${this.apiConfig.apiEndpoint}/listings/${listingId}/queue-entries/${subscriptionId}/reserve`,
      null,
      { headers: headers },
    );
  }
}
