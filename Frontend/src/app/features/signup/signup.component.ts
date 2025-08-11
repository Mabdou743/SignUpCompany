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
  selectedLogo: File | null = null;
  logoPreview: string | null = null;

  constructor(private fb: FormBuilder, private authService: AuthService, private router: Router ) {
    this.signUpForm = this.fb.group({
      arabicName: ['', Validators.required],
      englishName: ['', Validators.required],
      email: ['', [Validators.required, Validators.email]],
      phoneNumber: [''],
      websiteUrl: [''],
      logo: [null]
    });
  }
  ngOnInit() {
  }

  onLogoChange(event: any): void {
    this.selectedLogo = event.target.files[0] ?? null;
    if (this.selectedLogo) {
      this.logoPreview = URL.createObjectURL(this.selectedLogo);
    } else {
      this.logoPreview = null;
    }
  }

  onSubmit(): void {
    this.submitted = true;
    this.errorMessage = ''; 
    this.loading = true;

    if (this.signUpForm.invalid) {
      this.loading = false;
      return;
    }

    const formData = new FormData();
    formData.append('ArabicName', this.signUpForm.value.arabicName);
    formData.append('EnglishName', this.signUpForm.value.englishName);
    formData.append('Email', this.signUpForm.value.email);
    formData.append('PhoneNumber', this.signUpForm.value.phoneNumber);
    formData.append('WebsiteUrl', this.signUpForm.value.websiteUrl);
    if (this.selectedLogo) {
      formData.append('Logo', this.selectedLogo);
    }

    this.authService.signup(formData).subscribe({
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
