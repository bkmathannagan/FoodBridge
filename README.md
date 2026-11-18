# FoodBridge – Food Rescue and Donation Management System

> **"Save Food. Share Hope. Feed Lives."**

A complete, responsive, three-tier database-driven web application built using **ASP.NET Web Forms**, **VB.NET (.NET Framework 4.0)**, and **Microsoft Access (`App_Data/Food.mdb`)** via `OleDbConnection`.

---

## 1. Technology Stack

| Component | Technology Used |
| :--- | :--- |
| **Front-End & Back-End** | ASP.NET Web Forms (`.aspx`, `.master`, `.asax`) |
| **Programming Language** | Visual Basic .NET (`VB.NET`) |
| **IDE & Framework** | Visual Studio 2010 / `.NET Framework 4.0` |
| **Database** | Microsoft Access (`App_Data/Food.mdb`) |
| **Database Connectivity** | `System.Data.OleDb` (`OleDbConnection`, `OleDbCommand`, `OleDbDataAdapter`, `OleDbTransaction`) |
| **Architecture** | Strict Three-Tier Architecture (`Presentation Layer`, `BLL`, `DAL`, `Models`) |
| **Password Security** | SHA-256 Cryptographic Hashing (`SHA256Managed`) |
| **Reports** | ASP.NET `GridView` with Print & Export to Excel (`.xls`) |

---

## 2. Default Testing Accounts

All four default testing accounts are pre-populated in `App_Data/Food.mdb` (and can also be auto-filled with one click on the `Account/Login.aspx` page):

| Role | Email Address | Password | Default Landing Dashboard |
| :--- | :--- | :--- | :--- |
| **Administrator** | `admin@foodbridge.com` | `FoodBridge@123` | `~/Admin/Dashboard.aspx` |
| **Food Donor** | `donor@foodbridge.com` | `FoodBridge@123` | `~/Donor/Dashboard.aspx` |
| **NGO / Receiver** | `ngo@foodbridge.com` | `FoodBridge@123` | `~/NGO/Dashboard.aspx` |
| **Volunteer** | `volunteer@foodbridge.com` | `FoodBridge@123` | `~/Volunteer/Dashboard.aspx` |

---

## 3. Project Folder Structure (Three-Tier Architecture)

```text
FoodBridge/
├── Admin/
│   ├── Dashboard.aspx / .vb
│   ├── ManageDonors.aspx / .vb
│   ├── ManageNGOs.aspx / .vb
│   ├── ManageVolunteers.aspx / .vb
│   ├── ManageDonations.aspx / .vb
│   ├── ManageRequests.aspx / .vb
│   ├── AssignVolunteers.aspx / .vb
│   ├── ManageCategories.aspx / .vb
│   ├── Reports.aspx / .vb
│   └── Settings.aspx / .vb
├── Donor/
│   ├── Dashboard.aspx / .vb
│   ├── Profile.aspx / .vb
│   ├── AddDonation.aspx / .vb
│   ├── MyDonations.aspx / .vb
│   └── DonationDetails.aspx / .vb
├── NGO/
│   ├── Dashboard.aspx / .vb
│   ├── Profile.aspx / .vb
│   ├── AvailableFood.aspx / .vb
│   ├── FoodRequest.aspx / .vb
│   └── RequestHistory.aspx / .vb
├── Volunteer/
│   ├── Dashboard.aspx / .vb
│   ├── MyTasks.aspx / .vb
│   ├── TaskDetails.aspx / .vb
│   └── UpdateDelivery.aspx / .vb
├── Account/
│   ├── Login.aspx / .vb
│   ├── RegisterDonor.aspx / .vb
│   ├── RegisterNGO.aspx / .vb
│   ├── RegisterVolunteer.aspx / .vb
│   ├── ForgotPassword.aspx / .vb
│   └── Logout.aspx / .vb
├── BLL/
│   ├── DonorBLL.vb
│   ├── NGOBLL.vb
│   ├── VolunteerBLL.vb
│   ├── DonationBLL.vb
│   ├── RequestBLL.vb
│   ├── AssignmentBLL.vb
│   └── ReportBLL.vb
├── DAL/
│   ├── DBConnection.vb
│   ├── DonorDAL.vb
│   ├── NGODAL.vb
│   ├── VolunteerDAL.vb
│   ├── DonationDAL.vb
│   ├── RequestDAL.vb
│   ├── AssignmentDAL.vb
│   └── ReportDAL.vb
├── Models/
│   ├── Donor.vb
│   ├── NGO.vb
│   ├── Volunteer.vb
│   ├── FoodDonation.vb
│   ├── FoodRequest.vb
│   └── Entities.vb
├── App_Data/
│   ├── Food.mdb
│   └── DatabaseCreator.vb
├── Content/
│   ├── css/site.css
│   ├── js/site.js
│   └── images/
├── Uploads/
│   ├── Food/
│   └── Profiles/
├── Site.Master / .vb
├── Default.aspx / .vb
├── About.aspx / .vb
├── HowItWorks.aspx / .vb
├── Contact.aspx / .vb
├── Global.asax / .vb
├── Web.config
├── Robots.txt
├── Sitemap.xml
├── FoodBridge.vbproj
└── FoodBridge.sln
```

