import { useContext, useEffect, useState } from "react";
import { AuthType } from "../../shared/constants/AppEnums";
import { defaultUser } from "../../shared/constants/AppConst";
import jwtAxios from "../services/ApiConfig";
import { AuthUser } from "../../types/models/AuthUser";
import { fetchStart, fetchSuccess, useInfoViewActionsContext } from "../core/InfoView/InfoViewContext";
import AppContext from "./AppContext";
import AppContextPropsType from "../../types/AppContextPropsType";

export const useAuthToken = (): [boolean, AuthUser | null] => {
  const [loading, setLoading] = useState(true);
  const {user, updateAuthUser} = useContext<AppContextPropsType>(AppContext);
  const dispatch = useInfoViewActionsContext()!;

  useEffect(() => {
    const validateAuth = async () => {
      dispatch(fetchStart());
      const token = localStorage.getItem('token');
      if (!token) {
        dispatch(fetchSuccess());
        return;
      }
      jwtAxios.defaults.headers.common['x-auth-token'] = token;
      try {
        const res = await jwtAxios.get('/auth');

        dispatch(fetchSuccess());
        updateAuthUser({
            authType: AuthType.JWT_AUTH,
            displayName: res.data.name,
            email: res.data.email,
            role: defaultUser.role,
            token: res.data._id,
            uid: res.data._id,
            photoURL: res.data.avatar,
          });
        return;
      } catch (err) {
        dispatch(fetchSuccess());
        return;
      }
    };

    const checkAuth = () => {
      Promise.all([validateAuth()]).then(() => {
        setLoading(false);
      });
    };
    checkAuth();
  }, [dispatch,updateAuthUser]);

  return [loading, user];
};

export const useAuthUser = (): AuthUser | null => {
  const {user} = useContext<AppContextPropsType>(AppContext);
  if (user) {
    return user;
  }
  return null;
};
