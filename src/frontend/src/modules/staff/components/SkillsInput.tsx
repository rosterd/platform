import React from 'react';
import Autocomplete from '@material-ui/lab/Autocomplete';
import {components} from 'types/models';
import TextField from '@material-ui/core/TextField';
import {useField} from 'formik';

type Skill = components['schemas']['SkillModel'];
interface Props {
  label: string;
  skills: Skill[];
}

const SkillsInput = ({label, skills}: Props): JSX.Element => {
  const [field, meta, helpers] = useField(label);
  const {setValue} = helpers;

  return (
    <>
      <Autocomplete
        multiple
        id='tags-standard'
        options={skills}
        getOptionLabel={(option) => option?.skillName}
        onChange={(event, newValue) => {
          const skillIds = newValue.map(({skillId}) => skillId);
          setValue(skillIds);
        }}
        renderInput={(params) => <TextField {...params} variant='standard' label={label} {...field} placeholder='Select Skills' />}
      />
      {meta.touched && meta.error ? <div className='error'>{meta.error}</div> : null}
    </>
  );
};

export default SkillsInput;
