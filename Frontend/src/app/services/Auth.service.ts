import { inject, Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { environment } from '../../environments/environment';
import { Observable } from 'rxjs';
import { ApiResponse } from '../types/ApiResponse';
import { IVerifyOtp } from '../types/IVerifyOtp';
import { ISetPassword } from '../types/IsetPassword';
import { ISignIn } from '../types/ISignIn';
import { ILoginResponseData } from '../types/ILoginResponseData';
import { CookieService } from 'ngx-cookie-service';
import { Router } from '@angular/router';
import { IcompanyData } from '../types/IcompanyData';

@Injectable({ providedIn: 'root' })
export class AuthService {
    private apiUrl = `${environment.apiUrl}`;
    private cookie = inject(CookieService);

    constructor(private http: HttpClient, private router: Router) { }

    getAuthToken() {
        return this.cookie.get('Token') || '';
    }

    isAuthenticated(): boolean {
        const token = this.getAuthToken();
        return !!token
    }

    getEmail() {
        const Token = this.getAuthToken();
        const tokenparts = Token.split('.');

        if (tokenparts.length !== 3)
            return null

        const payload = JSON.parse(atob(tokenparts[1]))
        console.log("Payload: ", payload);
        return payload[
            'http://schemas.xmlsoap.org/ws/2005/05/identity/claims/emailaddress'
        ];
    }

    signup(data: FormData): Observable<ApiResponse<null>> {
        return this.http.post<ApiResponse<null>>(
            `${this.apiUrl}/api/Company/sign-up`, data
        );
    }

    verifyOtp(data: IVerifyOtp): Observable<ApiResponse<null>> {
        return this.http.post<ApiResponse<null>>(
            `${this.apiUrl}/api/Company/verify-otp`, data
        );
    }

    setPassword(data: ISetPassword): Observable<ApiResponse<null>> {
        return this.http.post<ApiResponse<null>>(
            `${this.apiUrl}/api/Company/set-password`, data
        );
    }

    signin(data: ISignIn): Observable<ApiResponse<ILoginResponseData>> {
        return this.http.post<ApiResponse<ILoginResponseData>>(
            `${this.apiUrl}/api/Company/sign-in`, data
        );
    }

    signout() {
        this.cookie.delete('Token');
        this.router.navigate(['/signin']);
    }


}