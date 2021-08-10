import {AxiosRequestConfig} from 'axios';
import {components} from 'types/models';

type AddAdminUserRequest = components['schemas']['AddAdminUserRequest'];

export const addFacilityAdmin = (data: AddAdminUserRequest): AxiosRequestConfig & {scope?: string} => ({
  method: 'POST',
  url: 'adminusers/facility-admins',
  data,
});

export const addOrganizationAdmin = (data: AddAdminUserRequest): AxiosRequestConfig & {scope?: string} => ({
  method: 'POST',
  url: 'adminusers/organization-admins',
  data,
});
