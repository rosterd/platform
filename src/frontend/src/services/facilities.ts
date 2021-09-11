import {AxiosRequestConfig} from 'axios';
import {components} from 'types/models';

export type AddFacilityRequest = components['schemas']['AddFacilityRequest'];
export type UpdateFacilityRequest = components['schemas']['UpdateFacilityRequest'];

const url = 'facilities';

export const getFacilities = (): AxiosRequestConfig & {scope?: string} => ({url});

export const addFacility = (data: AddFacilityRequest): AxiosRequestConfig & {scope?: string} => ({
  method: 'POST',
  url,
  data,
});

export const updateFacility = (data: UpdateFacilityRequest): AxiosRequestConfig & {scope?: string} => ({method: 'PUT', url, data});

export const deleteFacility = (facilityId: number): AxiosRequestConfig & {scope?: string} => ({method: 'DELETE', url: `${url}/${facilityId}`});
