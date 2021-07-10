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
        audience: 'http://localhost:3000/dummy-api',
      };

      if (requestConfig.scope) {
        tokenOptions = {...tokenOptions, scope: requestConfig.scope};
      }
      const accessToken = isAuthenticated ? await getAccessTokenSilently(tokenOptions) : 'anonymous';

      const axiosClient = axios.create({
        baseURL: 'https://localhost:5001/api/v1/',
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
