import React from 'react';
import * as yup from 'yup';
import FormModal, {FormProps, ModalProps} from 'shared/components/FormModal';
import {Field, Form} from 'formik';
import {TextField} from 'formik-material-ui';
import {DeleteJobRequest} from 'services';

interface DeleteJobModalProps {
  open: boolean;
  handleClose: () => void;
  onDeleteJob: (values: DeleteJobRequest) => void;
}

const DeleteJobModal = (props: DeleteJobModalProps): JSX.Element => {
  const {open, handleClose, onDeleteJob} = props;
  const initialValues = {
    jobCancellationReason: '',
  };
  const validationSchema = yup.object({
    jobCancellationReason: yup.string().required('Please enter the reason'),
  });

  const formProps: FormProps<DeleteJobRequest> = {
    onSubmit: onDeleteJob,
    initialValues,
    validationSchema,
  };

  const modalProps: ModalProps = {
    open,
    onClose: handleClose,
    title: 'Delete Job',
    submitButtonLabel: 'Delete',
    successMessage: 'Successfully deleted',
  };

  return (
    <FormModal formProps={formProps} modalProps={modalProps}>
      <Form>
        <Field component={TextField} name='jobCancellationReason' label='Reason' fullWidth multiline />
      </Form>
    </FormModal>
  );
};

export default DeleteJobModal;
