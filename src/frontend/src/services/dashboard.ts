import {AxiosRequestConfig} from 'axios';

const url = 'dashboard';

const getDashboard = (): AxiosRequestConfig & {scope?: string} => ({url});

export default getDashboard;
