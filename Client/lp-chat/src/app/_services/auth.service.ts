import { Injectable } from '@angular/core';
import { UserForRegister } from '../_models/user-for-register';
import { HttpClient } from '@angular/common/http';
import { environment } from 'src/environments/environment';
import { UserForLogin } from '../_models/user-for-login';
import { map } from 'rxjs/operators';
import { JwtHelperService } from '@auth0/angular-jwt';

@Injectable({
  providedIn: 'root'
})
export class AuthService {
  baseUrl = environment.apiUrl;
  jwtHelper = new JwtHelperService();

  constructor(private http: HttpClient) { }

  register(user: UserForRegister) {
    return this.http.post(this.baseUrl + '/auth/register', user);
  }

  login(user: UserForLogin) {
    return this.http.post(this.baseUrl + '/auth/login', user)
      .pipe(
        map((response: any) => {
          const user = response;
          if (user) {
            localStorage.setItem('token', user.payload);
          }
        }));
  }

  loggedIn() {
    const token = localStorage.getItem('token');
    return !this.jwtHelper.isTokenExpired(token);
  }
}
