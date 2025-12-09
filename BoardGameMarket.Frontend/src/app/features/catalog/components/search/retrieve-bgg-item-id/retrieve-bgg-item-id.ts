import { AfterViewInit, Component, EventEmitter, Output } from '@angular/core';
import { FormControl, FormsModule, ReactiveFormsModule } from '@angular/forms';
import { MatFormFieldModule } from '@angular/material/form-field';
import { CatalogItemGetterService } from '../../../services/catalog-item-getter.service';
import { MatInputModule } from '@angular/material/input';
import { catchError, debounceTime, distinctUntilChanged, EMPTY, filter, map, of, switchMap } from 'rxjs';
import { V } from '@angular/cdk/keycodes';

@Component({
  selector: 'bgm-retrieve-bgg-item-id',
  imports: [MatFormFieldModule, FormsModule, MatInputModule, ReactiveFormsModule],
  templateUrl: './retrieve-bgg-item-id.html',
  styleUrl: './retrieve-bgg-item-id.css',
})
export class RetrieveBggItemId implements AfterViewInit {
  @Output() idRetrieved = new EventEmitter<string | null>();

  formControl = new FormControl('');
  constructor(private catalogGetterService: CatalogItemGetterService) { }

  ngAfterViewInit(): void {
    this.formControl?.valueChanges.pipe(
      debounceTime(300),
      distinctUntilChanged(),
      filter(value => !!value),
      switchMap(url => {
        return this.catalogGetterService.getItemIdByBggUrl(url).pipe(
          catchError(() => {
            return of(null);
          }));
      })
    ).subscribe({
      next: value => {
        this.idRetrieved.emit(value);
      },
    });
  }
}
