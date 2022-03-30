import React, {useState} from 'react';
import moment from 'moment';
import Box from '@material-ui/core/Box';
import AppAnimate from '@crema/core/AppAnimate';
import {makeStyles, Button, Grid} from '@material-ui/core';
import MaterialTable, {MTableToolbar} from 'material-table';
import IntlMessages from '@crema/utility/IntlMessages';
import {Fonts} from 'shared/constants/AppEnums';
import AddIcon from '@material-ui/icons/Add';
import {components} from 'types/models';
import {DeleteJobRequest} from 'services';
import usePaging from 'shared/hooks/usePaging';
import PublishJobModal from './components/PublishJobModal';
import JobModal from './components/JobModal';
import DeleteJobModal from './components/DeleteJobModal';
import JobStatus from './components/JobStatus';

type GetJobsResponse = components['schemas']['JobModelPagedList'];
type Job = components['schemas']['JobModel'];

const useStyles = makeStyles((theme) => ({
  root: {
    flexGrow: 1,
    backgroundColor: theme.palette.background.paper,
  },
  buttonContainer: {
    textAlign: 'right',
  },
}));

const Jobs = (): JSX.Element => {
  const classes = useStyles();
  const [status, setStatus] = useState('Published');
  const [showJobModal, setShowJobModal] = useState(false);
  const [jobDetails, setJobDetails] = useState({} as Job);
  const [deleteJobId, setDeleteJobId] = useState<undefined | number>(undefined);
  const [showAddJobModal, setShowAddJobModal] = useState(false);
  const [showDeleteJobModal, setShowDeleteJobModal] = useState(false);
  const {handlePageChange, currentPage, totalCount, items, loading, fetchData, addData, deleteData, setLoading} = usePaging<Job, GetJobsResponse>(
    `jobs?status=${status}`,

    'jobId',
  );

  const handleStatusChange = (jobStatus) => {
    setStatus(jobStatus);
    fetchData({url: `jobs?status=${jobStatus}`});
  };

  const handleClose = () => {
    setShowAddJobModal(false);
    setShowJobModal(false);
    setShowDeleteJobModal(false);
    setLoading(false);
  };

  const handleRowClick = (_: any, rowData?: Job) => {
    setJobDetails(rowData || ({} as Job));
    setShowJobModal(true);
  };

  const handleDeleteJob = async (value: DeleteJobRequest) => deleteData(deleteJobId, value);

  const onDeleteJob = (_: any, deletedJob) => {
    setDeleteJobId(deletedJob?.jobId);
    setShowDeleteJobModal(true);
  };

  const getLocalDateTime = (dateTime) => moment.utc(dateTime).local().format('DD/MM/YYYY hh:mm:ss A');

  return (
    <AppAnimate animation='transition.slideUpIn' delay={200}>
      <Box>
        <Box mb={{xs: 4, sm: 4, xl: 6}}>
          <Grid container direction='row' justify='space-between' alignItems='center'>
            <Grid item xs={6}>
              <Box component='h2' color='text.primary' fontSize={16} fontWeight={Fonts.BOLD}>
                <IntlMessages id='jobs.heading' />
              </Box>
            </Grid>
            <Grid item xs={6} className={classes.buttonContainer}>
              <Button variant='contained' color='primary' startIcon={<AddIcon />} onClick={() => setShowAddJobModal(true)}>
                Publish Job
              </Button>
            </Grid>
          </Grid>
        </Box>
        <Box>
          <div className={classes.root}>
            <MaterialTable
              onRowClick={handleRowClick}
              title=''
              columns={[
                {title: 'Title', field: 'jobTitle'},
                {
                  title: 'From',
                  field: 'jobStartDateTimeUtc',
                  type: 'datetime',
                  // eslint-disable-next-line react/prop-types
                  render: ({jobStartDateTimeUtc}) => <span>{getLocalDateTime(jobStartDateTimeUtc)}</span>,
                },
                {
                  title: 'To',
                  field: 'jobEndDateTimeUtc',
                  type: 'datetime',
                  // eslint-disable-next-line react/prop-types
                  render: ({jobEndDateTimeUtc}) => <span>{getLocalDateTime(jobEndDateTimeUtc)}</span>,
                },
                {title: 'Status', field: 'jobStatus'},
              ]}
              actions={[
                {
                  icon: 'delete',
                  tooltip: 'delete job',
                  onClick: onDeleteJob,
                },
              ]}
              options={{
                actionsColumnIndex: -1,
              }}
              components={{
                Toolbar: (props) => (
                  <div style={{display: 'flex', justifyContent: 'flex-end', alignItems: 'center'}}>
                    <JobStatus status={status} onChange={handleStatusChange} />
                    <MTableToolbar {...props} />
                  </div>
                ),
              }}
              onChangePage={handlePageChange}
              page={currentPage}
              totalCount={totalCount}
              isLoading={loading}
              data={items.filter((job) => job.jobStatus === status) || []}
            />
          </div>
        </Box>
        <PublishJobModal open={showAddJobModal} handleClose={handleClose} onPublishJob={addData} />
        <JobModal open={showJobModal} handleClose={handleClose} details={jobDetails} />
        <DeleteJobModal open={showDeleteJobModal} handleClose={handleClose} onDeleteJob={handleDeleteJob} />
      </Box>
    </AppAnimate>
  );
};

export default Jobs;
