import React from 'react';
import AppAnimate from '@crema/core/AppAnimate';
import {GridContainer} from '@crema';
import {Grid, Box} from '@material-ui/core';
import PersonIcon from '@material-ui/icons/Person';
import WorkIcon from '@material-ui/icons/Work';
import MonetizationOnIcon from '@material-ui/icons/MonetizationOn';
import StatsCard from './components/StatsCard';
import {isBreakPointDown} from '@crema/utility/Utils';
import IntlMessages from '@crema/utility/IntlMessages';
import {red, blue, indigo} from '@material-ui/core/colors';
import makeStyles from '@material-ui/core/styles/makeStyles';
import {Fonts} from 'shared/constants/AppEnums';

const useStyles = makeStyles((theme) => ({
  textUppercase: {
    textTransform: 'uppercase',
  },
}));

const Dashboard = () => {
  const classes = useStyles();

  return (
    <AppAnimate animation='transition.slideUpIn' delay={200}>
      <>
        <Box
          component='h2'
          color='text.primary'
          className={classes.textUppercase}
          fontSize={16}
          mb={{xs: 4, sm: 4, xl: 6}}
          fontWeight={Fonts.BOLD}>
          <IntlMessages id='dashboard.quickStats' />
        </Box>
        <Box paddingTop={{xl: 4}} clone>
          <GridContainer>
            <Grid item xs={12} sm={6} md={4}>
              <StatsCard
                icon={
                  <WorkIcon
                    style={{fontSize: isBreakPointDown('md') ? 30 : 40}}
                  />
                }
                bgColor={red[500]}
                data={{count: '20'}}
                heading={<IntlMessages id='dashboard.stats.jobs' />}
              />
            </Grid>
            <Grid item xs={12} sm={6} md={4}>
              <StatsCard
                icon={
                  <PersonIcon
                    style={{fontSize: isBreakPointDown('md') ? 30 : 40}}
                  />
                }
                bgColor={blue[500]}
                data={{count: '16'}}
                heading={<IntlMessages id='dashboard.stats.resources' />}
              />
            </Grid>
            <Grid item xs={12} sm={6} md={4}>
              <StatsCard
                icon={
                  <MonetizationOnIcon
                    style={{fontSize: isBreakPointDown('md') ? 30 : 40}}
                  />
                }
                bgColor={indigo[500]}
                data={{count: '2000'}}
                heading={<IntlMessages id='dashboard.stats.save' />}
              />
            </Grid>
          </GridContainer>
        </Box>
      </>
    </AppAnimate>
  );
};

export default Dashboard;
