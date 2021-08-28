import {AxiosRequestConfig} from 'axios';
import {components} from 'types/models';

type AddAdminUserRequest = components['schemas']['AddAdminUserRequest'];
type AddAdminWhoIsAlsoStaffRequest = components['schemas']['AddAdminWhoIsAlsoStaffRequest'];

const facilityUrl = 'adminusers/facility-admins';
const organizationUrl = 'adminusers/organization-admins';

export const addFacilityAdmin = (data: AddAdminWhoIsAlsoStaffRequest): AxiosRequestConfig & {scope?: string} => ({
  method: 'POST',
  url: facilityUrl,
  data,
});

export const addOrganizationAdmin = (data: AddAdminUserRequest): AxiosRequestConfig & {scope?: string} => ({
  method: 'POST',
  url: organizationUrl,
  data,
});

export const getFacilityAdmins = (): AxiosRequestConfig & {scope?: string} => ({url: 'adminusers/admins'});

export const getOrganizationAdmins = (): AxiosRequestConfig & {scope?: string} => ({url: organizationUrl});
