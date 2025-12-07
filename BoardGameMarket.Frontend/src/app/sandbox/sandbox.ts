import { HttpClient } from '@angular/common/http';
import { ChangeDetectionStrategy, Component, OnInit } from '@angular/core';
import { Observable } from 'rxjs';
import { ListingDetail } from '../features/listings/models/listing.model';
import { ListingDetailsCard } from '../features/listings/components/display/listing-details-card/listing-details-card';
import { AsyncPipe } from '@angular/common';
import { SearchCatalog } from "../features/catalog/components/search/search-catalog/search-catalog";

@Component({
  selector: 'bgm-sandbox',
  imports: [ListingDetailsCard, AsyncPipe, SearchCatalog],
  templateUrl: './sandbox.html',
  styleUrl: './sandbox.css',
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class Sandbox implements OnInit {

  getListing$?: Observable<ListingDetail>;

  constructor(private httpClient: HttpClient) { }

  ngOnInit(): void {
    this.getListing$ = this.httpClient.get<ListingDetail>("https://localhost:7155/api/testing/getlisting");
  }

}
