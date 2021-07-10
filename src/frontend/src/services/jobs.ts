import {AxiosRequestConfig} from 'axios';

export const getJobss = (): AxiosRequestConfig & {scope?: string} => ({url: 'jobs'});
export const publishJosb = (): AxiosRequestConfig & {scope?: string} => ({url: 'skills'});
