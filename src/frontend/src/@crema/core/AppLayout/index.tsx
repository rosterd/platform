import React, {useContext, useEffect} from 'react';
import AppContext from '../../utility/AppContext';
import Layouts from './Layouts';
import useStyles from '../../../shared/jss/common/common.style';
import AppContextPropsType from '../../../types/AppContextPropsType';
import LayoutContextProvider from './LayoutContextProvider';
import {useAuth0} from '@auth0/auth0-react';
import {AuthType} from 'shared/constants/AppEnums';

interface CremaLayoutProps {}

const CremaLayout: React.FC<CremaLayoutProps> = () => {
  useStyles();
  const {navStyle, updateAuthUser} = useContext<AppContextPropsType>(
    AppContext,
  );
  const AppLayout = Layouts[navStyle];
  const {user, isAuthenticated, loginWithRedirect, isLoading} = useAuth0();

  useEffect(() => {
    if (!isLoading && !isAuthenticated) {
      loginWithRedirect();
    } else {
      updateAuthUser({
        uid: '',
        displayName: user?.nickname,
        email: user?.email,
        authType: AuthType.AUTH0,
        role: ['admin'],
      });
    }
  }, [isAuthenticated, loginWithRedirect, user, isLoading]);

  return isAuthenticated ? (
    <LayoutContextProvider>
      <AppLayout />
    </LayoutContextProvider>
  ) : (
    <></>
  );
};

export default React.memo(CremaLayout);
