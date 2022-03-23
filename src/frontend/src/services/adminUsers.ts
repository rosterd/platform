import {AxiosRequestConfig} from 'axios';
import {components} from 'types/models';

type AddAdminUserRequest = components['schemas']['AddAdminUserRequest'];
type AddAdminWhoIsAlsoStaffRequest = components['schemas']['AddAdminWhoIsAlsoStaffRequest'];
const url = 'adminusers';
const facilityUrl = `${url}/facility-admins`;
const organizationUrl = `${url}/organization-admins`;

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

export const deleteOrganizationAdmin = (staffId: number | undefined | null): AxiosRequestConfig & {scope?: string} => ({
  method: 'DELETE',
  url: `${organizationUrl}/${staffId}`,
});

export const deleteFacilityAdmin = (staffId: number | undefined | null): AxiosRequestConfig & {scope?: string} => ({
  method: 'DELETE',
  url: `${facilityUrl}/${staffId}`,
});

export const getAdmins = (): AxiosRequestConfig & {scope?: string} => ({url});
