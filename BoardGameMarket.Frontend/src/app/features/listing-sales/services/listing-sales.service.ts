import { HttpClient } from '@angular/common/http';
import { Inject, Injectable } from '@angular/core';
import { API_CONFIG_SERVICE } from '../../../core/api-config/api-config.service';
import { ApiConfig } from '../../../core/api-config/api-config.interface';
import { AuthStubService } from '../../../core/auth/auth-stub.service';
import { Observable } from 'rxjs';
import { ListingSale } from '../models/listing-sale.model';

@Injectable({
  providedIn: 'root',
})
export class ListingSalesService {
  constructor(
    private httpClient: HttpClient,
    @Inject(API_CONFIG_SERVICE) private apiConfig: ApiConfig,
    private authService: AuthStubService,
  ) {}

  getListingSale(listingSaleId: string): Observable<ListingSale> {
    return this.httpClient.get<ListingSale>(
      `${this.apiConfig.apiEndpoint}/listings-sold/${listingSaleId}`,
      {
        headers: this.authService.headers,
      },
    );
  }

  getMyListingSales(): Observable<ListingSale[]> {
    return this.httpClient.get<ListingSale[]>(`${this.apiConfig.apiEndpoint}/listings-sold/my`, {
      headers: this.authService.headers,
    });
  }
}
