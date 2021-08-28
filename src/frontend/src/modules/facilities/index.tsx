import React, {useState, useEffect} from 'react';
import MaterialTable from 'material-table';
import Box from '@material-ui/core/Box';
import useRequest from 'shared/hooks/useRequest';
import AppAnimate from '@crema/core/AppAnimate';
import IntlMessages from '@crema/utility/IntlMessages';
import {Fonts} from 'shared/constants/AppEnums';
import {Button, Grid, makeStyles} from '@material-ui/core';
import AddIcon from '@material-ui/icons/Add';
import {getFacilities, addFacility} from 'services';
import {components} from 'types/models';
import AddFacilityModal, {AddFacilityFormValues} from './components/AddFacilityModal';

type GetFacilitiesResponse = components['schemas']['FacilityModelPagedList'];
type AddFacilityRequest = components['schemas']['AddFacilityRequest'];
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

  const handleAddFacility = async (values: AddFacilityFormValues) => {
    setLoading(true);
    const requestBody: AddFacilityRequest = {
      ...values,
      latitude: 0,
      longitude: 0,
    };
    const facilityRes = await requestMaker<Facility>(addFacility(requestBody));
    setLoading(false);
    if (facilityRes) {
      setFacilities([facilityRes, ...facilities]);
    }
  };

  return (
    <AppAnimate animation='transition.slideUpIn' delay={200}>
      <Box>
        <Box mb={{xs: 4, sm: 4, xl: 6}}>
          <Grid container direction='row' justify='space-between' alignItems='center'>
            <Grid item xs={6}>
              <Box component='h2' color='text.primary' fontSize={16} fontWeight={Fonts.BOLD}>
                <IntlMessages id='facilities.heading' />
              </Box>
            </Grid>
            <Grid item xs={6}>
              <Box textAlign='right'>
                <Button variant='contained' color='primary' startIcon={<AddIcon />} onClick={() => setShowAddFacility(true)}>
                  Add Facility
                </Button>
              </Box>
            </Grid>
          </Grid>
        </Box>
        <Box className={classes.materialTable}>
          <MaterialTable
            title=''
            columns={[
              {title: 'Name', field: 'facilityName'},
              {title: 'Address', field: 'address'},
              {title: 'City', field: 'city'},
              {title: 'Phone', field: 'phoneNumber1'},
            ]}
            data={facilities}
            isLoading={loading}
          />
        </Box>
        <AddFacilityModal open={showAddFacility} onAddFacility={handleAddFacility} handleClose={() => setShowAddFacility(false)} />
      </Box>
    </AppAnimate>
  );
};

export default Facilities;
