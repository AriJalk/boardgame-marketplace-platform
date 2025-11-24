import { Routes } from '@angular/router';
import { ListingGroupsPage } from './features/listing-groups/components/display/listing-groups-page/listing-groups-page';
import { ListingGroupDetailsPage } from './features/listing-groups/components/display/listing-group-details-page/listing-group-details-page';
import { ListingGroupCreationPage } from './features/listing-groups/components/creation/listing-group-creation-page/listing-group-creation-page';

export const routes: Routes = [
    { path: "listing-groups", component: ListingGroupsPage },
    { path: "listing-groups/creation-page", component: ListingGroupCreationPage },
    { path: "listing-groups/:listingGroupId", component: ListingGroupDetailsPage },
    { path: "", redirectTo: 'listing-groups', pathMatch: 'full' }
];
