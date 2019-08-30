import { Injectable } from '@angular/core';
import { UserForRegister } from '../_models/user-for-register';
import { HttpClient } from '@angular/common/http';
import { environment } from 'src/environments/environment';
import { UserForLogin } from '../_models/user-for-login';
import { map } from 'rxjs/operators';

@Injectable({
  providedIn: 'root'
})
export class AuthService {
  baseUrl = environment.apiUrl;

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
}
