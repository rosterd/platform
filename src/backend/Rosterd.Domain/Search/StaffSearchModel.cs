using System.Collections.Generic;
using Azure.Search.Documents.Indexes;
using Azure.Search.Documents.Indexes.Models;

namespace Rosterd.Domain.Search
{
    public class StaffSearchModel
    {
        public static string Key() => nameof(StaffId);

        [SearchableField(AnalyzerName = LexicalAnalyzerName.Values.EnLucene)]
        public string Auth0OrganizationId { get; set; }

        [SearchableField(AnalyzerName = LexicalAnalyzerName.Values.EnLucene)]
        public string Auth0IdForStaff { get; set; }

        [SimpleField(IsKey = true, IsFilterable = true)]
        public string StaffId { get; set; }

        [SearchableField(AnalyzerName = LexicalAnalyzerName.Values.EnLucene)]
        public string FirstName { get; set; }

        [SearchableField(AnalyzerName = LexicalAnalyzerName.Values.EnLucene)]
        public string LastName { get; set; }

        [SearchableField(AnalyzerName = LexicalAnalyzerName.Values.EnLucene)]
        public string Email { get; set; }

        [SearchableField]
        public string MobilePhoneNumber { get; set; }

        [SimpleField]
        public bool IsActive { get; set; }

        [SearchableField(AnalyzerName = LexicalAnalyzerName.Values.EnLucene)]
        public string JobTitle { get; set; }

        [SearchableField(AnalyzerName = LexicalAnalyzerName.Values.EnLucene)]
        public string[] SkillsIds { get; set; }

        [SearchableField(AnalyzerName = LexicalAnalyzerName.Values.EnLucene)]
        public string[] SkillNames { get; set; }

        [SearchableField(AnalyzerName = LexicalAnalyzerName.Values.EnLucene)]
        public string SkillsNamesCsvString => SkillNames.IsNullOrEmpty() ? string.Empty : string.Join(',', SkillNames);

        [SearchableField(AnalyzerName = LexicalAnalyzerName.Values.EnLucene)]
        public string SkillIdsCsvString => SkillsIds.IsNullOrEmpty() ? string.Empty : string.Join(',', SkillsIds);

        //----------Staff Preferences

        [SearchableField(AnalyzerName = LexicalAnalyzerName.Values.EnLucene)]
        public string StaffPreferenceCity { get; set; } = string.Empty;

        [SimpleField]
        public bool StaffPreferenceIsNightShiftOk { get; set; }

        [SearchableField(AnalyzerName = LexicalAnalyzerName.Values.EnLucene)]
        public string DeviceId { get; set; } = string.Empty;
    }
}
