import axiosCustom from './ApiConfig';

export interface Facility {
  id: number;
}

export const getFacilities = async (): Promise<Facility[]> =>
  await axiosCustom.get('failities?PageNumber=1&PageSize=1');

export const EditFacilitiy = async (): Promise<Facility> =>
  await axiosCustom.get('failities');

export const AddFacilitiy = async (): Promise<Facility> =>
  await axiosCustom.get('failities');
