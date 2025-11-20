import { Routes } from '@angular/router';
import { ListingGroupDetailsPage } from './features/listing-groups/components/display/listing-group-details-page/listing-group-details-page';
import { ListingGroupsPage } from './features/listing-groups/components/display/listing-groups-page/listing-groups-page';

export const routes: Routes = [
    { path: "listing-groups", component: ListingGroupsPage },
    { path: "listing-groups/:listingGroupId", component: ListingGroupDetailsPage },
    { path: "", redirectTo: 'listing-groups', pathMatch: 'full'}
];
