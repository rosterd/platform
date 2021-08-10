import {AxiosRequestConfig} from 'axios';
import {components} from 'types/models';

type AddUpdateSkillRequest = components['schemas']['AddUpdateSkillRequest'];

const url = 'skills';

export const getSkills = (): AxiosRequestConfig & {scope?: string} => ({url});

export const addSkill = (data: AddUpdateSkillRequest): AxiosRequestConfig & {scope?: string} => ({url, method: 'POST', data});

export const updateSkill = (data: AddUpdateSkillRequest): AxiosRequestConfig & {scope?: string} => ({url, method: 'PUT', data});

export const deleteSkill = (skillId?: number): AxiosRequestConfig & {scope?: string} => ({url: `${url}/${skillId}`, method: 'DELETE'});
