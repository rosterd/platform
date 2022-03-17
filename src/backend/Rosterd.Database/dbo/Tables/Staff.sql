CREATE TABLE [dbo].[Staff] (
    [StaffId]           BIGINT          IDENTITY (1, 1) NOT NULL,
    [Auth0Id]           NVARCHAR (130)  NOT NULL,
    [OrganizationId]    BIGINT          NOT NULL,
    [FirstName]         NVARCHAR (1000) NOT NULL,
    [LastName]          NVARCHAR (1000) NOT NULL,
    [JobTitle]          NVARCHAR (1000) NULL,
    [IsActive]          BIT             CONSTRAINT [DF_Staff_IsActive] DEFAULT ((1)) NOT NULL,
    [Email]             NVARCHAR (1000) NULL,
    [MobilePhoneNumber] NVARCHAR (1000) NULL,
    [Comments]          NVARCHAR (1000) NULL,
    [StaffRole]         NVARCHAR (1000) NULL,
    CONSTRAINT [Pk_Staff_StaffId] PRIMARY KEY CLUSTERED ([StaffId] ASC),
    CONSTRAINT [FK_Staff_Organization] FOREIGN KEY ([OrganizationId]) REFERENCES [dbo].[Organization] ([OrganizationId])
);














GO
CREATE UNIQUE NONCLUSTERED INDEX [IX_Staff_Auth0Id]
    ON [dbo].[Staff]([Auth0Id] ASC);

