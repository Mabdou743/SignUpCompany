import { Routes } from '@angular/router';
import { SignUpComponent } from './features/signup/signup.component';
import { VerifyOTPComponent } from './features/verifyOTP/verifyOTP.component';
import { SetpasswordComponent } from './features/setpassword/setpassword.component';
import { SigninComponent } from './features/signin/signin.component';
import { HomePageComponent } from './features/homePage/homePage.component';
import { AuthGuard } from './guards/auth.guard';

export const routes: Routes = [
    { path: '', redirectTo: 'signup', pathMatch: 'full' },
    { path: 'signup', component: SignUpComponent },
    { path: 'verify-otp/:email', component: VerifyOTPComponent },
    { path: 'set-password/:email', component: SetpasswordComponent },
    { path: 'signin', component: SigninComponent },
    { path: 'home', component: HomePageComponent, canActivate: [AuthGuard] },
];
