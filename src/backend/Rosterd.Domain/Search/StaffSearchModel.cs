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
        public string MiddleName { get; set; }

        [SearchableField(AnalyzerName = LexicalAnalyzerName.Values.EnLucene)]
        public string LastName { get; set; }

        [SearchableField(AnalyzerName = LexicalAnalyzerName.Values.EnLucene)]
        public string Email { get; set; }

        [SearchableField]
        public string HomePhoneNumber { get; set; }

        [SearchableField]
        public string MobilePhoneNumber { get; set; }

        [SearchableField]
        public string OtherPhoneNumber { get; set; }

        [SearchableField]
        public string IsActive { get; set; }

        [SearchableField(AnalyzerName = LexicalAnalyzerName.Values.EnLucene)]
        public string JobTitle { get; set; }

        [SearchableField(AnalyzerName = LexicalAnalyzerName.Values.EnLucene)]
        public string[] Skills { get; set; }

        [SearchableField(AnalyzerName = LexicalAnalyzerName.Values.EnLucene)]
        public string SkillsCsvString => Skills.IsNullOrEmpty() ? string.Empty : string.Join(',', Skills);

        [SearchableField]
        public string FacilityId { get; set; }

        [SearchableField(AnalyzerName = LexicalAnalyzerName.Values.EnLucene)]
        public string FacilityName { get; set; }

        [SearchableField(AnalyzerName = LexicalAnalyzerName.Values.EnLucene)]
        public string FacilityAddress { get; set; }

        [SearchableField(AnalyzerName = LexicalAnalyzerName.Values.EnLucene)]
        public string FacilitySuburb { get; set; }

        [SearchableField(AnalyzerName = LexicalAnalyzerName.Values.EnLucene)]
        public string FacilityCity { get; set; }

        [SearchableField(AnalyzerName = LexicalAnalyzerName.Values.EnLucene)]
        public string FacilityCountry { get; set; }

        [SearchableField(AnalyzerName = LexicalAnalyzerName.Values.EnLucene)]
        public string FacilityPhoneNumber1 { get; set; }

        [SearchableField(AnalyzerName = LexicalAnalyzerName.Values.EnLucene)]
        public string FacilityPhoneNumber2 { get; set; }
    }
}
