-- SQL Database Setup Script (ALL TEAM TABLES)
-- ============================================================


-- TABLE: Users  (Kai)
-- Stores registered museum members.
-- UserId is the shared key referenced by EventBooking,Favourites, Tickets, and Donations.
IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'Users')
BEGIN
    CREATE TABLE Users (
        UserId       INT IDENTITY(1,1) PRIMARY KEY,
        FirstName    NVARCHAR(100)  NOT NULL,
        LastName     NVARCHAR(100)  NOT NULL,
        Email        NVARCHAR(255)  NOT NULL,
        Phone        NVARCHAR(20)   NULL,
        Username     NVARCHAR(50)   NOT NULL,
        PasswordHash NVARCHAR(255)  NOT NULL,
        Title        NVARCHAR(10)   NULL,
        Address      NVARCHAR(255)  NULL,
        City         NVARCHAR(100)  NULL,
        County       NVARCHAR(100)  NULL,
        Postcode     NVARCHAR(20)   NULL,
        CreatedAt    DATETIME2      NOT NULL DEFAULT GETDATE(),

        CONSTRAINT UQ_Users_Username UNIQUE (Username),
        CONSTRAINT UQ_Users_Email    UNIQUE (Email),
        CONSTRAINT CHK_Users_Email   CHECK  (Email LIKE '%@%.%')
    );
END;


-- TABLE: Events  (Louisa)
-- Stores all exhibition and workshop listings displayed on the Events page.
IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'Events')
BEGIN
    CREATE TABLE Events (
        EventId         INT IDENTITY(1,1) PRIMARY KEY,
        Title           NVARCHAR(200)   NOT NULL,
        EventType       NVARCHAR(100)   NOT NULL DEFAULT '',
        Genre           NVARCHAR(100)   NOT NULL DEFAULT '',
        Description     NVARCHAR(MAX)   NOT NULL DEFAULT '',
        FullDescription NVARCHAR(MAX)   NOT NULL DEFAULT '',
        ImagePath       NVARCHAR(500)   NOT NULL DEFAULT '',
        Location        NVARCHAR(200)   NOT NULL DEFAULT '',
        StartDate       DATETIME2       NOT NULL,
        EndDate         DATETIME2       NOT NULL,
        TimeInfo        NVARCHAR(200)   NOT NULL DEFAULT '',
        ThemeColour     NVARCHAR(10)    NOT NULL DEFAULT '',
        IsFreeEntry     BIT             NOT NULL DEFAULT 1,
        SpotsPerSlot    INT             NOT NULL DEFAULT 15,

        CONSTRAINT CHK_Events_DateRange CHECK (EndDate >= StartDate),
        CONSTRAINT CHK_Events_Spots     CHECK (SpotsPerSlot BETWEEN 1 AND 100)
    );
END;


-- TABLE: EventBooking  (Louisa)
-- Each row is one ticket booking made by a visitor.
-- UserId is nullable because guests can book without logging in.
IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'EventBooking')
BEGIN
    CREATE TABLE EventBooking (
        EventBookingId  INT IDENTITY(1,1) PRIMARY KEY,
        EventId         INT             NOT NULL,
        UserId          INT             NULL,
        TicketCode      NVARCHAR(30)    NOT NULL DEFAULT '',
        BookingDate     NVARCHAR(100)   NOT NULL DEFAULT '',
        BookingTime     NVARCHAR(20)    NOT NULL DEFAULT '',
        Quantity        INT             NOT NULL DEFAULT 1,
        Email           NVARCHAR(200)   NOT NULL,
        Phone           NVARCHAR(20)    NULL,
        CreatedAt       DATETIME2       NOT NULL DEFAULT GETDATE(),

        CONSTRAINT CHK_EventBooking_Quantity CHECK (Quantity BETWEEN 1 AND 5),

        CONSTRAINT FK_EventBooking_Events
            FOREIGN KEY (EventId) REFERENCES Events(EventId)
            ON DELETE CASCADE
    );

    CREATE INDEX IX_EventBooking_EventId ON EventBooking(EventId);
    CREATE INDEX IX_EventBooking_Slot    ON EventBooking(EventId, BookingDate, BookingTime);
END;


-- TABLE: Contacts  (Louisa)
-- Stores every message submitted through the Contact Us form.
IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'Contacts')
BEGIN
    CREATE TABLE Contacts (
        ContactId           INT IDENTITY(1,1) PRIMARY KEY,
        Name                NVARCHAR(100)   NOT NULL,
        Email               NVARCHAR(200)   NOT NULL,
        Phone               NVARCHAR(20)    NULL,
        Subject             NVARCHAR(200)   NOT NULL,
        Message             NVARCHAR(2000)  NOT NULL,
        Department          NVARCHAR(100)   NOT NULL DEFAULT '',
        SubscribeNewsletter BIT             NOT NULL DEFAULT 0,
        CreatedAt           DATETIME2       NOT NULL DEFAULT GETDATE(),

        CONSTRAINT CHK_Contacts_Name  CHECK (LEN(Name) > 0),
        CONSTRAINT CHK_Contacts_Email CHECK (Email LIKE '%@%.%')
    );
END;

