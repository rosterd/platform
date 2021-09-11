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

type GetSkillsResponse = components['schemas']['SkillModelPagedList'];
type AddStaffRequest = components['schemas']['AddStaffRequest'];
type Skill = components['schemas']['SkillModel'];
type Staff = components['schemas']['StaffModel'];
interface AddStaffModalProps {
  open: boolean;
  handleClose: () => void;
  staffMember?: Staff;
  onAddStaff: (values: AddStaffRequest) => void;
  onUpdateStaff: (values: AddStaffRequest) => void;
}

const initialState: Skill[] = [];
const defaultInitialValue = {firstName: '', lastName: '', email: '', jobTitle: '', comments: '', mobilePhoneNumber: '', skillIds: []};
const AddStaffModal = (props: AddStaffModalProps): JSX.Element => {
  const [skills, setSkills] = useState(initialState);
  const [initialValues, setInitialValues] = React.useState<AddStaffRequest>(defaultInitialValue);
  const {staffMember} = props;

  const {requestMaker} = useRequest();

  useEffect(() => {
    (async () => {
      const skillsResponse = await requestMaker<GetSkillsResponse>(getSkills());
      setSkills(skillsResponse.items || []);
    })();
  }, []);

  useEffect(() => {
    if (staffMember) {
      const {firstName = '', lastName = '', email = '', jobTitle = '', comments = '', mobilePhoneNumber = '', staffSkills = []} = staffMember;
      setInitialValues({
        firstName,
        lastName,
        email,
        jobTitle,
        comments,
        mobilePhoneNumber,
        skillIds: staffSkills?.map((skill) => skill.skillId || 0),
      });
    } else {
      setInitialValues(defaultInitialValue);
    }
  }, [staffMember]);

  const validationSchema = yup.object({
    firstName: yup.string().required('Please enter Fist name'),
    lastName: yup.string().required('Please enter Last name'),
    email: yup.string().email().required('Please enter valid email'),
    mobilePhoneNumber: yup.string().required('Please enter mobile'),
    jobTitle: yup.string().required('Please enter job title'),
  });

  return (
    <Formik
      enableReinitialize
      initialValues={initialValues}
      validationSchema={validationSchema}
      onSubmit={async (values: AddStaffRequest, {setSubmitting, resetForm}) => {
        setSubmitting(true);
        try {
          const response = staffMember ? await props.onAddStaff(values) : await props.onUpdateStaff(values);
          console.log(response);
          resetForm();
        } catch (err) {
          console.log(err);
        }
        setSubmitting(false);
      }}>
      {({submitForm, isSubmitting}) => (
        <Dialog fullWidth maxWidth='sm' open={props.open} onClose={props.handleClose} aria-labelledby='form-dialog-title'>
          <DialogTitle id='form-dialog-title'>{staffMember ? 'Update Staff' : 'Add Staff'}</DialogTitle>
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
              <Field component={TextField} name='comments' label='Comments' fullWidth multiline />
              <br />
              {isSubmitting && <LinearProgress />}
            </Form>
          </DialogContent>
          <DialogActions>
            <Button onClick={props.handleClose} color='primary'>
              Close
            </Button>
            <Button onClick={submitForm} color='primary' disabled={isSubmitting} variant='contained'>
              {staffMember ? 'Update ' : 'Add '}
            </Button>
          </DialogActions>
        </Dialog>
      )}
    </Formik>
  );
};

export default AddStaffModal;
