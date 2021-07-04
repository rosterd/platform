using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using FluentValidation;
using Rosterd.Domain.Models.FacilitiesModels;
using Rosterd.Domain.Models.JobModels;
using Rosterd.Domain.ValidationAttributes;
using Rosterd.Infrastructure.Extensions;

namespace Rosterd.Admin.Api.Requests.Job
{
    public class AddJobRequest
    {
        [Required]
        [StringLength(1000)]
        public string JobTitle { get; set; }

        [Required]
        [StringLength(8000)]
        public string Description { get; set; }

        [ValidNumberRequired]
        public long? FacilityId { get; set; }

        [Required]
        public DateTime JobStartDateTimeUtc { get; set; }

        [Required]
        public DateTime JobEndDateTimeUtc { get; set; }

        [StringLength(1000)]
        public string Comments { get; set; }

        public long? GracePeriodToCancelMinutes { get; set; }

        public bool? NoGracePeriod { get; set; }

        [StringLength(2000)]
        public string Responsibilities { get; set; }

        [StringLength(2000)]
        public string Experience { get; set; }

        public bool IsDayShift { get; set; }

        public bool IsNightShift { get; set; }

        [CollectionIsRequiredAndShouldNotBeEmpty]
        public List<long> SkillsRequiredForJob { get; set; }

        /// <summary>
        /// Only required if an existing job was cancelled and new one created based of the old one
        /// </summary>
        public long? PreviouslyCancelledJobId { get; set; }

        public JobModel ToDomainModel()
        {
            var jobModel = new JobModel
            {
                JobTitle = JobTitle,
                Description = Description,
                Facility = new FacilityModel { FacilityId = FacilityId.Value },
                JobStartDateTimeUtc = JobStartDateTimeUtc,
                JobEndDateTimeUtc = JobEndDateTimeUtc,
                Comments = Comments,
                GracePeriodToCancelMinutes = GracePeriodToCancelMinutes,
                NoGracePeriod = NoGracePeriod,
                Responsibilities = Responsibilities,
                Experience = Experience,
                IsDayShift = IsDayShift,
                IsNightShift = IsNightShift,
                JobSkills = SkillsRequiredForJob.AlwaysList().Select(s => new JobSkillModel
                {
                    SkillId = s
                }).AlwaysList()
            };

            return jobModel;
        }
    }

    public class AddJobRequestValidator : AbstractValidator<AddJobRequest>
    {
        public AddJobRequestValidator()
        {
            //Grace period validation
            RuleFor(x => x.NoGracePeriod).Custom((noGracePeriod, context) => {
                var gracePeriodToCancelMinutes = ((AddJobRequest)context.InstanceToValidate).GracePeriodToCancelMinutes;

                if(noGracePeriod == null && gracePeriodToCancelMinutes == null)
                    context.AddFailure("Either no-grace-period' or 'grace-period-to-cancel-minutes' must be specified");
            });

            //Day or night shift validation
            RuleFor(x => x.IsDayShift).Custom((isDayShift, context) => {
                var isNightShift = ((AddJobRequest)context.InstanceToValidate).IsNightShift;

                if(isDayShift && isNightShift)
                    context.AddFailure("Job cant both night and day shift");

                if(!isDayShift && !isNightShift)
                    context.AddFailure("Job has to be either a day or night shift");
            });

            //To date should be greater than from date
            RuleFor(x => x.JobEndDateTimeUtc).Custom((jobEndDateTimeUtc, context) => {
                var jobStartDateTimeUtc = ((AddJobRequest)context.InstanceToValidate).JobStartDateTimeUtc;

                if(jobStartDateTimeUtc >= jobEndDateTimeUtc)
                    context.AddFailure("Job end date time must be greater than start date time");
            });

            //Other validations we can doo
            //1. Check if the facility is valid
            //2. Check if the user creating can create a job
            //3. ...
        }
    }
}
