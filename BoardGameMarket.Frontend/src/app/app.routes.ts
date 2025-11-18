import { Routes } from '@angular/router';
import { ListingGroupDetailsPage } from './features/listing-groups/components/display/listing-group-details-page/listing-group-details-page';
import { ListingGroupsGrid } from './features/listing-groups/components/display/listing-groups-grid/listing-groups-grid';
import { ListingGroupsPage } from './features/listing-groups/components/display/listing-groups-page/listing-groups-page';

export const routes: Routes = [
    { path: "listing-groups/:listingGroupId", component: ListingGroupDetailsPage },
    { path: "", component: ListingGroupsPage }
];
