import { Component, OnInit } from '@angular/core';
import { FormGroup, FormControl, Validators } from '@angular/forms';
import { AuthService } from 'src/app/_services/auth.service';
import { ToastrService } from 'ngx-toastr';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css']
})
export class LoginComponent implements OnInit {
  loginForm = new FormGroup({
    username: new FormControl('', Validators.required),
    password: new FormControl('', Validators.required)
  });

  constructor(private auth: AuthService, private toastr: ToastrService) { }

  ngOnInit() {
  }

  onSubmit() {
    if (this.loginForm.valid) {
      let user = this.loginForm.value;
      this.loginForm.disable();

      this.auth.login(this.loginForm.value).subscribe(
        next => this.toastr.success('Logged in'),
        error => { 
          this.toastr.error('Username or password do not match');
          this.loginForm.enable();
        },
        () => console.log('redirect...')
      );
    }
  }

}
