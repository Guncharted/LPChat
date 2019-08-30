import { Component, OnInit, ViewChild, ElementRef, AfterViewInit } from '@angular/core';
import { NgForm, AbstractControl, FormControl, FormGroup, Validators, ValidatorFn } from '@angular/forms';
import { passwordMatch } from 'src/app/_directives/password-match.directive';
import { UserForRegister } from 'src/app/_models/user-for-register';
import { AuthService } from 'src/app/_services/auth.service';
import { ToastrService } from 'ngx-toastr';

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.css']
})
export class RegisterComponent implements OnInit {
  regForm = new FormGroup({
    username: new FormControl('', [
      Validators.required, 
      Validators.minLength(5), 
      Validators.maxLength(120), 
      Validators.pattern('^[a-zA-Z0-9]+$')]),

    firstName: new FormControl(''),
    lastName: new FormControl(''),
    password: new FormControl('', [
      Validators.required, 
      Validators.minLength(8), 
      Validators.maxLength(18),
      Validators.pattern(/^(?=\D*\d)(?=[^a-z]*[a-z])(?=[^A-Z]*[A-Z]).{8,18}$/)
    ]),
    confirmPassword: new FormControl('', [
      Validators.required, 
      Validators.minLength(8), 
      Validators.maxLength(18)
    ])
  }, { validators:  passwordMatch });

  

  constructor(private auth: AuthService, private toastr: ToastrService) { }

  ngOnInit() {
  }

  onSubmit() {
    let user: UserForRegister;

    if (this.regForm.valid) {
      user = this.regForm.value;
      this.regForm.disable();
      this.auth.register(user).subscribe((result: any) => {
        if (result.succeeded){
          this.toastr.success(result.title)
        }
      }, (er) => {
        this.toastr.error(er.error.title);
        this.regForm.enable();
      });
    }
  }
}
