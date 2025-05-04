# ASP.NET MVC Project – Based on Figma Design

A full-stack ASP.NET Core MVC app with:
- User authentication (Identity + Google)
- Role-based access (Admin/Member)
- SignalR real-time notifications
- Cookie consent & dark mode
- Data validation (JavaScript + ModelState).

---
## 🔧 Setup & Database
- The admin is created in AdminSetup in Presentation
- Some seed data is added in `AppDbContext` using `OnModelCreating`.
- ⚠️ The `NotificationTypeEntity` is currently commented out. If the database is reset, it may need to be included in a future migration.
- The application uses **two database connections**:
  - One for Microsoft Identity (users)
  - One for the rest of the app’s data (projects, members, etc.)

---
## ✅ Try It Out

- Create a user by filling in the registration form. Log in to explore the site and view project details. You can see you "profile" in the top right. Then log out.
- Log in as admin:  
  **Email:** `admin@admin.com`  
  **Password:** `Admin123!`  
  As admin, you can create, update, and delete, and see **real-time notifications**.
- Create a member, connect it to a user (e.g., the one you just created), and log in as that user to get member access. A member can create projects, recieve messages when project is created.
- The **"Save" button** is intentionally left visible to demonstrate access control. It redirects to the admin login view if you're not authorized.

---
## 🍪 Cookie Consent
- On first load, the user is asked to allow cookies.
- A **dark mode cookie** is available if selected.

---
## 👥 Login & Roles
You can log in via:
- A **seeded admin account** (`admin@admin.com` / `Admin123!`)
- **Registering a new user**
- **Google sign-in** (OAuth)

> Login and signup modals are validated with **JavaScript** and **ModelState**.
> I have made some model properties to accept no input, and some have Required or more on them.  
> If the email is correct but the password is wrong, a generic error is shown ("Invalid login").
**Note:** The "Remember Me" option is currently not implemented.
---
## 🔐 Permissions
- **Regular users** can:
  - View and sort projects
  - View details
  - Receive real-time notifications when projects are created
  - Create projects (if connected to a member)
- **Admins** can:
  - Create, update, and delete projects, members, and clients
  - Change project status (On hold, Active, Completed)
  - Receive real-time notifications when members or projects are created

---
## ⚠️ Good to KNOW!
- I have left alot of console.log in my javascript to help with debugging. 
- I made some changes to the design for better visibility.  
- Deleting a member that's connected to a user can cause a **null reference error** (needs more logic).
- The **Client page is limited** – only the name can be entered.
- There are alot of things to improve on all fronts. 
- "Remember Me" is visible but not functional.
- the add member on edit is not implemented
