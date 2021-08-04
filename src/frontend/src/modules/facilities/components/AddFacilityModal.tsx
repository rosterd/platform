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

export interface AddFacilityFormValues {
  facilityName: string;
  address: string;
  suburb: string;
  city: string;
  country: string;
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
    address: yup.string().required('Please enter Facility Address'),
  });

  return (
    <Formik
      initialValues={{
        facilityName: '',
        address: '',
        suburb: '',
        city: '',
        country: '',
        phoneNumber1: '',
        phoneNumber2: '',
      }}
      validationSchema={validationSchema}
      validate={(values: AddFacilityFormValues) => {
        const errors: Partial<AddFacilityFormValues> = {};
        console.log(values);
        return errors;
      }}
      onSubmit={async (values, {setSubmitting, resetForm}) => {
        setSubmitting(true);
        await props.onAddFacility(values);
        resetForm();
        setSubmitting(false);
      }}>
      {({submitForm, isSubmitting}) => (
        <Dialog fullWidth maxWidth='sm' open={props.open} onClose={props.handleClose} aria-labelledby='form-dialog-title'>
          <DialogTitle id='form-dialog-title'>Add Resource</DialogTitle>
          <DialogContent>
            <Form>
              <Field component={TextField} name='facilityName' label='Facility Name' fullWidth />
              <br />
              <Field component={TextField} name='address' label='Facility Address' fullWidth />
              <br />
              <Field component={TextField} name='suburb' label='Suburb' fullWidth />
              <br />
              <Field component={TextField} name='city' label='City' fullWidth />
              <br />
              <Field component={TextField} name='country' label='Country' fullWidth />
              <br />
              <Field component={TextField} name='phoneNumber1' label='Phone Number' fullWidth />
              <br />

              {isSubmitting && <LinearProgress />}
            </Form>
          </DialogContent>
          <DialogActions>
            <Button onClick={props.handleClose} color='primary'>
              Cancel
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
