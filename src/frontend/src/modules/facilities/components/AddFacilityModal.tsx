/* eslint-disable react/prop-types */
import React from 'react';
import FormModal, {FormProps, ModalProps} from 'shared/components/FormModal';
import FacililyForm, {FacilityFormValues, initialValues, validationSchema} from './FacilityForm';

interface AddFacilityModalProps {
  open: boolean;
  onAddFacility: (values: FacilityFormValues) => void;
  handleClose: () => void;
}

const AddFacilityModal: React.FC<AddFacilityModalProps> = (props): JSX.Element => {
  const {onAddFacility, handleClose, open} = props;

  const formProps: FormProps<FacilityFormValues> = {
    onSubmit: onAddFacility,
    initialValues,
    validationSchema,
  };

  const modalProps: ModalProps = {
    open,
    onClose: handleClose,
    title: 'Add Facility',
    submitButtonLabel: 'Add Facility',
  };

  return (
    <FormModal formProps={formProps} modalProps={modalProps}>
      <FacililyForm />
    </FormModal>
  );
};

export default AddFacilityModal;
