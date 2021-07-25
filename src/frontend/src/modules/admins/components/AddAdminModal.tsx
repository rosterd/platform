/* eslint-disable react/prop-types */
import React from 'react';
import Dialog from '@material-ui/core/Dialog';
import DialogActions from '@material-ui/core/DialogActions';
import DialogContent from '@material-ui/core/DialogContent';
import DialogTitle from '@material-ui/core/DialogTitle';
import {Formik, Form, Field} from 'formik';
import {Button, LinearProgress} from '@material-ui/core';
import {TextField, Select} from 'formik-material-ui';
import MenuItem from '@material-ui/core/MenuItem';
import InputLabel from '@material-ui/core/InputLabel';
import FormControl from '@material-ui/core/FormControl';
import {components} from 'types/models';

type Facility = components['schemas']['FacilityModel'];

interface AddAdminModalProps {
  open: boolean;
  facilities: Facility[];
  handleClose: () => void;
}

interface FormValues {
  name: string;
  email: string;
  facility: string;
}

const AddAdminModal: React.FC<AddAdminModalProps> = (props): JSX.Element => (
  <Formik
    initialValues={{
      name: '',
      email: '',
      facility: '',
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
      <Dialog fullWidth maxWidth='sm' open={props.open} onClose={props.handleClose} aria-labelledby='form-dialog-title'>
        <DialogTitle id='form-dialog-title'>Invite Admin</DialogTitle>
        <DialogContent>
          <Form>
            <Field component={TextField} name='name' label='Name' fullWidth />
            <br />
            <Field component={TextField} name='email' label='Email' fullWidth />
            <br />
            <FormControl fullWidth>
              <InputLabel htmlFor='facility'>Facility</InputLabel>
              <Field
                component={Select}
                name='facility'
                inputProps={{
                  id: 'facility',
                }}>
                <MenuItem value={10}>Select Facility</MenuItem>
                {props.facilities.map((facility) => (
                  <MenuItem value={facility.facilityId || ''} key={facility.facilityId}>
                    {facility.facilityName}
                  </MenuItem>
                ))}
              </Field>
            </FormControl>
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

export default AddAdminModal;
