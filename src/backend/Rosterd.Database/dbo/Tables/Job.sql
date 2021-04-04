CREATE TABLE [dbo].[Job] (
    [JobId]                          BIGINT          IDENTITY (1, 1) NOT NULL,
    [JobTitle]                       NVARCHAR (1000) NOT NULL,
    [Description]                    VARCHAR (8000)  NOT NULL,
    [FacilityId]                     BIGINT          NOT NULL,
    [JobStartDateTimeUTC]            DATETIME2 (7)   NOT NULL,
    [JobEndDateTimeUTC]              DATETIME2 (7)   NOT NULL,
    [JobPostedDateTimeUTC]           DATETIME2 (7)   NOT NULL,
    [Comments]                       NVARCHAR (1000) NULL,
    [GracePeriodToCancelMinutes]     BIGINT          NULL,
    [NoGracePeriod]                  BIT             NULL,
    [JobStatusId]                    BIGINT          NOT NULL,
    [JobsStatusName]                 NVARCHAR (1000) NOT NULL,
    [LastJobStatusChangeDateTimeUTC] DATETIME2 (7)   NOT NULL,
    [Responsibilities]               NVARCHAR (MAX)  NULL,
    [Experience]                     NVARCHAR (MAX)  NULL,
    [PreviouslyCancelledJobId]       BIGINT          NULL,
    [IsDayShift]                     BIT             NULL,
    [IsNightShift]                   BIT             NULL,
    CONSTRAINT [Pk_Job_JobId] PRIMARY KEY CLUSTERED ([JobId] ASC),
    CONSTRAINT [Fk_Job_Facility] FOREIGN KEY ([FacilityId]) REFERENCES [dbo].[Facility] ([FacilityId]),
    CONSTRAINT [Unq_Job_JobId] UNIQUE NONCLUSTERED ([JobId] ASC)
);








GO
CREATE NONCLUSTERED INDEX [IX_Fk_Job_Facility]
    ON [dbo].[Job]([FacilityId] ASC);

