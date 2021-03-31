import axios from 'axios';
import flatten from 'lodash/flatten';

const axiosCustom = axios.create({
  withCredentials: true,
});
axiosCustom.defaults.headers.common.Accept = 'application/json';
axiosCustom.defaults.headers.common['Content-Type'] = 'application/json';

// TODO make base api configurable based on ENV.
axiosCustom.defaults.baseURL = `/api/v1/`;

axiosCustom.interceptors.request.use((req) => {
  const token = localStorage.getItem('token');
  if (!token) return req;
  req.headers = {
    Authorization: `Token ${token}`,
    ...req.headers,
  };
  return req;
});

axiosCustom.interceptors.response.use(
  (response) => ({...response, error: undefined}),
  (error) => {
    let errorMessages: string[] = [];
    if (error.response) {
      const {data} = error.response;

      if (typeof data !== 'string') {
        errorMessages = flatten(Object.values(data));
      } else {
        errorMessages.push(data);
      }
    }

    return Promise.resolve({data: undefined, error: errorMessages});
  },
);

export type CustomResponse<T> = {
  data: T;
  error: string[] | undefined;
};

export default axiosCustom;
