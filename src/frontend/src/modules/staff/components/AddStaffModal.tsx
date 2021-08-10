import React from 'react';
import Dialog from '@material-ui/core/Dialog';
import DialogActions from '@material-ui/core/DialogActions';
import DialogContent from '@material-ui/core/DialogContent';
import DialogTitle from '@material-ui/core/DialogTitle';
import {Formik, Form, Field} from 'formik';
import {Button, LinearProgress} from '@material-ui/core';
import {TextField} from 'formik-material-ui';
import * as yup from 'yup';

interface AddStaffModalProps {
  open: boolean;
  handleClose: () => void;
}

interface FormValues {
  email: string;
  name: string;
  mobile: number;
  skill: string;
  address: string;
}

const AddStaffModal = (props: AddStaffModalProps) => {
  const validationSchema = yup.object({
    name: yup.string().required('Please enter name'),
    email: yup.string().email().required('Please enter valid email'),
    mobile: yup.string().required('Please enter mobile'),
    skill: yup.string().email().required('Please enter atleast one skill'),
    address: yup.string().email().required('Please enter address'),
  });
  return (
    <Formik
      initialValues={{
        name: '',
        email: '',
        mobile: '',
        skill: '',
        address: '',
      }}
      validationSchema={validationSchema}
      validate={(values) => {
        const errors: Partial<FormValues> = {};
        if (!values.email) {
          errors.email = 'Required';
        } else if (!/^[A-Z0-9._%+-]+@[A-Z0-9.-]+\.[A-Z]{2,4}$/i.test(values.email)) {
          errors.email = 'Invalid email address';
        }
        return errors;
      }}
      onSubmit={(values, {setSubmitting}) => {
        setTimeout(() => {
          setSubmitting(false);
          alert(JSON.stringify(values, null, 2));
        }, 500);
      }}>
      {({submitForm, isSubmitting}) => (
        <Dialog fullWidth maxWidth='sm' open={props.open} onClose={props.handleClose} aria-labelledby='form-dialog-title'>
          <DialogTitle id='form-dialog-title'>Add Staff</DialogTitle>
          <DialogContent>
            <Form>
              <Field component={TextField} name='name' label='Name' fullWidth />
              <br />
              <Field component={TextField} name='email' type='email' label='Email' fullWidth />
              <br />
              <Field component={TextField} name='mobile' type='tel' label='Mobile Number' fullWidth />
              <br />
              <Field component={TextField} name='skill' label='Skill' fullWidth />
              <br />
              <Field component={TextField} name='address' label='Address' fullWidth />
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

export default AddStaffModal;
