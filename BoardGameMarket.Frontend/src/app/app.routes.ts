import { Routes } from '@angular/router';
import { ListingDetailComponent } from './features/listings/components/display/listing-detail-component/listing-detail-component';
import { ListingGroupsGrid } from './features/listings/components/display/listing-groups-grid/listing-groups-grid';

export const routes: Routes = [
    { path: "listings/:listingId", component: ListingDetailComponent },
    { path: "", component: ListingGroupsGrid }
];
