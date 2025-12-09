import { HttpClient } from '@angular/common/http';
import {
  ChangeDetectionStrategy,
  ChangeDetectorRef,
  Component,
  Inject,
  OnInit,
} from '@angular/core';
import { Observable } from 'rxjs';
import { ListingDetail } from '../features/listings/models/listing.model';
import { ListingDetailsCard } from '../features/listings/components/display/listing-details-card/listing-details-card';
import { AsyncPipe } from '@angular/common';
import { SearchCatalog } from '../features/catalog/components/search/search-catalog/search-catalog';
import { ApiConfig } from '../core/api-config/api-config.interface';
import { API_CONFIG_SERVICE } from '../core/api-config/api-config.service';

@Component({
  selector: 'bgm-sandbox',
  imports: [ListingDetailsCard, SearchCatalog],
  templateUrl: './sandbox.html',
  styleUrl: './sandbox.css',
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class Sandbox implements OnInit {
  listings: ListingDetail[] = [];

  constructor(
    private httpClient: HttpClient,
    @Inject(API_CONFIG_SERVICE) private apiConfig: ApiConfig,
    private cd: ChangeDetectorRef,
  ) {}

  ngOnInit(): void {
    this.httpClient
      .get<ListingDetail>(`${this.apiConfig.apiEndpoint}/listings/getdummylisting`)
      .subscribe({
        next: (data) => {
          console.log(data);
          this.listings = [...this.listings, data];
          this.cd.markForCheck();
        },
      });
  }
}
