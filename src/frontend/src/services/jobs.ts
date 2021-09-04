import {AxiosRequestConfig} from 'axios';
import {components} from 'types/models';

type AddJobRequest = components['schemas']['AddJobRequest'];

const url = 'jobs';

export const getJobs = (): AxiosRequestConfig & {scope?: string} => ({url});

export const publishJob = (data: AddJobRequest): AxiosRequestConfig & {scope?: string} => ({url, method: 'POST', data});

export const deleteJob = (jobId = 0): AxiosRequestConfig & {scope?: string} => ({url: `${url}/${jobId}`, method: 'DELETE'});
