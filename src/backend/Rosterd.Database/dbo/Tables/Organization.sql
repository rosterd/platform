CREATE TABLE [dbo].[Organization] (
    [OrganizationId]      BIGINT          IDENTITY (1, 1) NOT NULL,
    [OrganizationName]    NVARCHAR (1000) NOT NULL,
    [Auth0OrganizationId] NVARCHAR (130)  NOT NULL,
    [Address]             NVARCHAR (1000) NULL,
    [Phone]               NVARCHAR (1000) NULL,
    [Comments]            NVARCHAR (1000) NULL,
    [IsActive]            BIT             NOT NULL,
    CONSTRAINT [Pk_Organization_OrganizationId] PRIMARY KEY CLUSTERED ([OrganizationId] ASC)
);










GO
CREATE UNIQUE NONCLUSTERED INDEX [IX_Organization_Auth0OrganizationId]
    ON [dbo].[Organization]([Auth0OrganizationId] ASC);

