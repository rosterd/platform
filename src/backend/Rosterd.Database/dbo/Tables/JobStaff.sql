CREATE TABLE [dbo].[JobStaff] (
    [JobStaffId]        BIGINT        IDENTITY (1, 1) NOT NULL,
    [JobId]             BIGINT        NOT NULL,
    [StaffId]           BIGINT        NOT NULL,
    [UpdateDateTimeUtc] DATETIME2 (7) NOT NULL,
    CONSTRAINT [Pk_JobStaff_JobStaffId] PRIMARY KEY CLUSTERED ([JobStaffId] ASC),
    CONSTRAINT [Fk_JobStaff_Job] FOREIGN KEY ([JobId]) REFERENCES [dbo].[Job] ([JobId]),
    CONSTRAINT [Fk_JobStaff_Staff] FOREIGN KEY ([StaffId]) REFERENCES [dbo].[Staff] ([StaffId])
);






GO



GO
CREATE NONCLUSTERED INDEX [IX_JobId_StaffId]
    ON [dbo].[JobStaff]([JobId] ASC, [StaffId] ASC);

