import { Component, OnInit } from '@angular/core';
import { HomeService } from '../../services/Home.service';
import { IcompanyData } from '../../types/IcompanyData';
import { Router } from '@angular/router';
import { AuthService } from '../../services/Auth.service';
import { environment } from '../../../environments/environment';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-homePage',
  templateUrl: './homePage.component.html',
  styleUrls: ['./homePage.component.css'],
  imports: [CommonModule]
})
export class HomePageComponent implements OnInit {
  homeData?: IcompanyData;
  LogoSrc: string = '';
  successMessage = '';
  errorMessage = '';
  email: string = '';
  selectedFile?: File;
  logoPreviewUrl?: string | ArrayBuffer | null;

  constructor(
    private homeService: HomeService,
    private authService: AuthService,
  ) {}

  ngOnInit() {
    this.email = this.authService.getEmail() || '';
    this.loadHomeData();
  }

  loadHomeData() {
    this.homeService.homedata(this.email).subscribe({
      next: (res) => {
        this.homeData = res.data;
        if (this.homeData?.logoFileName) {
          this.LogoSrc = `${environment.apiUrl}/Images/${this.homeData.logoFileName}`;
        } else {
          this.LogoSrc = 'public/images/default-logo.png';
        }
        this.successMessage = res.message || '';
        this.errorMessage = '';
      },
      error: (err) => {
        this.errorMessage = err.error?.message || 'Failed to load company data.';
        this.successMessage = '';
      }
    });
  }

  onFileSelected(event: any): void {
    const file: File = event.target.files[0];
    if (file) {
      this.selectedFile = file;
      this.previewLogo(file);
      this.errorMessage = '';
      this.successMessage = '';
    }
  }

  uploadLogo(): void {
    if (!this.selectedFile) return;

    const formData = new FormData();
    formData.append('logo', this.selectedFile);

    const companyId = this.authService.getId();
    if (!companyId) {
      this.errorMessage = 'Invalid company token.';
      return;
    }
    formData.append('companyId', companyId);

    this.homeService.uploadLogo(formData).subscribe({
      next: (res) => {
        this.successMessage = res.message || 'Logo uploaded successfully!';
        this.errorMessage = '';
        this.resetPreview();
        this.loadHomeData();
      },
      error: (err) => {
        this.errorMessage = err.error?.message || 'Failed to upload logo.';
        this.successMessage = '';
      }
    });
  }
  
  previewLogo(file: File): void {
    const reader = new FileReader();
    reader.onload = () => {
      this.logoPreviewUrl = reader.result;
    };
    reader.readAsDataURL(file);
  }

  resetPreview(): void {
    this.selectedFile = undefined;
    this.logoPreviewUrl = null;
    this.errorMessage = '';
    const fileInput = document.querySelector('input[type="file"]') as HTMLInputElement;
    if (fileInput) fileInput.value = '';
  }

  logout() {
    this.authService.signout();
  }
}
