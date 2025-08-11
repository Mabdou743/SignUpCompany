import { ComponentFixture, TestBed } from '@angular/core/testing';
import { ReactiveFormsModule } from '@angular/forms';
import { ActivatedRoute } from '@angular/router';
import { Router } from '@angular/router';
import { of } from 'rxjs';

import { SetpasswordComponent } from './setpassword.component';
import { AuthService } from '../../services/Auth.service';

describe('SetpasswordComponent', () => {
  let component: SetpasswordComponent;
  let fixture: ComponentFixture<SetpasswordComponent>;
  let mockAuthService: jasmine.SpyObj<AuthService>;
  let mockRouter: jasmine.SpyObj<Router>;

  beforeEach(async () => {
    const authServiceSpy = jasmine.createSpyObj('AuthService', ['setPassword']);
    const routerSpy = jasmine.createSpyObj('Router', ['navigate']);

    await TestBed.configureTestingModule({
      imports: [SetpasswordComponent, ReactiveFormsModule],
      providers: [
        { provide: AuthService, useValue: authServiceSpy },
        { provide: Router, useValue: routerSpy },
        {
          provide: ActivatedRoute,
          useValue: {
            snapshot: {
              paramMap: {
                get: () => 'test@example.com'
              }
            }
          }
        }
      ]
    })
    .compileComponents();

    mockAuthService = TestBed.inject(AuthService) as jasmine.SpyObj<AuthService>;
    mockRouter = TestBed.inject(Router) as jasmine.SpyObj<Router>;
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(SetpasswordComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });

  it('should initialize with email from route params', () => {
    expect(component.email).toBe('test@example.com');
  });

  it('should validate password requirements', () => {
    const passwordControl = component.setPasswordForm.get('password');
    
    passwordControl?.setValue('weak');
    expect(passwordControl?.invalid).toBeTruthy();
    
    passwordControl?.setValue('StrongPass1!');
    expect(passwordControl?.valid).toBeTruthy();
  });

  it('should validate password match', () => {
    component.setPasswordForm.patchValue({
      password: 'StrongPass1!',
      confirmPassword: 'DifferentPass1!'
    });
    
    expect(component.setPasswordForm.errors?.['passwordMatchValidator']).toBeTruthy();
    
    component.setPasswordForm.patchValue({
      password: 'StrongPass1!',
      confirmPassword: 'StrongPass1!'
    });
    
    expect(component.setPasswordForm.errors).toBeNull();
  });
});
