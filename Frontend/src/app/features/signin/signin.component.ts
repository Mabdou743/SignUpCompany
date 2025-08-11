import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { AuthService } from '../../services/Auth.service';
import { ActivatedRoute, Router, RouterLink } from '@angular/router';
import { CommonModule } from '@angular/common';
import { ISignIn } from '../../types/ISignIn';
import { CookieService } from 'ngx-cookie-service';

@Component({
  selector: 'app-signin',
  templateUrl: './signin.component.html',
  styleUrls: ['./signin.component.css'],
  imports: [ReactiveFormsModule, CommonModule, RouterLink]
})
export class SigninComponent implements OnInit {
  signInForm: FormGroup
  submitted = false;
  successMessage = '';
  errorMessage = '';
  loading: boolean = false;
  
  constructor(private fb: FormBuilder, private authService: AuthService, private route: ActivatedRoute, private router: Router, private cookie: CookieService) {
    this.signInForm = this.fb.group({
      email: ['', [Validators.required]],
      password: ['', [Validators.required, Validators.minLength(6), Validators.pattern("^(?=.*[A-Z])(?=.*[0-9])(?=.*[\\W_]).{6,}$")]],
    })
  }

  ngOnInit() {
  }

  onSubmit(): void {
    this.loading = true;
    this.submitted = true;
    this.errorMessage = '';
    this.successMessage = '';

    const signInForm: ISignIn = {
      Email: this.signInForm.value.email,
      Password: this.signInForm.value.password,
    }

    this.authService.signin(signInForm).subscribe({
      next: (res) => {
        console.log(res.data.token);
        this.cookie.set('Token', res.data.token)
        this.successMessage = res.message || 'Login successfully!';
        this.loading = false;
        setTimeout(() => {
          this.router.navigate(['/home']);
        }, 1000)
      },
      error: (err) => {
        this.errorMessage = err.error.message || 'An error occurred while Sign In.';
        this.loading = false;
        if (this.errorMessage.includes("Sign Up")) {
          setTimeout(() => {
            this.router.navigate(['Sign Up'])
          }, 2000)
        } else if (this.errorMessage.includes("verifiy")) {
          setTimeout(() => {
            this.router.navigate(['verify-otp', this.signInForm.value.email])
          }, 2000)
        } else if (this.errorMessage.includes("password")) {
          setTimeout(() => {
            this.router.navigate(['set-password', this.signInForm.value.email])
          }, 2000)
        }
      }

    })
  }

}
