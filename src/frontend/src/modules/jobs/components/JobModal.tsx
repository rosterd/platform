import {createStyles, Dialog, DialogContent, DialogTitle, makeStyles, Theme} from '@material-ui/core';
import TextField from '@material-ui/core/TextField';
import {AxiosRequestConfig} from 'axios';
import moment from 'moment';
import React, {useEffect, useState} from 'react';
import {getJob} from 'services';
import useRequest from 'shared/hooks/useRequest';
import {components} from 'types/models';

type Job = components['schemas']['JobModel'];

interface Props {
  details: Job;
  open: boolean;
  handleClose: () => void;
}

const useStyles = makeStyles((theme: Theme) =>
  createStyles({
    root: {
      display: 'flex',
      flexDirection: 'column',
      margin: theme.spacing(1),
    },
  }),
);

const JobModal = (props: Props): JSX.Element => {
  const classes = useStyles();

  const {details, open, handleClose} = props;
  const {
    jobTitle,
    description,
    jobStartDateTimeUtc,
    jobEndDateTimeUtc,
    gracePeriodToCancelMinutes,
    isNightShift,
    experience,
    comments,
    responsibilities,
  } = details;
  const [skillsString, setSkillsString] = useState('');
  const {requestMaker} = useRequest();

  const localStartDateTime = moment.utc(jobStartDateTimeUtc).local().format('YYYY-MM-DD hh:mm:ss A');
  const localEndDateTime = moment.utc(jobEndDateTimeUtc).local().format('YYYY-MM-DD hh:mm:ss A');

  const fetchData = async (config: AxiosRequestConfig) => {
    const job = await requestMaker<Job>(config);
    setSkillsString((job.jobSkills || []).map((jobSkill) => jobSkill.skillName).join(','));
  };

  useEffect(() => {
    (async () => {
      if (!details.jobId) {
        return;
      }
      await fetchData(getJob(details.jobId));
    })();
  }, [details]);

  return (
    <Dialog fullWidth maxWidth='sm' open={open} onClose={handleClose} aria-labelledby='job details'>
      <DialogTitle id='form-dialog-title'>Jobs Details</DialogTitle>
      <DialogContent className={classes.root}>
        <TextField
          label='jobTitle'
          disabled
          defaultValue={jobTitle}
          InputProps={{
            readOnly: true,
          }}
        />
        <TextField label='Description' disabled multiline defaultValue={description} />
        <TextField
          label='Start Time'
          defaultValue={localStartDateTime}
          InputProps={{
            readOnly: true,
          }}
        />
        <TextField
          label='End Time'
          defaultValue={localEndDateTime}
          InputProps={{
            readOnly: true,
          }}
        />

        <TextField
          label='Grace Period(in mins)'
          disabled
          defaultValue={gracePeriodToCancelMinutes}
          InputProps={{
            readOnly: true,
          }}
        />
        <TextField
          label='Night Shift'
          disabled
          defaultValue={isNightShift}
          InputProps={{
            readOnly: true,
          }}
        />
        <TextField
          label='Skills'
          disabled
          defaultValue={skillsString}
          InputProps={{
            readOnly: true,
          }}
        />
        <TextField
          label='Experience'
          disabled
          multiline
          defaultValue={experience}
          InputProps={{
            readOnly: true,
          }}
        />
        <TextField
          label='Comments'
          disabled
          multiline
          defaultValue={comments}
          InputProps={{
            readOnly: true,
          }}
        />
        <TextField
          label='Responsibilities'
          disabled
          multiline
          defaultValue={responsibilities}
          InputProps={{
            readOnly: true,
          }}
        />
      </DialogContent>
    </Dialog>
  );
};

export default JobModal;
