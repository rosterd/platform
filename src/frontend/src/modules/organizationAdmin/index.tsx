import React, {useState, useEffect} from 'react';
import MaterialTable from 'material-table';
import Box from '@material-ui/core/Box';
import useRequest from 'shared/hooks/useRequest';
import AppAnimate from '@crema/core/AppAnimate';
import IntlMessages from '@crema/utility/IntlMessages';
import {Fonts} from 'shared/constants/AppEnums';
import {Button, Grid, makeStyles} from '@material-ui/core';
import AddIcon from '@material-ui/icons/Add';
import {addOrganizationAdmin} from 'services';
import {components} from 'types/models';
import AddAdminModal from 'shared/components/AddAdminModal';

type AddAdminUserRequest = components['schemas']['AddAdminUserRequest'];
type AdminUserModel = components['schemas']['Auth0UserModel'];

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

const initialState: AdminUserModel[] = [];

const OrganizationAdmin: React.FC = (): JSX.Element => {
  const classes = useStyles();
  const [admins, setAdmins] = useState(initialState);
  const [showAddAdminModal, setShowAddAdminModal] = useState(false);
  const [loading, setLoading] = useState(false);
  const {requestMaker} = useRequest();

  useEffect(() => {
    (async () => {
      setLoading(true);
      console.log('Get admin have to implement');
      setLoading(false);
    })();
  }, []);

  const handleAddFacilityAdmin = async (values: AddAdminUserRequest) => {
    const admin = await requestMaker<AdminUserModel>(addOrganizationAdmin(values));
    setAdmins([...admins, admin]);
  };

  return (
    <AppAnimate animation='transition.slideUpIn' delay={200}>
      <Box>
        <Box mb={{xs: 4, sm: 4, xl: 6}}>
          <Grid container direction='row' justify='space-between' alignItems='center'>
            <Grid item xs={6}>
              <Box component='h2' color='text.primary' fontSize={16} fontWeight={Fonts.BOLD}>
                <IntlMessages id='organization.admins.heading' />
              </Box>
            </Grid>
            <Grid item xs={6} className={classes.buttonContainer}>
              <Button variant='contained' color='primary' startIcon={<AddIcon />} onClick={() => setShowAddAdminModal(true)}>
                Inivite Organization Admin
              </Button>
            </Grid>
          </Grid>
        </Box>
        <Box className={classes.materialTable}>
          <MaterialTable
            title=''
            columns={[
              {title: 'First Name', field: 'firstName'},
              {title: 'Last Name', field: 'lastName'},
              {title: 'Email', field: 'email'},
              {title: 'Phone', field: 'mobilePhoneNumber'},
            ]}
            data={admins}
            isLoading={loading}
          />
        </Box>
        <AddAdminModal isOrganisationAdmin open={showAddAdminModal} onAddAdmin={handleAddFacilityAdmin} handleClose={() => setShowAddAdminModal(false)} />
      </Box>
    </AppAnimate>
  );
};

export default OrganizationAdmin;
