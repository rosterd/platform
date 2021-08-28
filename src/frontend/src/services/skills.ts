import {AxiosRequestConfig} from 'axios';
import {components} from 'types/models';

type AddSkillRequest = components['schemas']['AddSkillRequest'];
type UpdateSkillRequest = components['schemas']['UpdateSkillRequest'];

const url = 'skills';

export const getSkills = (): AxiosRequestConfig & {scope?: string} => ({url});

export const addSkill = (data: AddSkillRequest): AxiosRequestConfig & {scope?: string} => ({url, method: 'POST', data});

export const updateSkill = (data: UpdateSkillRequest): AxiosRequestConfig & {scope?: string} => ({url, method: 'PUT', data});

export const deleteSkill = (skillId?: number): AxiosRequestConfig & {scope?: string} => ({url: `${url}/${skillId}`, method: 'DELETE'});
