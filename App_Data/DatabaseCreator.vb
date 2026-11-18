Imports System
Imports System.IO
Imports System.Data
Imports System.Data.OleDb
Imports System.Security.Cryptography
Imports System.Text

Module DatabaseCreator
    Public Function HashPassword(ByVal password As String) As String
        Using sha As New SHA256Managed()
            Dim bytes As Byte() = Encoding.UTF8.GetBytes(password.Trim())
            Dim hash As Byte() = sha.ComputeHash(bytes)
            Dim sb As New StringBuilder()
            For Each b As Byte In hash
                sb.Append(b.ToString("x2"))
            Next
            Return sb.ToString()
        End Using
    End Function

    Sub Main()
        Try
            Dim appDataDir As String = "e:\BCA Projects\FoodBridge\App_Data"
            If Not Directory.Exists(appDataDir) Then
                Directory.CreateDirectory(appDataDir)
            End If

            Dim mdbPath As String = Path.Combine(appDataDir, "Food.mdb")
            If File.Exists(mdbPath) Then
                File.Delete(mdbPath)
            End If

            Dim connStr As String = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" & mdbPath & ";Jet OLEDB:Engine Type=5;"
            Dim cat As Object = CreateObject("ADOX.Catalog")
            cat.Create(connStr)
            If cat.ActiveConnection IsNot Nothing Then
                cat.ActiveConnection.Close()
            End If
            cat = Nothing

            Console.WriteLine("Created empty Food.mdb successfully.")

            Using conn As New OleDbConnection("Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" & mdbPath & ";")
                conn.Open()

                ' 1. Admin Table
                ExecuteSQL(conn, "CREATE TABLE [Admin] (" & _
                    "[AdminID] COUNTER PRIMARY KEY, " & _
                    "[FullName] VARCHAR(100) NOT NULL, " & _
                    "[EmailAddress] VARCHAR(120) NOT NULL, " & _
                    "[MobileNumber] VARCHAR(20), " & _
                    "[PasswordHash] VARCHAR(128) NOT NULL, " & _
                    "[RoleTitle] VARCHAR(50), " & _
                    "[CreatedDate] DATETIME, " & _
                    "[LastLogin] DATETIME, " & _
                    "[IsActive] BIT)")
                ExecuteSQL(conn, "CREATE UNIQUE INDEX idx_Admin_Email ON [Admin] ([EmailAddress])")

                ' 2. Donors Table
                ExecuteSQL(conn, "CREATE TABLE [Donors] (" & _
                    "[DonorID] COUNTER PRIMARY KEY, " & _
                    "[DonorName] VARCHAR(150) NOT NULL, " & _
                    "[DonorType] VARCHAR(50) NOT NULL, " & _
                    "[ContactPerson] VARCHAR(100) NOT NULL, " & _
                    "[MobileNumber] VARCHAR(20) NOT NULL, " & _
                    "[EmailAddress] VARCHAR(120) NOT NULL, " & _
                    "[Address] VARCHAR(255) NOT NULL, " & _
                    "[City] VARCHAR(80) NOT NULL, " & _
                    "[PasswordHash] VARCHAR(128) NOT NULL, " & _
                    "[RegistrationDate] DATETIME, " & _
                    "[AccountStatus] VARCHAR(30) NOT NULL, " & _
                    "[ProfileImage] VARCHAR(255))")
                ExecuteSQL(conn, "CREATE UNIQUE INDEX idx_Donors_Email ON [Donors] ([EmailAddress])")
                ExecuteSQL(conn, "CREATE INDEX idx_Donors_City ON [Donors] ([City])")

                ' 3. NGOs Table
                ExecuteSQL(conn, "CREATE TABLE [NGOs] (" & _
                    "[NGOID] COUNTER PRIMARY KEY, " & _
                    "[OrganizationName] VARCHAR(150) NOT NULL, " & _
                    "[RegistrationNumber] VARCHAR(80) NOT NULL, " & _
                    "[ContactPerson] VARCHAR(100) NOT NULL, " & _
                    "[MobileNumber] VARCHAR(20) NOT NULL, " & _
                    "[EmailAddress] VARCHAR(120) NOT NULL, " & _
                    "[Address] VARCHAR(255) NOT NULL, " & _
                    "[City] VARCHAR(80) NOT NULL, " & _
                    "[NumberOfBeneficiaries] INTEGER, " & _
                    "[PasswordHash] VARCHAR(128) NOT NULL, " & _
                    "[RegistrationDate] DATETIME, " & _
                    "[VerificationStatus] VARCHAR(30) NOT NULL, " & _
                    "[ProfileImage] VARCHAR(255))")
                ExecuteSQL(conn, "CREATE UNIQUE INDEX idx_NGOs_Email ON [NGOs] ([EmailAddress])")
                ExecuteSQL(conn, "CREATE INDEX idx_NGOs_City ON [NGOs] ([City])")

                ' 4. Volunteers Table
                ExecuteSQL(conn, "CREATE TABLE [Volunteers] (" & _
                    "[VolunteerID] COUNTER PRIMARY KEY, " & _
                    "[VolunteerName] VARCHAR(100) NOT NULL, " & _
                    "[Gender] VARCHAR(20) NOT NULL, " & _
                    "[MobileNumber] VARCHAR(20) NOT NULL, " & _
                    "[EmailAddress] VARCHAR(120) NOT NULL, " & _
                    "[Address] VARCHAR(255) NOT NULL, " & _
                    "[City] VARCHAR(80) NOT NULL, " & _
                    "[AvailabilityStatus] VARCHAR(30) NOT NULL, " & _
                    "[PasswordHash] VARCHAR(128) NOT NULL, " & _
                    "[RegistrationDate] DATETIME, " & _
                    "[AccountStatus] VARCHAR(30) NOT NULL, " & _
                    "[ProfileImage] VARCHAR(255))")
                ExecuteSQL(conn, "CREATE UNIQUE INDEX idx_Volunteers_Email ON [Volunteers] ([EmailAddress])")
                ExecuteSQL(conn, "CREATE INDEX idx_Volunteers_City ON [Volunteers] ([City])")

                ' 5. FoodCategories Table
                ExecuteSQL(conn, "CREATE TABLE [FoodCategories] (" & _
                    "[CategoryID] COUNTER PRIMARY KEY, " & _
                    "[CategoryName] VARCHAR(80) NOT NULL, " & _
                    "[Description] VARCHAR(255), " & _
                    "[IsActive] BIT)")
                ExecuteSQL(conn, "CREATE UNIQUE INDEX idx_Category_Name ON [FoodCategories] ([CategoryName])")

                ' 6. Cities Table
                ExecuteSQL(conn, "CREATE TABLE [Cities] (" & _
                    "[CityID] COUNTER PRIMARY KEY, " & _
                    "[CityName] VARCHAR(80) NOT NULL, " & _
                    "[StateName] VARCHAR(80) NOT NULL, " & _
                    "[PINPrefix] VARCHAR(10), " & _
                    "[IsActive] BIT)")
                ExecuteSQL(conn, "CREATE UNIQUE INDEX idx_Cities_Name ON [Cities] ([CityName])")

                ' 7. FoodDonations Table
                ExecuteSQL(conn, "CREATE TABLE [FoodDonations] (" & _
                    "[DonationID] COUNTER PRIMARY KEY, " & _
                    "[DonorID] INTEGER NOT NULL, " & _
                    "[FoodName] VARCHAR(150) NOT NULL, " & _
                    "[FoodCategory] VARCHAR(80) NOT NULL, " & _
                    "[FoodDescription] MEMO, " & _
                    "[FoodType] VARCHAR(20) NOT NULL, " & _
                    "[FoodQuantity] DOUBLE NOT NULL, " & _
                    "[AvailableQuantity] DOUBLE NOT NULL, " & _
                    "[QuantityUnit] VARCHAR(30) NOT NULL, " & _
                    "[NumberOfPersonsServed] INTEGER, " & _
                    "[FoodPreparationDate] DATETIME, " & _
                    "[FoodPreparationTime] VARCHAR(20), " & _
                    "[ExpiryDate] DATETIME, " & _
                    "[ExpiryTime] VARCHAR(20), " & _
                    "[PickupAddress] VARCHAR(255) NOT NULL, " & _
                    "[City] VARCHAR(80) NOT NULL, " & _
                    "[PINCode] VARCHAR(15) NOT NULL, " & _
                    "[PreferredPickupDate] DATETIME, " & _
                    "[PreferredPickupTime] VARCHAR(20), " & _
                    "[SpecialInstructions] MEMO, " & _
                    "[FoodImage] VARCHAR(255), " & _
                    "[DonationDate] DATETIME, " & _
                    "[ApprovalStatus] VARCHAR(30) NOT NULL, " & _
                    "[CollectionStatus] VARCHAR(30) NOT NULL, " & _
                    "[AdminRemarks] VARCHAR(255), " & _
                    "CONSTRAINT fk_Donations_Donor FOREIGN KEY ([DonorID]) REFERENCES [Donors] ([DonorID]))")
                ExecuteSQL(conn, "CREATE INDEX idx_Donations_Category ON [FoodDonations] ([FoodCategory])")
                ExecuteSQL(conn, "CREATE INDEX idx_Donations_City ON [FoodDonations] ([City])")
                ExecuteSQL(conn, "CREATE INDEX idx_Donations_Status ON [FoodDonations] ([ApprovalStatus])")

                ' 8. FoodRequests Table
                ExecuteSQL(conn, "CREATE TABLE [FoodRequests] (" & _
                    "[RequestID] COUNTER PRIMARY KEY, " & _
                    "[NGOID] INTEGER NOT NULL, " & _
                    "[DonationID] INTEGER NOT NULL, " & _
                    "[RequestedQuantity] DOUBLE NOT NULL, " & _
                    "[NumberOfBeneficiaries] INTEGER NOT NULL, " & _
                    "[RequiredDate] DATETIME, " & _
                    "[DeliveryAddress] VARCHAR(255) NOT NULL, " & _
                    "[City] VARCHAR(80) NOT NULL, " & _
                    "[ContactPerson] VARCHAR(100) NOT NULL, " & _
                    "[ContactNumber] VARCHAR(20) NOT NULL, " & _
                    "[RequestRemarks] MEMO, " & _
                    "[RequestDate] DATETIME, " & _
                    "[RequestStatus] VARCHAR(30) NOT NULL, " & _
                    "[AdminRemarks] VARCHAR(255), " & _
                    "CONSTRAINT fk_Requests_NGO FOREIGN KEY ([NGOID]) REFERENCES [NGOs] ([NGOID]), " & _
                    "CONSTRAINT fk_Requests_Donation FOREIGN KEY ([DonationID]) REFERENCES [FoodDonations] ([DonationID]))")
                ExecuteSQL(conn, "CREATE INDEX idx_Requests_Status ON [FoodRequests] ([RequestStatus])")

                ' 9. VolunteerAssignments Table
                ExecuteSQL(conn, "CREATE TABLE [VolunteerAssignments] (" & _
                    "[TaskID] COUNTER PRIMARY KEY, " & _
                    "[DonationID] INTEGER NOT NULL, " & _
                    "[RequestID] INTEGER NOT NULL, " & _
                    "[VolunteerID] INTEGER NOT NULL, " & _
                    "[DonorAddress] VARCHAR(255) NOT NULL, " & _
                    "[ReceiverAddress] VARCHAR(255) NOT NULL, " & _
                    "[AssignedQuantity] DOUBLE NOT NULL, " & _
                    "[QuantityUnit] VARCHAR(30), " & _
                    "[AssignmentDate] DATETIME, " & _
                    "[CollectionDate] DATETIME, " & _
                    "[DeliveryDate] DATETIME, " & _
                    "[TaskStatus] VARCHAR(40) NOT NULL, " & _
                    "[CollectionRemarks] MEMO, " & _
                    "[DeliveryRemarks] MEMO, " & _
                    "CONSTRAINT fk_Assign_Donation FOREIGN KEY ([DonationID]) REFERENCES [FoodDonations] ([DonationID]), " & _
                    "CONSTRAINT fk_Assign_Request FOREIGN KEY ([RequestID]) REFERENCES [FoodRequests] ([RequestID]), " & _
                    "CONSTRAINT fk_Assign_Volunteer FOREIGN KEY ([VolunteerID]) REFERENCES [Volunteers] ([VolunteerID]))")
                ExecuteSQL(conn, "CREATE INDEX idx_Assign_Status ON [VolunteerAssignments] ([TaskStatus])")

                ' 10. DonationTransactions Table
                ExecuteSQL(conn, "CREATE TABLE [DonationTransactions] (" & _
                    "[TransactionID] COUNTER PRIMARY KEY, " & _
                    "[DonationID] INTEGER NOT NULL, " & _
                    "[RequestID] INTEGER, " & _
                    "[TaskID] INTEGER, " & _
                    "[ActionByRole] VARCHAR(30) NOT NULL, " & _
                    "[ActionByName] VARCHAR(100) NOT NULL, " & _
                    "[StatusFrom] VARCHAR(40), " & _
                    "[StatusTo] VARCHAR(40) NOT NULL, " & _
                    "[QuantityChanged] DOUBLE, " & _
                    "[Remarks] VARCHAR(255), " & _
                    "[TransactionDate] DATETIME)")
                ExecuteSQL(conn, "CREATE INDEX idx_Trans_Donation ON [DonationTransactions] ([DonationID])")

                ' 11. FoodDelivery Table
                ExecuteSQL(conn, "CREATE TABLE [FoodDelivery] (" & _
                    "[DeliveryID] COUNTER PRIMARY KEY, " & _
                    "[TaskID] INTEGER NOT NULL, " & _
                    "[DonationID] INTEGER NOT NULL, " & _
                    "[RequestID] INTEGER NOT NULL, " & _
                    "[VolunteerID] INTEGER NOT NULL, " & _
                    "[NGOID] INTEGER NOT NULL, " & _
                    "[DeliveredQuantity] DOUBLE NOT NULL, " & _
                    "[QuantityUnit] VARCHAR(30), " & _
                    "[ReceivedByPerson] VARCHAR(100), " & _
                    "[ReceiverContact] VARCHAR(20), " & _
                    "[DeliveryCondition] VARCHAR(50), " & _
                    "[DeliveryRemarks] MEMO, " & _
                    "[DeliveryTimestamp] DATETIME)")
                ExecuteSQL(conn, "CREATE UNIQUE INDEX idx_Delivery_Task ON [FoodDelivery] ([TaskID])")

                ' 12. Feedback Table
                ExecuteSQL(conn, "CREATE TABLE [Feedback] (" & _
                    "[FeedbackID] COUNTER PRIMARY KEY, " & _
                    "[UserRole] VARCHAR(30) NOT NULL, " & _
                    "[UserID] INTEGER, " & _
                    "[UserName] VARCHAR(100) NOT NULL, " & _
                    "[EmailAddress] VARCHAR(120) NOT NULL, " & _
                    "[Rating] INTEGER NOT NULL, " & _
                    "[Subject] VARCHAR(150) NOT NULL, " & _
                    "[Message] MEMO NOT NULL, " & _
                    "[SubmittedDate] DATETIME, " & _
                    "[AdminReply] MEMO, " & _
                    "[Status] VARCHAR(30) NOT NULL)")

                ' 13. Complaints Table
                ExecuteSQL(conn, "CREATE TABLE [Complaints] (" & _
                    "[ComplaintID] COUNTER PRIMARY KEY, " & _
                    "[UserRole] VARCHAR(30) NOT NULL, " & _
                    "[UserID] INTEGER, " & _
                    "[UserName] VARCHAR(100) NOT NULL, " & _
                    "[DonationID] INTEGER, " & _
                    "[Subject] VARCHAR(150) NOT NULL, " & _
                    "[Description] MEMO NOT NULL, " & _
                    "[Priority] VARCHAR(20) NOT NULL, " & _
                    "[Status] VARCHAR(30) NOT NULL, " & _
                    "[SubmittedDate] DATETIME, " & _
                    "[ResolutionNotes] MEMO)")

                ' 14. Announcements Table
                ExecuteSQL(conn, "CREATE TABLE [Announcements] (" & _
                    "[AnnouncementID] COUNTER PRIMARY KEY, " & _
                    "[Title] VARCHAR(150) NOT NULL, " & _
                    "[Content] MEMO NOT NULL, " & _
                    "[TargetRole] VARCHAR(30) NOT NULL, " & _
                    "[PublishDate] DATETIME, " & _
                    "[ExpiryDate] DATETIME, " & _
                    "[IsActive] BIT)")

                ' 15. UserLogin Table (Audit & centralized login tracking)
                ExecuteSQL(conn, "CREATE TABLE [UserLogin] (" & _
                    "[LoginID] COUNTER PRIMARY KEY, " & _
                    "[EmailAddress] VARCHAR(120) NOT NULL, " & _
                    "[UserRole] VARCHAR(30) NOT NULL, " & _
                    "[ReferenceUserID] INTEGER NOT NULL, " & _
                    "[LastLoginDate] DATETIME, " & _
                    "[IPAddress] VARCHAR(50), " & _
                    "[LoginStatus] VARCHAR(30))")
                ExecuteSQL(conn, "CREATE INDEX idx_UserLogin_Email ON [UserLogin] ([EmailAddress])")

                ' 16. PasswordReset Table
                ExecuteSQL(conn, "CREATE TABLE [PasswordReset] (" & _
                    "[ResetID] COUNTER PRIMARY KEY, " & _
                    "[EmailAddress] VARCHAR(120) NOT NULL, " & _
                    "[UserRole] VARCHAR(30) NOT NULL, " & _
                    "[ResetToken] VARCHAR(100) NOT NULL, " & _
                    "[RequestDate] DATETIME, " & _
                    "[ExpiryDate] DATETIME, " & _
                    "[IsUsed] BIT)")

                Console.WriteLine("All 16 tables and indexes created successfully.")

                ' Seed Data
                SeedSampleData(conn)
                Console.WriteLine("All sample data seeded successfully.")
            End Using
        Catch ex As Exception
            Console.Error.WriteLine("ERROR: " & ex.ToString())
            Environment.Exit(1)
        End Try
    End Sub

    Sub ExecuteSQL(ByVal conn As OleDbConnection, ByVal sql As String)
        Using cmd As New OleDbCommand(sql, conn)
            cmd.ExecuteNonQuery()
        End Using
    End Sub

    Sub SeedSampleData(ByVal conn As OleDbConnection)
        Dim defaultPwd As String = HashPassword("FoodBridge@123")

        ' 1. Seed Admin
        Using cmd As New OleDbCommand("INSERT INTO [Admin] ([FullName], [EmailAddress], [MobileNumber], [PasswordHash], [RoleTitle], [CreatedDate], [LastLogin], [IsActive]) VALUES (?, ?, ?, ?, ?, ?, ?, ?)", conn)
            cmd.Parameters.AddWithValue("@p1", "Rajesh Sharma (Chief Coordinator)")
            cmd.Parameters.AddWithValue("@p2", "admin@foodbridge.com")
            cmd.Parameters.AddWithValue("@p3", "9876543210")
            cmd.Parameters.AddWithValue("@p4", defaultPwd)
            cmd.Parameters.AddWithValue("@p5", "Super Administrator")
            cmd.Parameters.AddWithValue("@p6", DateTime.Now.AddDays(-60).ToString("yyyy-MM-dd HH:mm:ss"))
            cmd.Parameters.AddWithValue("@p7", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"))
            cmd.Parameters.AddWithValue("@p8", True)
            cmd.ExecuteNonQuery()
        End Using

        ' 2. Seed Cities
        Dim cities As String(,) = {
            {"Bengaluru", "Karnataka", "560"},
            {"Mumbai", "Maharashtra", "400"},
            {"Chennai", "Tamil Nadu", "600"},
            {"Hyderabad", "Telangana", "500"},
            {"New Delhi", "Delhi", "110"},
            {"Pune", "Maharashtra", "411"},
            {"Kochi", "Kerala", "682"},
            {"Ahmedabad", "Gujarat", "380"}
        }
        For i As Integer = 0 To cities.GetLength(0) - 1
            Using cmd As New OleDbCommand("INSERT INTO [Cities] ([CityName], [StateName], [PINPrefix], [IsActive]) VALUES (?, ?, ?, ?)", conn)
                cmd.Parameters.AddWithValue("@p1", cities(i, 0))
                cmd.Parameters.AddWithValue("@p2", cities(i, 1))
                cmd.Parameters.AddWithValue("@p3", cities(i, 2))
                cmd.Parameters.AddWithValue("@p4", True)
                cmd.ExecuteNonQuery()
            End Using
        Next

        ' 3. Seed FoodCategories (All 8 mandatory categories)
        Dim categories As String(,) = {
            {"Cooked Food", "Freshly prepared meals, rice, curries, chapatis, and banquet buffet surplus"},
            {"Raw Vegetables", "Fresh farm vegetables, greens, and uncut kitchen produce"},
            {"Fruits", "Seasonal fresh fruits and fruit baskets from markets and events"},
            {"Bakery Products", "Bread loaves, buns, cakes, pastries, and biscuits"},
            {"Packed Food", "Sealed meal boxes, snacks, tiffins, and packaged food items"},
            {"Groceries", "Dry rations, rice bags, pulses, flour, cooking oil, and spices"},
            {"Dairy Products", "Pasteurized milk packets, paneer, curd, and butter"},
            {"Other", "Beverages, nutritional supplements, and miscellaneous edible items"}
        }
        For i As Integer = 0 To categories.GetLength(0) - 1
            Using cmd As New OleDbCommand("INSERT INTO [FoodCategories] ([CategoryName], [Description], [IsActive]) VALUES (?, ?, ?)", conn)
                cmd.Parameters.AddWithValue("@p1", categories(i, 0))
                cmd.Parameters.AddWithValue("@p2", categories(i, 1))
                cmd.Parameters.AddWithValue("@p3", True)
                cmd.ExecuteNonQuery()
            End Using
        Next

        ' 4. Seed Donors (Default donor@foodbridge.com + additional donors)
        InsertDonor(conn, "Grand Royal Palace Hotel", "Hotel", "Vikram Malhotra", "9845011223", "donor@foodbridge.com", "45 MG Road, Indiranagar", "Bengaluru", defaultPwd, DateTime.Now.AddDays(-45), "Active")
        InsertDonor(conn, "Sri Krishna Marriage Hall", "Marriage Hall", "Anand Rao", "9845099887", "krishnahall@foodbridge.com", "12 Temple Street, Malleshwaram", "Bengaluru", defaultPwd, DateTime.Now.AddDays(-30), "Active")
        InsertDonor(conn, "FreshMart Supermarkets Pvt Ltd", "Supermarket", "Meera Nair", "9820122334", "freshmart@foodbridge.com", "88 Link Road, Andheri West", "Mumbai", defaultPwd, DateTime.Now.AddDays(-25), "Active")
        InsertDonor(conn, "Spice Garden Catering Services", "Catering Service", "Karthik Sundaram", "9840155667", "spicegarden@foodbridge.com", "23 Anna Salai, T. Nagar", "Chennai", defaultPwd, DateTime.Now.AddDays(-15), "Active")

        ' 5. Seed NGOs (Default ngo@foodbridge.com + additional NGOs)
        InsertNGO(conn, "Asha Deepa Charitable Trust", "NGO-KA-2019-4481", "Sister Maria Thomas", "9900112233", "ngo@foodbridge.com", "104 Hope Avenue, Koramangala 4th Block", "Bengaluru", 250, defaultPwd, DateTime.Now.AddDays(-40), "Verified")
        InsertNGO(conn, "Seva Bharathi Shelter Home", "NGO-KA-2021-8832", "Dr. Ramesh Hegde", "9900445566", "sevabharathi@foodbridge.com", "72 Gandhi Nagar, Jayanagar", "Bengaluru", 180, defaultPwd, DateTime.Now.AddDays(-28), "Verified")
        InsertNGO(conn, "Annapoorna Community Kitchen", "NGO-MH-2020-3109", "Sunita Deshmukh", "9820987654", "annapoorna@foodbridge.com", "19 Station Road, Dadar", "Mumbai", 320, defaultPwd, DateTime.Now.AddDays(-20), "Verified")
        InsertNGO(conn, "Udayam Children Welfare Society", "NGO-TN-2022-5510", "Suresh Kumar", "9840911222", "udayam@foodbridge.com", "54 Besant Nagar", "Chennai", 120, defaultPwd, DateTime.Now.AddDays(-5), "Pending")

        ' 6. Seed Volunteers (Default volunteer@foodbridge.com + additional volunteers)
        InsertVolunteer(conn, "Arjun Reddy", "Male", "9740112233", "volunteer@foodbridge.com", "18 HSR Layout Sector 2", "Bengaluru", "Available", defaultPwd, DateTime.Now.AddDays(-35), "Active")
        InsertVolunteer(conn, "Priya Darshini", "Female", "9740556677", "priya.v@foodbridge.com", "65 BTM Layout 2nd Stage", "Bengaluru", "Available", defaultPwd, DateTime.Now.AddDays(-22), "Active")
        InsertVolunteer(conn, "Rohan Kulkarni", "Male", "9820778899", "rohan.v@foodbridge.com", "31 Bandra East", "Mumbai", "Available", defaultPwd, DateTime.Now.AddDays(-18), "Active")
        InsertVolunteer(conn, "Deepak Verma", "Male", "9840334455", "deepak.v@foodbridge.com", "90 Adyar Main Road", "Chennai", "Unavailable", defaultPwd, DateTime.Now.AddDays(-12), "Active")

        ' 7. Seed FoodDonations (Approved, Pending, Delivered, Urgent, etc.)
        ' Donation 1: Approved & Available (Cooked Food - Urgent)
        InsertDonation(conn, 1, "Royal Banquet Veg Biryani & Paneer Butter Masala", "Cooked Food", _
            "Freshly prepared basmati veg biryani, paneer gravy, and raita from corporate luncheon event. Hygienically packed in food-grade steel containers.", _
            "Veg", 45.0, 30.0, "Kg", 120, DateTime.Today, "11:30", DateTime.Today.AddDays(1), "18:00", _
            "45 MG Road, Indiranagar", "Bengaluru", "560038", DateTime.Today, "15:00", _
            "Bring insulated carrier boxes if possible.", "~/Content/images/food-biryani.svg", DateTime.Now.AddHours(-4), "Approved", "Partially Assigned")

        ' Donation 2: Approved & Fully Available (Bakery Products)
        InsertDonation(conn, 1, "Assorted Whole Wheat Bread Loaves & Dinner Rolls", "Bakery Products", _
            "Freshly baked morning batch of whole wheat bread loaves and soft dinner rolls from hotel bakery.", _
            "Veg", 25.0, 25.0, "Kg", 80, DateTime.Today, "07:00", DateTime.Today.AddDays(2), "22:00", _
            "45 MG Road, Indiranagar", "Bengaluru", "560038", DateTime.Today, "17:00", _
            "Packed in sealed bakery cartons.", "~/Content/images/food-bakery.svg", DateTime.Now.AddHours(-3), "Approved", "Pending")

        ' Donation 3: Delivered (Complete End-to-End Rescue Flow)
        InsertDonation(conn, 2, "Traditional South Indian Sadhya Meal Box", "Cooked Food", _
            "Complete wedding feast surplus including steamed rice, sambar, rasam, poriyal, payasam.", _
            "Veg", 60.0, 0.0, "Kg", 180, DateTime.Today.AddDays(-2), "12:00", DateTime.Today.AddDays(-1), "20:00", _
            "12 Temple Street, Malleshwaram", "Bengaluru", "560003", DateTime.Today.AddDays(-2), "14:30", _
            "Gate 2 service entrance.", "~/Content/images/food-meals.svg", DateTime.Now.AddDays(-2), "Delivered", "Delivered")

        ' Donation 4: Pending Admin Approval (Groceries)
        InsertDonation(conn, 3, "Premium Sona Masoori Rice & Toor Dal Packs", "Groceries", _
            "Unopened 5Kg packs of Sona Masoori rice and 1Kg packs of organic Toor Dal.", _
            "Veg", 50.0, 50.0, "Kg", 200, DateTime.Today.AddDays(-5), "09:00", DateTime.Today.AddDays(30), "23:59", _
            "88 Link Road, Andheri West", "Mumbai", "400053", DateTime.Today.AddDays(1), "11:00", _
            "Collect from store warehouse bay 3.", "~/Content/images/food-groceries.svg", DateTime.Now.AddHours(-1), "Pending", "Pending")

        ' Donation 5: Approved & Available (Fruits)
        InsertDonation(conn, 2, "Fresh Robusta Bananas & Shimla Apples", "Fruits", _
            "Grade-A fresh fruit baskets left over from auspicious ceremony.", _
            "Veg", 35.0, 35.0, "Kg", 110, DateTime.Today, "08:00", DateTime.Today.AddDays(3), "21:00", _
            "12 Temple Street, Malleshwaram", "Bengaluru", "560003", DateTime.Today, "16:30", _
            "Crates ready at reception.", "~/Content/images/food-fruits.svg", DateTime.Now.AddHours(-2), "Approved", "Pending")

        ' Donation 6: Expired sample (for Expired Food Report)
        InsertDonation(conn, 4, "Mixed Vegetable Pulao & Kurma", "Cooked Food", _
            "Evening buffet surplus from catering event.", _
            "Veg", 20.0, 20.0, "Kg", 50, DateTime.Today.AddDays(-4), "18:00", DateTime.Today.AddDays(-3), "10:00", _
            "23 Anna Salai, T. Nagar", "Chennai", "600017", DateTime.Today.AddDays(-4), "20:00", _
            "Call before arrival.", "~/Content/images/food-biryani.svg", DateTime.Now.AddDays(-4), "Expired", "Expired")

        ' 8. Seed FoodRequests
        ' Request 1: For Donation 1 (Approved & Assigned to Volunteer 1)
        InsertRequest(conn, 1, 1, 15.0, 60, DateTime.Today, "104 Hope Avenue, Koramangala 4th Block", "Bengaluru", "Sister Maria Thomas", "9900112233", "Urgent dinner requirement for shelter residents.", DateTime.Now.AddHours(-3), "Assigned", "Approved and volunteer Arjun Reddy assigned.")

        ' Request 2: For Donation 3 (Delivered)
        InsertRequest(conn, 1, 3, 60.0, 180, DateTime.Today.AddDays(-2), "104 Hope Avenue, Koramangala 4th Block", "Bengaluru", "Sister Maria Thomas", "9900112233", "Full meal distribution for children and elderly home.", DateTime.Now.AddDays(-2), "Delivered", "Successfully delivered.")

        ' Request 3: For Donation 2 (Pending Admin Approval)
        InsertRequest(conn, 2, 2, 10.0, 45, DateTime.Today, "72 Gandhi Nagar, Jayanagar", "Bengaluru", "Dr. Ramesh Hegde", "9900445566", "Morning breakfast bread requirement for shelter.", DateTime.Now.AddHours(-1), "Pending", "")

        ' 9. Seed VolunteerAssignments
        ' Task 1: Active task for Donation 1 / Request 1 assigned to Volunteer 1 (Arjun Reddy)
        Using cmd As New OleDbCommand("INSERT INTO [VolunteerAssignments] ([DonationID], [RequestID], [VolunteerID], [DonorAddress], [ReceiverAddress], [AssignedQuantity], [QuantityUnit], [AssignmentDate], [TaskStatus], [CollectionRemarks], [DeliveryRemarks]) VALUES (?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?)", conn)
            cmd.Parameters.AddWithValue("@p1", 1)
            cmd.Parameters.AddWithValue("@p2", 1)
            cmd.Parameters.AddWithValue("@p3", 1)
            cmd.Parameters.AddWithValue("@p4", "45 MG Road, Indiranagar, Bengaluru - 560038")
            cmd.Parameters.AddWithValue("@p5", "104 Hope Avenue, Koramangala 4th Block, Bengaluru")
            cmd.Parameters.AddWithValue("@p6", 15.0)
            cmd.Parameters.AddWithValue("@p7", "Kg")
            cmd.Parameters.AddWithValue("@p8", DateTime.Now.AddHours(-2).ToString("yyyy-MM-dd HH:mm:ss"))
            cmd.Parameters.AddWithValue("@p9", "Accepted")
            cmd.Parameters.AddWithValue("@p10", "En route to Grand Royal Palace Hotel for pickup.")
            cmd.Parameters.AddWithValue("@p11", "")
            cmd.ExecuteNonQuery()
        End Using

        ' Task 2: Completed task for Donation 3 / Request 2 assigned to Volunteer 1 (Arjun Reddy)
        Using cmd As New OleDbCommand("INSERT INTO [VolunteerAssignments] ([DonationID], [RequestID], [VolunteerID], [DonorAddress], [ReceiverAddress], [AssignedQuantity], [QuantityUnit], [AssignmentDate], [CollectionDate], [DeliveryDate], [TaskStatus], [CollectionRemarks], [DeliveryRemarks]) VALUES (?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?)", conn)
            cmd.Parameters.AddWithValue("@p1", 3)
            cmd.Parameters.AddWithValue("@p2", 2)
            cmd.Parameters.AddWithValue("@p3", 1)
            cmd.Parameters.AddWithValue("@p4", "12 Temple Street, Malleshwaram, Bengaluru - 560003")
            cmd.Parameters.AddWithValue("@p5", "104 Hope Avenue, Koramangala 4th Block, Bengaluru")
            cmd.Parameters.AddWithValue("@p6", 60.0)
            cmd.Parameters.AddWithValue("@p7", "Kg")
            cmd.Parameters.AddWithValue("@p8", DateTime.Now.AddDays(-2).ToString("yyyy-MM-dd HH:mm:ss"))
            cmd.Parameters.AddWithValue("@p9", DateTime.Now.AddDays(-2).AddHours(1).ToString("yyyy-MM-dd HH:mm:ss"))
            cmd.Parameters.AddWithValue("@p10", DateTime.Now.AddDays(-2).AddHours(2).ToString("yyyy-MM-dd HH:mm:ss"))
            cmd.Parameters.AddWithValue("@p11", "Successfully Delivered")
            cmd.Parameters.AddWithValue("@p12", "Collected 60 Kg in hot insulated vessels.")
            cmd.Parameters.AddWithValue("@p13", "Handed over to Sister Maria Thomas in fresh condition.")
            cmd.ExecuteNonQuery()
        End Using

        ' 10. Seed FoodDelivery for Task 2
        Using cmd As New OleDbCommand("INSERT INTO [FoodDelivery] ([TaskID], [DonationID], [RequestID], [VolunteerID], [NGOID], [DeliveredQuantity], [QuantityUnit], [ReceivedByPerson], [ReceiverContact], [DeliveryCondition], [DeliveryRemarks], [DeliveryTimestamp]) VALUES (?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?)", conn)
            cmd.Parameters.AddWithValue("@p1", 2)
            cmd.Parameters.AddWithValue("@p2", 3)
            cmd.Parameters.AddWithValue("@p3", 2)
            cmd.Parameters.AddWithValue("@p4", 1)
            cmd.Parameters.AddWithValue("@p5", 1)
            cmd.Parameters.AddWithValue("@p6", 60.0)
            cmd.Parameters.AddWithValue("@p7", "Kg")
            cmd.Parameters.AddWithValue("@p8", "Sister Maria Thomas")
            cmd.Parameters.AddWithValue("@p9", "9900112233")
            cmd.Parameters.AddWithValue("@p10", "Fresh & Hot")
            cmd.Parameters.AddWithValue("@p11", "Delivered on time and served 180 beneficiaries.")
            cmd.Parameters.AddWithValue("@p12", DateTime.Now.AddDays(-2).AddHours(2).ToString("yyyy-MM-dd HH:mm:ss"))
            cmd.ExecuteNonQuery()
        End Using

        ' 11. Seed DonationTransactions
        InsertTransaction(conn, 1, 0, 0, "Donor", "Grand Royal Palace Hotel", "New", "Pending", 45.0, "Donation registered by donor.")
        InsertTransaction(conn, 1, 0, 0, "Admin", "Rajesh Sharma", "Pending", "Approved", 45.0, "Verified food freshness and approved.")
        InsertTransaction(conn, 1, 1, 1, "Admin", "Rajesh Sharma", "Approved", "Assigned", 15.0, "Allocated 15 Kg to Asha Deepa Trust & assigned Arjun Reddy.")
        InsertTransaction(conn, 3, 2, 2, "Volunteer", "Arjun Reddy", "Food Collected", "Delivered", 60.0, "Confirmed delivery of 60 Kg Sadhya meals.")

        ' 12. Seed Announcements
        Using cmd As New OleDbCommand("INSERT INTO [Announcements] ([Title], [Content], [TargetRole], [PublishDate], [ExpiryDate], [IsActive]) VALUES (?, ?, ?, ?, ?, ?)", conn)
            cmd.Parameters.AddWithValue("@p1", "Zero Food Waste Drive 2026 - Wedding & Banquet Season")
            cmd.Parameters.AddWithValue("@p2", "All marriage halls, hotels, and catering partners are requested to log surplus food at least 3 hours prior to expiry for rapid volunteer dispatch.")
            cmd.Parameters.AddWithValue("@p3", "All")
            cmd.Parameters.AddWithValue("@p4", DateTime.Today.AddDays(-5).ToString("yyyy-MM-dd HH:mm:ss"))
            cmd.Parameters.AddWithValue("@p5", DateTime.Today.AddDays(60).ToString("yyyy-MM-dd HH:mm:ss"))
            cmd.Parameters.AddWithValue("@p6", True)
            cmd.ExecuteNonQuery()
        End Using
        Using cmd As New OleDbCommand("INSERT INTO [Announcements] ([Title], [Content], [TargetRole], [PublishDate], [ExpiryDate], [IsActive]) VALUES (?, ?, ?, ?, ?, ?)", conn)
            cmd.Parameters.AddWithValue("@p1", "Food Safety & Hygiene Packaging Guidelines")
            cmd.Parameters.AddWithValue("@p2", "Please ensure cooked food temperature and preparation timestamps are accurately entered. Volunteers carry thermal boxes for all verified pickups.")
            cmd.Parameters.AddWithValue("@p3", "Donor")
            cmd.Parameters.AddWithValue("@p4", DateTime.Today.AddDays(-2).ToString("yyyy-MM-dd HH:mm:ss"))
            cmd.Parameters.AddWithValue("@p5", DateTime.Today.AddDays(90).ToString("yyyy-MM-dd HH:mm:ss"))
            cmd.Parameters.AddWithValue("@p6", True)
            cmd.ExecuteNonQuery()
        End Using

        ' 13. Seed Feedback & Complaints
        Using cmd As New OleDbCommand("INSERT INTO [Feedback] ([UserRole], [UserID], [UserName], [EmailAddress], [Rating], [Subject], [Message], [SubmittedDate], [AdminReply], [Status]) VALUES (?, ?, ?, ?, ?, ?, ?, ?, ?, ?)", conn)
            cmd.Parameters.AddWithValue("@p1", "NGO")
            cmd.Parameters.AddWithValue("@p2", 1)
            cmd.Parameters.AddWithValue("@p3", "Sister Maria Thomas (Asha Deepa Trust)")
            cmd.Parameters.AddWithValue("@p4", "ngo@foodbridge.com")
            cmd.Parameters.AddWithValue("@p5", 5)
            cmd.Parameters.AddWithValue("@p6", "Timely food rescue helped feed 180 shelter residents!")
            cmd.Parameters.AddWithValue("@p7", "FoodBridge connected us with Sri Krishna Marriage Hall within 30 minutes and volunteer Arjun delivered hot meals right on time.")
            cmd.Parameters.AddWithValue("@p8", DateTime.Now.AddDays(-1).ToString("yyyy-MM-dd HH:mm:ss"))
            cmd.Parameters.AddWithValue("@p9", "Thank you for your kind words and partnership in zero hunger!")
            cmd.Parameters.AddWithValue("@p10", "Reviewed")
            cmd.ExecuteNonQuery()
        End Using

        Using cmd As New OleDbCommand("INSERT INTO [Complaints] ([UserRole], [UserID], [UserName], [DonationID], [Subject], [Description], [Priority], [Status], [SubmittedDate], [ResolutionNotes]) VALUES (?, ?, ?, ?, ?, ?, ?, ?, ?, ?)", conn)
            cmd.Parameters.AddWithValue("@p1", "Volunteer")
            cmd.Parameters.AddWithValue("@p2", 1)
            cmd.Parameters.AddWithValue("@p3", "Arjun Reddy")
            cmd.Parameters.AddWithValue("@p4", 1)
            cmd.Parameters.AddWithValue("@p5", "Gate security delay during late evening pickup")
            cmd.Parameters.AddWithValue("@p6", "Requesting donors to inform security desk about FoodBridge volunteer arrival for faster loading.")
            cmd.Parameters.AddWithValue("@p7", "Medium")
            cmd.Parameters.AddWithValue("@p8", "Resolved")
            cmd.Parameters.AddWithValue("@p9", DateTime.Now.AddDays(-1).ToString("yyyy-MM-dd HH:mm:ss"))
            cmd.Parameters.AddWithValue("@p10", "Added gate pass instructions to donor notification checklist.")
            cmd.ExecuteNonQuery()
        End Using
    End Sub

    Sub InsertDonor(ByVal conn As OleDbConnection, ByVal name As String, ByVal dtype As String, ByVal contact As String, ByVal mobile As String, ByVal email As String, ByVal addr As String, ByVal city As String, ByVal pwd As String, ByVal regDate As DateTime, ByVal status As String)
        Using cmd As New OleDbCommand("INSERT INTO [Donors] ([DonorName], [DonorType], [ContactPerson], [MobileNumber], [EmailAddress], [Address], [City], [PasswordHash], [RegistrationDate], [AccountStatus], [ProfileImage]) VALUES (?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?)", conn)
            cmd.Parameters.AddWithValue("@p1", name)
            cmd.Parameters.AddWithValue("@p2", dtype)
            cmd.Parameters.AddWithValue("@p3", contact)
            cmd.Parameters.AddWithValue("@p4", mobile)
            cmd.Parameters.AddWithValue("@p5", email)
            cmd.Parameters.AddWithValue("@p6", addr)
            cmd.Parameters.AddWithValue("@p7", city)
            cmd.Parameters.AddWithValue("@p8", pwd)
            cmd.Parameters.AddWithValue("@p9", regDate.ToString("yyyy-MM-dd HH:mm:ss"))
            cmd.Parameters.AddWithValue("@p10", status)
            cmd.Parameters.AddWithValue("@p11", "~/Content/images/avatar-donor.svg")
            cmd.ExecuteNonQuery()
        End Using
    End Sub

    Sub InsertNGO(ByVal conn As OleDbConnection, ByVal name As String, ByVal regNo As String, ByVal contact As String, ByVal mobile As String, ByVal email As String, ByVal addr As String, ByVal city As String, ByVal benef As Integer, ByVal pwd As String, ByVal regDate As DateTime, ByVal status As String)
        Using cmd As New OleDbCommand("INSERT INTO [NGOs] ([OrganizationName], [RegistrationNumber], [ContactPerson], [MobileNumber], [EmailAddress], [Address], [City], [NumberOfBeneficiaries], [PasswordHash], [RegistrationDate], [VerificationStatus], [ProfileImage]) VALUES (?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?)", conn)
            cmd.Parameters.AddWithValue("@p1", name)
            cmd.Parameters.AddWithValue("@p2", regNo)
            cmd.Parameters.AddWithValue("@p3", contact)
            cmd.Parameters.AddWithValue("@p4", mobile)
            cmd.Parameters.AddWithValue("@p5", email)
            cmd.Parameters.AddWithValue("@p6", addr)
            cmd.Parameters.AddWithValue("@p7", city)
            cmd.Parameters.AddWithValue("@p8", benef)
            cmd.Parameters.AddWithValue("@p9", pwd)
            cmd.Parameters.AddWithValue("@p10", regDate.ToString("yyyy-MM-dd HH:mm:ss"))
            cmd.Parameters.AddWithValue("@p11", status)
            cmd.Parameters.AddWithValue("@p12", "~/Content/images/avatar-ngo.svg")
            cmd.ExecuteNonQuery()
        End Using
    End Sub

    Sub InsertVolunteer(ByVal conn As OleDbConnection, ByVal name As String, ByVal gender As String, ByVal mobile As String, ByVal email As String, ByVal addr As String, ByVal city As String, ByVal avail As String, ByVal pwd As String, ByVal regDate As DateTime, ByVal status As String)
        Using cmd As New OleDbCommand("INSERT INTO [Volunteers] ([VolunteerName], [Gender], [MobileNumber], [EmailAddress], [Address], [City], [AvailabilityStatus], [PasswordHash], [RegistrationDate], [AccountStatus], [ProfileImage]) VALUES (?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?)", conn)
            cmd.Parameters.AddWithValue("@p1", name)
            cmd.Parameters.AddWithValue("@p2", gender)
            cmd.Parameters.AddWithValue("@p3", mobile)
            cmd.Parameters.AddWithValue("@p4", email)
            cmd.Parameters.AddWithValue("@p5", addr)
            cmd.Parameters.AddWithValue("@p6", city)
            cmd.Parameters.AddWithValue("@p7", avail)
            cmd.Parameters.AddWithValue("@p8", pwd)
            cmd.Parameters.AddWithValue("@p9", regDate.ToString("yyyy-MM-dd HH:mm:ss"))
            cmd.Parameters.AddWithValue("@p10", status)
            cmd.Parameters.AddWithValue("@p11", "~/Content/images/avatar-volunteer.svg")
            cmd.ExecuteNonQuery()
        End Using
    End Sub

    Sub InsertDonation(ByVal conn As OleDbConnection, ByVal donorId As Integer, ByVal foodName As String, ByVal category As String, ByVal desc As String, ByVal ftype As String, ByVal qty As Double, ByVal availQty As Double, ByVal unit As String, ByVal persons As Integer, ByVal prepDate As DateTime, ByVal prepTime As String, ByVal expDate As DateTime, ByVal expTime As String, ByVal addr As String, ByVal city As String, ByVal pin As String, ByVal pickDate As DateTime, ByVal pickTime As String, ByVal instr As String, ByVal img As String, ByVal donDate As DateTime, ByVal appStatus As String, ByVal colStatus As String)
        Using cmd As New OleDbCommand("INSERT INTO [FoodDonations] ([DonorID], [FoodName], [FoodCategory], [FoodDescription], [FoodType], [FoodQuantity], [AvailableQuantity], [QuantityUnit], [NumberOfPersonsServed], [FoodPreparationDate], [FoodPreparationTime], [ExpiryDate], [ExpiryTime], [PickupAddress], [City], [PINCode], [PreferredPickupDate], [PreferredPickupTime], [SpecialInstructions], [FoodImage], [DonationDate], [ApprovalStatus], [CollectionStatus], [AdminRemarks]) VALUES (?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?)", conn)
            cmd.Parameters.AddWithValue("@p1", donorId)
            cmd.Parameters.AddWithValue("@p2", foodName)
            cmd.Parameters.AddWithValue("@p3", category)
            cmd.Parameters.AddWithValue("@p4", desc)
            cmd.Parameters.AddWithValue("@p5", ftype)
            cmd.Parameters.AddWithValue("@p6", qty)
            cmd.Parameters.AddWithValue("@p7", availQty)
            cmd.Parameters.AddWithValue("@p8", unit)
            cmd.Parameters.AddWithValue("@p9", persons)
            cmd.Parameters.AddWithValue("@p10", prepDate.ToString("yyyy-MM-dd"))
            cmd.Parameters.AddWithValue("@p11", prepTime)
            cmd.Parameters.AddWithValue("@p12", expDate.ToString("yyyy-MM-dd"))
            cmd.Parameters.AddWithValue("@p13", expTime)
            cmd.Parameters.AddWithValue("@p14", addr)
            cmd.Parameters.AddWithValue("@p15", city)
            cmd.Parameters.AddWithValue("@p16", pin)
            cmd.Parameters.AddWithValue("@p17", pickDate.ToString("yyyy-MM-dd"))
            cmd.Parameters.AddWithValue("@p18", pickTime)
            cmd.Parameters.AddWithValue("@p19", instr)
            cmd.Parameters.AddWithValue("@p20", img)
            cmd.Parameters.AddWithValue("@p21", donDate.ToString("yyyy-MM-dd HH:mm:ss"))
            cmd.Parameters.AddWithValue("@p22", appStatus)
            cmd.Parameters.AddWithValue("@p23", colStatus)
            cmd.Parameters.AddWithValue("@p24", "Verified by Admin")
            cmd.ExecuteNonQuery()
        End Using
    End Sub

    Sub InsertRequest(ByVal conn As OleDbConnection, ByVal ngoId As Integer, ByVal donId As Integer, ByVal reqQty As Double, ByVal benef As Integer, ByVal reqDateNeeded As DateTime, ByVal addr As String, ByVal city As String, ByVal contact As String, ByVal phone As String, ByVal remarks As String, ByVal reqDate As DateTime, ByVal status As String, ByVal adminRem As String)
        Using cmd As New OleDbCommand("INSERT INTO [FoodRequests] ([NGOID], [DonationID], [RequestedQuantity], [NumberOfBeneficiaries], [RequiredDate], [DeliveryAddress], [City], [ContactPerson], [ContactNumber], [RequestRemarks], [RequestDate], [RequestStatus], [AdminRemarks]) VALUES (?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?)", conn)
            cmd.Parameters.AddWithValue("@p1", ngoId)
            cmd.Parameters.AddWithValue("@p2", donId)
            cmd.Parameters.AddWithValue("@p3", reqQty)
            cmd.Parameters.AddWithValue("@p4", benef)
            cmd.Parameters.AddWithValue("@p5", reqDateNeeded.ToString("yyyy-MM-dd"))
            cmd.Parameters.AddWithValue("@p6", addr)
            cmd.Parameters.AddWithValue("@p7", city)
            cmd.Parameters.AddWithValue("@p8", contact)
            cmd.Parameters.AddWithValue("@p9", phone)
            cmd.Parameters.AddWithValue("@p10", remarks)
            cmd.Parameters.AddWithValue("@p11", reqDate.ToString("yyyy-MM-dd HH:mm:ss"))
            cmd.Parameters.AddWithValue("@p12", status)
            cmd.Parameters.AddWithValue("@p13", adminRem)
            cmd.ExecuteNonQuery()
        End Using
    End Sub

    Sub InsertTransaction(ByVal conn As OleDbConnection, ByVal donId As Integer, ByVal reqId As Integer, ByVal taskId As Integer, ByVal role As String, ByVal name As String, ByVal sFrom As String, ByVal sTo As String, ByVal qty As Double, ByVal remText As String)
        Using cmd As New OleDbCommand("INSERT INTO [DonationTransactions] ([DonationID], [RequestID], [TaskID], [ActionByRole], [ActionByName], [StatusFrom], [StatusTo], [QuantityChanged], [Remarks], [TransactionDate]) VALUES (?, ?, ?, ?, ?, ?, ?, ?, ?, ?)", conn)
            cmd.Parameters.AddWithValue("@p1", donId)
            cmd.Parameters.AddWithValue("@p2", reqId)
            cmd.Parameters.AddWithValue("@p3", taskId)
            cmd.Parameters.AddWithValue("@p4", role)
            cmd.Parameters.AddWithValue("@p5", name)
            cmd.Parameters.AddWithValue("@p6", sFrom)
            cmd.Parameters.AddWithValue("@p7", sTo)
            cmd.Parameters.AddWithValue("@p8", qty)
            cmd.Parameters.AddWithValue("@p9", remText)
            cmd.Parameters.AddWithValue("@p10", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"))
            cmd.ExecuteNonQuery()
        End Using
    End Sub
End Module
