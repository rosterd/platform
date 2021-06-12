using System;
using System.Collections.Generic;
using Azure.Search.Documents.Indexes;
using Azure.Search.Documents.Indexes.Models;

namespace Rosterd.Domain.Search
{
    public class JobSearchModel
    {
        public static string Key() => nameof(JobId);

        [SimpleField(IsKey = true, IsFilterable = true)]
        public string JobId { get; set; }

        [SearchableField(AnalyzerName = LexicalAnalyzerName.Values.EnLucene)]
        public string JobTitle { get; set; }

        [SearchableField(AnalyzerName = LexicalAnalyzerName.Values.EnLucene)]
        public string Description { get; set; }

        [SearchableField]
        public DateTime JobStartDateTimeUtc { get; set; }

        [SearchableField]
        public DateTime JobEndDateTimeUtc { get; set; }

        [SearchableField]
        public DateTime JobPostedDateTimeUtc { get; set; }

        [SearchableField]
        public string Comments { get; set; }

        [SearchableField]
        public long? GracePeriodToCancelMinutes { get; set; }

        [SearchableField]
        public bool? NoGracePeriod { get; set; }

        [SearchableField]
        public DateTime? JobGracePeriodEndDateTimeUtc { get; set; }

        [SearchableField(AnalyzerName = LexicalAnalyzerName.Values.EnLucene)]
        public string JobStatusName { get; set; }

        [SearchableField(AnalyzerName = LexicalAnalyzerName.Values.EnLucene)]
        public string Responsibilities { get; set; }

        [SearchableField(AnalyzerName = LexicalAnalyzerName.Values.EnLucene)]
        public string Experience { get; set; }

        [SearchableField]
        public bool IsDayShift { get; set; }

        [SearchableField]
        public bool IsNightShift { get; set; }

        [SearchableField(AnalyzerName = LexicalAnalyzerName.Values.EnLucene)]
        public string[] Skills { get; set; }

        [SearchableField(AnalyzerName = LexicalAnalyzerName.Values.EnLucene)]
        public string SkillsSpaceSeperatedString => Skills.IsNullOrEmpty() ? string.Empty : string.Join(' ', Skills);

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
