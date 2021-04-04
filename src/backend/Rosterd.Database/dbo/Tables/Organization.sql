CREATE TABLE [dbo].[Organization] (
    [OrganizationId]   BIGINT          IDENTITY (1, 1) NOT NULL,
    [TenantId]         BIGINT          NOT NULL,
    [OrganizationName] NVARCHAR (1000) NOT NULL,
    [Address]          NVARCHAR (1000) NULL,
    CONSTRAINT [Pk_Organization_OrganizationId] PRIMARY KEY CLUSTERED ([OrganizationId] ASC),
    CONSTRAINT [Fk_Organization_Tenant] FOREIGN KEY ([TenantId]) REFERENCES [dbo].[Tenant] ([TenantId])
);




GO
CREATE NONCLUSTERED INDEX [IX_Fk_Organization_Tenant]
    ON [dbo].[Organization]([TenantId] ASC);

