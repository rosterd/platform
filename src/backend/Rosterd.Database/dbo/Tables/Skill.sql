CREATE TABLE [dbo].[Skill] (
    [SkillId]     BIGINT          IDENTITY (1, 1) NOT NULL,
    [SkillName]   NVARCHAR (1000) NOT NULL,
    [Description] NVARCHAR (1000) NULL,
    CONSTRAINT [Pk_Skill_SkillId] PRIMARY KEY CLUSTERED ([SkillId] ASC)
);



