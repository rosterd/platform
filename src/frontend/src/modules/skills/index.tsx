import React, {useEffect, useState} from 'react';
import MaterialTable from 'material-table';
import Box from '@material-ui/core/Box';
import AppAnimate from '@crema/core/AppAnimate';
import IntlMessages from '@crema/utility/IntlMessages';
import {Fonts} from 'shared/constants/AppEnums';
import {makeStyles} from '@material-ui/core';

const useStyles = makeStyles(() => ({
  materialTable: {
    '& .MuiTableCell-paddingCheckbox': {
      paddingLeft: 16,
    },
  },
}));

interface Skill {
  name: string;
}

const initialState: Skill[] = [];

const Resources = () => {
  const classes = useStyles();
  const [skills, setSkills] = useState(initialState);
  useEffect(() => {
    setTimeout(() => {
      setSkills([
        {name: 'Nurse'},
        {name: 'Chef'},
        {name: 'Health care assistant'},
      ]);
    }, 500);
  }, []);
  console.log(skills);
  return (
    <AppAnimate animation='transition.slideUpIn' delay={200}>
      <Box>
        <Box
          component='h2'
          color='text.primary'
          fontSize={16}
          mb={{xs: 4, sm: 4, xl: 6}}
          fontWeight={Fonts.BOLD}>
          <IntlMessages id='skills.heading' />
        </Box>
        <Box className={classes.materialTable}>
          <MaterialTable
            title=''
            columns={[{title: 'Name', field: 'name'}]}
            data={skills}
            editable={{
              onRowAdd: (newData) =>
                new Promise((resolve, reject) => {
                  setTimeout(() => {
                    setSkills([...skills, newData]);
                    resolve(true);
                  }, 1000);
                }),
              onRowUpdate: (newData, oldData) =>
                new Promise((resolve, reject) => {
                  setTimeout(() => {
                    const dataUpdate = [...skills];
                    const index = (oldData as any).tableData.id;
                    dataUpdate[index] = newData;
                    setSkills([...dataUpdate]);
                    resolve(true);
                  }, 1000);
                }),
              onRowDelete: (oldData) =>
                new Promise((resolve, reject) => {
                  setTimeout(() => {
                    const dataDelete = [...skills];
                    const index = (oldData as any).tableData.id;
                    dataDelete.splice(index, 1);
                    setSkills([...dataDelete]);
                    resolve(true);
                  }, 1000);
                }),
            }}
            options={{
              actionsColumnIndex: -1,
            }}
          />
        </Box>
      </Box>
    </AppAnimate>
  );
};

export default Resources;
