/* eslint-disable react/prop-types */
import React from 'react';
import Dialog from '@material-ui/core/Dialog';
import DialogActions from '@material-ui/core/DialogActions';
import DialogContent from '@material-ui/core/DialogContent';
import DialogTitle from '@material-ui/core/DialogTitle';
import {Formik, Form, Field} from 'formik';
import {Button, LinearProgress} from '@material-ui/core';
import {TextField} from 'formik-material-ui';

interface AddFacilityModalProps {
  open: boolean;
  handleClose: () => void;
}

interface FormValues {
  name: string;
  suburb: string;
  city: string;
}

const AddFacilityModal: React.FC<AddFacilityModalProps> = (
  props,
): JSX.Element => (
  <Formik
    initialValues={{
      name: '',
      suburb: '',
      city: '',
    }}
    validate={(values: FormValues) => {
      const errors: Partial<FormValues> = {};
      console.log(values);
      return errors;
    }}
    onSubmit={(values, {setSubmitting}) => {
      setTimeout(() => {
        setSubmitting(false);
        alert(JSON.stringify(values, null, 2));
      }, 500);
    }}>
    {({submitForm, isSubmitting}) => (
      <Dialog
        fullWidth
        maxWidth='sm'
        open={props.open}
        onClose={props.handleClose}
        aria-labelledby='form-dialog-title'>
        <DialogTitle id='form-dialog-title'>Add Resource</DialogTitle>
        <DialogContent>
          <Form>
            <Field component={TextField} name='name' label='Name' fullWidth />
            <br />
            <Field
              component={TextField}
              name='suburb'
              label='Suburb'
              fullWidth
            />
            <br />
            <Field component={TextField} name='city' label='City' fullWidth />

            {isSubmitting && <LinearProgress />}
          </Form>
        </DialogContent>
        <DialogActions>
          <Button onClick={props.handleClose} color='primary'>
            Cancel
          </Button>
          <Button
            onClick={submitForm}
            color='primary'
            disabled={isSubmitting}
            variant='contained'>
            Add Facility
          </Button>
        </DialogActions>
      </Dialog>
    )}
  </Formik>
);

export default AddFacilityModal;
