CREATE TABLE dbo.Notification (
     [NotificationId]       BIGINT  NOT NULL,
     [Title]                VARCHAR  NOT NULL,
     [Description]          VARCHAR  NOT NULL,
     [CreatedDateTime]      DATETIME  NOT NULL,
     [IsRead]               BIT  NOT NULL DEFAULT 0,
     [JobId]                BIGINT  NOT NULL,
     CONSTRAINT pk_Notification PRIMARY KEY ( NotificationId ),
     CONSTRAINT fk_Job FOREIGN KEY ( JobId ) REFERENCES dbo.Job( JobId )
);
