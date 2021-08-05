CREATE TABLE [dbo].[Staff] (
    [StaffId]           BIGINT          IDENTITY (1, 1) NOT NULL,
    [Auth0Id]           NVARCHAR (MAX)  NOT NULL,
    [FirstName]         NVARCHAR (1000) NOT NULL,
    [MiddleName]        NVARCHAR (1000) NULL,
    [LastName]          NVARCHAR (1000) NOT NULL,
    [JobTitle]          NVARCHAR (1000) NULL,
    [IsAvailable]       BIT             NOT NULL,
    [IsActive]          BIT             CONSTRAINT [DF_Staff_IsActive] DEFAULT ((1)) NULL,
    [DateOfBirth]       DATE            NULL,
    [Email]             NVARCHAR (1000) NULL,
    [HomePhoneNumber]   NVARCHAR (1000) NULL,
    [MobilePhoneNumber] NVARCHAR (1000) NULL,
    [OtherPhoneNumber]  NVARCHAR (1000) NULL,
    [Address]           NVARCHAR (1000) NULL,
    [Comments]          NVARCHAR (1000) NULL,
    CONSTRAINT [Pk_Resource_ResourceId] PRIMARY KEY CLUSTERED ([StaffId] ASC)
);



