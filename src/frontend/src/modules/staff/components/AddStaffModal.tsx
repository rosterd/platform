import React, {useEffect, useState} from 'react';
import Dialog from '@material-ui/core/Dialog';
import DialogActions from '@material-ui/core/DialogActions';
import DialogContent from '@material-ui/core/DialogContent';
import DialogTitle from '@material-ui/core/DialogTitle';
import {Formik, Form, Field} from 'formik';
import {Button, LinearProgress} from '@material-ui/core';
import {TextField} from 'formik-material-ui';
import useRequest from 'shared/hooks/useRequest';
import * as yup from 'yup';
import {getSkills} from 'services';
import {components} from 'types/models';
import SkillsInput from 'shared/components/Skills';

interface FormValues {
  email: string;
  firstName: string;
  lastName: string;
  mobilePhoneNumber: string;
  skillIds: number[];
  jobTitle: string;
}

interface AddStaffModalProps {
  open: boolean;
  handleClose: () => void;
  onAddStaff: (values: FormValues) => void;
}

type GetSkillsResponse = components['schemas']['SkillModelPagedList'];
type Skill = components['schemas']['SkillModel'];
const initialState: Skill[] = [];

const AddStaffModal = (props: AddStaffModalProps): JSX.Element => {
  const [skills, setSkills] = useState(initialState);
  const {requestMaker} = useRequest();

  useEffect(() => {
    (async () => {
      const skillsResponse = await requestMaker<GetSkillsResponse>(getSkills());
      setSkills(skillsResponse.items || []);
    })();
  }, []);

  const validationSchema = yup.object({
    firstName: yup.string().required('Please enter Fist name'),
    lastName: yup.string().required('Please enter Last name'),
    email: yup.string().email().required('Please enter valid email'),
    mobilePhoneNumber: yup.string().required('Please enter mobile'),
    jobTitle: yup.string().required('Please enter job title'),
  });
  return (
    <Formik
      initialValues={{
        firstName: '',
        lastName: '',
        email: '',
        jobTitle: '',
        mobilePhoneNumber: '',
        skillIds: [],
      }}
      validationSchema={validationSchema}
      onSubmit={async (values: FormValues, {setSubmitting, resetForm}) => {
        setSubmitting(true);
        await props.onAddStaff(values);
        resetForm();
        setSubmitting(false);
      }}>
      {({submitForm, isSubmitting}) => (
        <Dialog fullWidth maxWidth='sm' open={props.open} onClose={props.handleClose} aria-labelledby='form-dialog-title'>
          <DialogTitle id='form-dialog-title'>Add Staff</DialogTitle>
          <DialogContent>
            <Form>
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
              {isSubmitting && <LinearProgress />}
            </Form>
          </DialogContent>
          <DialogActions>
            <Button onClick={props.handleClose} color='primary'>
              Close
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
