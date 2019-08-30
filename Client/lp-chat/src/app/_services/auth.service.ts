import { Injectable } from '@angular/core';
import { UserForRegister } from '../_models/user-for-register';
import { HttpClient } from '@angular/common/http';
import { environment } from 'src/environments/environment';

@Injectable({
  providedIn: 'root'
})
export class AuthService {
  baseUrl = environment.apiUrl;

  constructor(private http: HttpClient) { }

  register(user: UserForRegister) {
    return this.http.post(this.baseUrl + '/auth/register', user);
  }
}
