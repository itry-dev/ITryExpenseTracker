# ITryExpenseTracker

A simple expense tracker netcore Api project written in C#.  
## How to get started
1. fill the **ConnectionStrings:DefaultConnection** value in appsettings.json  
```
"ConnectionStrings": {
    "DefaultConnection": "--your database connection string here"
  }
```
2. fill the **Jwt** section values
```
"Jwt": {
    "Key": "--your top secret password here--",
    "Issuer": "--the issuer here--",
    "Audience": "--the audience here--"
  }
```
3. fill the **EmailConfiguration** values (for password recovery function)
```
"EmailConfiguration": {
    "SmtpServer": "--smtp server here--",
    "SmtpUser": "--user here--",
    "SmtpPassword": "--password here--",
    "SmtpPort": 25,
    "MailFrom": "--the mail sender here--"
  }
```
4. run migrations using the  
**[ITryExpenseTracker.Migrations](ITryExpenseTracker.Migrations/readme.md)**  

Use the User Authentication Api to login  
**[ITryeExpenseTracker.Core.Authentication](ITryExpenseTracker.Core.Authentication/readme.md)**  

Use the User Api to handle user data
**[ITryExpenseTracker.User.Api](ITryExpenseTracker.User.Api/readme.md)**

Use the main project Api for: 
- Expense: CRUD
- Supplier: CRUD
- Category: get categorie 
- Category (require admin role): Add,Update,Delete