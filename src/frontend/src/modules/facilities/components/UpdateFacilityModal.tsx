/* eslint-disable react/prop-types */
import React from 'react';
import FormModal, {FormProps, ModalProps} from 'shared/components/FormModal';
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
  const {open, onUpdateFacility, handleClose} = props;
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

  const formProps: FormProps<FacilityFormValues> = {
    onSubmit: onUpdateFacility,
    initialValues,
    validationSchema,
  };

  const modalProps: ModalProps = {
    open,
    onClose: handleClose,
    title: 'Update Facility',
    submitButtonLabel: 'Update Facility',
    closeAfter: true,
  };

  return (
    <FormModal formProps={formProps} modalProps={modalProps}>
      <FacililyForm />
    </FormModal>
  );
};

export default UpdateFacilityModal;
