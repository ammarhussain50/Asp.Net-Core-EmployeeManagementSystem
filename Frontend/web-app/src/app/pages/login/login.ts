import { Component, inject } from '@angular/core';
import { Auth } from '../../services/auth';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { LucideAngularModule, Eye, EyeOff, EyeClosed } from 'lucide-angular';
import { CommonModule } from '@angular/common';
import { Router } from '@angular/router';

@Component({
  selector: 'app-login',
  imports: [CommonModule, ReactiveFormsModule,LucideAngularModule],
  templateUrl: './login.html',
  styleUrl: './login.css'
})
export class Login {

  authservice = inject(Auth)
  router = inject(Router)
  fb = inject(FormBuilder)
  hide = true;
  loginForm! : FormGroup
   readonly eye = Eye;
  readonly eyeClosed = EyeClosed;

  ngOnInit() : void {
    this.loginForm = this.fb.group({
      Email: ['', [Validators.required, Validators.email]], // required + valid email
      Password: ['', [Validators.required, Validators.minLength(6)]] // required + min 6 chars
    });

    if (this.authservice.isLoggedIn) {
      this.router.navigate(['/']);
    }

  }

  login(){
    this.authservice.login(this.loginForm.value.Email,this.loginForm.value.Password).subscribe((results)=>{
      console.log(results);
      this.authservice.SaveToken(results)
       alert('Login successful!');
      if(results.role === 'Admin'){
          this.router.navigate(['/']);
        }else{
          this.router.navigate(['/employee-dashboard']);
        }
      
    })
  }



}






