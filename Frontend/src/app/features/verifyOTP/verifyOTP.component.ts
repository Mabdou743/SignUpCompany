import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { AuthService } from '../../services/Auth.service';
import { CommonModule } from '@angular/common';
import { ActivatedRoute, Router } from '@angular/router';
import { IVerifyOtp } from '../../types/IVerifyOtp';

@Component({
  selector: 'app-verifyOTP',
  templateUrl: './verifyOTP.component.html',
  styleUrls: ['./verifyOTP.component.css'],
  standalone: true,
  imports: [ReactiveFormsModule, CommonModule]
})
export class VerifyOTPComponent implements OnInit {
  verifyOTPForm: FormGroup;
  email: string = "";
  submitted = false;
  errorMessage = '';
  successMessage = '';
  loading = false;

  constructor(private fb: FormBuilder, private authService: AuthService, private route: ActivatedRoute, private router: Router) {
    this.verifyOTPForm = this.fb.group({
      otp: ['', [Validators.required, Validators.minLength(6), Validators.maxLength(6)]],
    });
  }

  ngOnInit() {
    const emailParam = this.route.snapshot.paramMap.get('email');
    this.email = emailParam ? emailParam : '';
  }

  onSubmit(): void {
    this.submitted = true;
    this.errorMessage = '';
    this.successMessage = '';
    this.loading = true;

    if (this.verifyOTPForm.invalid){
      this.loading = false;
      return;
    }
    

    const verifyOtpData: IVerifyOtp = {
      Email: this.email,
      OTP: this.verifyOTPForm.value.otp
    };

    this.authService.verifyOtp(verifyOtpData).subscribe({
      next: (res) => {
        console.log(res.message);
        this.successMessage = res.message;
        this.loading = false;

        setTimeout(() => {
          this.router.navigate(['/set-password', this.email]);
        }, 3000);
      },
      error: (err) => {
        this.errorMessage = err.error.message;
        this.loading = false;
      }
    });
  }
}