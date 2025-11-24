import { Component, signal } from '@angular/core';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatSidenavModule } from '@angular/material/sidenav';
import { MatToolbarModule } from '@angular/material/toolbar';
import { RouterOutlet } from '@angular/router';
import { SideNav } from './core/side-nav/side-nav';
import { AddListingGroupPage } from "./features/listing-groups/components/creation/add-listing-group-page/add-listing-group-page";

@Component({
  selector: 'app-root',
  imports: [RouterOutlet, SideNav, MatSidenavModule, MatToolbarModule, MatIconModule, MatButtonModule,
    AddListingGroupPage],
  templateUrl: './app.html',
  styleUrl: './app.css'
})
export class App {
  protected readonly title = signal('frontend');
}
