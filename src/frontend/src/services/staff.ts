import {AxiosRequestConfig} from 'axios';
import {components} from 'types/models';

type AddStaffRequest = components['schemas']['AddStaffRequest'];
type UpdateStaffRequest = components['schemas']['UpdateStaffRequest'];

export const url = 'staff';

export const getStaff = (): AxiosRequestConfig & {scope?: string} => ({url});

export const addStaff = (data: AddStaffRequest): AxiosRequestConfig & {scope?: string} => ({url, method: 'POST', data});

export const updateStaff = (data: UpdateStaffRequest): AxiosRequestConfig & {scope?: string} => ({url, method: 'PUT', data});

export const deleteStaff = (staffId?: number): AxiosRequestConfig & {scope?: string} => ({url: `${url}/${staffId}`, method: 'DELETE'});
