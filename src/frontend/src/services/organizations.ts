import {AxiosRequestConfig} from 'axios';
import {components} from 'types/models';

type AddOrganizationRequest = components['schemas']['AddOrganizationRequest'];

const url = 'organizations';

export const getOrganizations = (): AxiosRequestConfig & {scope?: string} => ({url: `${url}?pageSize=50`});

export const addOrganization = (data: AddOrganizationRequest): AxiosRequestConfig & {scope?: string} => ({method: 'POST', url, data});