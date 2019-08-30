import { Injectable } from '@angular/core';
import { UserForRegister } from '../_models/user-for-register';

@Injectable({
  providedIn: 'root'
})
export class AuthService {

  constructor() { }

  register(user: UserForRegister) {
  }
}
