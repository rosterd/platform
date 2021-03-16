import React, {useContext} from 'react';
import {Box} from '@material-ui/core';
import {makeStyles} from '@material-ui/core/styles';
import AppContext from '../../../@crema/utility/AppContext';
import {ThemeMode} from '../../constants/AppEnums';
import Hidden from '@material-ui/core/Hidden';
import AppContextPropsType from '../../../types/AppContextPropsType';

const AppLogo = () => {
  const {themeMode} = useContext<AppContextPropsType>(AppContext);
  const useStyles = makeStyles(() => ({
    logoRoot: {
      display: 'flex',
      flexDirection: 'row',
      cursor: 'pointer',
      alignItems: 'center',
    },
    logo: {
      height: 30,
      marginRight: 10,
    },
  }));
  const classes = useStyles();
  return (
    <Box className={classes.logoRoot}>
      <Hidden smUp>
        <img
          className={classes.logo}
          src={
            themeMode === ThemeMode.DARK
              ? '/assets/images/rosterd_logo.svg'
              : '/assets/images/rosterd_logo.svg'
          }
          alt='rosterd-logo'
        />
      </Hidden>
      <Hidden xsDown>
        <img
          className={classes.logo}
          src={
            themeMode === ThemeMode.DARK
              ? '/assets/images/logo-white-with-name.png'
              : '/assets/images/rosterd_logo.svg'
          }
          alt='rosterd-logo'
        />
      </Hidden>
    </Box>
  );
};

export default AppLogo;
