/* eslint-disable react/prop-types */
import React, {useEffect, useState} from 'react';
import {Field, FieldProps} from 'formik';
import {Select} from '@material-ui/core';
import {getOrganizations, getFacilities, getSkills} from 'services';
import useRequest from 'shared/hooks/useRequest';
import {TextField} from 'formik-material-ui';
import * as yup from 'yup';
import {components} from 'types/models';
import MenuItem from '@material-ui/core/MenuItem';
import InputLabel from '@material-ui/core/InputLabel';
import FormControl from '@material-ui/core/FormControl';
import SkillsInput from 'shared/components/Skills';
import FormModal, {FormProps, ModalProps} from '../FormModal';

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
  const {isOrganisationAdmin = false, open, handleClose, onAddAdmin} = props;

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
  const initialValues = {
    firstName: '',
    lastName: '',
    email: '',
    phoneNumber: '',
    auth0OrganizationId: '',
    facilityIds: [''],
  };
  const formProps: FormProps<AddAdminUserRequest> = {
    onSubmit: onAddAdmin,
    initialValues,
    validationSchema,
  };

  const modalProps: ModalProps = {
    open,
    onClose: handleClose,
    title: 'Invite Admin',
    submitButtonLabel: 'Invite',
    closeAfter: true,
  };

  return (
    <FormModal formProps={formProps} modalProps={modalProps}>
      {isOrganisationAdmin && !!origanizations.length && (
        <FormControl fullWidth>
          <InputLabel htmlFor='age-simple'>Organizationn</InputLabel>
          <Field name='auth0OrganizationId'>
            {({field: {value, ...field}, form}: FieldProps) => (
              <Select
                id='auth0OrganizationId'
                value={value[0]}
                {...field}
                onChange={(event: React.ChangeEvent<{name?: string; value: unknown}>) => {
                  form.setFieldValue(field.name, event.target.value);
                }}>
                {origanizations.map((organization) => (
                  <MenuItem key={organization.organizationId} value={organization?.auth0OrganizationId || 0}>
                    {organization.organizationName}
                  </MenuItem>
                ))}
              </Select>
            )}
          </Field>
        </FormControl>
      )}
      {!isOrganisationAdmin && (
        <FormControl fullWidth>
          <InputLabel htmlFor='age-simple'>Facility</InputLabel>
          <Field name='facilityIds'>
            {({field: {value, ...field}, form}: FieldProps) => (
              <Select
                id='facilityIds'
                value={value[0]}
                {...field}
                onChange={(event: React.ChangeEvent<{name?: string; value: unknown}>) => {
                  form.setFieldValue(field.name, [event.target.value]);
                }}>
                {facilities.map((facility) => (
                  <MenuItem key={facility.facilityId} value={facility?.facilityId || 0}>
                    {facility.facilityName}
                  </MenuItem>
                ))}
              </Select>
            )}
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
    </FormModal>
  );
};

export default AddAdminModal;
