import React, {useContext} from 'react';
import Box from '@material-ui/core/Box';
import {makeStyles} from '@material-ui/core/styles';

import AppContext from '../../utility/AppContext';
import Layouts from './Layouts';
import {ContentView} from '../../index';
import useStyles from '../../../shared/jss/common/common.style';
import AppContextPropsType from '../../../types/AppContextPropsType';
import LayoutContextProvider from './LayoutContextProvider';

const useStyle = makeStyles(() => ({
  appAuth: {
    flex: 1,
    display: 'flex',
    position: 'relative',
    height: '100vh',
    backgroundColor: '#f3f4f6',
    backgroundSize: 'cover',

    '& .scrollbar-container': {
      flex: 1,
      display: 'flex',
      flexDirection: 'column',
    },
    '& .main-content-view': {
      padding: 20,
    },
    '& .footer': {
      marginRight: 0,
      marginLeft: 0,
    },
  },
}));

interface CremaLayoutProps {}

const CremaLayout: React.FC<CremaLayoutProps> = () => {
  useStyles();
  const {navStyle, user} = useContext<AppContextPropsType>(AppContext);
  const AppLayout = Layouts[navStyle];
  const classes = useStyle();
  return (
    <>
      {user ? (
        <LayoutContextProvider>
          <AppLayout />
        </LayoutContextProvider>
      ) : (
        <Box className={classes.appAuth}>
          <ContentView />
        </Box>
      )}
    </>
  );
};

export default React.memo(CremaLayout);
