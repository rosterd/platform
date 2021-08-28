import React from 'react';
import Autocomplete from '@material-ui/lab/Autocomplete';
import {components} from 'types/models';
import TextField from '@material-ui/core/TextField';
import {useField} from 'formik';

type Skill = components['schemas']['SkillModel'];
interface Props {
  label: string;
  name: string;
  skills: Skill[];
}

const SkillsInput = ({label, name, skills}: Props): JSX.Element => {
  const [field, meta, helpers] = useField<number[]>(name);
  const {setValue} = helpers;

  const selectedSkillIds = field.value || [];
  const selectedSkills = selectedSkillIds.map((skillId) => skills.find((skill) => skill.skillId === skillId));
  return (
    <>
      <Autocomplete
        multiple
        id='tags-standard'
        options={skills}
        value={selectedSkills}
        getOptionSelected={(option, value) => value?.skillId === option?.skillId}
        getOptionLabel={(option) => option?.skillName || ''}
        onChange={(event, newValue) => {
          const skillIds = newValue.map((skill) => skill?.skillId || 0);
          setValue(skillIds);
        }}
        renderInput={(params) => <TextField {...params} variant='standard' label={label} {...field} placeholder='Select Skills' />}
      />
      {meta.touched && meta.error ? <div className='error'>{meta.error}</div> : null}
    </>
  );
};

export default SkillsInput;
