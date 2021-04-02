CREATE TABLE [dbo].[Tenant] (
    [TenantId]   BIGINT          IDENTITY (1, 1) NOT NULL,
    [TenantName] NVARCHAR (1000) NOT NULL,
    CONSTRAINT [Pk_Tenant_TenantId] PRIMARY KEY CLUSTERED ([TenantId] ASC)
);

