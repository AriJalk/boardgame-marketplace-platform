import { AfterViewInit, Component, EventEmitter, OnDestroy, Output } from '@angular/core';
import { FormControl, FormsModule, ReactiveFormsModule } from '@angular/forms';
import { MatFormFieldModule } from '@angular/material/form-field';
import { CatalogItemGetterService } from '../../../services/catalog-item-getter.service';
import { MatInputModule } from '@angular/material/input';
import {
  catchError,
  debounceTime,
  distinctUntilChanged,
  filter,
  finalize,
  of,
  Subscription,
  switchMap,
  tap,
} from 'rxjs';
import { CatalogItem } from '../../../models/catalog-item.model';
import { MatIconModule } from '@angular/material/icon';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';

@Component({
  selector: 'bgm-retrieve-bgg-item-id',
  imports: [
    MatFormFieldModule,
    FormsModule,
    MatInputModule,
    ReactiveFormsModule,
    MatIconModule,
    MatProgressSpinnerModule,
  ],
  templateUrl: './retrieve-bgg-item-id.html',
  styleUrl: './retrieve-bgg-item-id.css',
})
export class RetrieveBggItemId implements AfterViewInit, OnDestroy {
  @Output() catalogItemRetrieved = new EventEmitter<CatalogItem | null>();

  urlControl = new FormControl('');
  name?: string;

  isLoading: boolean = false;

  private valueChangeSubscription!: Subscription;

  constructor(private catalogGetterService: CatalogItemGetterService) {}

  ngAfterViewInit(): void {
    this.valueChangeSubscription = this.urlControl?.valueChanges
      .pipe(
        debounceTime(300),
        distinctUntilChanged(),
        filter((value) => !!value),
        tap(() => (this.isLoading = true)),
        switchMap((url) => {
          return this.catalogGetterService.getItemIdByBggUrl(url).pipe(
            catchError(() => {
              return of(null);
            }),
            finalize(() => (this.isLoading = false)),
          );
        }),
      )
      .subscribe({
        next: (value) => {
          if (value !== null) {
            this.catalogItemRetrieved.emit(value);
            this.name = value?.name;
            this.urlControl.disable();
          }
        },
      });
  }

  ngOnDestroy(): void {
    this.valueChangeSubscription.unsubscribe();
  }
}
