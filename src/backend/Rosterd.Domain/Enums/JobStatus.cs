using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Rosterd.Domain.Exceptions;

namespace Rosterd.Domain.Enums
{
    public enum JobStatus
    {
        [Display(Name = "Published", Description = "")]
        Published = 1,

        [Display(Name = "Accepted", Description = "")]
        Accepted = 2,

        [Display(Name = "No Show", Description = "")]
        NoShow = 3,

        [Display(Name = "In Progress", Description = "")]
        InProgress = 4,

        [Display(Name = "Feedback Pending", Description = "")]
        FeedbackPending = 5,

        [Display(Name = "Completed", Description = "")]
        Completed = 6,

        [Display(Name = "Expired", Description = "")]
        Expired = 7,

        [Display(Name = "Cancelled", Description = "")]
        Cancelled = 8,

        //DO NOT REMOVE THE COMMENT BELOW
        //If you are adding or removing any status here then update the JobStatusFinder static class dictionary to reflect
    }

    /// <summary>
    /// All Job status's need to be added here
    /// Enum.Parse() takes 3ms and is not good enough, dictionary look is by far the fastest.
    /// </summary>
    public static class JobStatusFinder
    {
        private static readonly Dictionary<string, JobStatus> JobStatusDisDictionary = new Dictionary<string, JobStatus>
        {
            { "Published", JobStatus.Published },
            { "Accepted", JobStatus.Accepted },
            { "NoShow", JobStatus.NoShow },
            { "InProgress", JobStatus.InProgress },
            { "FeedbackPending", JobStatus.FeedbackPending },
            { "Completed", JobStatus.Completed },
            { "Expired", JobStatus.Expired },
            { "Cancelled", JobStatus.Cancelled }
        };


        public static JobStatus? GetJobStatus(string jobStatusEnumName)
        {
            if (JobStatusDisDictionary.TryGetValue(jobStatusEnumName, out var jobStatus))
                return jobStatus;

            throw new EntityNotFoundException($"The job status {jobStatusEnumName} was not found");
        }
    }
}
