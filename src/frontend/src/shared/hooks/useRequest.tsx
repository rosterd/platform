import {useAuth0, GetTokenSilentlyOptions} from '@auth0/auth0-react';
import axios, {AxiosError, AxiosRequestConfig, AxiosResponse} from 'axios';
import {useCallback} from 'react';

interface HookResponse {
  requestMaker: <T>(request: AxiosRequestConfig & {scope?: string}) => Promise<T>;
}

export const useRequest = (): HookResponse => {
  const {getAccessTokenSilently, isAuthenticated} = useAuth0();

  const memoizedFn = useCallback(
    async (requestConfig) => {
      let tokenOptions: GetTokenSilentlyOptions = {
        audience: process.env.REACT_APP_AUTH0_AUDIENCE,
      };

      if (requestConfig.scope) {
        tokenOptions = {...tokenOptions, scope: requestConfig.scope};
      }
      const accessToken = isAuthenticated ? await getAccessTokenSilently(tokenOptions) : 'anonymous';

      const axiosClient = axios.create({
        baseURL: process.env.REACT_APP_API_BASE_URL,
        headers: {
          'Content-Type': 'application/json',
        },
      });

      return axiosClient({
        ...requestConfig,
        headers: {
          ...requestConfig.headers,
          Authorization: `Bearer ${accessToken}`,
        },
      })
        .then((res: AxiosResponse) => res.data)
        .catch((error: AxiosError) => {
          if (error.response) {
            console.log(error.response.data);
            console.log(error.response.status);
            console.log(error.response.headers);
          }
          return error;
        });
    },
    [isAuthenticated, getAccessTokenSilently],
  );
  return {
    requestMaker: memoizedFn,
  };
};

export default useRequest;
