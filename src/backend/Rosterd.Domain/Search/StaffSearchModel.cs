using System;
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

        [SimpleField(IsFilterable = true)]
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

        [SimpleField(IsFilterable = true)]
        public bool StaffPreferenceIsNightShiftOk { get; set; }

        [SearchableField(AnalyzerName = LexicalAnalyzerName.Values.EnLucene)]
        public string DeviceId { get; set; } = string.Empty;

        [SimpleField(IsFilterable = true)]
        public bool MondayAvailable { get; set; }

        [SimpleField(IsFilterable = true)]
        public bool TuesdayAvailable { get; set; }

        [SimpleField(IsFilterable = true)]
        public bool WednesdayAvailable { get; set; }

        [SimpleField(IsFilterable = true)]
        public bool ThursdayAvailable { get; set; }

        [SimpleField(IsFilterable = true)]
        public bool FridayAvailable { get; set; }

        [SimpleField(IsFilterable = true)]
        public bool SaturdayAvailable { get; set; }

        [SimpleField(IsFilterable = true)]
        public bool SundayAvailable { get; set; }

        public static string GetDayOfWeekFilter(DayOfWeek dayOfWeek)
        {
            switch (dayOfWeek)
            {
                case DayOfWeek.Monday:
                    return nameof(MondayAvailable);

                case DayOfWeek.Tuesday:
                    return nameof(TuesdayAvailable);

                case DayOfWeek.Wednesday:
                    return nameof(WednesdayAvailable);

                case DayOfWeek.Thursday:
                    return nameof(ThursdayAvailable);

                case DayOfWeek.Friday:
                    return nameof(FridayAvailable);

                case DayOfWeek.Saturday:
                    return nameof(SaturdayAvailable);

                case DayOfWeek.Sunday:
                    return nameof(SundayAvailable);
            }

            return string.Empty;
        }
    }
}
