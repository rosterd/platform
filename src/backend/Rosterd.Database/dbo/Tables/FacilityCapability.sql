CREATE TABLE [dbo].[FacilityCapability] (
    [FacilityCapabilityId] BIGINT IDENTITY (1, 1) NOT NULL,
    [FacilityId]           BIGINT NOT NULL,
    [CapabilityId]         BIGINT NOT NULL,
    CONSTRAINT [Pk_FacilityCapability_FacilityCapabilityId] PRIMARY KEY CLUSTERED ([FacilityCapabilityId] ASC),
    CONSTRAINT [Fk_FacilityCapability_Capability] FOREIGN KEY ([CapabilityId]) REFERENCES [dbo].[Capability] ([CapabilityId]),
    CONSTRAINT [Fk_FacilityCapability_Facility] FOREIGN KEY ([FacilityId]) REFERENCES [dbo].[Facility] ([FacilityId])
);




GO
CREATE NONCLUSTERED INDEX [IX_Fk_FacilityCapability_Facility]
    ON [dbo].[FacilityCapability]([FacilityId] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_Fk_FacilityCapability_Capability]
    ON [dbo].[FacilityCapability]([CapabilityId] ASC);

