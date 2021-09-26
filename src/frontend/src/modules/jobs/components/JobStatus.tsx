import * as React from 'react';
import MenuItem from '@material-ui/core/MenuItem';
import Select from '@material-ui/core/Select';
import {components} from 'types/models';
import makeStyles from '@material-ui/core/styles/makeStyles';

type JobStatus = components['schemas']['JobStatus'];
interface Props {
  status: string;
  onChange: (string) => void;
}

interface JobStatusWithLabel {
  label: string;
  value: JobStatus;
}
const jobStatus: JobStatusWithLabel[] = [
  {label: 'Published', value: 'Published'},
  {label: 'Accepted', value: 'Accepted'},
  {label: 'No Show', value: 'NoShow'},
  {label: 'In Progress', value: 'InProgress'},
  {label: 'Feedback Pending', value: 'FeedbackPending'},
  {label: 'Completed', value: 'Completed'},
  {label: 'Expired', value: 'Expired'},
  {label: 'Cancelled', value: 'Cancelled'},
];

const useStyles = makeStyles(() => ({
  root: {
    minWidth: '200px',
  },
}));

const JobStatus = (props: Props): JSX.Element => {
  const classes = useStyles();
  const {status, onChange} = props;

  const handleChange = (event) => {
    onChange(event.target.value);
  };

  return (
    <Select labelId='jobsStatus' id='jobsStatus' value={status} onChange={handleChange} label='Job Status' className={classes.root}>
      {jobStatus.map(({label, value}) => (
        <MenuItem value={value} key={value}>
          {label}
        </MenuItem>
      ))}
    </Select>
  );
};

export default JobStatus;
