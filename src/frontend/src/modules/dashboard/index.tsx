import React from 'react';
import {ResponsiveContainer, BarChart, Bar} from 'recharts';
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
import StatsCard from './components/StatsCard';

const useStyles = makeStyles<Theme>(() => ({
  textUppercase: {
    textTransform: 'uppercase',
  },
}));

const data = [
  {
    name: 'Page A',
    uv: 4000,
    pv: 2400,
    amt: 2400,
  },
  {
    name: 'Page B',
    uv: 3000,
    pv: 1398,
    amt: 2210,
  },
  {
    name: 'Page C',
    uv: 2000,
    pv: 9800,
    amt: 2290,
  },
  {
    name: 'Page D',
    uv: 2780,
    pv: 3908,
    amt: 2000,
  },
  {
    name: 'Page E',
    uv: 1890,
    pv: 4800,
    amt: 2181,
  },
  {
    name: 'Page F',
    uv: 2390,
    pv: 3800,
    amt: 2500,
  },
  {
    name: 'Page G',
    uv: 3490,
    pv: 4300,
    amt: 2100,
  },
];

const Dashboard = (): JSX.Element => {
  const classes = useStyles();

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
                data={{count: '20'}}
                heading={<IntlMessages id='dashboard.stats.jobs' />}
              />
            </Grid>
            <Grid item xs={12} sm={6} md={4}>
              <StatsCard
                icon={<PersonIcon style={{fontSize: isBreakPointDown('md') ? 30 : 40}} />}
                bgColor={blue[500]}
                data={{count: '16'}}
                heading={<IntlMessages id='dashboard.stats.staff' />}
              />
            </Grid>
            <Grid item xs={12} sm={6} md={4}>
              <StatsCard
                icon={<MonetizationOnIcon style={{fontSize: isBreakPointDown('md') ? 30 : 40}} />}
                bgColor={indigo[500]}
                data={{count: '2000'}}
                heading={<IntlMessages id='dashboard.stats.save' />}
              />
            </Grid>
          </GridContainer>
        </Box>
        <ResponsiveContainer width='50%' height='20%'>
          <BarChart width={100} height={500} data={data}>
            <Bar dataKey='uv' fill='#0D2273' />
          </BarChart>
        </ResponsiveContainer>
      </>
    </AppAnimate>
  );
};

export default Dashboard;
