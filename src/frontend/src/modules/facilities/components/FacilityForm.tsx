import {Field, Form} from 'formik';
import {TextField} from 'formik-material-ui';
import React from 'react';
import {AddressFieldType, addressInitialValue} from 'shared/components/Address/AddressInput';
import AddressInput from 'shared/components/Address';
import * as yup from 'yup';

const FacililyForm = (): JSX.Element => (
  <Form>
    <Field component={TextField} name='facilityName' label='Facility Name' fullWidth />
    <br />
    <AddressInput isRequired label='Facility Address' name='address' />
    <br />
    <Field component={TextField} name='phoneNumber1' label='Phone Number' fullWidth />
    <br />
    <Field component={TextField} name='phoneNumber2' label='Phone Number 2' fullWidth />
    <br />
  </Form>
);

export const validationSchema = yup.object({
  facilityName: yup.string().required('Please enter Facility Name'),
  phoneNumber1: yup.string().required('Phone number is required'),
});

export const initialValues = {
  facilityName: '',
  address: addressInitialValue,
  phoneNumber1: '',
  phoneNumber2: '',
};

export interface FacilityFormValues {
  facilityName: string;
  address: AddressFieldType;
  phoneNumber1: string;
  phoneNumber2?: string | null;
}

export default FacililyForm;
