import { HttpClient } from '@angular/common/http';
import { inject, Injectable, signal } from '@angular/core';
import { environment } from '../../environments/environment';
import { LoginTokenDto } from '../types/auth';
import { Router } from '@angular/router';

@Injectable({
  providedIn: 'root',
})
export class Auth {
  http = inject(HttpClient);
  router = inject(Router);

  // 👇 reactive signal state
  // IsLoggedIn = signal<boolean>(false);

  constructor() {
    // app load hote hi check karo user logged in hai ya nahi
    const token = localStorage.getItem('token');
    // this.IsLoggedIn.set(!!token);
  }

  login(Email: string, Password: string) {
    return this.http.post<LoginTokenDto>(environment.apiUrl + '/api/Auth/login', { Email, Password });
  }

  SaveToken(token: LoginTokenDto) {
    localStorage.setItem('auth', JSON.stringify(token));
    localStorage.setItem('token', token.token); // 👈 dhyaan: backend "Token" return karta hai (capital T)
    // this.IsLoggedIn.set(true); // 👈 state update
  }

  get isLoggedIn() {
  return localStorage.getItem('token') ? true : false;
}
get isEmployee() {
  return JSON.parse(localStorage.getItem('auth')!)?.role === 'Employee';
}

get AuthDetail(): LoginTokenDto | null {
  if (!this.isLoggedIn) return null;
  let token: LoginTokenDto = JSON.parse(localStorage.getItem('auth')!);
  return token;
}

getProfile(){
  return this.http.get(environment.apiUrl + '/api/Profile/Profile');
}

  logout() {
    localStorage.removeItem('auth');
    localStorage.removeItem('token');
    // this.IsLoggedIn.set(false); // 👈 state update
    this.router.navigateByUrl('/login');
  }
}




