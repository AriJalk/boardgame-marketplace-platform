import { Routes } from '@angular/router';
import { ListingGroupsPage } from './features/listing-groups/components/display/listing-groups-page/listing-groups-page';
import { ListingGroupDetailsPage } from './features/listing-groups/components/display/listing-group-details-page/listing-group-details-page';
import { AddListingGroupPage } from './features/listing-groups/components/creation/add-listing-group-page/add-listing-group-page';

export const routes: Routes = [
    { path: "listing-groups", component: ListingGroupsPage },
    { path: "listing-groups/create", component: AddListingGroupPage },
    { path: "listing-groups/:listingGroupId", component: ListingGroupDetailsPage },
    { path: "", redirectTo: 'listing-groups', pathMatch: 'full' }
];
