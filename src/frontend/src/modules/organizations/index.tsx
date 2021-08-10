import React, {useState, useEffect} from 'react';
import MaterialTable from 'material-table';
import Box from '@material-ui/core/Box';
import useRequest from 'shared/hooks/useRequest';
import AppAnimate from '@crema/core/AppAnimate';
import IntlMessages from '@crema/utility/IntlMessages';
import {Fonts} from 'shared/constants/AppEnums';
import {Button, Grid, makeStyles} from '@material-ui/core';
import AddIcon from '@material-ui/icons/Add';
import {getOrganizations, addOrganization} from 'services';
import {components} from 'types/models';
import AddOrganizationModal, {AddOrganizationFormValues} from './components/AddOrganizationModal';

type GetOrganizationsResponse = components['schemas']['OrganizationModelPagedList'];
type Organization = components['schemas']['OrganizationModel'];
type AddOrganizationRequest = components['schemas']['AddOrganizationRequest'];

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

const initialState: Organization[] = [];

const Organizations: React.FC = (): JSX.Element => {
  const classes = useStyles();
  const [organizations, setOrganizations] = useState(initialState);
  const [showAddOrganization, setShowAddOrganization] = useState(false);
  const [loading, setLoading] = useState(false);
  const {requestMaker} = useRequest();

  useEffect(() => {
    (async () => {
      setLoading(true);
      const organizationsRes = await requestMaker<GetOrganizationsResponse>(getOrganizations());
      setLoading(false);
      if (organizationsRes) {
        setOrganizations((organizationsRes?.items || []).filter((org) => org.auth0OrganizationId?.indexOf('org_') !== -1) || []);
      }
    })();
  }, []);

  const handleAddOrganization = async (values: AddOrganizationFormValues) => {
    const requestBody: AddOrganizationRequest = {
      organization: values,
    };

    const organizationsRes = await requestMaker<Organization>(addOrganization(requestBody));
    if (organizationsRes) {
      setOrganizations([organizationsRes, ...organizations]);
    }
  };

  return (
    <AppAnimate animation='transition.slideUpIn' delay={200}>
      <Box>
        <Box mb={{xs: 4, sm: 4, xl: 6}}>
          <Grid container direction='row' justify='space-between' alignItems='center'>
            <Grid item xs={6}>
              <Box component='h2' color='text.primary' fontSize={16} fontWeight={Fonts.BOLD}>
                <IntlMessages id='organizations.heading' />
              </Box>
            </Grid>
            <Grid item xs={6} className={classes.buttonContainer}>
              <Button variant='contained' color='primary' startIcon={<AddIcon />} onClick={() => setShowAddOrganization(true)}>
                Add Organization
              </Button>
            </Grid>
          </Grid>
        </Box>
        <Box className={classes.materialTable}>
          <MaterialTable
            title=''
            columns={[
              {title: 'Organization Name', field: 'organizationName'},
              {title: 'Address', field: 'address'},
              {title: 'Comments', field: 'comments'},
            ]}
            data={organizations}
            isLoading={loading}
          />
        </Box>
        <AddOrganizationModal open={showAddOrganization} handleClose={() => setShowAddOrganization(false)} onAddOrganization={handleAddOrganization} />
      </Box>
    </AppAnimate>
  );
};

export default Organizations;
