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
import {components} from 'types/models';

type AddAdminUserRequest = components['schemas']['AddAdminUserRequest'];

interface AddAdminModalProps {
  open: boolean;
  onAddAdmin: (value: AddAdminUserRequest) => void;
  handleClose: () => void;
}

const AddAdminModal: React.FC<AddAdminModalProps> = (props): JSX.Element => {
  const validationSchema = yup.object({
    firstName: yup.string().required('Please enter First Name'),
    lastName: yup.string().required('Please enter Last Name'),
    email: yup.string().required('Please enter Email Address'),
  });

  return (
    <Formik
      initialValues={{
        firstName: '',
        lastName: '',
        email: '',
        phoneNumber: '',
      }}
      validationSchema={validationSchema}
      onSubmit={async (values, {setSubmitting, resetForm}) => {
        await props.onAddAdmin(values);
        setSubmitting(false);
        resetForm();
      }}>
      {({submitForm, isSubmitting}) => (
        <Dialog fullWidth maxWidth='sm' open={props.open} onClose={props.handleClose} aria-labelledby='form-dialog-title'>
          <DialogTitle id='form-dialog-title'>Invite Admin</DialogTitle>
          <DialogContent>
            <Form>
              <Field component={TextField} name='firstName' label='First Name' fullWidth />
              <br />
              <Field component={TextField} name='lastName' label='Last Name' fullWidth />
              <br />
              <Field component={TextField} name='email' label='Email' fullWidth />
              <br />
              <Field component={TextField} name='phoneNumber' label='Phone Number' fullWidth />
              <br />
              {isSubmitting && <LinearProgress />}
            </Form>
          </DialogContent>
          <DialogActions>
            <Button onClick={props.handleClose} color='primary'>
              Cancel
            </Button>
            <Button onClick={submitForm} color='primary' disabled={isSubmitting} variant='contained'>
              Invite
            </Button>
          </DialogActions>
        </Dialog>
      )}
    </Formik>
  );
};

export default AddAdminModal;
