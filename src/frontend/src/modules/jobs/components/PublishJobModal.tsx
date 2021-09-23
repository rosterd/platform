import React, {useState, useEffect, useMemo} from 'react';
import {Form, Field} from 'formik';
import {FormControl, FormControlLabel, InputLabel, MenuItem} from '@material-ui/core';
import {Select, Switch, TextField} from 'formik-material-ui';
import {DateTimePicker} from 'formik-material-ui-pickers';
import {MuiPickersUtilsProvider} from '@material-ui/pickers';
import DateFnsUtils from '@date-io/date-fns';
import {getFacilities, getSkills} from 'services';
import {components} from 'types/models';
import useRequest from 'shared/hooks/useRequest';
import SkillsInput from 'shared/components/Skills';
import * as yup from 'yup';
import {useAuthUser} from '@crema/utility/AppHooks';
import {AuthUser} from 'types/models/AuthUser';
import FormModal, {FormProps, ModalProps} from 'shared/components/FormModal';

interface PublishJobModalProps {
  open: boolean;
  handleClose: () => void;
  onPublishJob: (values: any) => void;
}

type GetSkillsResponse = components['schemas']['SkillModelPagedList'];
type GetFacilitiesResponse = components['schemas']['FacilityModelPagedList'];
type Skill = components['schemas']['SkillModel'];
type Facility = components['schemas']['FacilityModel'];

const initialState: Skill[] = [];
const initialFacilitiesState: Facility[] = [];
const formValues = {
  jobTitle: '',
  description: '',
  jobStartDateTimeUtc: '',
  jobEndDateTimeUtc: '',
  comments: '',
  gracePeriodToCancelMinutes: 0,
  responsibilities: '',
  experience: '',
  isNightShift: false,
  skillsRequiredForJob: [],
};

const PublishJobModal = (props: PublishJobModalProps): JSX.Element => {
  const [skills, setSkills] = useState(initialState);
  const {requestMaker} = useRequest();
  const authuser: AuthUser | null = useAuthUser();
  const [facilities, setFacilities] = useState(initialFacilitiesState);
  const {open, handleClose, onPublishJob} = props;
  const isOrganisationAdmin = useMemo(() => (authuser!.role || []).indexOf('OrganizationAdmin') !== -1, [authuser]);
  const initialValues = isOrganisationAdmin ? {...formValues, facilityId: 0} : formValues;

  const validationSchema = yup.object({
    jobTitle: yup.string().required('Please enter Job Title'),
    description: yup.string().required('Please enter Job Description'),
    jobStartDateTimeUtc: yup.date().required('Please select the start time'),
    jobEndDateTimeUtc: yup.date().required('Please select the end time'),
    gracePeriodToCancelMinutes: yup.number().typeError('Grace period must be a number in minutes'),
    skillsRequiredForJob: yup.array().min(1, 'At least one skill is required'),
  });

  useEffect(() => {
    (async () => {
      if (isOrganisationAdmin) {
        const facilitiesResponse = await requestMaker<GetFacilitiesResponse>(getFacilities());
        setFacilities(facilitiesResponse.items || []);
      }
    })();
  }, [isOrganisationAdmin]);

  useEffect(() => {
    (async () => {
      const skillsResponse = await requestMaker<GetSkillsResponse>(getSkills());
      setSkills(skillsResponse.items || []);
    })();
  }, []);

  const formProps: FormProps<any> = {
    onSubmit: onPublishJob,
    initialValues,
    validationSchema,
  };

  const modalProps: ModalProps = {
    open,
    onClose: handleClose,
    title: 'Publish Job',
    submitButtonLabel: 'Publish',
  };

  return (
    <FormModal formProps={formProps} modalProps={modalProps}>
      <MuiPickersUtilsProvider utils={DateFnsUtils}>
        <Form>
          {isOrganisationAdmin && (
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
          <Field component={TextField} name='jobTitle' label='Title' fullWidth />
          <br />
          <Field component={TextField} name='description' label='Description' fullWidth multiline />
          <br />
          <Field component={DateTimePicker} name='jobStartDateTimeUtc' label='Start Time' fullWidth />
          <br />
          <Field component={DateTimePicker} name='jobEndDateTimeUtc' label='End Time' fullWidth />
          <br />
          <Field component={TextField} name='gracePeriodToCancelMinutes' label='Grace Period(in mins)' fullWidth />
          <br />
          <br />
          <FormControlLabel control={<Field name='isNightShift' type='checkbox' component={Switch} />} label='Night Shift' />
          <br />
          <SkillsInput skills={skills} label='Skills' name='skillsRequiredForJob' />
          <br />
          <Field component={TextField} name='experience' label='Experience' fullWidth multiline />
          <br />
          <Field component={TextField} name='comments' label='Comments' fullWidth multiline />
          <br />
          <Field component={TextField} name='responsibilities' label='Responsibilities' fullWidth multiline />
        </Form>
      </MuiPickersUtilsProvider>
    </FormModal>
  );
};

export default PublishJobModal;
