import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Observable, of } from 'rxjs';
import { Inject, Injectable } from '@angular/core';
import { QueuePageResponse as ListingQueueResponse } from './models/quque-entry.model';
import { API_CONFIG_SERVICE } from '../../core/api-config/api-config.service';
import { ApiConfig } from '../../core/api-config/api-config.interface';
import { AuthStubService } from '../../core/auth/auth-stub.service';

@Injectable({
  providedIn: 'root',
})
export class QueueService {
  constructor(
    private httpClient: HttpClient,
    private authService: AuthStubService,
    @Inject(API_CONFIG_SERVICE) private apiConfig: ApiConfig,
  ) {}

  getQueues(listingId: string): Observable<ListingQueueResponse> {
    const headers = new HttpHeaders();
    headers.set('Atuhorization', this.authService.loggedInUser?.userId ?? '');
    const response = this.httpClient.get<ListingQueueResponse>(
      `${this.apiConfig.apiEndpoint}/listings/${listingId}/queue`,
      { headers: headers },
    );

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
      `${this.apiConfig.apiEndpoint}/listings/${listingId}/queue/${queueEntryId}`,
      { requestedIndex },
    );
  }

  isSubscribed(listingId: string): Observable<boolean> {
    const headers = new HttpHeaders();
    headers.set('Authorization', this.authService.loggedInUser?.userId ?? '');
    const response = this.httpClient.get<boolean>(
      `${this.apiConfig.apiEndpoint}/listings/${listingId}/queue/isQueued`,
      { headers: headers },
    );
    // response.subscribe(data => console.log(data));
    return response;
  }
}
