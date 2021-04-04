CREATE TABLE [dbo].[StaffFacility] (
    [StaffFacilityId] BIGINT          IDENTITY (1, 1) NOT NULL,
    [StaffId]         BIGINT          NOT NULL,
    [FacilityId]      BIGINT          NOT NULL,
    [FacilityName]    NVARCHAR (1000) NULL,
    CONSTRAINT [Pk_StaffFacility_StaffFacilityId] PRIMARY KEY CLUSTERED ([StaffFacilityId] ASC),
    CONSTRAINT [Fk_StaffFacility_Staff] FOREIGN KEY ([StaffId]) REFERENCES [dbo].[Staff] ([StaffId])
);




GO
CREATE NONCLUSTERED INDEX [IX_Fk_StaffFacility_Staff]
    ON [dbo].[StaffFacility]([StaffId] ASC);

