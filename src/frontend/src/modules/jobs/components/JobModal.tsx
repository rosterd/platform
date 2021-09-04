import {createStyles, Dialog, DialogContent, DialogTitle, makeStyles, Theme} from '@material-ui/core';
import TextField from '@material-ui/core/TextField';
import React from 'react';
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
    jobSkills,
    experience,
    comments,
    responsibilities,
  } = details;

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
          defaultValue={jobStartDateTimeUtc}
          InputProps={{
            readOnly: true,
          }}
        />
        <TextField
          label='End Time'
          defaultValue={jobEndDateTimeUtc}
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
          defaultValue={(jobSkills || []).join(',')}
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
