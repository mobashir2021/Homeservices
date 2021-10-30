
-- --------------------------------------------------
-- Entity Designer DDL Script for SQL Server 2005, 2008, 2012 and Azure
-- --------------------------------------------------
-- Date Created: 06/01/2020 21:16:18
-- Generated from EDMX file: C:\Users\admin\source\repos\HomeServiceProjectClientLIve - Copy (2)\HomeServiceProjectClient\OnDemandService\Models\HomeServices.edmx
-- --------------------------------------------------

SET QUOTED_IDENTIFIER OFF;
GO
USE [HomeServices];
GO
IF SCHEMA_ID(N'dbo') IS NULL EXECUTE(N'CREATE SCHEMA [dbo]');
GO

-- --------------------------------------------------
-- Dropping existing FOREIGN KEY constraints
-- --------------------------------------------------

IF OBJECT_ID(N'[dbo].[FK_CardInformation_Customer]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[CardInformation] DROP CONSTRAINT [FK_CardInformation_Customer];
GO
IF OBJECT_ID(N'[dbo].[FK_City_Country]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[City] DROP CONSTRAINT [FK_City_Country];
GO
IF OBJECT_ID(N'[dbo].[FK_Customer_Pincode]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Customer] DROP CONSTRAINT [FK_Customer_Pincode];
GO
IF OBJECT_ID(N'[dbo].[FK_CustomerComplaint_Complaint]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[CustomerComplaint] DROP CONSTRAINT [FK_CustomerComplaint_Complaint];
GO
IF OBJECT_ID(N'[dbo].[FK_CustomerComplaint_Customer]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[CustomerComplaint] DROP CONSTRAINT [FK_CustomerComplaint_Customer];
GO
IF OBJECT_ID(N'[dbo].[FK_IsNotificationShown_Customer]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[IsNotificationShown] DROP CONSTRAINT [FK_IsNotificationShown_Customer];
GO
IF OBJECT_ID(N'[dbo].[FK_IsNotificationShown_Orders]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[IsNotificationShown] DROP CONSTRAINT [FK_IsNotificationShown_Orders];
GO
IF OBJECT_ID(N'[dbo].[FK_IsNotificationShown_PartnerProfessional]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[IsNotificationShown] DROP CONSTRAINT [FK_IsNotificationShown_PartnerProfessional];
GO
IF OBJECT_ID(N'[dbo].[FK_OrderRequests_Customer]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[OrderRequests] DROP CONSTRAINT [FK_OrderRequests_Customer];
GO
IF OBJECT_ID(N'[dbo].[FK_OrderRequests_Orders]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[OrderRequests] DROP CONSTRAINT [FK_OrderRequests_Orders];
GO
IF OBJECT_ID(N'[dbo].[FK_OrderRequests_PartnerProfessional]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[OrderRequests] DROP CONSTRAINT [FK_OrderRequests_PartnerProfessional];
GO
IF OBJECT_ID(N'[dbo].[FK_Orders_PaymentMethod]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Orders] DROP CONSTRAINT [FK_Orders_PaymentMethod];
GO
IF OBJECT_ID(N'[dbo].[FK_Orders_Pincode]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Orders] DROP CONSTRAINT [FK_Orders_Pincode];
GO
IF OBJECT_ID(N'[dbo].[FK_Orders_SubCategory]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Orders] DROP CONSTRAINT [FK_Orders_SubCategory];
GO
IF OBJECT_ID(N'[dbo].[FK_OrderWithProfessional_Orders]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[OrderWithProfessional] DROP CONSTRAINT [FK_OrderWithProfessional_Orders];
GO
IF OBJECT_ID(N'[dbo].[FK_OrderWithProfessional_PartnerProfessional]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[OrderWithProfessional] DROP CONSTRAINT [FK_OrderWithProfessional_PartnerProfessional];
GO
IF OBJECT_ID(N'[dbo].[FK_PartnerProfessional_City]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[PartnerProfessional] DROP CONSTRAINT [FK_PartnerProfessional_City];
GO
IF OBJECT_ID(N'[dbo].[FK_PartnerProfessional_Pincode]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[PartnerProfessional] DROP CONSTRAINT [FK_PartnerProfessional_Pincode];
GO
IF OBJECT_ID(N'[dbo].[FK_PartnerProfessional_SubCategory]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[PartnerProfessional] DROP CONSTRAINT [FK_PartnerProfessional_SubCategory];
GO
IF OBJECT_ID(N'[dbo].[FK_Pincode_City]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Pincode] DROP CONSTRAINT [FK_Pincode_City];
GO
IF OBJECT_ID(N'[dbo].[FK_Roles_Users]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Roles] DROP CONSTRAINT [FK_Roles_Users];
GO
IF OBJECT_ID(N'[dbo].[FK_ServiceProvided_PartnerProfessional]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[ServiceProvided] DROP CONSTRAINT [FK_ServiceProvided_PartnerProfessional];
GO
IF OBJECT_ID(N'[dbo].[FK_SubCategory_Category]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[SubCategory] DROP CONSTRAINT [FK_SubCategory_Category];
GO

-- --------------------------------------------------
-- Dropping existing tables
-- --------------------------------------------------

IF OBJECT_ID(N'[dbo].[CardInformation]', 'U') IS NOT NULL
    DROP TABLE [dbo].[CardInformation];
GO
IF OBJECT_ID(N'[dbo].[Category]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Category];
GO
IF OBJECT_ID(N'[dbo].[City]', 'U') IS NOT NULL
    DROP TABLE [dbo].[City];
GO
IF OBJECT_ID(N'[dbo].[CompanyInfo]', 'U') IS NOT NULL
    DROP TABLE [dbo].[CompanyInfo];
GO
IF OBJECT_ID(N'[dbo].[Complaint]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Complaint];
GO
IF OBJECT_ID(N'[dbo].[Country]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Country];
GO
IF OBJECT_ID(N'[dbo].[Customer]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Customer];
GO
IF OBJECT_ID(N'[dbo].[CustomerComplaint]', 'U') IS NOT NULL
    DROP TABLE [dbo].[CustomerComplaint];
GO
IF OBJECT_ID(N'[dbo].[IsNotificationShown]', 'U') IS NOT NULL
    DROP TABLE [dbo].[IsNotificationShown];
GO
IF OBJECT_ID(N'[dbo].[OrderRating]', 'U') IS NOT NULL
    DROP TABLE [dbo].[OrderRating];
GO
IF OBJECT_ID(N'[dbo].[OrderRequests]', 'U') IS NOT NULL
    DROP TABLE [dbo].[OrderRequests];
GO
IF OBJECT_ID(N'[dbo].[Orders]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Orders];
GO
IF OBJECT_ID(N'[dbo].[OrderWithProfessional]', 'U') IS NOT NULL
    DROP TABLE [dbo].[OrderWithProfessional];
GO
IF OBJECT_ID(N'[dbo].[PartnerProfessional]', 'U') IS NOT NULL
    DROP TABLE [dbo].[PartnerProfessional];
GO
IF OBJECT_ID(N'[dbo].[PaymentMethod]', 'U') IS NOT NULL
    DROP TABLE [dbo].[PaymentMethod];
GO
IF OBJECT_ID(N'[dbo].[Pincode]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Pincode];
GO
IF OBJECT_ID(N'[dbo].[PreferOTP]', 'U') IS NOT NULL
    DROP TABLE [dbo].[PreferOTP];
GO
IF OBJECT_ID(N'[dbo].[Roles]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Roles];
GO
IF OBJECT_ID(N'[dbo].[ServiceProvided]', 'U') IS NOT NULL
    DROP TABLE [dbo].[ServiceProvided];
GO
IF OBJECT_ID(N'[dbo].[SMSApi]', 'U') IS NOT NULL
    DROP TABLE [dbo].[SMSApi];
GO
IF OBJECT_ID(N'[dbo].[SubCategory]', 'U') IS NOT NULL
    DROP TABLE [dbo].[SubCategory];
GO
IF OBJECT_ID(N'[dbo].[Users]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Users];
GO
IF OBJECT_ID(N'[dbo].[WatsappCredentials]', 'U') IS NOT NULL
    DROP TABLE [dbo].[WatsappCredentials];
GO
IF OBJECT_ID(N'[hos].[PAT_DEMOGRAPHICS]', 'U') IS NOT NULL
    DROP TABLE [hos].[PAT_DEMOGRAPHICS];
GO
IF OBJECT_ID(N'[userhomeservices].[BillPayment]', 'U') IS NOT NULL
    DROP TABLE [userhomeservices].[BillPayment];
GO
IF OBJECT_ID(N'[userhomeservices].[BillPaymentDetails]', 'U') IS NOT NULL
    DROP TABLE [userhomeservices].[BillPaymentDetails];
GO
IF OBJECT_ID(N'[userhomeservices].[DeviceToken]', 'U') IS NOT NULL
    DROP TABLE [userhomeservices].[DeviceToken];
GO
IF OBJECT_ID(N'[userhomeservices].[MailCredentials]', 'U') IS NOT NULL
    DROP TABLE [userhomeservices].[MailCredentials];
GO

-- --------------------------------------------------
-- Creating all tables
-- --------------------------------------------------

-- Creating table 'CardInformations'
CREATE TABLE [dbo].[CardInformations] (
    [CardInformationId] int IDENTITY(1,1) NOT NULL,
    [CardNo] varchar(1000)  NULL,
    [ExpiryDate] varchar(100)  NULL,
    [BankName] varchar(100)  NULL,
    [IFSCCode] varchar(100)  NULL,
    [CustomerId] int  NOT NULL
);
GO

-- Creating table 'Categories'
CREATE TABLE [dbo].[Categories] (
    [CategoryId] int IDENTITY(1,1) NOT NULL,
    [CategoryName] varchar(100)  NOT NULL,
    [CategoryImage] varchar(500)  NULL,
    [IsActive] bit  NULL
);
GO

-- Creating table 'Cities'
CREATE TABLE [dbo].[Cities] (
    [CityId] int IDENTITY(1,1) NOT NULL,
    [CityName] varchar(100)  NULL,
    [CountryId] int  NULL
);
GO

-- Creating table 'Complaints'
CREATE TABLE [dbo].[Complaints] (
    [ComplaintId] int IDENTITY(1,1) NOT NULL,
    [ComplaintType] varchar(1000)  NULL
);
GO

-- Creating table 'Countries'
CREATE TABLE [dbo].[Countries] (
    [CountryId] int IDENTITY(1,1) NOT NULL,
    [CountryName] varchar(100)  NOT NULL
);
GO

-- Creating table 'Customers'
CREATE TABLE [dbo].[Customers] (
    [CustomerId] int IDENTITY(1,1) NOT NULL,
    [CustomerName] varchar(200)  NULL,
    [UserId] int  NULL,
    [MobileNo] varchar(15)  NULL,
    [Gender] varchar(10)  NULL,
    [Address] varchar(500)  NULL,
    [PincodeId] int  NULL,
    [Cityid] int  NULL,
    [IsGuest] bit  NULL
);
GO

-- Creating table 'CustomerComplaints'
CREATE TABLE [dbo].[CustomerComplaints] (
    [CustomerComplaintId] int IDENTITY(1,1) NOT NULL,
    [ComplaintId] int  NULL,
    [MiscellanousData] varchar(1000)  NULL,
    [Reasons] varchar(1000)  NULL,
    [ComplaintDateTime] datetime  NULL,
    [CustomerId] int  NOT NULL
);
GO

-- Creating table 'Orders'
CREATE TABLE [dbo].[Orders] (
    [OrderId] int IDENTITY(1,1) NOT NULL,
    [SubCategoryId] int  NOT NULL,
    [OrderDate] datetime  NOT NULL,
    [OrderTime] varchar(50)  NULL,
    [CustomerId] int  NULL,
    [GuestName] varchar(100)  NULL,
    [GuestPhone] varchar(15)  NULL,
    [GuestEmail] varchar(100)  NULL,
    [GuestPincodeId] int  NULL,
    [GuestCityId] int  NULL,
    [IsStarted] bit  NULL,
    [IsDelivered] bit  NULL,
    [IsPaymentDone] bit  NULL,
    [DeliveredDateTime] datetime  NULL,
    [IsCancelled] bit  NULL,
    [Rating] varchar(100)  NULL,
    [Description] varchar(1000)  NULL,
    [OrderPlacedOn] datetime  NULL,
    [IsActive] bit  NULL,
    [IsGoingOn] bit  NULL,
    [PaymentId] int  NULL,
    [SelectedDate] datetime  NULL,
    [IsLocked] int  NULL
);
GO

-- Creating table 'PartnerProfessionals'
CREATE TABLE [dbo].[PartnerProfessionals] (
    [PartnerProfessionalId] int IDENTITY(1,1) NOT NULL,
    [PartnerName] varchar(200)  NOT NULL,
    [UserId] int  NULL,
    [MobileNo] varchar(15)  NULL,
    [Gender] varchar(10)  NULL,
    [Address] varchar(500)  NULL,
    [PincodeId] int  NULL,
    [Cityid] int  NULL,
    [Ratings] varchar(50)  NULL,
    [TotalIncome] varchar(100)  NULL,
    [SubCategoryId] int  NULL
);
GO

-- Creating table 'PaymentMethods'
CREATE TABLE [dbo].[PaymentMethods] (
    [Paymentid] int IDENTITY(1,1) NOT NULL,
    [PaymentType] varchar(100)  NOT NULL
);
GO

-- Creating table 'Pincodes'
CREATE TABLE [dbo].[Pincodes] (
    [PincodeId] int IDENTITY(1,1) NOT NULL,
    [Pincode1] varchar(100)  NULL,
    [Latitude] decimal(9,6)  NULL,
    [Longitude] decimal(9,6)  NULL,
    [CityId] int  NULL
);
GO

-- Creating table 'Roles'
CREATE TABLE [dbo].[Roles] (
    [RoleId] int IDENTITY(1,1) NOT NULL,
    [UserId] int  NOT NULL,
    [Roles] varchar(100)  NOT NULL
);
GO

-- Creating table 'SubCategories'
CREATE TABLE [dbo].[SubCategories] (
    [SubCategoryId] int IDENTITY(1,1) NOT NULL,
    [SubCategoryName] varchar(100)  NULL,
    [SubCategoryImage] varchar(500)  NULL,
    [Price] decimal(10,2)  NULL,
    [CategoryId] int  NOT NULL
);
GO

-- Creating table 'Users'
CREATE TABLE [dbo].[Users] (
    [UserId] int IDENTITY(1,1) NOT NULL,
    [UserName] varchar(200)  NOT NULL,
    [Password] varchar(100)  NOT NULL,
    [Email] varchar(200)  NOT NULL
);
GO

-- Creating table 'OrderWithProfessionals'
CREATE TABLE [dbo].[OrderWithProfessionals] (
    [OrderProfessionalId] int IDENTITY(1,1) NOT NULL,
    [OrderId] int  NOT NULL,
    [ProfessionalId] int  NOT NULL,
    [SelectedDateTime] datetime  NULL,
    [IsCancelled] bit  NULL,
    [Remarks] varchar(1000)  NULL,
    [IsCompleted] bit  NULL
);
GO

-- Creating table 'OrderRequests'
CREATE TABLE [dbo].[OrderRequests] (
    [OrderRequestId] int IDENTITY(1,1) NOT NULL,
    [OrderId] int  NOT NULL,
    [Partnerid] int  NULL,
    [CustomerId] int  NULL,
    [SelectedDate] datetime  NULL,
    [SelectedTime] varchar(20)  NULL,
    [IsApproved] bit  NULL,
    [IsCancelled] bit  NULL,
    [Status] varchar(100)  NULL,
    [NotifyStatus] int  NULL,
    [IsCustomerNotify] int  NULL
);
GO

-- Creating table 'IsNotificationShowns'
CREATE TABLE [dbo].[IsNotificationShowns] (
    [IsNotificationShownId] int IDENTITY(1,1) NOT NULL,
    [OrderId] int  NULL,
    [ProfessionalId] int  NULL,
    [CustomerId] int  NULL
);
GO

-- Creating table 'OrderRatings'
CREATE TABLE [dbo].[OrderRatings] (
    [OrderRatingId] int IDENTITY(1,1) NOT NULL,
    [Ratings] varchar(1000)  NULL,
    [OrderId] int  NULL,
    [Usercomments] varchar(1000)  NULL
);
GO

-- Creating table 'PreferOTPs'
CREATE TABLE [dbo].[PreferOTPs] (
    [PreferOTPId] int IDENTITY(1,1) NOT NULL,
    [Watsapp] bit  NULL,
    [SMS] bit  NULL
);
GO

-- Creating table 'WatsappCredentials'
CREATE TABLE [dbo].[WatsappCredentials] (
    [WatsappCredentialsId] int IDENTITY(1,1) NOT NULL,
    [WatsappUsername] varchar(1000)  NULL,
    [WatsappPassword] varchar(1000)  NULL
);
GO

-- Creating table 'SMSApis'
CREATE TABLE [dbo].[SMSApis] (
    [SMSApiId] int IDENTITY(1,1) NOT NULL,
    [SMSUrl] varchar(3000)  NULL,
    [SMSApiKey] varchar(2000)  NULL,
    [Sender] varchar(1000)  NULL
);
GO

-- Creating table 'ServiceProvideds'
CREATE TABLE [dbo].[ServiceProvideds] (
    [ServiceProvidedId] int IDENTITY(1,1) NOT NULL,
    [PartnerId] int  NOT NULL,
    [CategoryId] int  NULL,
    [SubcategoryId] int  NULL
);
GO

-- Creating table 'BillPayments'
CREATE TABLE [dbo].[BillPayments] (
    [BillPaymentId] int IDENTITY(1,1) NOT NULL,
    [OrderId] int  NOT NULL,
    [CustomerId] int  NOT NULL,
    [ProfessionalId] int  NOT NULL,
    [IsApproved] bit  NULL,
    [BillCreationDate] datetime  NULL,
    [TotalPrice] int  NULL,
    [CategoryId] int  NULL,
    [Remarks] varchar(1000)  NULL,
    [Rating] int  NULL,
    [Comment] varchar(1000)  NULL,
    [CGST] varchar(100)  NULL,
    [SGST] varchar(100)  NULL
);
GO

-- Creating table 'MailCredentials'
CREATE TABLE [dbo].[MailCredentials] (
    [MailCredentialsId] int IDENTITY(1,1) NOT NULL,
    [Email] varchar(300)  NULL,
    [Password] varchar(200)  NULL,
    [Host] varchar(2300)  NULL,
    [EnableSSL] bit  NULL,
    [PortNo] int  NULL,
    [UseDefaultCredentials] bit  NULL
);
GO

-- Creating table 'CompanyInfoes'
CREATE TABLE [dbo].[CompanyInfoes] (
    [CompanyInfoId] int IDENTITY(1,1) NOT NULL,
    [Companyname] varchar(500)  NULL,
    [CompanyAddress] varchar(1000)  NULL,
    [CompanyPhoneNo1] varchar(100)  NULL,
    [CompanyPhoneNo2] varchar(100)  NULL,
    [CompanyLogo] varchar(1000)  NULL,
    [CompanySlogan] varchar(1000)  NULL
);
GO

-- Creating table 'BillPaymentDetails'
CREATE TABLE [dbo].[BillPaymentDetails] (
    [BillPaymentDetailsId] int IDENTITY(1,1) NOT NULL,
    [BillPaymentId] int  NOT NULL,
    [CategoryId] int  NULL,
    [SubCategoryId] int  NULL,
    [IsApproved] bit  NULL,
    [Remarks] varchar(1000)  NULL,
    [Rating] int  NULL,
    [Comment] varchar(1000)  NULL,
    [Price] varchar(100)  NULL,
    [Quantity] int  NULL
);
GO

-- Creating table 'DeviceTokens'
CREATE TABLE [dbo].[DeviceTokens] (
    [DeviceTokenId] int IDENTITY(1,1) NOT NULL,
    [Token] varchar(2000)  NULL,
    [Usertype] varchar(20)  NULL,
    [Userid] int  NULL
);
GO

-- Creating table 'PAT_DEMOGRAPHICS'
CREATE TABLE [dbo].[PAT_DEMOGRAPHICS] (
    [BED_NO] int  NULL,
    [MRNO] int  NOT NULL,
    [FIRSTNAME] varchar(100)  NULL,
    [AGE] varchar(100)  NULL,
    [GENDER] varchar(100)  NULL,
    [DOAH] datetime  NULL,
    [DOAICU] datetime  NULL,
    [DIAGNOSIS] varchar(100)  NULL,
    [LOCATION] varchar(100)  NULL,
    [ENTRY_DATE] datetime  NULL,
    [ICUDAY] int  NULL,
    [ENTERED_BY] varchar(50)  NULL,
    [MOBILE] varchar(20)  NULL,
    [OUTCOME] varchar(50)  NULL,
    [PAT_STATUS] varchar(50)  NULL
);
GO

-- Creating table 'BillPayment1'
CREATE TABLE [dbo].[BillPayment1] (
    [BillPaymentId] int IDENTITY(1,1) NOT NULL,
    [OrderId] int  NOT NULL,
    [CustomerId] int  NOT NULL,
    [ProfessionalId] int  NOT NULL,
    [IsApproved] bit  NULL,
    [BillCreationDate] datetime  NULL,
    [TotalPrice] int  NULL,
    [CategoryId] int  NULL,
    [Remarks] varchar(1000)  NULL,
    [Rating] int  NULL,
    [Comment] varchar(1000)  NULL,
    [CGST] varchar(100)  NULL,
    [SGST] varchar(100)  NULL
);
GO

-- Creating table 'BillPaymentDetail1'
CREATE TABLE [dbo].[BillPaymentDetail1] (
    [BillPaymentDetailsId] int IDENTITY(1,1) NOT NULL,
    [BillPaymentId] int  NOT NULL,
    [CategoryId] int  NULL,
    [SubCategoryId] int  NULL,
    [Price] varchar(100)  NULL,
    [IsApproved] bit  NULL,
    [Remarks] varchar(1000)  NULL,
    [Rating] int  NULL,
    [Comment] varchar(1000)  NULL,
    [Quantity] int  NULL
);
GO

-- Creating table 'DeviceToken1'
CREATE TABLE [dbo].[DeviceToken1] (
    [DeviceTokenId] int IDENTITY(1,1) NOT NULL,
    [Token] varchar(2000)  NULL,
    [Userid] int  NULL,
    [Usertype] varchar(100)  NULL
);
GO

-- Creating table 'MailCredential1'
CREATE TABLE [dbo].[MailCredential1] (
    [MailCredentialsId] int IDENTITY(1,1) NOT NULL,
    [Email] varchar(300)  NULL,
    [Password] varchar(200)  NULL,
    [Host] varchar(2300)  NULL,
    [EnableSSL] bit  NULL,
    [PortNo] int  NULL,
    [UseDefaultCredentials] bit  NULL
);
GO

-- --------------------------------------------------
-- Creating all PRIMARY KEY constraints
-- --------------------------------------------------

-- Creating primary key on [CardInformationId] in table 'CardInformations'
ALTER TABLE [dbo].[CardInformations]
ADD CONSTRAINT [PK_CardInformations]
    PRIMARY KEY CLUSTERED ([CardInformationId] ASC);
GO

-- Creating primary key on [CategoryId] in table 'Categories'
ALTER TABLE [dbo].[Categories]
ADD CONSTRAINT [PK_Categories]
    PRIMARY KEY CLUSTERED ([CategoryId] ASC);
GO

-- Creating primary key on [CityId] in table 'Cities'
ALTER TABLE [dbo].[Cities]
ADD CONSTRAINT [PK_Cities]
    PRIMARY KEY CLUSTERED ([CityId] ASC);
GO

-- Creating primary key on [ComplaintId] in table 'Complaints'
ALTER TABLE [dbo].[Complaints]
ADD CONSTRAINT [PK_Complaints]
    PRIMARY KEY CLUSTERED ([ComplaintId] ASC);
GO

-- Creating primary key on [CountryId] in table 'Countries'
ALTER TABLE [dbo].[Countries]
ADD CONSTRAINT [PK_Countries]
    PRIMARY KEY CLUSTERED ([CountryId] ASC);
GO

-- Creating primary key on [CustomerId] in table 'Customers'
ALTER TABLE [dbo].[Customers]
ADD CONSTRAINT [PK_Customers]
    PRIMARY KEY CLUSTERED ([CustomerId] ASC);
GO

-- Creating primary key on [CustomerComplaintId] in table 'CustomerComplaints'
ALTER TABLE [dbo].[CustomerComplaints]
ADD CONSTRAINT [PK_CustomerComplaints]
    PRIMARY KEY CLUSTERED ([CustomerComplaintId] ASC);
GO

-- Creating primary key on [OrderId] in table 'Orders'
ALTER TABLE [dbo].[Orders]
ADD CONSTRAINT [PK_Orders]
    PRIMARY KEY CLUSTERED ([OrderId] ASC);
GO

-- Creating primary key on [PartnerProfessionalId] in table 'PartnerProfessionals'
ALTER TABLE [dbo].[PartnerProfessionals]
ADD CONSTRAINT [PK_PartnerProfessionals]
    PRIMARY KEY CLUSTERED ([PartnerProfessionalId] ASC);
GO

-- Creating primary key on [Paymentid] in table 'PaymentMethods'
ALTER TABLE [dbo].[PaymentMethods]
ADD CONSTRAINT [PK_PaymentMethods]
    PRIMARY KEY CLUSTERED ([Paymentid] ASC);
GO

-- Creating primary key on [PincodeId] in table 'Pincodes'
ALTER TABLE [dbo].[Pincodes]
ADD CONSTRAINT [PK_Pincodes]
    PRIMARY KEY CLUSTERED ([PincodeId] ASC);
GO

-- Creating primary key on [RoleId] in table 'Roles'
ALTER TABLE [dbo].[Roles]
ADD CONSTRAINT [PK_Roles]
    PRIMARY KEY CLUSTERED ([RoleId] ASC);
GO

-- Creating primary key on [SubCategoryId] in table 'SubCategories'
ALTER TABLE [dbo].[SubCategories]
ADD CONSTRAINT [PK_SubCategories]
    PRIMARY KEY CLUSTERED ([SubCategoryId] ASC);
GO

-- Creating primary key on [UserId] in table 'Users'
ALTER TABLE [dbo].[Users]
ADD CONSTRAINT [PK_Users]
    PRIMARY KEY CLUSTERED ([UserId] ASC);
GO

-- Creating primary key on [OrderProfessionalId] in table 'OrderWithProfessionals'
ALTER TABLE [dbo].[OrderWithProfessionals]
ADD CONSTRAINT [PK_OrderWithProfessionals]
    PRIMARY KEY CLUSTERED ([OrderProfessionalId] ASC);
GO

-- Creating primary key on [OrderRequestId] in table 'OrderRequests'
ALTER TABLE [dbo].[OrderRequests]
ADD CONSTRAINT [PK_OrderRequests]
    PRIMARY KEY CLUSTERED ([OrderRequestId] ASC);
GO

-- Creating primary key on [IsNotificationShownId] in table 'IsNotificationShowns'
ALTER TABLE [dbo].[IsNotificationShowns]
ADD CONSTRAINT [PK_IsNotificationShowns]
    PRIMARY KEY CLUSTERED ([IsNotificationShownId] ASC);
GO

-- Creating primary key on [OrderRatingId] in table 'OrderRatings'
ALTER TABLE [dbo].[OrderRatings]
ADD CONSTRAINT [PK_OrderRatings]
    PRIMARY KEY CLUSTERED ([OrderRatingId] ASC);
GO

-- Creating primary key on [PreferOTPId] in table 'PreferOTPs'
ALTER TABLE [dbo].[PreferOTPs]
ADD CONSTRAINT [PK_PreferOTPs]
    PRIMARY KEY CLUSTERED ([PreferOTPId] ASC);
GO

-- Creating primary key on [WatsappCredentialsId] in table 'WatsappCredentials'
ALTER TABLE [dbo].[WatsappCredentials]
ADD CONSTRAINT [PK_WatsappCredentials]
    PRIMARY KEY CLUSTERED ([WatsappCredentialsId] ASC);
GO

-- Creating primary key on [SMSApiId] in table 'SMSApis'
ALTER TABLE [dbo].[SMSApis]
ADD CONSTRAINT [PK_SMSApis]
    PRIMARY KEY CLUSTERED ([SMSApiId] ASC);
GO

-- Creating primary key on [ServiceProvidedId] in table 'ServiceProvideds'
ALTER TABLE [dbo].[ServiceProvideds]
ADD CONSTRAINT [PK_ServiceProvideds]
    PRIMARY KEY CLUSTERED ([ServiceProvidedId] ASC);
GO

-- Creating primary key on [BillPaymentId] in table 'BillPayments'
ALTER TABLE [dbo].[BillPayments]
ADD CONSTRAINT [PK_BillPayments]
    PRIMARY KEY CLUSTERED ([BillPaymentId] ASC);
GO

-- Creating primary key on [MailCredentialsId] in table 'MailCredentials'
ALTER TABLE [dbo].[MailCredentials]
ADD CONSTRAINT [PK_MailCredentials]
    PRIMARY KEY CLUSTERED ([MailCredentialsId] ASC);
GO

-- Creating primary key on [CompanyInfoId] in table 'CompanyInfoes'
ALTER TABLE [dbo].[CompanyInfoes]
ADD CONSTRAINT [PK_CompanyInfoes]
    PRIMARY KEY CLUSTERED ([CompanyInfoId] ASC);
GO

-- Creating primary key on [BillPaymentDetailsId] in table 'BillPaymentDetails'
ALTER TABLE [dbo].[BillPaymentDetails]
ADD CONSTRAINT [PK_BillPaymentDetails]
    PRIMARY KEY CLUSTERED ([BillPaymentDetailsId] ASC);
GO

-- Creating primary key on [DeviceTokenId] in table 'DeviceTokens'
ALTER TABLE [dbo].[DeviceTokens]
ADD CONSTRAINT [PK_DeviceTokens]
    PRIMARY KEY CLUSTERED ([DeviceTokenId] ASC);
GO

-- Creating primary key on [MRNO] in table 'PAT_DEMOGRAPHICS'
ALTER TABLE [dbo].[PAT_DEMOGRAPHICS]
ADD CONSTRAINT [PK_PAT_DEMOGRAPHICS]
    PRIMARY KEY CLUSTERED ([MRNO] ASC);
GO

-- Creating primary key on [BillPaymentId] in table 'BillPayment1'
ALTER TABLE [dbo].[BillPayment1]
ADD CONSTRAINT [PK_BillPayment1]
    PRIMARY KEY CLUSTERED ([BillPaymentId] ASC);
GO

-- Creating primary key on [BillPaymentDetailsId] in table 'BillPaymentDetail1'
ALTER TABLE [dbo].[BillPaymentDetail1]
ADD CONSTRAINT [PK_BillPaymentDetail1]
    PRIMARY KEY CLUSTERED ([BillPaymentDetailsId] ASC);
GO

-- Creating primary key on [DeviceTokenId] in table 'DeviceToken1'
ALTER TABLE [dbo].[DeviceToken1]
ADD CONSTRAINT [PK_DeviceToken1]
    PRIMARY KEY CLUSTERED ([DeviceTokenId] ASC);
GO

-- Creating primary key on [MailCredentialsId] in table 'MailCredential1'
ALTER TABLE [dbo].[MailCredential1]
ADD CONSTRAINT [PK_MailCredential1]
    PRIMARY KEY CLUSTERED ([MailCredentialsId] ASC);
GO

-- --------------------------------------------------
-- Creating all FOREIGN KEY constraints
-- --------------------------------------------------

-- Creating foreign key on [CustomerId] in table 'CardInformations'
ALTER TABLE [dbo].[CardInformations]
ADD CONSTRAINT [FK_CardInformation_Customer]
    FOREIGN KEY ([CustomerId])
    REFERENCES [dbo].[Customers]
        ([CustomerId])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_CardInformation_Customer'
CREATE INDEX [IX_FK_CardInformation_Customer]
ON [dbo].[CardInformations]
    ([CustomerId]);
GO

-- Creating foreign key on [CategoryId] in table 'SubCategories'
ALTER TABLE [dbo].[SubCategories]
ADD CONSTRAINT [FK_SubCategory_Category]
    FOREIGN KEY ([CategoryId])
    REFERENCES [dbo].[Categories]
        ([CategoryId])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_SubCategory_Category'
CREATE INDEX [IX_FK_SubCategory_Category]
ON [dbo].[SubCategories]
    ([CategoryId]);
GO

-- Creating foreign key on [CountryId] in table 'Cities'
ALTER TABLE [dbo].[Cities]
ADD CONSTRAINT [FK_City_Country]
    FOREIGN KEY ([CountryId])
    REFERENCES [dbo].[Countries]
        ([CountryId])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_City_Country'
CREATE INDEX [IX_FK_City_Country]
ON [dbo].[Cities]
    ([CountryId]);
GO

-- Creating foreign key on [CityId] in table 'Pincodes'
ALTER TABLE [dbo].[Pincodes]
ADD CONSTRAINT [FK_Pincode_City]
    FOREIGN KEY ([CityId])
    REFERENCES [dbo].[Cities]
        ([CityId])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_Pincode_City'
CREATE INDEX [IX_FK_Pincode_City]
ON [dbo].[Pincodes]
    ([CityId]);
GO

-- Creating foreign key on [ComplaintId] in table 'CustomerComplaints'
ALTER TABLE [dbo].[CustomerComplaints]
ADD CONSTRAINT [FK_CustomerComplaint_Complaint]
    FOREIGN KEY ([ComplaintId])
    REFERENCES [dbo].[Complaints]
        ([ComplaintId])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_CustomerComplaint_Complaint'
CREATE INDEX [IX_FK_CustomerComplaint_Complaint]
ON [dbo].[CustomerComplaints]
    ([ComplaintId]);
GO

-- Creating foreign key on [PincodeId] in table 'Customers'
ALTER TABLE [dbo].[Customers]
ADD CONSTRAINT [FK_Customer_Pincode]
    FOREIGN KEY ([PincodeId])
    REFERENCES [dbo].[Pincodes]
        ([PincodeId])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_Customer_Pincode'
CREATE INDEX [IX_FK_Customer_Pincode]
ON [dbo].[Customers]
    ([PincodeId]);
GO

-- Creating foreign key on [CustomerId] in table 'CustomerComplaints'
ALTER TABLE [dbo].[CustomerComplaints]
ADD CONSTRAINT [FK_CustomerComplaint_Customer]
    FOREIGN KEY ([CustomerId])
    REFERENCES [dbo].[Customers]
        ([CustomerId])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_CustomerComplaint_Customer'
CREATE INDEX [IX_FK_CustomerComplaint_Customer]
ON [dbo].[CustomerComplaints]
    ([CustomerId]);
GO

-- Creating foreign key on [CustomerId] in table 'Orders'
ALTER TABLE [dbo].[Orders]
ADD CONSTRAINT [FK_Orders_SubCategory]
    FOREIGN KEY ([CustomerId])
    REFERENCES [dbo].[Customers]
        ([CustomerId])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_Orders_SubCategory'
CREATE INDEX [IX_FK_Orders_SubCategory]
ON [dbo].[Orders]
    ([CustomerId]);
GO

-- Creating foreign key on [PaymentId] in table 'Orders'
ALTER TABLE [dbo].[Orders]
ADD CONSTRAINT [FK_Orders_PaymentMethod]
    FOREIGN KEY ([PaymentId])
    REFERENCES [dbo].[PaymentMethods]
        ([Paymentid])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_Orders_PaymentMethod'
CREATE INDEX [IX_FK_Orders_PaymentMethod]
ON [dbo].[Orders]
    ([PaymentId]);
GO

-- Creating foreign key on [GuestPincodeId] in table 'Orders'
ALTER TABLE [dbo].[Orders]
ADD CONSTRAINT [FK_Orders_Pincode]
    FOREIGN KEY ([GuestPincodeId])
    REFERENCES [dbo].[Pincodes]
        ([PincodeId])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_Orders_Pincode'
CREATE INDEX [IX_FK_Orders_Pincode]
ON [dbo].[Orders]
    ([GuestPincodeId]);
GO

-- Creating foreign key on [PincodeId] in table 'PartnerProfessionals'
ALTER TABLE [dbo].[PartnerProfessionals]
ADD CONSTRAINT [FK_PartnerProfessional_Pincode]
    FOREIGN KEY ([PincodeId])
    REFERENCES [dbo].[Pincodes]
        ([PincodeId])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_PartnerProfessional_Pincode'
CREATE INDEX [IX_FK_PartnerProfessional_Pincode]
ON [dbo].[PartnerProfessionals]
    ([PincodeId]);
GO

-- Creating foreign key on [UserId] in table 'Roles'
ALTER TABLE [dbo].[Roles]
ADD CONSTRAINT [FK_Roles_Users]
    FOREIGN KEY ([UserId])
    REFERENCES [dbo].[Users]
        ([UserId])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_Roles_Users'
CREATE INDEX [IX_FK_Roles_Users]
ON [dbo].[Roles]
    ([UserId]);
GO

-- Creating foreign key on [Cityid] in table 'PartnerProfessionals'
ALTER TABLE [dbo].[PartnerProfessionals]
ADD CONSTRAINT [FK_PartnerProfessional_City]
    FOREIGN KEY ([Cityid])
    REFERENCES [dbo].[Cities]
        ([CityId])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_PartnerProfessional_City'
CREATE INDEX [IX_FK_PartnerProfessional_City]
ON [dbo].[PartnerProfessionals]
    ([Cityid]);
GO

-- Creating foreign key on [SubCategoryId] in table 'PartnerProfessionals'
ALTER TABLE [dbo].[PartnerProfessionals]
ADD CONSTRAINT [FK_PartnerProfessional_SubCategory]
    FOREIGN KEY ([SubCategoryId])
    REFERENCES [dbo].[SubCategories]
        ([SubCategoryId])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_PartnerProfessional_SubCategory'
CREATE INDEX [IX_FK_PartnerProfessional_SubCategory]
ON [dbo].[PartnerProfessionals]
    ([SubCategoryId]);
GO

-- Creating foreign key on [OrderId] in table 'OrderWithProfessionals'
ALTER TABLE [dbo].[OrderWithProfessionals]
ADD CONSTRAINT [FK_OrderWithProfessional_Orders]
    FOREIGN KEY ([OrderId])
    REFERENCES [dbo].[Orders]
        ([OrderId])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_OrderWithProfessional_Orders'
CREATE INDEX [IX_FK_OrderWithProfessional_Orders]
ON [dbo].[OrderWithProfessionals]
    ([OrderId]);
GO

-- Creating foreign key on [ProfessionalId] in table 'OrderWithProfessionals'
ALTER TABLE [dbo].[OrderWithProfessionals]
ADD CONSTRAINT [FK_OrderWithProfessional_PartnerProfessional]
    FOREIGN KEY ([ProfessionalId])
    REFERENCES [dbo].[PartnerProfessionals]
        ([PartnerProfessionalId])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_OrderWithProfessional_PartnerProfessional'
CREATE INDEX [IX_FK_OrderWithProfessional_PartnerProfessional]
ON [dbo].[OrderWithProfessionals]
    ([ProfessionalId]);
GO

-- Creating foreign key on [CustomerId] in table 'OrderRequests'
ALTER TABLE [dbo].[OrderRequests]
ADD CONSTRAINT [FK_OrderRequests_Customer]
    FOREIGN KEY ([CustomerId])
    REFERENCES [dbo].[Customers]
        ([CustomerId])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_OrderRequests_Customer'
CREATE INDEX [IX_FK_OrderRequests_Customer]
ON [dbo].[OrderRequests]
    ([CustomerId]);
GO

-- Creating foreign key on [OrderId] in table 'OrderRequests'
ALTER TABLE [dbo].[OrderRequests]
ADD CONSTRAINT [FK_OrderRequests_Orders]
    FOREIGN KEY ([OrderId])
    REFERENCES [dbo].[Orders]
        ([OrderId])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_OrderRequests_Orders'
CREATE INDEX [IX_FK_OrderRequests_Orders]
ON [dbo].[OrderRequests]
    ([OrderId]);
GO

-- Creating foreign key on [Partnerid] in table 'OrderRequests'
ALTER TABLE [dbo].[OrderRequests]
ADD CONSTRAINT [FK_OrderRequests_PartnerProfessional]
    FOREIGN KEY ([Partnerid])
    REFERENCES [dbo].[PartnerProfessionals]
        ([PartnerProfessionalId])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_OrderRequests_PartnerProfessional'
CREATE INDEX [IX_FK_OrderRequests_PartnerProfessional]
ON [dbo].[OrderRequests]
    ([Partnerid]);
GO

-- Creating foreign key on [CustomerId] in table 'IsNotificationShowns'
ALTER TABLE [dbo].[IsNotificationShowns]
ADD CONSTRAINT [FK_IsNotificationShown_Customer]
    FOREIGN KEY ([CustomerId])
    REFERENCES [dbo].[Customers]
        ([CustomerId])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_IsNotificationShown_Customer'
CREATE INDEX [IX_FK_IsNotificationShown_Customer]
ON [dbo].[IsNotificationShowns]
    ([CustomerId]);
GO

-- Creating foreign key on [OrderId] in table 'IsNotificationShowns'
ALTER TABLE [dbo].[IsNotificationShowns]
ADD CONSTRAINT [FK_IsNotificationShown_Orders]
    FOREIGN KEY ([OrderId])
    REFERENCES [dbo].[Orders]
        ([OrderId])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_IsNotificationShown_Orders'
CREATE INDEX [IX_FK_IsNotificationShown_Orders]
ON [dbo].[IsNotificationShowns]
    ([OrderId]);
GO

-- Creating foreign key on [ProfessionalId] in table 'IsNotificationShowns'
ALTER TABLE [dbo].[IsNotificationShowns]
ADD CONSTRAINT [FK_IsNotificationShown_PartnerProfessional]
    FOREIGN KEY ([ProfessionalId])
    REFERENCES [dbo].[PartnerProfessionals]
        ([PartnerProfessionalId])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_IsNotificationShown_PartnerProfessional'
CREATE INDEX [IX_FK_IsNotificationShown_PartnerProfessional]
ON [dbo].[IsNotificationShowns]
    ([ProfessionalId]);
GO

-- Creating foreign key on [PartnerId] in table 'ServiceProvideds'
ALTER TABLE [dbo].[ServiceProvideds]
ADD CONSTRAINT [FK_ServiceProvided_PartnerProfessional]
    FOREIGN KEY ([PartnerId])
    REFERENCES [dbo].[PartnerProfessionals]
        ([PartnerProfessionalId])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_ServiceProvided_PartnerProfessional'
CREATE INDEX [IX_FK_ServiceProvided_PartnerProfessional]
ON [dbo].[ServiceProvideds]
    ([PartnerId]);
GO

-- --------------------------------------------------
-- Script has ended
-- --------------------------------------------------