USE [master];
GO

IF DATABASEPROPERTYEX ('InstantHabit', 'Version' ) IS NOT NULL
BEGIN 
    -- It cancels all runing transactions and closes open connections
	ALTER Database [InstantHabit] SET SINGLE_USER
	WITH ROLLBACK IMMEDIATE;
	DROP DATABASE [InstantHabit];
END
GO

CREATE DATABASE [InstantHabit];
GO

-- This gives a simple backup that can be used to replace the entire db in the event of failure.
ALTER DATABASE [InstantHabit] SET RECOVERY SIMPLE;
GO

USE [InstantHabit];
GO

CREATE SCHEMA [InstantHabit];
GO

CREATE TABLE [InstantHabit].[Habits] (
    [Id] int identity(1,1) NOT NULL,
	[Name] nvarchar(500) NOT NULL,
	[CreationDate] datetime NOT NULL DEFAULT GETDATE(),
	[Description] nvarchar(500) NULL,
	[IsExtended] bit NOT NULL Default 0

	CONSTRAINT [PK_Habits] PRIMARY KEY ([Id])
);
GO


CREATE TABLE [InstantHabit].[Days] (
    [Id] int identity(1,1) NOT NULL,
	[HabitId] int NOT NULL,
	[DayNumber] int NOT NULL, 
	[Note] nvarchar(500) NULL

	CONSTRAINT [PK_Days] PRIMARY KEY ([Id])
	CONSTRAINT [FK_HabitId] FOREIGN KEY ([HabitId])  
	REFERENCES [InstantHabit].[Habits] ([Id]) 
	ON DELETE CASCADE -- It allows you to delete rows when 2 tables are dependent
);
GO


-- Declare Stored Procedures
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- Create New Habit Stored Procedure
CREATE OR ALTER PROCEDURE [InstantHabit].[CreateNewHabit_StoredProcedure]
@Name varchar(500)
AS
BEGIN
	INSERT INTO InstantHabit.Habits (Name, CreationDate, Description) VALUES (@Name, GETDATE(), NULL)
END
GO

--Deletes Habit From Day Table
CREATE OR ALTER PROCEDURE [InstantHabit].[DeleteAhabit_StoredProcedure]
@Id int
AS
BEGIN
    DELETE FROM InstantHabit.Habits
	WHERE Id = @Id 
END
GO

--Add/Update Habit Description Stored Procedure
CREATE OR ALTER PROCEDURE [InstantHabit].[AddDescription_StoredProcedure]
@Id int,
@Description varchar(500)
AS
BEGIN
    UPDATE InstantHabit.Habits
	SET Description = @Description
	WHERE Id = @Id
END
GO

--Add New Day Stored Procedure
CREATE OR ALTER PROCEDURE [InstantHabit].[AddNewDay_StoredProcedure]
@habitId int,
@dayNumber int
AS
BEGIN
    INSERT INTO InstantHabit.Days (HabitId, DayNumber, Note) VALUES (@habitId, @dayNumber, NULL)
END
GO

--Deletes Day From Day Table
CREATE OR ALTER PROCEDURE [InstantHabit].[DeleteDay_StoredProcedure]
@habitId int,
@dayNumber int
AS
BEGIN
    DELETE FROM InstantHabit.Days
	WHERE HabitId = @habitId AND DayNumber = @dayNumber
END
GO

--Add/Update Day Description Stored Procedure
CREATE OR ALTER PROCEDURE [InstantHabit].[AddDayDescription_StoredProcedure]
@habitId int,
@dayNumber int,
@Description varchar(500)
AS
BEGIN
    UPDATE InstantHabit.Days
	SET Note = @Description
	WHERE HabitId = @habitId AND DayNumber = @dayNumber
END
GO

--Deletes Selected Habit - Days From Day Table
CREATE OR ALTER PROCEDURE [InstantHabit].[DeleteDays_StoredProcedure]
@habitId int	
AS
BEGIN
    DELETE FROM InstantHabit.Days
	WHERE HabitId = @habitId 
END
GO

--Add/Update Habit isExtended Stored Procedure
CREATE OR ALTER PROCEDURE [InstantHabit].[SetIsExtended_StoredProcedure]
@habitId int

AS
BEGIN
    UPDATE InstantHabit.Habits
	SET IsExtended = 1
	WHERE Id = @habitId
END
GO