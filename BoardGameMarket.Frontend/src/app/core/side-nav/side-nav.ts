import { Component } from '@angular/core';
import { MatButtonModule } from '@angular/material/button';
import { RouterLink } from "@angular/router";

@Component({
  selector: 'bgm-side-nav',
  imports: [MatButtonModule, RouterLink],
  templateUrl: './side-nav.html',
  styleUrl: './side-nav.css',
})
export class SideNav {

}
