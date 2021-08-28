import {AxiosRequestConfig} from 'axios';
import {components} from 'types/models';

type AddFacilityRequest = components['schemas']['AddFacilityRequest'];

const url = 'facilities';

export const getFacilities = (): AxiosRequestConfig & {scope?: string} => ({url});

export const addFacility = (data: AddFacilityRequest): AxiosRequestConfig & {scope?: string} => ({
  method: 'POST',
  url,
  data,
});

export const updateFacility = (): AxiosRequestConfig & {scope?: string} => ({method: 'PUT', url, scope: 'create:facility'});

export const deleteFacility = (facilityId: number): AxiosRequestConfig & {scope?: string} => ({method: 'DELETE', url: `${url}/${facilityId}`});
