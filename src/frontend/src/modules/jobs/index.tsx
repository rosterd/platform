import React, {useEffect, useState} from 'react';
import {AxiosRequestConfig} from 'axios';
import Box from '@material-ui/core/Box';
import AppAnimate from '@crema/core/AppAnimate';
import {AppBar, Tabs, Tab, makeStyles, Typography, Button, Grid} from '@material-ui/core';
import MaterialTable from 'material-table';
import IntlMessages from '@crema/utility/IntlMessages';
import {Fonts} from 'shared/constants/AppEnums';
import AddIcon from '@material-ui/icons/Add';
import {components} from 'types/models';
import {deleteJob, DeleteJobRequest, getJobs, publishJob} from 'services';
import useRequest from 'shared/hooks/useRequest';
import {isPast, isFuture, parseISO} from 'date-fns';
import PublishJobModal from './components/PublishJobModal';
import JobModal from './components/JobModal';
import DeleteJobModal from './components/DeleteJobModal';

type GetJobsResponse = components['schemas']['JobModelPagedList'];
type AddJobRequest = components['schemas']['AddJobRequest'];
type Job = components['schemas']['JobModel'];

interface TabPanelProps {
  // eslint-disable-next-line react/require-default-props
  children?: React.ReactNode;
  index: number;
  value: any;
}

const useStyles = makeStyles((theme) => ({
  root: {
    flexGrow: 1,
    backgroundColor: theme.palette.background.paper,
  },
  buttonContainer: {
    textAlign: 'right',
  },
}));

function TabPanel(props: TabPanelProps) {
  const {children, value, index, ...other} = props;

  return (
    <div role='tabpanel' hidden={value !== index} id={`simple-tabpanel-${index}`} aria-labelledby={`simple-tab-${index}`} {...other}>
      {value === index && (
        <Box p={3}>
          <Typography>{children}</Typography>
        </Box>
      )}
    </div>
  );
}

const initialState: Job[] = [];

const Jobs = (): JSX.Element => {
  const classes = useStyles();
  const [tabIndex, setTabIndex] = useState(0);
  const [showJobModal, setShowJobModal] = useState(false);
  const [jobDetails, setJobDetails] = useState({} as Job);
  const [results, setResults] = useState({} as GetJobsResponse);
  const [activeJobs, setActiveJobs] = useState(initialState);
  const [jobs, setJobs] = useState(initialState);
  const [loading, setLoading] = useState(false);
  const [deleteJobId, setDeleteJobId] = useState<undefined | number>(undefined);
  const [showAddJobModal, setShowAddJobModal] = useState(false);
  const [showDeleteJobModal, setShowDeleteJobModal] = useState(false);

  const {requestMaker} = useRequest();

  const handleTabChange = (_: any, newValue: number) => {
    setTabIndex(newValue);
  };

  const getActiveJobs = (allJobs: Job[]) => allJobs.filter((job) => isFuture(parseISO(job.jobEndDateTimeUtc)) || job.jobStatus === 'Published');
  const getCompletedJobs = (allJobs: Job[]) =>
    allJobs.filter((job) => isPast(parseISO(job.jobEndDateTimeUtc)) || job.jobStatus !== 'Published' || job.jobStatus !== 'Published');

  const fetchData = async (config: AxiosRequestConfig) => {
    setLoading(true);
    const jobsResponse = await requestMaker<GetJobsResponse>(config);
    setLoading(false);
    setResults(jobsResponse);
    const allJobs = jobsResponse.items || [];
    setActiveJobs(getActiveJobs(allJobs));
    setJobs(getCompletedJobs(allJobs));
  };

  useEffect(() => {
    (async () => {
      await fetchData(getJobs());
    })();
  }, []);

  const handleClose = () => {
    setShowAddJobModal(false);
    setShowJobModal(false);
    setShowDeleteJobModal(false);
    setLoading(false);
  };

  const handlePublishJob = async (values: AddJobRequest) => {
    const jobRes = await requestMaker<Job>(publishJob(values));
    setLoading(false);
    if (jobRes) {
      setJobs([jobRes, ...jobs]);
    }
  };

  const handleRowClick = (_: any, rowData?: Job) => {
    setJobDetails(rowData || ({} as Job));
    setShowJobModal(true);
  };

  const handleDeleteJob = async (value: DeleteJobRequest) => {
    setLoading(true);
    await requestMaker(deleteJob(deleteJobId, value));
    setDeleteJobId(undefined);
    setLoading(false);
    setJobs(jobs.filter((job) => job.jobId !== deleteJobId));
  };

  const onDeleteJob = (_: any, deletedJob) => {
    setDeleteJobId(deletedJob?.jobId);
    setShowDeleteJobModal(true);
  };

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
            <AppBar position='static'>
              <Tabs value={tabIndex} onChange={handleTabChange} aria-label='Jobs'>
                <Tab label='Active' />
                <Tab label='Completed' />
              </Tabs>
            </AppBar>
            <TabPanel value={tabIndex} index={0}>
              <MaterialTable
                onRowClick={handleRowClick}
                title=''
                columns={[
                  {title: 'Title', field: 'jobTitle'},
                  {title: 'From', field: 'jobStartDateTimeUtc', type: 'datetime'},
                  {title: 'To', field: 'jobEndDateTimeUtc', type: 'datetime'},
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
                isLoading={loading}
                data={activeJobs}
                page={(results?.currentPage || 1) - 1}
                totalCount={results.totalCount}
              />
            </TabPanel>
            <TabPanel value={tabIndex} index={1}>
              <MaterialTable
                title=''
                isLoading={loading}
                columns={[
                  {title: 'Title', field: 'jobTitle'},
                  {title: 'From', field: 'jobStartDateTimeUtc', type: 'datetime'},
                  {title: 'To', field: 'jobEndDateTimeUtc', type: 'datetime'},
                  {title: 'Status', field: 'jobStatus'},
                ]}
                data={jobs}
                page={(results?.currentPage || 1) - 1}
                totalCount={results.totalCount}
              />
            </TabPanel>
          </div>
        </Box>
        <PublishJobModal open={showAddJobModal} handleClose={handleClose} onPublishJob={handlePublishJob} />
        <JobModal open={showJobModal} handleClose={handleClose} details={jobDetails} />
        <DeleteJobModal open={showDeleteJobModal} handleClose={handleClose} onDeleteJob={handleDeleteJob} />
      </Box>
    </AppAnimate>
  );
};

export default Jobs;
