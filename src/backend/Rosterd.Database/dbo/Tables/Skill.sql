CREATE TABLE [dbo].[Skill] (
    [SkillId]        BIGINT          IDENTITY (1, 1) NOT NULL,
    [OrganizationId] BIGINT          NOT NULL,
    [SkillName]      NVARCHAR (1000) NOT NULL,
    [Description]    NVARCHAR (1000) NULL,
    CONSTRAINT [Pk_Skill_SkillId] PRIMARY KEY CLUSTERED ([SkillId] ASC),
    CONSTRAINT [FK_Skill_Organization] FOREIGN KEY ([OrganizationId]) REFERENCES [dbo].[Organization] ([OrganizationId])
);






GO
CREATE NONCLUSTERED INDEX [IX_Skill_OrganizationId]
    ON [dbo].[Skill]([OrganizationId] ASC);

