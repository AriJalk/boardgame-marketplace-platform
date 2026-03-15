import { Routes } from '@angular/router';
import { ListingGroupsPage } from './features/listing-groups/components/display/listing-groups-page/listing-groups-page';
import { MsalGuard } from '@azure/msal-angular';
export const routes: Routes = [
  { path: 'listing-groups', component: ListingGroupsPage },
  {
    path: 'listing-groups/create',
    loadComponent: () =>
      import('./features/listing-groups/components/create-listing-group-page/create-listing-group-page').then(
        (m) => m.CreateListingGroupPage,
      ),
    canActivate: [MsalGuard],
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
    path: 'my-listings',
    loadComponent: () =>
      import('./features/users/my-listings-page/my-listings-page').then((m) => m.MyListingsPage),
    canActivate: [MsalGuard],
  },
  {
    path: 'users/:userId',
    loadComponent: () =>
      import('./features/users/user-details-page/user-details-page').then((m) => m.UserDetailsPage),
  },
  {
    path: 'my-reservations',
    loadComponent: () =>
      import('./features/users/my-reservations-page/my-reservations-page').then(
        (m) => m.MyReservationsPage,
      ),
    canActivate: [MsalGuard],
  },
  {
    path: 'my-listing-sales',
    loadComponent: () =>
      import('./features/users/my-listing-sales-page/my-listing-sales-page').then(
        (m) => m.MyListingsSalesPage,
      ),
    canActivate: [MsalGuard],
  },
  { path: '', redirectTo: 'listing-groups', pathMatch: 'full' },
];
