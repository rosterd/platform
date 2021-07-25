import {AxiosRequestConfig} from 'axios';

export const getOrganizations = (): AxiosRequestConfig & {scope?: string} => ({url: 'organizations'});

export const addOrganization = (data: any): AxiosRequestConfig & {scope?: string} => ({method: 'POST', url: 'organizations', data});
