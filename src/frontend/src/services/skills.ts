import {AxiosRequestConfig} from 'axios';

export const getSkills = (): AxiosRequestConfig & {scope?: string} => ({url: 'skills'});
export const setSkills = (): AxiosRequestConfig & {scope?: string} => ({url: 'skills'});
