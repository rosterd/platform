import httpClient from '../services/ApiConfig';

export const fetchDataApi = (url: string) => {
  console.log('fetchDataApi', url);
  return new Promise((resolve, reject) => {
    httpClient
      .get(url)
      .then(({data}) => resolve(data))
      .catch((error) => {
        console.log('error: ', error);
        return reject(error);
      });
    return Promise.resolve();
  });
};
export const saveDataApi = (url: string, payload: any) => {
  console.log('url, payload', url, payload);
  return new Promise((resolve, reject) => {
    httpClient
      .post(url, payload)
      .then(({data}) => resolve(data))
      .catch((error) => {
        console.log('error: ', error);
        return reject(error);
      });
    return Promise.resolve();
  });
};
export const updateDataApi = (url: string, payload: any) => new Promise((resolve, reject) => {
    httpClient
      .put(url, payload)
      .then(({data}) => resolve(data))
      .catch((error) => {
        console.log('error: ', error);
        return reject(error);
      });
    return Promise.resolve();
  });
export const deleteDataApi = (url: string) => new Promise((resolve, reject) => {
    httpClient
      .delete(url)
      .then(({data}) => resolve(data))
      .catch((error) => {
        console.log('error: ', error);
        return reject(error);
      });
    return Promise.resolve();
  });
