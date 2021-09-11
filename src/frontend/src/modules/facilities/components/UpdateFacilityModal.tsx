/* eslint-disable react/prop-types */
import React from 'react';
import Dialog from '@material-ui/core/Dialog';
import DialogActions from '@material-ui/core/DialogActions';
import DialogContent from '@material-ui/core/DialogContent';
import DialogTitle from '@material-ui/core/DialogTitle';
import {Formik} from 'formik';
import {Button} from '@material-ui/core';

import {Facility} from '..';
import FacililyForm, {FacilityFormValues, validationSchema, initialValues as formInitialValues} from './FacilityForm';

interface UpdateFacilityModalProps {
  facility?: Facility;
  open: boolean;
  onUpdateFacility: (values: FacilityFormValues) => void;
  handleClose: () => void;
}

const UpdateFacilityModal: React.FC<UpdateFacilityModalProps> = ({facility, ...props}): JSX.Element => {
  const [initialValues, setInitialValues] = React.useState<FacilityFormValues>(formInitialValues);

  React.useEffect(() => {
    if (facility) {
      const {suburb, city, address, country, latitude, longitude, ...rest} = facility;
      setInitialValues({
        address: {
          address,
          suburb,
          city,
          country,
          latitude,
          longitude,
        },
        ...rest,
      });
    }
  }, [facility]);

  return (
    <Formik
      enableReinitialize
      initialValues={initialValues}
      validationSchema={validationSchema}
      onSubmit={async (values, {setSubmitting, resetForm}) => {
        setSubmitting(true);
        try {
          await props.onUpdateFacility(values);
        } finally {
          resetForm();
          setSubmitting(false);
          props.handleClose();
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
          <DialogTitle id='form-dialog-title'>Update Facility</DialogTitle>
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
              Update Facility
            </Button>
          </DialogActions>
        </Dialog>
      )}
    </Formik>
  );
};

export default UpdateFacilityModal;
