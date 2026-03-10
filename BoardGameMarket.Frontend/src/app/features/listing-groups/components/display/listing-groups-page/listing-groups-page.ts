import { AfterViewInit, Component, OnInit } from '@angular/core';
import { PaginationControls } from '../../../../../common/pagination-controls/pagination-controls';
import { ListingGroupService } from '../../../services/listing-group.service';
import { ListingGroupSummary } from '../../../models/listing-group.model';
import { AsyncPipe, CommonModule } from '@angular/common';
import { BehaviorSubject, Observable } from 'rxjs';
import { ListingGroupSummaryCard } from '../listing-group-summary-card/listing-group-summary-card';
import { PagedResult } from '../../../../../common/paged-result';
import { AuthStubService } from '../../../../../core/auth/auth-stub.service';
import { UserSummary } from '../../../../../core/users/models/user.model';

@Component({
  selector: 'bgm-listing-groups-page',
  imports: [PaginationControls, CommonModule, AsyncPipe, ListingGroupSummaryCard],
  templateUrl: './listing-groups-page.html',
  styleUrl: './listing-groups-page.css',
})
export class ListingGroupsPage implements OnInit, AfterViewInit {
  readonly PAGE_SIZE = 8;
  private listingGroupsBehaviorSubject =
    new BehaviorSubject<PagedResult<ListingGroupSummary> | null>(null);
  listingGroups$ = this.listingGroupsBehaviorSubject.asObservable();

  cursorStack: (string | null)[] = [null];
  currentPageIndex = 0;

  user?: UserSummary;

  constructor(
    private listingGroupService: ListingGroupService,
    private authService: AuthStubService,
  ) {}

  ngOnInit(): void {
    this.loadPage(null);
    this.user = this.authService.loggedInUser;
  }

  ngAfterViewInit(): void {}

  loadPage(cursor: string | null) {
    this.listingGroupService.getListingGroupSummariesPage(this.PAGE_SIZE, cursor).subscribe({
      next: (data) => {
        this.listingGroupsBehaviorSubject.next(data);
      },
    });
  }

  previous_clicked() {
    if (this.currentPageIndex === 0) {
      return;
    }
    this.cursorStack.pop();
    this.currentPageIndex--;

    const cursor = this.cursorStack[this.currentPageIndex];
    this.loadPage(cursor);
  }

  next_clicked() {
    const page = this.listingGroupsBehaviorSubject.value;

    if (!page?.nextCursor) {
      return;
    }

    this.cursorStack.push(page.nextCursor);
    this.currentPageIndex++;

    this.loadPage(page.nextCursor);
  }
}
