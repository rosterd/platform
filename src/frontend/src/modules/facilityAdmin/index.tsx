import React, {useState, useEffect} from 'react';
import MaterialTable from 'material-table';
import Box from '@material-ui/core/Box';
import useRequest from 'shared/hooks/useRequest';
import AppAnimate from '@crema/core/AppAnimate';
import IntlMessages from '@crema/utility/IntlMessages';
import {Fonts} from 'shared/constants/AppEnums';
import {Button, Grid, makeStyles} from '@material-ui/core';
import AddIcon from '@material-ui/icons/Add';
import {addFacilityAdmin, deleteFacilityAdmin, getAdmins} from 'services';
import {components} from 'types/models';
import AddAdminModal from 'shared/components/AddAdminModal';
import {AxiosRequestConfig} from 'axios';

type AddAdminWhoIsAlsoStaffRequest = components['schemas']['AddAdminWhoIsAlsoStaffRequest'];
type StaffModel = components['schemas']['StaffModel'];
type StaffModelPagedList = components['schemas']['StaffModelPagedList'];

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

const initialState: StaffModel[] = [];

const FacilityAdmin: React.FC = (): JSX.Element => {
  const classes = useStyles();
  const [admins, setAdmins] = useState(initialState);
  const [showAddAdminModal, setShowAddAdminModal] = useState(false);
  const [loading, setLoading] = useState(false);
  const {requestMaker} = useRequest();

  const fetchData = async (config: AxiosRequestConfig) => {
    setLoading(true);
    const allAdmins = await requestMaker<StaffModelPagedList>(config);
    const facilityAdmins = allAdmins.items?.filter((admin) => admin.staffRole?.indexOf('FacilityAdmin') !== -1 && admin.isActive);
    setAdmins(facilityAdmins || []);
    setLoading(false);
  };

  useEffect(() => {
    (async () => {
      await fetchData(getAdmins());
    })();
  }, []);

  const handleAddFacilityAdmin = async (values: AddAdminWhoIsAlsoStaffRequest) => {
    const admin = await requestMaker<StaffModel>(addFacilityAdmin(values));
    setAdmins([...admins, admin]);
  };

  const onDelete = async (adminToDelete: StaffModel) => {
    setLoading(true);
    try {
      await requestMaker(deleteFacilityAdmin(adminToDelete.staffId));
      setAdmins(admins.filter((admin) => admin.staffId !== adminToDelete.staffId));
    } finally {
      setLoading(false);
    }
  };

  return (
    <AppAnimate animation='transition.slideUpIn' delay={200}>
      <Box>
        <Box mb={{xs: 4, sm: 4, xl: 6}}>
          <Grid container direction='row' justify='space-between' alignItems='center'>
            <Grid item xs={6}>
              <Box component='h2' color='text.primary' fontSize={16} fontWeight={Fonts.BOLD}>
                <IntlMessages id='facility.admins.heading' />
              </Box>
            </Grid>
            <Grid item xs={6} className={classes.buttonContainer}>
              <Button variant='contained' color='primary' startIcon={<AddIcon />} onClick={() => setShowAddAdminModal(true)}>
                Invite Facility Admin
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
            options={{
              actionsColumnIndex: -1,
              paging: false,
            }}
            editable={{
              onRowDelete: onDelete,
            }}
          />
        </Box>
        <AddAdminModal open={showAddAdminModal} onAddAdmin={handleAddFacilityAdmin} handleClose={() => setShowAddAdminModal(false)} />
      </Box>
    </AppAnimate>
  );
};

export default FacilityAdmin;