-- TABLE: Favourites  (Louisa)
-- Tracks which events a logged-in member has saved.
IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'Favourites')
BEGIN
    CREATE TABLE Favourites (
        FavouriteId     INT IDENTITY(1,1) PRIMARY KEY,
        UserId          INT             NOT NULL,
        EventId         INT             NULL,
        TourId          INT             NULL,
        CreatedAt       DATETIME2       NOT NULL DEFAULT GETDATE(),

        CONSTRAINT FK_Favourites_Events
            FOREIGN KEY (EventId) REFERENCES Events(EventId)
            ON DELETE SET NULL
    );

    CREATE INDEX IX_Favourites_UserId ON Favourites(UserId);
END;


-- TABLE: Tickets  (Louisa)
-- Links a logged-in member to their booking records.
IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'Tickets')
BEGIN
    CREATE TABLE Tickets (
        TicketId        INT IDENTITY(1,1) PRIMARY KEY,
        UserId          INT             NOT NULL,
        EventBookingId  INT             NULL,
        TourBookingId   INT             NULL,
        CreatedAt       DATETIME2       NOT NULL DEFAULT GETDATE(),

        CONSTRAINT FK_Tickets_EventBooking
            FOREIGN KEY (EventBookingId) REFERENCES EventBooking(EventBookingId)
            ON DELETE SET NULL
    );

    CREATE INDEX IX_Tickets_UserId ON Tickets(UserId);
END;


-- TABLE: Donations  (Tanzira)
-- Stores each donation made through the Support page.
IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'Donations')
BEGIN
    CREATE TABLE Donations (
        DonationId          INT IDENTITY(1,1) PRIMARY KEY,
        Amount              DECIMAL(10,2)   NOT NULL,
        Message             NVARCHAR(500)   NULL DEFAULT '',
        FirstName           NVARCHAR(100)   NOT NULL,
        LastName            NVARCHAR(100)   NOT NULL,
        Email               NVARCHAR(255)   NOT NULL,
        Phone               NVARCHAR(20)    NULL DEFAULT '',
        Address             NVARCHAR(255)   NOT NULL,
        City                NVARCHAR(100)   NOT NULL,
        Country             NVARCHAR(100)   NULL DEFAULT '',
        Postcode            NVARCHAR(20)    NULL DEFAULT '',
        SubscribeNewsletter BIT             NOT NULL DEFAULT 0,
        DonationDate        DATETIME2       NOT NULL DEFAULT GETDATE(),
        Status              NVARCHAR(50)    NOT NULL DEFAULT 'Completed',
        TransactionId       NVARCHAR(MAX)   NULL DEFAULT '',
        CreatedAt           DATETIME2       NOT NULL DEFAULT GETDATE(),

        CONSTRAINT CHK_Donations_Amount CHECK (Amount > 0),
        CONSTRAINT CHK_Donations_Email  CHECK (Email LIKE '%@%.%')
    );
END;


-- TABLE: OpeningHours  (Amine)
-- Museum opening hours displayed on the homepage.
IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'OpeningHours')
BEGIN
    CREATE TABLE OpeningHours (
        HourId      INT IDENTITY(1,1) PRIMARY KEY,
        DayOfWeek   NVARCHAR(20)    NOT NULL,
        OpenTime    TIME            NULL,
        CloseTime   TIME            NULL,
        IsClosed    BIT             NOT NULL DEFAULT 0,
        SpecialNote NVARCHAR(255)   NULL
    );
END;


-- ============================================================
-- SAMPLE DML QUERIES  (Data Manipulation Language)
-- These demonstrate INSERT, SELECT, UPDATE, DELETE 
-- They are commented out — run individually in Azure
-- SQL Query Editor to test.
-- ============================================================

-- INSERT: Register a new user (DML - INSERT)
-- INSERT INTO Users (FirstName, LastName, Email, Username, PasswordHash)
-- VALUES ('Jane', 'Smith', 'jane@email.com', 'janesmith', 'hashed_password_here');

-- INSERT: Add a new contact message (DML - INSERT)
-- INSERT INTO Contacts (Name, Email, Subject, Message, Department, SubscribeNewsletter)
-- VALUES ('Jane Smith', 'jane@email.com', 'Tour Question',
--         'When is the next guided tour?', 'Events & Bookings', 0);

-- SELECT with JOIN: Get all bookings for a specific event (DML - SELECT)
-- SELECT eb.EventBookingId, eb.TicketCode, eb.BookingDate, eb.BookingTime,
--        eb.Quantity, eb.Email, e.Title, e.Location
-- FROM EventBooking eb
-- JOIN Events e ON eb.EventId = e.EventId
-- WHERE e.EventId = 1;

-- SELECT with WHERE + aggregate: Count remaining spots for a time slot
-- SELECT (e.SpotsPerSlot - ISNULL(SUM(eb.Quantity), 0)) AS SpotsRemaining
-- FROM Events e
-- LEFT JOIN EventBooking eb ON e.EventId = eb.EventId
--     AND eb.BookingDate = 'Mon, 20 Apr 2026'
--     AND eb.BookingTime = '10:00 AM'
-- WHERE e.EventId = 1
-- GROUP BY e.SpotsPerSlot;

-- SELECT with filtering: Find all fashion exhibitions
-- SELECT EventId, Title, Location, StartDate, EndDate
-- FROM Events
-- WHERE Genre = 'Fashion' AND EventType = 'Exhibition'
-- ORDER BY StartDate DESC;

-- UPDATE: Change an event location (DML - UPDATE)
-- UPDATE Events SET Location = 'New Wing, Floor 3' WHERE EventId = 1;

-- DELETE: Remove a booking by ID (DML - DELETE)
-- DELETE FROM EventBooking WHERE EventBookingId = 5;