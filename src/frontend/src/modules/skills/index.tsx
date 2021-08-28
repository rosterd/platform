import React, {useEffect, useState} from 'react';
import MaterialTable from 'material-table';
import Box from '@material-ui/core/Box';
import AppAnimate from '@crema/core/AppAnimate';
import IntlMessages from '@crema/utility/IntlMessages';
import {Fonts} from 'shared/constants/AppEnums';
import {makeStyles} from '@material-ui/core';
import {components} from 'types/models';
import {getSkills, deleteSkill, addSkill, updateSkill} from 'services';
import useRequest from 'shared/hooks/useRequest';
import {AxiosRequestConfig} from 'axios';

type GetSkillsResponse = components['schemas']['SkillModelPagedList'];
type Skill = components['schemas']['SkillModel'];
type UpdateSkillRequest = components['schemas']['UpdateSkillRequest'];

const useStyles = makeStyles(() => ({
  materialTable: {
    '& .MuiTableCell-paddingCheckbox': {
      paddingLeft: 16,
    },
  },
}));

const initialState: Skill[] = [];

const Skills: React.FC = (): JSX.Element => {
  const classes = useStyles();
  const [results, setResults] = useState({} as GetSkillsResponse);
  const [skills, setSkills] = useState(initialState);
  const [loading, setLoading] = useState(false);
  const {requestMaker} = useRequest();

  const fetchData = async (config: AxiosRequestConfig) => {
    setLoading(true);
    const skillsResponse = await requestMaker<GetSkillsResponse>(config);
    setLoading(false);
    setResults(skillsResponse);
    setSkills(skillsResponse.items || []);
  };

  useEffect(() => {
    (async () => {
      await fetchData(getSkills());
    })();
  }, []);

  const onAdd = async (skillToAdd: Skill) => {
    setLoading(true);
    const addedSkill = await requestMaker<Skill>(addSkill(skillToAdd));
    setLoading(false);
    setSkills([addedSkill, ...skills]);
  };

  const onUpdate = async (skillToUpdate: Skill) => {
    setLoading(true);
    const updatedSkill = await requestMaker<Skill>(updateSkill(skillToUpdate as UpdateSkillRequest));
    setLoading(false);
    setSkills(skills.map((skill) => (skill.skillId === skillToUpdate.skillId ? updatedSkill : skill)));
  };

  const onDelete = async (deletedSkill: Skill) => {
    setLoading(true);
    await requestMaker<GetSkillsResponse>(deleteSkill(deletedSkill?.skillId));
    setLoading(false);
    setSkills(skills.filter((skill) => skill.skillId !== deletedSkill.skillId));
  };

  const handlePageChange = async (page: number, pageSize: number) => {
    const requestConfig = getSkills();
    await fetchData({...requestConfig, url: `${requestConfig.url}?pageNumber=${page + 1}&pageSize=${pageSize}`});
  };

  return (
    <AppAnimate animation='transition.slideUpIn' delay={200}>
      <Box>
        <Box component='h2' color='text.primary' fontSize={16} mb={{xs: 4, sm: 4, xl: 6}} fontWeight={Fonts.BOLD}>
          <IntlMessages id='skills.heading' />
        </Box>
        <Box className={classes.materialTable}>
          <MaterialTable
            title=''
            columns={[
              {title: 'Name', field: 'skillName'},
              {title: 'Description', field: 'description'},
            ]}
            data={skills}
            isLoading={loading}
            editable={{
              onRowAdd: onAdd,
              onRowUpdate: onUpdate,
              onRowDelete: onDelete,
            }}
            options={{
              actionsColumnIndex: -1,
            }}
            onChangePage={handlePageChange}
            page={(results?.currentPage || 1) - 1}
            totalCount={results.totalCount}
          />
        </Box>
      </Box>
    </AppAnimate>
  );
};

export default Skills;
