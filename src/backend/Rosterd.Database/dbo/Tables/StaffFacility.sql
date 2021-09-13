CREATE TABLE [dbo].[StaffFacility] (
    [StaffFacilityId] BIGINT IDENTITY (1, 1) NOT NULL,
    [StaffId]         BIGINT NOT NULL,
    [FacilityId]      BIGINT NOT NULL,
    CONSTRAINT [Pk_StaffFacility_StaffFacilityId] PRIMARY KEY CLUSTERED ([StaffFacilityId] ASC),
    CONSTRAINT [FK_StaffFacility_Staff] FOREIGN KEY ([StaffId]) REFERENCES [dbo].[Staff] ([StaffId])
);






GO
CREATE NONCLUSTERED INDEX [IX_StaffFacility_StaffId_FacilityId]
    ON [dbo].[StaffFacility]([StaffId] ASC, [FacilityId] ASC);

