import { Component, Input, OnInit } from '@angular/core';
import { ListingGroupForm } from '../../../listing-forms/listing-group-form/listing-group-form';
import { ListingGroupService } from '../../services/listing-group.service';
import { ListingFormsFactory } from '../../../listing-forms/services/listing-forms-factory.service';
import { FormGroup, ReactiveFormsModule } from '@angular/forms';
import {
  BehaviorSubject,
  exhaustMap,
  firstValueFrom,
  map,
  Observable,
  Subject,
  switchMap,
  take,
  throwError,
} from 'rxjs';
import { AsyncPipe } from '@angular/common';
import { MatButtonModule } from '@angular/material/button';
import { CreateListingGroupRequest } from '../../models/listing-group.model';
import { CreateListingRequest as CreateListingRequest } from '../../../listings/models/listing.model';
import { CreateListingItemRequest } from '../../../listing-items/models/listing-item.model';
import { ListingForm } from '../../../listing-forms/listing-form/listing-form';
import { CatalogItemGetterService } from '../../../catalog/services/catalog-item-getter.service';
import { Router } from '@angular/router';
import { MatSnackBar } from '@angular/material/snack-bar';
import { AuthService } from '../../../../core/auth/auth.service';

@Component({
  selector: 'bgm-create-listing-group',
  imports: [ListingGroupForm, ReactiveFormsModule, MatButtonModule],
  templateUrl: './create-listing-group-page.html',
  styleUrl: './create-listing-group-page.css',
})
export class CreateListingGroupPage implements OnInit {
  @Input({ required: true }) listingId!: string;

  formGroup!: FormGroup;

  private idempotencyKey = crypto.randomUUID();
  private requestSubject$ = new Subject<CreateListingGroupRequest>();

  constructor(
    private listingGroupService: ListingGroupService,
    private formFactory: ListingFormsFactory,
    private catalogService: CatalogItemGetterService,
    private router: Router,
    private snackBar: MatSnackBar,
    private authService: AuthService,
  ) {}

  ngOnInit(): void {
    this.formGroup = this.formFactory.creatingListingGroupFormGroup();

    this.requestSubject$
      .pipe(
        exhaustMap((request) => {
          return this.authService.withAuthCall((header) =>
            this.listingGroupService.createListingGroup(request, header, this.idempotencyKey),
          );
        }),
      )
      .subscribe({
        next: (id) => {
          this.router.navigate(['/listing-groups', id]);
          this.snackBar.open('Creation successful', 'Close', { duration: 2000 });
        },
        error: (err) => {
          console.error(err);
          this.snackBar.open('Creation failed', 'Close', { duration: 2000 });
        },
      });
  }

  private async createRequest() {
    if (this.formGroup.invalid) {
      console.log('invalid');
      return;
    }
    const fg = this.formGroup.getRawValue();

    const customNames = fg.listings
      .flatMap((l: any) => l.listingItems.filter((li: any) => li.catalogItem.type === 'custom'))
      .map((li: any) => li.catalogItem.name);

    const customItems = await firstValueFrom(
      this.catalogService.getCustomCatalogItemsByNames(customNames),
    );

    const customItemsByName = new Map(customItems.map((item) => [item.name, item]));

    const request: CreateListingGroupRequest = {
      location: fg.location,
      details: fg.details,
      pickupOptions: fg.pickupOptions,
      listings: fg.listings.map(
        (l: any): CreateListingRequest => ({
          title: l.title,
          price: l.price,
          details: l.details,
          listingItems: l.listingItems.map(
            (li: any): CreateListingItemRequest => ({
              catalogId:
                li.catalogItem.id ?? customItemsByName.get(li.catalogItem.name)?.id ?? null,
              details: li.details,
            }),
          ),
        }),
      ),
    };

    return request;
  }

  async submitForm() {
    const request = await this.createRequest();
    if (request) {
      // this.requestSubject$.next(request);
      this.authService
        .withAuthCall((header) =>
          this.listingGroupService.createListingGroup(request, header, this.idempotencyKey),
        )
        .subscribe({
          next: (id) => {
            this.router.navigate(['/listing-groups', id]);
            this.snackBar.open('Creation successful', 'Close', { duration: 2000 });
          },
          error: (err) => {
            console.error(err);
            this.snackBar.open('Creation failed', 'Close', { duration: 2000 });
          },
        });
    }
  }
}
