import { Component, OnInit } from '@angular/core';
import { ListingGroupDetails } from '../../../models/listing-group.model';
import { ActivatedRoute } from '@angular/router';
import { BggItem, CustomItem } from '../../../../catalog/models/catalog-item.model';
import { ListingDetailsCard } from '../../../../listings/components/display/listing-details-card/listing-details-card';

@Component({
  selector: 'bgm-listing-group-details-page',
  imports: [ListingDetailsCard],
  templateUrl: './listing-group-details-page.html',
  styleUrl: './listing-group-details-page.css',
})
export class ListingGroupDetailsPage implements OnInit {
  listingGroup?: ListingGroupDetails;

  constructor(private router: ActivatedRoute) { }

  ngOnInit(): void {
    this.router.paramMap.subscribe(params => {
      const id = params.get('listingGroupId');
      if (id) {
        this.listingGroup = {
          id: id,
          location: "Test location",
          postingTime: new Date(Date.now()),
          seller: { id: "1", displayName: "Test Seller" },
          pickupOptions: "Test pickup options",
          details: "Test details",
          listings: [
            {
              id: "1", title: "Puerto rico bundle", price: 50, items: [
                { catalogItem: { id: "1", name: "Puerto rico base", type: 'bgg', bggLink: 'https://www.boardgamegeek.com/boardgame/3076' } as BggItem, details: 'base game' },
                { catalogItem: { id: "2", name: "Puerto rico expansion", type: 'custom', bggLink: '2' } as CustomItem, details: 'exp' }
              ]
            },
            {
              id: "2", title: "Agricola bundle", price: 100, items: [
                { catalogItem: { id: "3", name: "Agricola base", type: 'bgg', bggLink: 'https://www.boardgamegeek.com/boardgame/31260' } as BggItem, details: 'base game' },
                { catalogItem: { id: "4", name: "Agricola expansion", type: 'custom', bggLink: '2' } as CustomItem, details: 'exp' }
              ]
            },
          ]
        }
      }
    });
  }
}
