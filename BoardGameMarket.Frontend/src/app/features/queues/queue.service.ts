import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Observable, of } from 'rxjs';
import { Inject, Injectable } from '@angular/core';
import { QueuePageResponse } from './models/quque-entry.model';
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

  getQueues(listingId: string): Observable<QueuePageResponse> {
    const headers = new HttpHeaders();
    headers.set('jwtToken', this.authService.loggedInUser?.userId ?? '');
    return this.httpClient.get<QueuePageResponse>(
      `${this.apiConfig.apiEndpoint}/listings/${listingId}/queues`,
      { headers: headers },
    );
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
    requestedIndex: number,
  ): Observable<void> {
    return this.httpClient.post<void>(
      `${this.apiConfig.apiEndpoint}/listings/${listingId}/queues/${queueEntryId}`,
      { requestedIndex },
    );
  }
}
