CREATE TABLE [dbo].[StaffSkill] (
    [StaffSkillId] BIGINT          IDENTITY (1, 1) NOT NULL,
    [StaffId]      BIGINT          NOT NULL,
    [SkillId]      BIGINT          NOT NULL,
    [SkillName]    NVARCHAR (1000) NULL,
    CONSTRAINT [Pk_StaffSkill_StaffSkillId] PRIMARY KEY CLUSTERED ([StaffSkillId] ASC),
    CONSTRAINT [Fk_StaffSkill_Staff] FOREIGN KEY ([StaffId]) REFERENCES [dbo].[Staff] ([StaffId])
);

