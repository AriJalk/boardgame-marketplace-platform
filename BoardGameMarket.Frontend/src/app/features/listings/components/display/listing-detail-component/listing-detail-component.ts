import { Component, Input, OnInit } from '@angular/core';
import { MatCardModule } from '@angular/material/card';
import { BggItem, CatalogItem, CustomItem } from '../../../../catalog/models/catalog-item.model';
import { ListingDetail } from '../../../models/listing.model';
import { BggItemCard } from '../../../../catalog/components/display/bgg-item-card/bgg-item-card';
import { CustomItemCard } from '../../../../catalog/components/display/custom-item-card/custom-item-card';
import { ActivatedRoute } from '@angular/router';

@Component({
  selector: 'bgm-listing-detail-component',
  imports: [MatCardModule, BggItemCard, CustomItemCard],
  templateUrl: './listing-detail-component.html',
  styleUrl: './listing-detail-component.css',
})
export class ListingDetailComponent implements OnInit {
  listingDetail?: ListingDetail

  constructor(private router: ActivatedRoute) { }

  ngOnInit(): void {
    this.router.paramMap.subscribe(params => {
      const id = params.get('listingId');
      if (id) {
        this.listingDetail = {
          id: id,
          price: 30,
          title: "1 test title",
          sellerDisplayName: "Seller name",
          items: [
            { id: "1", name: "Item1", type: 'bgg', bggLink: "1" } as BggItem,
            { id: "2", name: "Item2", type: 'custom' } as CustomItem
          ]
        }
      }
    });
  }


  isBgg(item: CatalogItem): item is BggItem {
    return item.type === 'bgg';
  }

  isCustom(item: CatalogItem): item is CustomItem {
    return item.type === 'custom';
  }
}
