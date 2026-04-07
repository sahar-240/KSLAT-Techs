-- TABLE: Users
-- Stores registered museum members
-- UserId is the shared key refrenced by EventBooking, TourBooking, Favourites, Tickets
IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'Users')
BEGIN
	CREATE TABLE Users (
		UserId INT IDENTITY(1,1) PRIMARY KEY,
		FirstName NVARCHAR(100) NOT NULL,
		LastName NVARCHAR(100) NOT NULL,
		Email NVARCHAR(255) NOT NULL,
		Phone NVARCHAR(20) NULL,
		Username NVARCHAR(50) NOT NULL,
		PasswordHash NVARCHAR(255) NOT NULL,
		Title NVARCHAR(10) NULL,
		Address NVARCHAR(255) NULL,
		City NVARCHAR(100) NULL,
		County NVARCHAR(100) NULL,
		Postcode NVARCHAR(20) NULL,
		CreatedAt DATETIME2 NOT NULL DEFAULT GETDATE(),

		CONSTRAINT UQ_Users_Email UNIQUE (Email),
		CONSTRAINT UQ_Users_Username UNIQUE (Username),
		CONSTRAINT CHK_Users_Phone CHECK (Phone IS NULL OR Phone LIKE '[0-9]%'),
		CONSTRAINT CHK_Users_Email CHECK (Email LIKE '%@%.%')
	);
END

-- TABLE: Donations
-- Stores each donation made through the support page
IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'Donations')
BEGIN
	CREATE TABLE Donations (
		DonationId INT IDENTITY(1,1) PRIMARY KEY,
		Amount DECIMAL(10, 2) NOT NULL,
		Message NVARCHAR(500) DEFAULT NULL,
		FirstName NVARCHAR(100) NOT NULL,
		LastName NVARCHAR(100) NOT NULL,
		Email NVARCHAR(255) NOT NULL,
		Phone NVARCHAR(20) DEFAULT NULL,
		Address NVARCHAR(255) NOT NULL,
		City NVARCHAR(100) NOT NULL,
		Country NVARCHAR(100) DEFAULT NULL,
		Postcode NVARCHAR(20) DEFAULT NULL,
		SubscribeNewsletter BIT NOT NULL DEFAULT 0,
		DonationDate DATETIME2 NOT NULL DEFAULT GETDATE(),
		Status NVARCHAR(20) NOT NULL DEFAULT 'Completed',
		TransactionId NVARCHAR(MAX) DEFAULT NULL,
		CreatedAt DATETIME2 NOT NULL DEFAULT GETDATE(),

		CONSTRAINT CHK_Donations_Amount CHECK (Amount > 0),
		CONSTRAINT CHK_Donations_Email CHECK (Email LIKE '%@%.%')
	);
END