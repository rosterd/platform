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

export interface AddOrganizationFormValues {
  organizationName: string;
  auth0OrganizationName?: string;
  phone: string;
  address: string;
  comments: string;
}

interface AddOrganizationModalProps {
  open: boolean;
  handleClose: () => void;
  onAddOrganization: (values: AddOrganizationFormValues) => void;
}

const AddOrganizationModal: React.FC<AddOrganizationModalProps> = (props): JSX.Element => {
  const validationSchema = yup.object({
    organizationName: yup.string().required('Please enter Organization Name'),
    phone: yup.string().required('Please enter Organization Phone'),
    address: yup.string().required('Please enter Organization Address'),
  });

  return (
    <Formik
      initialValues={{
        organizationName: '',
        phone: '',
        address: '',
        comments: '',
      }}
      validationSchema={validationSchema}
      validate={() => {
        const errors: Partial<AddOrganizationFormValues> = {};
        return errors;
      }}
      onSubmit={async (values, {setSubmitting, resetForm}) => {
        setSubmitting(true);
        await props.onAddOrganization({
          ...values,
          organizationName: values.organizationName.replace(/\s+/g, ''),
          auth0OrganizationName: values.organizationName,
        });
        setSubmitting(false);
        resetForm();
      }}>
      {({submitForm, isSubmitting}) => (
        <Dialog fullWidth maxWidth='sm' open={props.open} onClose={props.handleClose} aria-labelledby='form-dialog-title'>
          <DialogTitle id='form-dialog-title'>Add Organization</DialogTitle>
          <DialogContent>
            <Form>
              <Field component={TextField} name='organizationName' label='Organization Name' fullWidth />
              <br />
              <Field component={TextField} name='phone' label='Phone' fullWidth />
              <br />
              <Field component={TextField} name='address' label='Address' fullWidth />
              <br />
              <Field component={TextField} name='comments' label='Comments' fullWidth />
              <br />

              {isSubmitting && <LinearProgress />}
            </Form>
          </DialogContent>
          <DialogActions>
            <Button onClick={props.handleClose} color='primary'>
              Cancel
            </Button>
            <Button onClick={submitForm} color='primary' disabled={isSubmitting} variant='contained'>
              Add
            </Button>
          </DialogActions>
        </Dialog>
      )}
    </Formik>
  );
};

export default AddOrganizationModal;
