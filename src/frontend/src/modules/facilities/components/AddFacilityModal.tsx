/* eslint-disable react/prop-types */
import React from 'react';
import Dialog from '@material-ui/core/Dialog';
import DialogActions from '@material-ui/core/DialogActions';
import DialogContent from '@material-ui/core/DialogContent';
import DialogTitle from '@material-ui/core/DialogTitle';
import {Formik} from 'formik';
import {Button} from '@material-ui/core';
import FacililyForm, {FacilityFormValues, initialValues, validationSchema} from './FacilityForm';

interface AddFacilityModalProps {
  open: boolean;
  onAddFacility: (values: FacilityFormValues) => void;
  handleClose: () => void;
}

const AddFacilityModal: React.FC<AddFacilityModalProps> = (props): JSX.Element => (
  <Formik
    enableReinitialize
    initialValues={initialValues}
    validationSchema={validationSchema}
    onSubmit={async (values, {setSubmitting, resetForm}) => {
      setSubmitting(true);
      try {
        await props.onAddFacility(values);
      } catch {
        resetForm();
        setSubmitting(false);
      }
    }}>
    {({submitForm, isSubmitting, resetForm}) => (
      <Dialog
        fullWidth
        maxWidth='sm'
        open={props.open}
        onClose={() => {
          props.handleClose();
          resetForm();
        }}
        aria-labelledby='form-dialog-title'>
        <DialogTitle id='form-dialog-title'>Add Facility</DialogTitle>
        <DialogContent>
          <FacililyForm isSubmitting={isSubmitting} />
        </DialogContent>
        <DialogActions>
          <Button
            onClick={() => {
              props.handleClose();
              resetForm();
            }}
            color='primary'>
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

export default AddFacilityModal;
