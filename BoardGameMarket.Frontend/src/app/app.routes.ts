import { Routes } from '@angular/router';
import { ListingGroupsPage } from './features/listing-groups/components/display/listing-groups-page/listing-groups-page';
export const routes: Routes = [
  { path: 'listing-groups', component: ListingGroupsPage },
  {
    path: 'listing-groups/create',
    loadComponent: () =>
      import('./features/listing-groups/components/create-listing-group-page/create-listing-group-page').then(
        (m) => m.CreateListingGroupPage,
      ),
  },
  {
    path: 'listing-groups/:listingGroupId',
    loadComponent: () =>
      import('./features/listing-groups/components/display/listing-group-details-page/listing-group-details-page').then(
        (m) => m.ListingGroupDetailsPage,
      ),
  },
  {
    path: 'sandbox',
    loadComponent: () => import('./features/sandbox/sandbox').then((m) => m.Sandbox),
  },
  {
    path: 'my-subscriptions',
    loadComponent: () =>
      import('./features/queues/components/buyer-subscriptions-page/buyer-subscriptions-page').then(
        (m) => m.BuyerSubscriptionsPage,
      ),
  },
  { path: '', redirectTo: 'listing-groups', pathMatch: 'full' },
];
