CREATE TABLE [dbo].[Facility] (
    [FacilityId]    BIGINT          IDENTITY (1, 1) NOT NULL,
    [FacilityName]  NVARCHAR (1000) NOT NULL,
    [OrganzationId] BIGINT          NOT NULL,
    [Address]       NVARCHAR (1000) NOT NULL,
    [Suburb]        NVARCHAR (1000) NOT NULL,
    [City]          NVARCHAR (1000) NOT NULL,
    [Country]       NVARCHAR (1000) NOT NULL,
    [PhoneNumber1]  NVARCHAR (1000) NOT NULL,
    [PhoneNumber2]  NVARCHAR (1000) NULL,
    CONSTRAINT [Pk_Facility_FacilityId] PRIMARY KEY CLUSTERED ([FacilityId] ASC),
    CONSTRAINT [Fk_Facility_Organization] FOREIGN KEY ([OrganzationId]) REFERENCES [dbo].[Organization] ([OrganizationId])
);

