﻿CREATE TABLE [dbo].[Facility] (
    [FacilityId]    BIGINT          IDENTITY (1, 1) NOT NULL,
    [FacilityName]  NVARCHAR (1000) NOT NULL,
    [OrganzationId] BIGINT          NOT NULL,
    [Address]       NVARCHAR (1000) NOT NULL,
    [Suburb]        NVARCHAR (1000) NULL,
    [City]          NVARCHAR (1000) NOT NULL,
    [Country]       NVARCHAR (1000) NOT NULL,
    [Latitude]      DECIMAL (12, 9) NOT NULL,
    [Longitude]     DECIMAL (12, 9) NOT NULL,
    [PhoneNumber1]  NVARCHAR (1000) NOT NULL,
    [PhoneNumber2]  NVARCHAR (1000) NULL,
    [IsActive]      BIT             NOT NULL,
    CONSTRAINT [Pk_Facility_FacilityId] PRIMARY KEY CLUSTERED ([FacilityId] ASC),
    CONSTRAINT [Fk_Facility_Organization] FOREIGN KEY ([OrganzationId]) REFERENCES [dbo].[Organization] ([OrganizationId])
);










GO
CREATE NONCLUSTERED INDEX [IX_Fk_Facility_Organization]
    ON [dbo].[Facility]([OrganzationId] ASC);

