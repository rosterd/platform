/* eslint-disable react/prop-types */
import React from 'react';
import Dialog from '@material-ui/core/Dialog';
import DialogActions from '@material-ui/core/DialogActions';
import DialogContent from '@material-ui/core/DialogContent';
import DialogTitle from '@material-ui/core/DialogTitle';
import {Formik, Form, Field} from 'formik';
import {Button, LinearProgress} from '@material-ui/core';
import {TextField} from 'formik-material-ui';
import * as yup from 'yup';
import AddressInput from 'shared/components/Address';
import {AddressFieldType, addressInitialValue} from 'shared/components/Address/AddressInput';

export interface AddFacilityFormValues {
  facilityName: string;
  address: AddressFieldType;
  phoneNumber1: string;
  phoneNumber2: string;
}

interface AddFacilityModalProps {
  open: boolean;
  onAddFacility: (values: AddFacilityFormValues) => void;
  handleClose: () => void;
}

const AddFacilityModal: React.FC<AddFacilityModalProps> = (props): JSX.Element => {
  const validationSchema = yup.object({
    facilityName: yup.string().required('Please enter Facility Name'),
    phoneNumber1: yup.string().required('Phone number is required'),
  });

  return (
    <Formik
      initialValues={{
        facilityName: '',
        address: addressInitialValue,
        phoneNumber1: '',
        phoneNumber2: '',
      }}
      validationSchema={validationSchema}
      onSubmit={async (values, {setSubmitting, resetForm}) => {
        setSubmitting(true);
        await props.onAddFacility(values);
        resetForm();
        setSubmitting(false);
      }}>
      {({submitForm, isSubmitting}) => (
        <Dialog fullWidth maxWidth='sm' open={props.open} onClose={props.handleClose} aria-labelledby='form-dialog-title'>
          <DialogTitle id='form-dialog-title'>Add Staff</DialogTitle>
          <DialogContent>
            <Form>
              <Field component={TextField} name='facilityName' label='Facility Name' fullWidth />
              <br />
              <AddressInput isRequired label='Facility Address' name='address' />
              <br />
              <Field component={TextField} name='phoneNumber1' label='Phone Number' fullWidth />
              <br />
              <Field component={TextField} name='phoneNumber2' label='Phone Number 2' fullWidth />
              <br />

              {isSubmitting && <LinearProgress />}
            </Form>
          </DialogContent>
          <DialogActions>
            <Button onClick={props.handleClose} color='primary'>
              Close
            </Button>
            <Button onClick={submitForm} color='primary' disabled={isSubmitting} variant='contained'>
              Add Facility
            </Button>
          </DialogActions>
        </Dialog>
      )}
    </Formik>
  );
};

export default AddFacilityModal;
