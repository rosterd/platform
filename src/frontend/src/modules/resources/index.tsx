import React, {useState} from 'react';
import MaterialTable from 'material-table';
import Box from '@material-ui/core/Box';
import AppAnimate from '@crema/core/AppAnimate';
import IntlMessages from '@crema/utility/IntlMessages';
import {Fonts} from 'shared/constants/AppEnums';
import {Button, Grid, makeStyles} from '@material-ui/core';
import AddIcon from '@material-ui/icons/Add';
import AddResourceModal from './components/AddResourceModal';

const useStyles = makeStyles(() => ({
  materialTable: {
    '& .MuiTableCell-paddingCheckbox': {
      paddingLeft: 16,
    },
  },
  buttonContainer: {
    textAlign: 'right',
  },
}));

const Resources = () => {
  const classes = useStyles();
  const [showAddResource, setShowAddResource] = useState(false);
  return (
    <AppAnimate animation='transition.slideUpIn' delay={200}>
      <Box>
        <Box mb={{xs: 4, sm: 4, xl: 6}}>
          <Grid
            container
            direction='row'
            justify='space-between'
            alignItems='center'>
            <Grid item xs={6}>
              <Box
                component='h2'
                color='text.primary'
                fontSize={16}
                fontWeight={Fonts.BOLD}>
                <IntlMessages id='resources.heading' />
              </Box>
            </Grid>
            <Grid item xs={6} className={classes.buttonContainer}>
              <Button
                variant='contained'
                color='primary'
                startIcon={<AddIcon />}
                onClick={() => setShowAddResource(true)}>
                Add Resource
              </Button>
            </Grid>
          </Grid>
        </Box>
        <Box className={classes.materialTable}>
          <MaterialTable
            title=''
            columns={[
              {title: 'Name', field: 'name'},
              {title: 'Surname', field: 'surname'},
              {title: 'Skill', field: 'skill', type: 'numeric'},
              {
                title: 'Working Status',
                field: 'workingStatus',
                lookup: {0: 'Not working', 1: 'Working'},
              },
            ]}
            data={[
              {
                name: 'Mehmet',
                surname: 'Baran',
                skill: 'Nurse',
                workingStatus: 1,
              },
              {
                name: 'Zerya Betül',
                surname: 'Dan',
                skill: 'Chef',
                workingStatus: 0,
              },
            ]}
            actions={[
              {
                icon: 'save',
                tooltip: 'Save User',
                onClick: (event, rowData) => alert('You saved '),
              },
            ]}
          />
        </Box>
        <AddResourceModal
          open={showAddResource}
          handleClose={() => setShowAddResource(false)}
        />
      </Box>
    </AppAnimate>
  );
};

export default Resources;
