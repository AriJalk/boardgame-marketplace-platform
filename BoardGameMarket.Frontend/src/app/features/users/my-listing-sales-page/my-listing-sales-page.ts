import { Component, OnInit } from '@angular/core';
import { Observable } from 'rxjs';
import { ListingSale } from '../../listing-sales/models/listing-sale.model';
import { AsyncPipe } from '@angular/common';
import { ListingSaleCard } from '../../listing-sales/components/listing-sale-card/listing-sale-card';
import { ListingSalesService } from '../../listing-sales/services/listing-sales.service';

@Component({
  selector: 'bgm-my-listing-sales-page',
  imports: [AsyncPipe, ListingSaleCard],
  templateUrl: './my-listing-sales-page.html',
  styleUrl: './my-listing-sales-page.css',
})
export class MyListingsSalesPage implements OnInit {
  listingSales$!: Observable<ListingSale[]>;

  constructor(private listingSalesService: ListingSalesService) {}

  ngOnInit(): void {
    this.listingSales$ = this.listingSalesService.getMyListingSales();
  }
}
