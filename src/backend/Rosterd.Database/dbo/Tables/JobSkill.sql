CREATE TABLE [dbo].[JobSkill] (
    [JobSkillId] BIGINT          IDENTITY (1, 1) NOT NULL,
    [JobId]      BIGINT          NOT NULL,
    [SkillId]    BIGINT          NOT NULL,
    [SkillName]  NVARCHAR (1000) NULL,
    CONSTRAINT [Pk_Jobskill_JobSkillId] PRIMARY KEY CLUSTERED ([JobSkillId] ASC),
    CONSTRAINT [Fk_Jobskill_Job] FOREIGN KEY ([JobId]) REFERENCES [dbo].[Job] ([JobId])
);






GO
CREATE NONCLUSTERED INDEX [IX_Fk_Jobskill_Job]
    ON [dbo].[JobSkill]([JobId] ASC);

