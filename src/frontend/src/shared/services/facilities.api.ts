import axiosCustom, {CustomResponse} from './axiosInstance';

export interface Facility {
  id: number;
}

export const getFacilities = async (): Promise<CustomResponse<Facility[]>> =>
  await axiosCustom.get('failities?PageNumber=1&PageSize=1');

export const EditFacilitiy = async (): Promise<CustomResponse<Facility>> =>
  await axiosCustom.get('failities');

export const AddFacilitiy = async (): Promise<CustomResponse<Facility>> =>
  await axiosCustom.get('failities');
