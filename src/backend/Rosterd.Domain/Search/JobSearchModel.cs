using System;
using System.Collections.Generic;
using Azure.Search.Documents.Indexes;
using Azure.Search.Documents.Indexes.Models;

namespace Rosterd.Domain.Search
{
    public class JobSearchModel
    {
        public static string Key() => nameof(JobId);

        [SearchableField(AnalyzerName = LexicalAnalyzerName.Values.EnLucene)]
        public string Auth0OrganizationId { get; set; }

        [SimpleField(IsKey = true, IsFilterable = true, IsSortable = true)]
        public string JobId { get; set; }

        [SearchableField(AnalyzerName = LexicalAnalyzerName.Values.EnLucene)]
        public string JobTitle { get; set; }

        [SearchableField(AnalyzerName = LexicalAnalyzerName.Values.EnLucene)]
        public string Description { get; set; }

        [SimpleField(IsFilterable = true, IsSortable = true, IsFacetable = true)]
        public DateTimeOffset JobStartDateTimeUtc { get; set; }

        [SimpleField(IsFilterable = true, IsSortable = true, IsFacetable = true)]
        public DateTimeOffset JobEndDateTimeUtc { get; set; }

        [SimpleField(IsFilterable = true, IsSortable = true, IsFacetable = true)]
        public DateTimeOffset JobPostedDateTimeUtc { get; set; }

        [SearchableField]
        public string Comments { get; set; }

        [SearchableField]
        public string GracePeriodToCancelMinutes { get; set; }

        [SimpleField]
        public bool NoGracePeriod { get; set; }

        [SimpleField(IsFilterable = true, IsSortable = true, IsFacetable = true)]
        public DateTimeOffset? JobGracePeriodEndDateTimeUtc { get; set; }

        [SearchableField(AnalyzerName = LexicalAnalyzerName.Values.EnLucene)]
        public string JobStatusName { get; set; }

        [SearchableField(AnalyzerName = LexicalAnalyzerName.Values.EnLucene)]
        public string Responsibilities { get; set; }

        [SearchableField(AnalyzerName = LexicalAnalyzerName.Values.EnLucene)]
        public string Experience { get; set; }

        [SimpleField(IsFilterable = true)]
        public bool IsNightShift { get; set; }

        [SearchableField(AnalyzerName = LexicalAnalyzerName.Values.EnLucene)]
        public string[] SkillsIds { get; set; }

        [SearchableField(AnalyzerName = LexicalAnalyzerName.Values.EnLucene)]
        public string[] SkillNames { get; set; }

        [SearchableField(AnalyzerName = LexicalAnalyzerName.Values.EnLucene)]
        public string SkillsNamesCsvString => SkillNames.IsNullOrEmpty() ? string.Empty : string.Join(',', SkillNames);

        [SearchableField(AnalyzerName = LexicalAnalyzerName.Values.EnLucene)]
        public string SkillIdsCsvString => SkillsIds.IsNullOrEmpty() ? string.Empty : string.Join(',', SkillsIds);

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

        [SimpleField]
        public string FacilityLatitude { get; set; }

        [SimpleField]
        public string FacilityLongitude { get; set; }
    }
}
