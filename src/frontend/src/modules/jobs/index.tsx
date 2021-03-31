import React, {useState} from 'react';
import Box from '@material-ui/core/Box';
import AppAnimate from '@crema/core/AppAnimate';
import {
  AppBar,
  Tabs,
  Tab,
  makeStyles,
  Typography,
  Button,
  Grid,
} from '@material-ui/core';
import MaterialTable from 'material-table';
import IntlMessages from '@crema/utility/IntlMessages';
import {Fonts} from 'shared/constants/AppEnums';
import AddIcon from '@material-ui/icons/Add';
import PublishJobModal from './components/PublishJobModal';

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
    <div
      role='tabpanel'
      hidden={value !== index}
      id={`simple-tabpanel-${index}`}
      aria-labelledby={`simple-tab-${index}`}
      // eslint-disable-next-line react/jsx-props-no-spreading
      {...other}>
      {value === index && (
        <Box p={3}>
          <Typography>{children}</Typography>
        </Box>
      )}
    </div>
  );
}

const Jobs = (): JSX.Element => {
  const classes = useStyles();
  const [tabIndex, setTabIndex] = useState(0);
  const [showJobModal, setShowJobModal] = useState(false);

  const handleTabChange = (_: any, newValue: number) => {
    setTabIndex(newValue);
  };
  return (
    <AppAnimate animation='transition.slideUpIn' delay={200}>
      <Box>
        <Box mb={{xs: 4, sm: 4, xl: 6}}>
          <Grid
            container
            direction='row'
            justify='space-between'
            alignItems='center'>
            <Grid item xs={6}>
              <Box
                component='h2'
                color='text.primary'
                fontSize={16}
                fontWeight={Fonts.BOLD}>
                <IntlMessages id='jobs.heading' />
              </Box>
            </Grid>
            <Grid item xs={6} className={classes.buttonContainer}>
              <Button
                variant='contained'
                color='primary'
                startIcon={<AddIcon />}
                onClick={() => setShowJobModal(true)}>
                Publish Job
              </Button>
            </Grid>
          </Grid>
        </Box>
        <Box>
          <div className={classes.root}>
            <AppBar position='static'>
              <Tabs
                value={tabIndex}
                onChange={handleTabChange}
                aria-label='simple tabs example'>
                <Tab label='Active' />
                <Tab label='Fulfilled' />
              </Tabs>
            </AppBar>
            <TabPanel value={tabIndex} index={0}>
              <MaterialTable
                title=''
                columns={[
                  {title: 'Title', field: 'title'},
                  {title: 'From', field: 'from', type: 'date'},
                  {title: 'To', field: 'to', type: 'date'},
                  {title: 'Status', field: 'status'},
                  {title: 'Facility', field: 'facility'},
                ]}
                data={[
                  {
                    title: 'Staff Nurse',
                    from: '12/04/2021 11:00AM',
                    to: '13/04/2021 11:00AM',
                    status: 'Accepted',
                    facility: 'Mt Roskill',
                  },
                  {
                    title: 'Staff Nurse',
                    from: '11/04/2021 10:00PM',
                    to: '12/04/2021 06:00AM',
                    status: 'Pending',
                    facility: 'Mt Roskill',
                  },
                ]}
              />
            </TabPanel>
            <TabPanel value={tabIndex} index={1}>
              <MaterialTable
                title=''
                columns={[
                  {title: 'Title', field: 'title'},
                  {title: 'From', field: 'from', type: 'date'},
                  {title: 'To', field: 'to', type: 'date'},
                  {title: 'Status', field: 'status'},
                  {title: 'Facility', field: 'facility'},
                ]}
                data={[
                  {
                    title: 'Healthcare Assistant Level 2',
                    from: '12/03/2021 11:00AM',
                    to: '13/03/2021 05:00PM',
                    status: 'Completed',
                    facility: 'Mt Roskill',
                  },
                  {
                    title: 'Chef',
                    from: '02/02/2021 11:00AM',
                    to: '03/02/2021 12:00AM',
                    status: 'Completed',
                    facility: 'Mt Roskill',
                  },
                ]}
              />
            </TabPanel>
          </div>
        </Box>
        <PublishJobModal
          open={showJobModal}
          handleClose={() => setShowJobModal(false)}
        />
      </Box>
    </AppAnimate>
  );
};

export default Jobs;
