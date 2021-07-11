import {AxiosRequestConfig} from 'axios';

export const getFacilities = (): AxiosRequestConfig & {scope?: string} => ({url: 'facilities'});

export const setFacility = (): AxiosRequestConfig & {scope?: string} => ({method: 'POST', url: 'facilities'});
