import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { CatalogItem } from '../models/catalog-item.model';

@Injectable({
  providedIn: 'root',
})
export class CatalogItemGetterService {
  private url: string = "https://localhost:6063/api/items"
  constructor(private httpClient: HttpClient) { }

  get_items(itemIds: string[]): Observable<CatalogItem[]> {
    const request = "?ids=" + itemIds.join(",");
    return this.httpClient.get<CatalogItem[]>("ids?");
  }

  get_item(itemId: string): Observable<CatalogItem> {
    const request = "/" + itemId;
    return this.httpClient.get<CatalogItem>("/");
  }
}
