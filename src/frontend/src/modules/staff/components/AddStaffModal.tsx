import React from 'react';
import {Field} from 'formik';
import {TextField} from 'formik-material-ui';
import * as yup from 'yup';
import {components} from 'types/models';
import SkillsInput from 'shared/components/Skills';
import FormModal, {FormProps, ModalProps} from 'shared/components/FormModal';

type AddStaffRequest = components['schemas']['AddStaffRequest'];
type Skill = components['schemas']['SkillModel'];

interface AddStaffModalProps {
  open: boolean;
  handleClose: () => void;
  onAddStaff: (values: AddStaffRequest) => void;
  skills: Skill[];
}

const initialValues = {firstName: '', lastName: '', email: '', jobTitle: '', comments: '', mobilePhoneNumber: '', skillIds: []};
const AddStaffModal = (props: AddStaffModalProps): JSX.Element => {
  const {onAddStaff, handleClose, open, skills} = props;

  const validationSchema = yup.object({
    firstName: yup.string().required('Please enter Fist name'),
    lastName: yup.string().required('Please enter Last name'),
    email: yup.string().email().required('Please enter valid email'),
    mobilePhoneNumber: yup.string().required('Please enter mobile'),
    jobTitle: yup.string().required('Please enter job title'),
    skillIds: yup.array().min(1, 'At least one skill is required'),
  });

  const formProps: FormProps<AddStaffRequest> = {
    initialValues,
    validationSchema,
    onSubmit: onAddStaff,
  };

  const modalProps: ModalProps = {
    open,
    onClose: handleClose,
    submitButtonLabel: 'Add Staff',
    title: 'Add Staff',
  };

  return (
    <FormModal formProps={formProps} modalProps={modalProps}>
      <Field component={TextField} name='firstName' label='First Name' fullWidth />
      <br />
      <Field component={TextField} name='lastName' label='Last Name' fullWidth />
      <br />
      <Field component={TextField} name='email' type='email' label='Email' fullWidth />
      <br />
      <Field component={TextField} name='mobilePhoneNumber' type='tel' label='Mobile Number' fullWidth />
      <br />
      <Field component={TextField} name='jobTitle' label='Job title' fullWidth />
      <br />
      <SkillsInput skills={skills} label='Skills' name='skillIds' />
      <br />
      <Field component={TextField} name='comments' label='Comments' fullWidth multiline />
      <br />
    </FormModal>
  );
};

export default AddStaffModal;
