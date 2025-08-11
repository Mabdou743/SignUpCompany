import { Component, OnInit } from '@angular/core';
import { HomeService } from '../../services/Home.service';
import { IcompanyData } from '../../types/IcompanyData';
import { Router } from '@angular/router';
import { AuthService } from '../../services/Auth.service';

@Component({
  selector: 'app-homePage',
  templateUrl: './homePage.component.html',
  styleUrls: ['./homePage.component.css']
})
export class HomePageComponent implements OnInit {
  homeData?: IcompanyData;
  LogoSrc: string='';
  successMessage = '';
  errorMessage = '';
  email: string = ''; 

  constructor(private homeService: HomeService, private authService: AuthService, private router: Router) { }

  ngOnInit() {
    this.email = this.authService.getEmail(); 
    this.homeService.homedata(this.email).subscribe({
      next: (res) => {
        console.log(res.data);
        this.homeData = res.data;
        
        if (this.homeData.logoFileName) {
          this.LogoSrc = `http://localhost:5139/logos/${this.homeData.logoFileName}`;
        } else {
          this.LogoSrc = 'public/default-logo.png';
        }
        this.successMessage = res.message || 'Data loaded successfully!';
      },
      error: (err) => {
        console.log("Error");
        const allErrors: string[] = [];
        if (err.error?.errors) {
          for (const field in err.error.errors) {
            if (err.error.errors.hasOwnProperty(field)) {
              allErrors.push(...err.error.errors[field]);
            }
          }
        }
        this.errorMessage = allErrors[0] || 'An error occurred while loading data.';
      }
    })
  }

  logout() {
    this.authService.signout();
  }
}
