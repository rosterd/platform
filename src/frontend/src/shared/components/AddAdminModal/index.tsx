/* eslint-disable react/prop-types */
import React, {useEffect, useState} from 'react';
import Dialog from '@material-ui/core/Dialog';
import DialogActions from '@material-ui/core/DialogActions';
import DialogContent from '@material-ui/core/DialogContent';
import DialogTitle from '@material-ui/core/DialogTitle';
import {Formik, Form, Field} from 'formik';
import {Button, LinearProgress} from '@material-ui/core';
import {getOrganizations, getFacilities, getSkills} from 'services';
import useRequest from 'shared/hooks/useRequest';
import {TextField, Select} from 'formik-material-ui';
import * as yup from 'yup';
import {components} from 'types/models';
import MenuItem from '@material-ui/core/MenuItem';
import InputLabel from '@material-ui/core/InputLabel';
import FormControl from '@material-ui/core/FormControl';
import SkillsInput from 'shared/components/Skills';

type AddAdminUserRequest = components['schemas']['AddAdminUserRequest'];
type Organization = components['schemas']['OrganizationModel'];
type Facility = components['schemas']['FacilityModel'];
type GetOrganizationsResponse = components['schemas']['OrganizationModelPagedList'];
type GetFacilitiesResponse = components['schemas']['FacilityModelPagedList'];
type GetSkillsResponse = components['schemas']['SkillModelPagedList'];
type Skill = components['schemas']['SkillModel'];

interface AddAdminModalProps {
  open: boolean;
  onAddAdmin: (value: AddAdminUserRequest) => void;
  handleClose: () => void;
  isOrganisationAdmin?: boolean;
}

const initialOrganizationsState: Organization[] = [];
const initialFacilitiesState: Facility[] = [];
const initialSkillsState: Skill[] = [];

const AddAdminModal: React.FC<AddAdminModalProps> = (props): JSX.Element => {
  const [origanizations, setOrganizatons] = useState(initialOrganizationsState);
  const [facilities, setFacilities] = useState(initialFacilitiesState);
  const [skills, setSkills] = useState(initialSkillsState);

  const {requestMaker} = useRequest();
  const {isOrganisationAdmin = false} = props;

  useEffect(() => {
    (async () => {
      if (isOrganisationAdmin) {
        const origanizationsResponse = await requestMaker<GetOrganizationsResponse>(getOrganizations());
        setOrganizatons(origanizationsResponse.items || []);
      } else {
        const facilitiesResponse = await requestMaker<GetFacilitiesResponse>(getFacilities());
        setFacilities(facilitiesResponse.items || []);
        const skillsResponse = await requestMaker<GetSkillsResponse>(getSkills());
        setSkills(skillsResponse.items || []);
      }
    })();
  }, [isOrganisationAdmin]);

  const validationSchema = yup.object({
    firstName: yup.string().required('Please enter First Name'),
    lastName: yup.string().required('Please enter Last Name'),
    email: yup.string().required('Please enter Email Address'),
  });

  return (
    <Formik
      initialValues={{
        firstName: '',
        lastName: '',
        email: '',
        phoneNumber: '',
        auth0OrganizationId: '',
        facilityId: '',
      }}
      validationSchema={validationSchema}
      onSubmit={async (values, {setSubmitting, resetForm}) => {
        await props.onAddAdmin(values);
        setSubmitting(false);
        resetForm();
      }}>
      {({submitForm, isSubmitting}) => (
        <Dialog fullWidth maxWidth='sm' open={props.open} onClose={props.handleClose} aria-labelledby='form-dialog-title'>
          <DialogTitle id='form-dialog-title'>Invite Admin</DialogTitle>
          <DialogContent>
            <Form>
              {isOrganisationAdmin ? (
                <FormControl fullWidth>
                  <InputLabel htmlFor='age-simple'>Organization</InputLabel>
                  <Field
                    component={Select}
                    name='auth0OrganizationId'
                    inputProps={{
                      id: 'auth0OrganizationId',
                      fullWidth: true,
                    }}>
                    {origanizations.map((organization) => (
                      <MenuItem key={organization.organizationId} value={organization?.auth0OrganizationId || 0}>
                        {organization.organizationName}
                      </MenuItem>
                    ))}
                  </Field>
                </FormControl>
              ) : (
                <FormControl fullWidth>
                  <InputLabel htmlFor='age-simple'>Facility</InputLabel>
                  <Field
                    component={Select}
                    name='facilityId'
                    inputProps={{
                      id: 'facilityId',
                      fullWidth: true,
                    }}>
                    {facilities.map((facility) => (
                      <MenuItem key={facility.facilityId} value={facility?.facilityId || 0}>
                        {facility.facilityName}
                      </MenuItem>
                    ))}
                  </Field>
                </FormControl>
              )}
              <Field component={TextField} name='firstName' label='First Name' fullWidth />
              <br />
              <Field component={TextField} name='lastName' label='Last Name' fullWidth />
              <br />
              <Field component={TextField} name='email' label='Email' fullWidth />
              <br />
              <Field component={TextField} name='phoneNumber' label='Phone Number' fullWidth />
              <br />
              {!isOrganisationAdmin && <SkillsInput skills={skills} label='Skills' name='skillIds' />}
              <br />
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
};

export default AddAdminModal;
