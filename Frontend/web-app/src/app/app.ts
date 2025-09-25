import { Component, inject, signal, ViewChild } from '@angular/core';
import { RouterLink, RouterOutlet } from '@angular/router';
import { MatToolbarModule } from '@angular/material/toolbar';
import { MatIconModule } from '@angular/material/icon';
import { MatButtonModule } from '@angular/material/button';
import { MatSidenavModule, MatDrawer } from '@angular/material/sidenav';
import { Auth } from './services/auth';

@Component({
  selector: 'app-root',
  imports: [
    RouterOutlet,
    RouterLink,
    MatToolbarModule,
    MatIconModule,
    MatButtonModule,
    MatSidenavModule,
  ],
  templateUrl: './app.html',
  styleUrl: './app.css',
})
export class App {
  authservice = inject(Auth);

  @ViewChild('drawer') drawer!: MatDrawer;

  logout() {
    this.authservice.logout();
    if (this.drawer) {
      this.drawer.close();
    }
  }
}
