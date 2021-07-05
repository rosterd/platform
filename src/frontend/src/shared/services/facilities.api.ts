import axiosClient from './axiosInstance';

export interface Facility {
  id: number;
}

export const getFacilities = async (): Promise<Facility[]> => await axiosClient.get('failities?PageNumber=1&PageSize=1');

export const EditFacilitiy = async (): Promise<Facility> => await axiosClient.get('failities');

export const AddFacilitiy = async (): Promise<Facility> => await axiosClient.get('failities');
