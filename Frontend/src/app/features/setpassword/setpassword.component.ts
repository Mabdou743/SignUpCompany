import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { ISetPassword } from '../../types/IsetPassword';
import { CommonModule } from '@angular/common';
import { AuthService } from '../../services/Auth.service';

@Component({
  selector: 'app-setpassword',
  templateUrl: './setpassword.component.html',
  styleUrls: ['./setpassword.component.css'],
  standalone: true,
  imports: [ReactiveFormsModule, CommonModule]
})
export class SetpasswordComponent implements OnInit {
  setPasswordForm: FormGroup;
  email: string = "";
  submitted = false;
  successMessage = '';
  errorMessage = '';
  loading = false;

  constructor(
    private fb: FormBuilder,
    private authService: AuthService,
    private route: ActivatedRoute,
    private router: Router
  ) {
    this.setPasswordForm = this.fb.group({
      password: ['', [Validators.required, Validators.minLength(6), Validators.pattern("^(?=.*[A-Z])(?=.*[0-9])(?=.*[\\W_]).{6,}$")]],
      confirmPassword: ['', [Validators.required, Validators.minLength(6), Validators.pattern("^(?=.*[A-Z])(?=.*[0-9])(?=.*[\\W_]).{6,}$")]]
    },
      { validators: this.passwordMatchValidator }
    );
  }

  ngOnInit() {
    const emailParam = this.route.snapshot.paramMap.get('email');
    this.email = emailParam ? emailParam : '';
  }

  passwordMatchValidator(form: FormGroup) {
    const password = form.get('password')?.value;
    const confirmPassword = form.get('confirmPassword')?.value;
    if (password !== confirmPassword)
      return { passwordMatchValidator: true };

    return null;
  }

  onSubmit(): void {
    this.submitted = true;
    this.errorMessage = '';
    this.successMessage = '';
    this.loading = true;

    const setPasswordData: ISetPassword = {
      Email: this.email,
      Password: this.setPasswordForm.value.password,
      ConfirmPassword: this.setPasswordForm.value.confirmPassword
    }

    this.authService.setPassword(setPasswordData).subscribe({
      next: (res) => {
        this.successMessage = res.message || 'Password set successfully!';
        this.loading = false;

        setTimeout(() => {
          this.router.navigate(['/signin']);
        }, 3000)
      },
      error: (err) => {
        const allErrors: string[] = [];
        if (err.error?.errors) {
          for (const field in err.error.errors) {
            if (err.error.errors.hasOwnProperty(field)) {
              allErrors.push(...err.error.errors[field]);
            }
          }
        }
        this.errorMessage = allErrors[0] || 'An error occurred while setting password.';
        this.loading = false;
      }
    })
  }
}
