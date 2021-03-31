import {useContext, useEffect, useState} from 'react';
import {AuthType} from '../../shared/constants/AppEnums';
import {defaultUser} from '../../shared/constants/AppConst';
import {auth as firebaseAuth} from '../services/auth/firebase/firebase';
import {useInfoViewActionsContext} from '../core/InfoView/InfoViewContext';
import AppContext from './AppContext';
import AppContextPropsType from '../../types/AppContextPropsType';
import {AuthUser} from '../../types/models/AuthUser';

export const useAuthToken = (): [boolean, AuthUser | null] => {
  const [loading, setLoading] = useState(true);
  const {user, updateAuthUser} = useContext<AppContextPropsType>(AppContext);
  const dispatch = useInfoViewActionsContext()!;

  useEffect(() => {
    const firebaseCheck = () =>
      new Promise((resolve) => {
        firebaseAuth.onAuthStateChanged((authUser: any) => {
          if (authUser) {
            updateAuthUser({
              authType: AuthType.FIREBASE,
              uid: authUser.uid,
              displayName: authUser.displayName,
              email: authUser.email,
              role: defaultUser.role,
              photoURL: authUser.photoURL,
              token: authUser.refreshToken,
            });
          }
          resolve(true);
        });
        return Promise.resolve();
      });

    const checkAuth = () => {
      Promise.all([firebaseCheck()]).then(() => {
        setLoading(false);
      });
    };
    checkAuth();
  }, [dispatch, updateAuthUser]);

  return [loading, user];
};

export const useAuthUser = (): AuthUser | null => {
  const {user} = useContext<AppContextPropsType>(AppContext);
  if (user) {
    return user;
  }
  return null;
};
