CREATE TABLE [dbo].[Capability] (
    [CapabilityId]   BIGINT          IDENTITY (1, 1) NOT NULL,
    [CapabilityName] NVARCHAR (1000) NOT NULL,
    CONSTRAINT [Pk_FacilityFeature_FacilityFeatureId] PRIMARY KEY CLUSTERED ([CapabilityId] ASC)
);

