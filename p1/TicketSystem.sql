-- I wanna have a table for users that includes employee and manager
-- I wanna have a table for tickets
-- I need a table to make the connection between the two

-- Creates our Schema AKA Workspace
-- DROP SCHEMA TicketSystem;
CREATE SCHEMA TicketSystem;
GO

-- Creates User Table within our TicketSystem
-- For isManager we will test the TINYINT in which we will set the proper value within our program
-- DROP TABLE TicketSystem.Users
CREATE TABLE TicketSystem.Users
(
    EmployeeId INT NOT NULL IDENTITY PRIMARY KEY,
    FirstName VARCHAR(MAX) NOT NULL,
    LastName VARCHAR(MAX) NOT NULL,
    PW VARCHAR(32) NOT NULL,
    Email VARCHAR(MAX) NOT NULL,
    isManager TINYINT NOT NULL DEFAULT '0'
);
GO

--Creates a Ticket Table within our TicketSystem
-- DROP TABLE TicketSystem.Tickets
CREATE TABLE TicketSystem.Tickets
(
    TicketId INT NOT NULL IDENTITY PRIMARY KEY,
    employeeid INT NOT NULL FOREIGN KEY REFERENCES TicketSystem.Users(EmployeeId),
    Descriptions VARCHAR(MAX) NOT NULL,
    Amount DECIMAL(10,2) NOT NULL,
    StatusofTicket VARCHAR(8) DEFAULT('PENDING')
);
GO

INSERT INTO TicketSystem.Users (FirstName, LastName, Email, PW, isManager)
VALUES
    ('Pisith', 'Sork', 'pisithsork@yahoo.com', '123456', 1),
    ('John', 'Smith', 'johnsmith@aol.com', 'password', 0),
    ('Jane', 'Doe', 'janedoe@hotmail.com', 'password123', 0);

INSERT INTO TicketSystem.Tickets (employeeid, Descriptions, Amount, StatusofTicket)
VALUES
    (1,'Travel Gas', 65.32, DEFAULT),
    (1,'Food Lunch', 12.20, DEFAULT),
    (3, 'Other Online Certification', 100.00, DEFAULT),
    (2, 'Lodging Mariott', 650.90, DEFAULT),
    (1, 'Travel Business Flight', 345.78, DEFAULT)


UPDATE TicketSystem.Tickets
SET StatusofTicket = 'APPROVED'
WHERE employeeid = 1 AND Descriptions = 'Travel Gas';

ALTER TABLE TicketSystem.Users 
ADD CONSTRAINT Email UNIQUE (Email);


SELECT TicketSystem.Users.FirstName, TicketSystem.Users.LastName, TicketSystem.Tickets.Descriptions, TicketSystem.Tickets.Amount, TicketSystem.Tickets.StatusofTicket
FROM TicketSystem.Users, TicketSystem.Tickets
WHERE TicketSystem.Tickets.employeeid = TicketSystem.Users.EmployeeId
AND TicketSystem.Users.EmployeeId = 1;

SELECT * FROM TicketSystem.Tickets;
Select * FROM TicketSystem.Users;

DELETE FROM TicketSystem.Users
WHERE EmployeeId = 4;




-- NEWLY UPDATED TABLES AND VALUES
CREATE TABLE TicketSystem.PendingTickets
(
    TicketId INT NOT NULL IDENTITY PRIMARY KEY,
    employeeid INT NOT NULL FOREIGN KEY REFERENCES TicketSystem.Users(EmployeeId),
    Descriptions VARCHAR(MAX) NOT NULL,
    Amount DECIMAL(10,2) NOT NULL,
    StatusofTicket VARCHAR(8) DEFAULT('PENDING'),
    SubmittedAt DATETIME DEFAULT(CURRENT_TIMESTAMP)
);

CREATE TABLE TicketSystem.CompletedTickets
(
    ticketid INT NOT NULL FOREIGN KEY REFERENCES TicketSystem.PendingTickets(TicketId),
    employee INT NOT NULL FOREIGN KEY REFERENCES TicketSystem.Users(EmployeeId),
    Descriptions VARCHAR(MAX) NOT NULL,
    Amount Decimal(10,2) NOT NULL,
    StatusofTicket VARCHAR(8),
    CompletedAt DATETIME DEFAULT(CURRENT_TIMESTAMP)
);

INSERT INTO TicketSystem.PendingTickets (employeeid, Descriptions, Amount, StatusofTicket, SubmittedAt)
VALUES
    (2,'Travel Gas', 65.32, DEFAULT, DEFAULT),
    (3,'Food Lunch', 12.20, DEFAULT, DEFAULT),
    (3, 'Other Online Certification', 100.00, DEFAULT, DEFAULT),
    (6, 'Lodging Mariott', 650.90, DEFAULT, DEFAULT),
    (5, 'Travel Business Flight', 345.78, DEFAULT, DEFAULT),
    (5, 'Travel Gas', 43.00, DEFAULT, DEFAULT),
    (3, 'Online Classes', 250.00, DEFAULT, DEFAULT)