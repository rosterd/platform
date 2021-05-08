using System.Collections.Generic;
using Azure.Search.Documents.Indexes;
using Azure.Search.Documents.Indexes.Models;

namespace Rosterd.Domain.Search
{
    public class StaffSearchModel
    {
        [SimpleField(IsKey = true, IsFilterable = true)]
        public long StaffId { get; set; }

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
        public bool IsActive { get; set; }

        [SearchableField(AnalyzerName = LexicalAnalyzerName.Values.EnLucene, IsFilterable = true, IsSortable = true)]
        public string JobTitle { get; set; }

        [SearchableField(AnalyzerName = LexicalAnalyzerName.Values.EnLucene, IsFilterable = true, IsSortable = true, IsFacetable = true)]
        public string[] Skills { get; set; }

        [SimpleField(IsKey = true, IsFilterable = true)]
        public long FacilityId { get; set; }

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
