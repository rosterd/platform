CREATE TABLE [dbo].[JobStatusChange] (
    [JobStatusChangeId]          BIGINT          IDENTITY (1, 1) NOT NULL,
    [JobId]                      BIGINT          NOT NULL,
    [JobStatusId]                BIGINT          NOT NULL,
    [JobStatusChangeDateTimeUTC] DATETIME2 (1)   CONSTRAINT [defo_JobStatusChange_JobStatusChangeDateTimeUTC] DEFAULT (switchoffset(sysdatetimeoffset(),'+00:00')) NOT NULL,
    [JobStatusChangeReason]      NVARCHAR (1000) NULL,
    [JobStatusName]              NVARCHAR (1000) NULL,
    CONSTRAINT [Pk_JobStatusChange_JobStatusChangeId] PRIMARY KEY CLUSTERED ([JobStatusChangeId] ASC)
);

