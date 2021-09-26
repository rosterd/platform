import React from 'react';
import MaterialTable from 'material-table';
import Box from '@material-ui/core/Box';
import AppAnimate from '@crema/core/AppAnimate';
import IntlMessages from '@crema/utility/IntlMessages';
import {Fonts} from 'shared/constants/AppEnums';
import {makeStyles} from '@material-ui/core';
import {components} from 'types/models';

import usePaging from 'shared/hooks/usePaging';

type GetSkillsResponse = components['schemas']['SkillModelPagedList'];
type Skill = components['schemas']['SkillModel'];

const useStyles = makeStyles(() => ({
  materialTable: {
    '& .MuiTableCell-paddingCheckbox': {
      paddingLeft: 16,
    },
  },
}));

const Skills: React.FC = (): JSX.Element => {
  const classes = useStyles();
  const {handlePageChange, currentPage, totalCount, items, loading, addData, updateData} = usePaging<Skill, GetSkillsResponse>('skills', 'skillId');

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
              {title: 'Name', field: 'skillName', validate: (rowData) => (rowData.skillName ? true : 'Name can not be empty')},
              {title: 'Description', field: 'description'},
            ]}
            data={items}
            isLoading={loading}
            editable={{
              onRowAdd: addData,
              onRowUpdate: updateData,
            }}
            options={{
              actionsColumnIndex: -1,
            }}
            onChangePage={handlePageChange}
            page={currentPage}
            totalCount={totalCount}
          />
        </Box>
      </Box>
    </AppAnimate>
  );
};

export default Skills;
