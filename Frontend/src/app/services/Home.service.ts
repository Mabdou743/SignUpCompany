import { Injectable } from '@angular/core';
import { ApiResponse } from '../types/ApiResponse';
import { IcompanyData } from '../types/IcompanyData';
import { Observable } from 'rxjs';
import { HttpClient, HttpParams } from '@angular/common/http';
import { environment } from '../../environments/environment';

@Injectable({
  providedIn: 'root'
})
export class HomeService {
  private apiUrl = `${environment.apiUrl}`;

  constructor(
    private http: HttpClient, 
  ) { }

  homedata(email: string): Observable<ApiResponse<IcompanyData>> {
    const params = new HttpParams().set('email',email)
    return this.http.get<ApiResponse<IcompanyData>>(
      `${this.apiUrl}/api/company/home`,{params}
    )
  }

  uploadLogo(formData: FormData): Observable<ApiResponse<null>> {  
    return this.http.post<ApiResponse<null>>(
      `${this.apiUrl}/api/company/upload-logo`,
      formData,
    );
  }
}
