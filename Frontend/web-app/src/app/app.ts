import { Component, inject } from '@angular/core';
import { RouterLink, RouterOutlet } from '@angular/router';
import { Auth } from './services/auth';

@Component({
  selector: 'app-root',
  imports: [RouterOutlet, RouterLink],
  templateUrl: './app.html',
  styleUrl: './app.css',
})
export class App {
  authservice = inject(Auth);

  sidebarVisible: boolean | undefined;
  isSidebarOpen = true;

  toggleSidebar() {
    this.isSidebarOpen = !this.isSidebarOpen;
  }

  ngOnInit(): void {
    // default set
    if (!localStorage.getItem("modalOpen")) {
      localStorage.setItem("modalOpen", "false");
    }
    this.updateSidebar();

    // har 500ms me check karte rahenge localStorage
    setInterval(() => {
      this.updateSidebar();
    }, 500);
  }

  updateSidebar() {
    const modalOpen = localStorage.getItem("modalOpen");
    this.sidebarVisible = modalOpen !== "true";
  }

  logout() {
    this.authservice.logout();
  }
}








