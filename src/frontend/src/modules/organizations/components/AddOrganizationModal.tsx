/* eslint-disable react/prop-types */
import React from 'react';
import Dialog from '@material-ui/core/Dialog';
import DialogActions from '@material-ui/core/DialogActions';
import DialogContent from '@material-ui/core/DialogContent';
import DialogTitle from '@material-ui/core/DialogTitle';
import {Formik, Form, Field} from 'formik';
import {Button, LinearProgress, TextareaAutosize} from '@material-ui/core';
import {TextField} from 'formik-material-ui';
import useRequest from 'shared/hooks/useRequest';
import {getOrganizations} from 'services';

interface AddOrganizationModalProps {
  open: boolean;
  handleClose: () => void;
  updateOrganizations: (org: any) => void;
}

interface FormValues {
  name: string;
  address: string;
}

interface GetFacilitiesResponse {
  organizationId: number;
  organizationName: string;
  auth0OrganizationName: string;
  phone: string;
  address: string;
  comments: string;
}

const AddOrganizationModal: React.FC<AddOrganizationModalProps> = (props): JSX.Element => {
  const {requestMaker} = useRequest();
  return (
    <Formik
      initialValues={{
        name: '',
        address: '',
      }}
      validate={(values: FormValues) => {
        const errors: Partial<FormValues> = {};
        console.log(values);
        return errors;
      }}
      onSubmit={async (values, {setSubmitting}) => {
        setSubmitting(false);
        const organizationsRes = await requestMaker<GetFacilitiesResponse>(getOrganizations());
        if (organizationsRes) {
          // props.updateOrganizations(organizationsRes.items || []);
        }
        alert(JSON.stringify(values, null, 2));
      }}>
      {({submitForm, isSubmitting}) => (
        <Dialog fullWidth maxWidth='sm' open={props.open} onClose={props.handleClose} aria-labelledby='form-dialog-title'>
          <DialogTitle id='form-dialog-title'>Add Organization</DialogTitle>
          <DialogContent>
            <Form>
              <Field component={TextField} name='name' label='Name' fullWidth />
              <br />
              <Field component={TextareaAutosize} name='adress' label='Address' fullWidth />
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
