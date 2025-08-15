import { Component } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { AuthService } from '../../services/Auth.service';
import { ReactiveFormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { Router } from '@angular/router';
import { RouterLink } from '@angular/router';

@Component({
  selector: 'app-sign-up',
  templateUrl: './signup.component.html',
  styleUrls: ['./signup.component.css'],
  standalone: true,
  imports: [ReactiveFormsModule, CommonModule, RouterLink]
})
export class SignUpComponent {
  signUpForm: FormGroup;
  submitted = false;
  successMessage = '';
  errorMessage = '';
  loading = false;

  constructor(private fb: FormBuilder, private authService: AuthService, private router: Router ) {
    this.signUpForm = this.fb.group({
      arabicName: ['', Validators.required],
      englishName: ['', Validators.required],
      email: ['', [Validators.required, Validators.email]],
      phoneNumber: [''],
      websiteUrl: ['']
    });
  }

  ngOnInit() {
  }

  onSubmit(): void {
    this.submitted = true;
    this.errorMessage = ''; 
    this.loading = true;

    if (this.signUpForm.invalid) {
      this.loading = false;
      return;
    }

    const signupData = {
      ArabicName: this.signUpForm.value.arabicName,
      EnglishName: this.signUpForm.value.englishName,
      Email: this.signUpForm.value.email,
      PhoneNumber: this.signUpForm.value.phoneNumber,
      WebsiteUrl: this.signUpForm.value.websiteUrl
    };

    this.authService.signup(signupData).subscribe({
      next: (res) => {
        console.log("Message:", res.message);
        this.successMessage = res.message
        this.loading = false;
        
        setTimeout(() => {
          this.router.navigate(['/verify-otp', this.signUpForm.value.email]);
        }, 3000);
      },
      error: (err) => {
        this.errorMessage = err.error?.message;
        this.loading = false;
      }
    });
  }
}
