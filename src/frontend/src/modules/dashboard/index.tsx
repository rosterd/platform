import React, {useEffect, useState} from 'react';
// import {ResponsiveContainer, BarChart, Bar} from 'recharts';
import AppAnimate from '@crema/core/AppAnimate';
import {GridContainer} from '@crema';
import {Grid, Box, Theme} from '@material-ui/core';
import PersonIcon from '@material-ui/icons/Person';
import WorkIcon from '@material-ui/icons/Work';
import MonetizationOnIcon from '@material-ui/icons/MonetizationOn';
import {isBreakPointDown} from '@crema/utility/Utils';
import IntlMessages from '@crema/utility/IntlMessages';
import {red, blue, indigo} from '@material-ui/core/colors';
import makeStyles from '@material-ui/core/styles/makeStyles';
import {Fonts} from 'shared/constants/AppEnums';
import getDashboard from 'services/dashboard';
import useRequest from 'shared/hooks/useRequest';
import {components} from 'types/models/ApiModels';
import StatsCard from './components/StatsCard';

const useStyles = makeStyles<Theme>(() => ({
  textUppercase: {
    textTransform: 'uppercase',
  },
}));

type DashboardModel = components['schemas']['DashboardModel'];

const Dashboard = (): JSX.Element => {
  const classes = useStyles();
  const [stats, setStats] = useState<DashboardModel>({});
  const {requestMaker} = useRequest();

  useEffect(() => {
    (async () => {
      const {url} = getDashboard();
      const response = await requestMaker<DashboardModel>({url});
      setStats(response);
    })();
  }, []);

  return (
    <AppAnimate animation='transition.slideUpIn' delay={200}>
      <>
        <Box component='h2' color='text.primary' className={classes.textUppercase} fontSize={16} mb={{xs: 4, sm: 4, xl: 6}} fontWeight={Fonts.BOLD}>
          <IntlMessages id='dashboard.quickStats' />
        </Box>
        <Box paddingTop={{xl: 4}} clone>
          <GridContainer>
            <Grid item xs={12} sm={6} md={4}>
              <StatsCard
                icon={<WorkIcon style={{fontSize: isBreakPointDown('md') ? 30 : 40}} />}
                bgColor={red[500]}
                data={{count: stats.totalJobs || 0}}
                heading={<IntlMessages id='dashboard.stats.jobs' />}
              />
            </Grid>
            <Grid item xs={12} sm={6} md={4}>
              <StatsCard
                icon={<PersonIcon style={{fontSize: isBreakPointDown('md') ? 30 : 40}} />}
                bgColor={blue[500]}
                data={{count: stats.totalStaff || 0}}
                heading={<IntlMessages id='dashboard.stats.staff' />}
              />
            </Grid>
            <Grid item xs={12} sm={6} md={4}>
              <StatsCard
                icon={<MonetizationOnIcon style={{fontSize: isBreakPointDown('md') ? 30 : 40}} />}
                bgColor={indigo[500]}
                data={{count: stats.amountSaved || 0}}
                heading={<IntlMessages id='dashboard.stats.save' />}
              />
            </Grid>
          </GridContainer>
        </Box>
        {/* <ResponsiveContainer width='50%' height='20%'>
          <BarChart width={100} height={500} data={data}>
            <Bar dataKey='uv' fill='#0D2273' />
          </BarChart>
        </ResponsiveContainer> */}
      </>
    </AppAnimate>
  );
};

export default Dashboard;
