
-- This script creates the tables 
--   Events, EventBooking, Contacts, Favourites, Tickets
-- Run this in the Azure SQL Query Editor if tables need to be recreated from scratch.



-- TABLE: Events
-- Stores all exhibition and workshop listings displayed
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

        -- EndDate must be on or after StartDate
        CONSTRAINT CHK_Events_DateRange CHECK (EndDate >= StartDate),
        -- SpotsPerSlot must be between 1 and 100
        CONSTRAINT CHK_Events_Spots CHECK (SpotsPerSlot BETWEEN 1 AND 100)
    );
END;


TABLE: EventBooking
-- Each row is one ticket booking made by a visitor.
-- Links to an Event via the EventId foreign key.
-- UserId is nullable because guests can book without logging in.
-- ============================================================
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

        -- Quantity must be between 1 and 5 tickets per booking
        CONSTRAINT CHK_EventBooking_Quantity CHECK (Quantity BETWEEN 1 AND 5),

        CONSTRAINT FK_EventBooking_Events
            FOREIGN KEY (EventId) REFERENCES Events(EventId)
            ON DELETE CASCADE
    );

    -- Index for looking up bookings by event
    CREATE INDEX IX_EventBooking_EventId ON EventBooking(EventId);
    -- Composite index for checking available spots per time slot
    CREATE INDEX IX_EventBooking_Slot ON EventBooking(EventId, BookingDate, BookingTime);
END;

-- TABLE: Contacts
-- Stores every message submitted through the Contact Us form.
-- The SubscribeNewsletter flag tracks newsletter opt-in.
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

        -- Name must not be empty
        CONSTRAINT CHK_Contacts_Name CHECK (LEN(Name) > 0),
        -- Email must contain an @ symbol
        CONSTRAINT CHK_Contacts_Email CHECK (Email LIKE '%@%.%')
    );
END;


-- TABLE: Favourites
-- Tracks which events or tours a logged-in user has saved.
-- EventId and TourId are both nullable; one is set per row.
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

    -- Index for looking up a specific user's saved items
    CREATE INDEX IX_Favourites_UserId ON Favourites(UserId);
END;


-- TABLE: Tickets
-- Links a logged-in user to their booking records so they
-- can view all their tickets.
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

    -- Index for looking up a specific user's tickets
    CREATE INDEX IX_Tickets_UserId ON Tickets(UserId);
END;


-- CLEANUP: Remove any duplicate event rows
-- Only the first 6 events (seeded by DbSeeder) should exist.
DELETE FROM EventBooking WHERE EventId > 6;
DELETE FROM Events WHERE EventId > 6;
DBCC CHECKIDENT ('Events', RESEED, 6);