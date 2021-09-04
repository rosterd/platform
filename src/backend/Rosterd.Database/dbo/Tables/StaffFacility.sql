CREATE TABLE [dbo].[StaffFacility] (
    [StaffFacilityId] BIGINT NOT NULL,
    [StaffId]         BIGINT NOT NULL,
    [FacilityId]      BIGINT NOT NULL,
    CONSTRAINT [Pk_StaffFacility_StaffFacilityId] PRIMARY KEY CLUSTERED ([StaffFacilityId] ASC),
    CONSTRAINT [FK_StaffFacility_Staff] FOREIGN KEY ([StaffId]) REFERENCES [dbo].[Staff] ([StaffId])
);

