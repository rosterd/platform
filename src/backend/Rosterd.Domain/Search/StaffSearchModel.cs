using System.Collections.Generic;
using Azure.Search.Documents.Indexes;
using Azure.Search.Documents.Indexes.Models;

namespace Rosterd.Domain.Search
{
    public class StaffSearchModel
    {
        public static string Key() => nameof(StaffId);

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

        [SearchableField]
        public string IsActive { get; set; }

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
    }
}
