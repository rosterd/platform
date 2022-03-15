import React, {useContext, useEffect} from 'react';
import {useAuth0} from '@auth0/auth0-react';
import {AuthType} from 'shared/constants/AppEnums';
import AppContext from '../../utility/AppContext';
import Layouts from './Layouts';
import useStyles from '../../../shared/jss/common/common.style';
import AppContextPropsType from '../../../types/AppContextPropsType';
import LayoutContextProvider from './LayoutContextProvider';

interface CremaLayoutProps {}

const CremaLayout: React.FC<CremaLayoutProps> = () => {
  useStyles();
  const {navStyle, updateAuthUser} = useContext<AppContextPropsType>(AppContext);
  const AppLayout = Layouts[navStyle];
  const {user, isAuthenticated, loginWithRedirect, isLoading, getIdTokenClaims} = useAuth0();

  useEffect(() => {
    (async () => {
      if (!isLoading && !isAuthenticated) {
        loginWithRedirect();
      } else {
        const idToken = await getIdTokenClaims();
        const roles = idToken && process.env.REACT_APP_AUTH0_ROLES_NAMESPACE && idToken[process.env.REACT_APP_AUTH0_ROLES_NAMESPACE];
        const orgId = idToken?.org_id;
        if (orgId) localStorage.setItem('organization_id', orgId);

        updateAuthUser({
          uid: user?.sub || '',
          displayName: user?.given_name,
          email: user?.email,
          authType: AuthType.AUTH0,
          role: roles || [],
          orgId: idToken?.org_id,
          photoURL: user?.picture,
        });
      }
    })();
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
