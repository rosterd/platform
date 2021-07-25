import React, {useState, useEffect} from 'react';
import MaterialTable from 'material-table';
import Box from '@material-ui/core/Box';
import useRequest from 'shared/hooks/useRequest';
import AppAnimate from '@crema/core/AppAnimate';
import IntlMessages from '@crema/utility/IntlMessages';
import {Fonts} from 'shared/constants/AppEnums';
import {Button, Grid, makeStyles} from '@material-ui/core';
import AddIcon from '@material-ui/icons/Add';
import {getFacilities} from 'services';
import {components} from 'types/models';
import AddAdminModal from './components/AddAdminModal';

type GetFacilitiesResponse = components['schemas']['FacilityModelPagedList'];
type Facility = components['schemas']['FacilityModel'];

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

const initialState: Facility[] = [];

const Facilities: React.FC = (): JSX.Element => {
  const classes = useStyles();
  const [facilities, setFacilities] = useState(initialState);
  const [showAddFacility, setShowAddFacility] = useState(false);
  const [loading, setLoading] = useState(false);
  const {requestMaker} = useRequest();

  useEffect(() => {
    (async () => {
      setLoading(true);
      const facilitiesRes = await requestMaker<GetFacilitiesResponse>(getFacilities());
      setLoading(false);
      if (facilitiesRes) {
        setFacilities(facilitiesRes.items || []);
      }
    })();
  }, []);

  return (
    <AppAnimate animation='transition.slideUpIn' delay={200}>
      <Box>
        <Box mb={{xs: 4, sm: 4, xl: 6}}>
          <Grid container direction='row' justify='space-between' alignItems='center'>
            <Grid item xs={6}>
              <Box component='h2' color='text.primary' fontSize={16} fontWeight={Fonts.BOLD}>
                <IntlMessages id='admins.heading' />
              </Box>
            </Grid>
            <Grid item xs={6} className={classes.buttonContainer}>
              <Button variant='contained' color='primary' startIcon={<AddIcon />} onClick={() => setShowAddFacility(true)}>
                Invite Admin
              </Button>
            </Grid>
          </Grid>
        </Box>
        <Box className={classes.materialTable}>
          <MaterialTable
            title=''
            columns={[
              {title: 'Name', field: 'facilityName'},
              {title: 'Email', field: 'address'},
              {title: 'Facility', field: 'facilityName'},
            ]}
            data={facilities}
            isLoading={loading}
          />
        </Box>
        <AddAdminModal facilities={facilities} open={showAddFacility} handleClose={() => setShowAddFacility(false)} />
      </Box>
    </AppAnimate>
  );
};

export default Facilities;
