import React, {useEffect, useState} from 'react';
import Autocomplete from '@material-ui/lab/Autocomplete';
import {components} from 'types/models';
import TextField from '@material-ui/core/TextField';
import {useField} from 'formik';
import {FormHelperText} from '@material-ui/core';

type Skill = components['schemas']['SkillModel'];
interface Props {
  label: string;
  name: string;
  skills: Skill[];
}

const initialSelectedSkills: Skill[] = [];

function notEmpty<T>(value: T | null | undefined): value is T {
  return value !== null && value !== undefined;
}

const SkillsInput = ({label, name, skills}: Props): JSX.Element => {
  const [field, meta, helpers] = useField<number[]>(name);
  const [selectedSkills, setSelectedSkills] = useState(initialSelectedSkills);
  const {setValue} = helpers;

  useEffect(() => {
    const selectedSkillIds = field.value;
    const selectedSkillsFromSkillIds = (selectedSkillIds || []).map((skillId) => skills.find((skill) => skill.skillId === skillId)).filter(notEmpty);
    setSelectedSkills(selectedSkillsFromSkillIds);
  }, [field.value]);

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
        renderInput={(params) => <TextField error={!!(meta.touched && meta.error)} {...params} variant='standard' label={label} placeholder='Select Skills' />}
      />
      {meta.touched && meta.error ? <FormHelperText error>{meta.error}</FormHelperText> : null}
    </>
  );
};

export default SkillsInput;
