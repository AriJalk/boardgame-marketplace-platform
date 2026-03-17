import { Component, Input, OnInit } from '@angular/core';
import { ListingGroupForm } from '../../../listing-forms/listing-group-form/listing-group-form';
import { ListingGroupService } from '../../services/listing-group.service';
import { ListingFormsFactory } from '../../../listing-forms/services/listing-forms-factory.service';
import { FormGroup, ReactiveFormsModule } from '@angular/forms';
import {
  BehaviorSubject,
  firstValueFrom,
  map,
  Observable,
  switchMap,
  take,
  throwError,
} from 'rxjs';
import { AsyncPipe } from '@angular/common';
import { MatButtonModule } from '@angular/material/button';
import { CatalogItemGetterService } from '../../../catalog/services/catalog-item-getter.service';
import { Router } from '@angular/router';
import { UpdateListingGroupRequest } from '../../models/listing-group.model';
import { UpdateListingRequest as UpdateListingRequest } from '../../../listings/models/listing.model';
import { UpdateListingItemRequest } from '../../../listing-items/models/listing-item.model';
import { MatSnackBar } from '@angular/material/snack-bar';
import { AuthService } from '../../../../core/auth/auth.service';

@Component({
  selector: 'bgm-edit-listing-group',
  imports: [ListingGroupForm, ReactiveFormsModule, AsyncPipe, MatButtonModule],
  templateUrl: './edit-listing-group-page.html',
  styleUrl: './edit-listing-group-page.css',
})
export class EditListingGroupPage implements OnInit {
  @Input({ required: true }) listingId!: string;

  private formGroupSubject = new BehaviorSubject<FormGroup | null>(null);
  formGroup$ = this.formGroupSubject.asObservable();

  constructor(
    private listingGroupService: ListingGroupService,
    private formFactory: ListingFormsFactory,
    private catalogService: CatalogItemGetterService,
    private router: Router,
    private snackBar: MatSnackBar,
    private authService: AuthService,
  ) {}

  ngOnInit(): void {
    this.listingGroupService
      .getListingGroupDetails(this.listingId)
      .pipe(take(1))
      .subscribe((details) => {
        const formGroup = this.formFactory.creatingListingGroupFormGroup(details);
        this.formGroupSubject.next(formGroup);
      });
  }

  async submitForm() {
    const fg = this.formGroupSubject.value?.getRawValue();
    if (fg == null || fg.invalid) {
      console.log('invalid');
      return;
    }

    const customNames = fg.listings
      .flatMap((l: any) =>
        l.listingItems.filter(
          (li: any) => li.catalogItem.type === 'custom' && li.catalogItem.id == null,
        ),
      )
      .map((li: any) => li.catalogItem.name);

    const customItems = await firstValueFrom(
      this.catalogService.getCustomCatalogItemsByNames(customNames),
    );

    const customItemsByName = new Map(customItems.map((item) => [item.name, item]));

    const request: UpdateListingGroupRequest = {
      id: fg.id,
      details: fg.details,
      pickupOptions: fg.pickupOptions,
      location: fg.location,
      listings: fg.listings.map(
        (l: any): UpdateListingRequest => ({
          id: l.id,
          title: l.title,
          price: l.price,
          details: l.details,
          listingItems: l.listingItems.map(
            (li: any): UpdateListingItemRequest => ({
              id: li.id,
              catalogId:
                li.catalogItem.id ?? customItemsByName.get(li.catalogItem.name)?.id ?? null,
              details: li.details,
            }),
          ),
        }),
      ),
    };

    this.authService
      .withAuthHeader()
      .pipe(
        switchMap((header) => {
          if (!header) {
            return throwError(() => new Error('User not authenticated'));
          }
          return this.listingGroupService.updateListingGroup(request, header);
        }),
      )
      .subscribe({
        next: (id) => {
          this.router.navigate(['/listing-groups', id]);
          this.snackBar.open('Update successful', 'Close', { duration: 2000 });
        },
        error: (err) => {
          console.error(err);
          this.snackBar.open('Update failed', 'Close', { duration: 2000 });
        },
      });
  }
}
