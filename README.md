# Company Sign-Up Portal

A web application where companies can register, verify their email via OTP, set a password, and log in to their account.  
Built with:
- **Backend:** ASP.NET Core Web API (Clean Architecture with 4 layers: Data, Repository, Services, API)
- **Frontend:** Angular 19
- **Database:** PostgreSQL

---

## Features

### 1. Sign Up as a Company
- Form fields:
  - **Company Arabic Name** (required)
  - **Company English Name** (required)
  - **Email Address** (required, valid, and unique)
  - **Phone Number** (optional, must be valid if provided)
  - **Website URL** (optional)
  - **Company Logo** (optional, preview before submission)
- After successful registration:
  - Email is sent to the provided address with an OTP.
  - Redirects to OTP validation page.

### 2. OTP Verification
- User enters the received OTP (shown in tooltip for test/demo).
- If valid, user proceeds to set password.

### 3. Set Password
- **New Password** and **Confirm Password** fields.
- Password rules:
  - At least one uppercase letter.
  - At least one special character.
  - At least one number.
  - Minimum length: 6 characters.
- If password is valid and OTP is still valid, it is saved for the company account.

### 4. Sign In
- User can log in with email and password.
- On success:
  - Redirect to home page showing:
    - Company Logo
    - Greeting: `Hello {Company Name}`
    - Logout button

---

## API Endpoints

### `POST /api/company/sign-up`
Registers a new company.

**Request (multipart/form-data):**
```json
{
  "ArabicName": "شركة الاختبار",
  "EnglishName": "Test Company",
  "Email": "test@example.com",
  "PhoneNumber": "+20123456789",
  "WebsiteUrl": "https://company.com",
  "Logo": "file.png"
}