---

## 4. Database Schema (`App_Data/Food.mdb`)

All 16 MS Access tables are created with `AutoNumber` (`COUNTER`) Primary Keys, Foreign Key relationships, and search indexes:
1. `Admin`
2. `Donors`
3. `NGOs`
4. `Volunteers`
5. `FoodCategories`
6. `FoodDonations`
7. `FoodRequests`
8. `VolunteerAssignments`
9. `DonationTransactions`
10. `FoodDelivery`
11. `Feedback`
12. `Complaints`
13. `Announcements`
14. `Cities`
15. `UserLogin`
16. `PasswordReset`

---

## 5. IIS Deployment Instructions

1. **Open IIS Manager**:
   - Press `Win + R`, type `inetmgr`, and press Enter.
2. **Configure Application Pool (32-Bit Compatibility for MS Access Jet OLEDB 4.0)**:
   - In IIS Manager, click **Application Pools** -> **Add Application Pool**:
     - **Name**: `FoodBridgeAppPool`
     - **.NET Framework Version**: `.NET Framework v4.0.30319`
     - **Managed Pipeline Mode**: `Integrated` (or `Classic`)
   - Right-click `FoodBridgeAppPool` -> **Advanced Settings**:
     - Set **Enable 32-Bit Applications** = `True` (Mandatory for `Microsoft.Jet.OLEDB.4.0`).
3. **Add Application / Website**:
   - Right-click **Default Web Site** -> **Add Application**:
     - **Alias**: `FoodBridge`
     - **Application pool**: `FoodBridgeAppPool`
     - **Physical path**: `E:\BCA Projects\FoodBridge`
4. **Folder Permissions**:
   - Right-click `E:\BCA Projects\FoodBridge\App_Data` and `E:\BCA Projects\FoodBridge\Uploads` in Windows Explorer -> **Properties** -> **Security**.
   - Grant **Modify** and **Write** permissions to `IIS_IUSRS` and `NETWORK SERVICE` so MS Access can create `.ldb` lock files and users can upload food images.
5. **Run in Visual Studio 2010**:
   - Open `FoodBridge.sln` in Visual Studio 2010 and press `F5` (uses the built-in 32-bit Visual Studio Development Server `WebDev.WebServer40.EXE`).

---

## 6. Database Backup, Restore, and Reset Instructions

- **Backup**:
  - Copy `App_Data\Food.mdb` to a secure backup location (e.g., `Backups\Food_YYYYMMDD.mdb`) when no active write transactions are running.
- **Restore**:
  - Replace `App_Data\Food.mdb` with your backup `.mdb` file and ensure `IIS_IUSRS` has Read/Write permissions on both `App_Data` and `Food.mdb`.
- **Re-initialize Clean Database from Script**:
  - Compile and run `App_Data\DatabaseCreator.vb`:
    ```powershell
    C:\Windows\Microsoft.NET\Framework\v4.0.30319\vbc.exe /platform:x86 /out:App_Data\DatabaseCreator.exe App_Data\DatabaseCreator.vb
    .\App_Data\DatabaseCreator.exe
    ```
