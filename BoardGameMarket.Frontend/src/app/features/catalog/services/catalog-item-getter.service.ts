import { HttpClient } from '@angular/common/http';
import { Inject, Injectable } from '@angular/core';
import { delay, Observable, of } from 'rxjs';
import { BggItem, CatalogItem } from '../models/catalog-item.model';
import { API_CONFIG_SERVICE } from '../../../core/api-config/api-config.service';
import { ApiConfig } from '../../../core/api-config/api-config.interface';

@Injectable({
  providedIn: 'root',
})
export class CatalogItemGetterService {
  constructor(private httpClient: HttpClient, @Inject(API_CONFIG_SERVICE) private apiConfig: ApiConfig) { }

  getItems(itemIds: string[]): Observable<CatalogItem[]> {
    const request = "?ids=" + itemIds.join(",");
    return this.httpClient.get<CatalogItem[]>("ids?");
  }

  getItem(itemId: string): Observable<CatalogItem> {
    const request = "/" + itemId;
    return this.httpClient.get<CatalogItem>("/");
  }

  searchItems(searchString: string): Observable<BggItem[]> {
    const dummy: BggItem[] = [
      { id: "420498", bggLink: "", name: searchString, type: 'bgg' },
      { id: "12536", bggLink: "bggLink", name: searchString + ";DUMMY", type: 'bgg' },
    ];
    return of(dummy).pipe(delay(400));
  }

  private extractBggId(url: string): number | null {
    const match = url.match(/^(?:https?:\/\/)?(?:www\.)?boardgamegeek\.com\/[^/]+\/(\d+)(?:\/|$)/);
    console.log(match);
    return match ? Number(match[1]) : null;
  }

  getItemIdByBggUrl(url: string | null): Observable<string | null> {
    if (url !== null) {
      const id = this.extractBggId(url);
      if (id !== null) {
        return this.httpClient.get<string>(`${this.apiConfig.apiEndpoint}/catalog/bgg/${id}`);
      }
    }
    return of(null);
  }
}
