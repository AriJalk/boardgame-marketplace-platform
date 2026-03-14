import { AfterViewInit, Component, OnInit } from '@angular/core';
import { PaginationControls } from '../../../../../common/pagination-controls/pagination-controls';
import { ListingGroupService } from '../../../services/listing-group.service';
import { ListingGroupSummary } from '../../../models/listing-group.model';
import { AsyncPipe, CommonModule } from '@angular/common';
import { BehaviorSubject, Observable } from 'rxjs';
import { ListingGroupSummaryCard } from '../listing-group-summary-card/listing-group-summary-card';
import { PagedResult } from '../../../../../common/paged-result';
import { UserSummary } from '../../../../../core/users/models/user.model';
import { AuthService } from '../../../../../core/auth/auth.service';
import { AccountInfo } from '@azure/msal-browser';
import { UsersService } from '../../../../users/services/users-service';

@Component({
  selector: 'bgm-listing-groups-page',
  imports: [PaginationControls, CommonModule, AsyncPipe, ListingGroupSummaryCard, AsyncPipe],
  templateUrl: './listing-groups-page.html',
  styleUrl: './listing-groups-page.css',
})
export class ListingGroupsPage implements OnInit {
  readonly PAGE_SIZE = 8;
  private listingGroupsBehaviorSubject =
    new BehaviorSubject<PagedResult<ListingGroupSummary> | null>(null);
  listingGroups$ = this.listingGroupsBehaviorSubject.asObservable();

  cursorStack: (string | null)[] = [null];
  currentPageIndex = 0;

  user$?: Observable<UserSummary | undefined>;

  private topElement: Element | null = null;

  constructor(
    private listingGroupService: ListingGroupService,
    private usersService: UsersService,
  ) {}

  ngOnInit() {
    this.user$ = this.usersService.loggedUser$;

    this.loadPage(null);
  }

  private async loadLoggedUser() {}

  async loadPage(cursor: string | null) {
    this.listingGroupService.getListingGroupSummariesPage(this.PAGE_SIZE, cursor).subscribe({
      next: (data) => {
        this.listingGroupsBehaviorSubject.next(data);
        if (!this.topElement) {
          this.topElement = document.getElementById('page-top');
        }
        this.topElement?.scrollIntoView();
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
