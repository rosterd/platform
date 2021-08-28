import React, {useState, useEffect} from 'react';
import MaterialTable from 'material-table';
import Box from '@material-ui/core/Box';
import useRequest from 'shared/hooks/useRequest';
import AppAnimate from '@crema/core/AppAnimate';
import IntlMessages from '@crema/utility/IntlMessages';
import {Fonts} from 'shared/constants/AppEnums';
import {Button, Grid, makeStyles} from '@material-ui/core';
import AddIcon from '@material-ui/icons/Add';
import {addFacilityAdmin, getFacilityAdmins} from 'services';
import {components} from 'types/models';
import AddAdminModal from 'shared/components/AddAdminModal';
import {AxiosRequestConfig} from 'axios';

type AddAdminUserRequest = components['schemas']['AddAdminUserRequest'];
type AdminUserModel = components['schemas']['Auth0UserModel'];
type GetAdminsResponse = components['schemas']['Auth0UserModelPagedList'];

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

const FacilityAdmin: React.FC = (): JSX.Element => {
  const classes = useStyles();
  const [admins, setAdmins] = useState(initialState);
  const [results, setResults] = useState({} as GetAdminsResponse);
  const [showAddAdminModal, setShowAddAdminModal] = useState(false);
  const [loading, setLoading] = useState(false);
  const {requestMaker} = useRequest();

  const fetchData = async (config: AxiosRequestConfig) => {
    setLoading(true);
    const adminsRes = await requestMaker<GetAdminsResponse>(config);
    setLoading(false);
    setResults(adminsRes);
    setAdmins(adminsRes.items || []);
  };

  useEffect(() => {
    (async () => {
      await fetchData(getFacilityAdmins());
    })();
  }, []);

  const handleAddFacilityAdmin = async (values: AddAdminUserRequest) => {
    const admin = await requestMaker<AdminUserModel>(addFacilityAdmin(values));
    setAdmins([...admins, admin]);
  };

  const handlePageChange = async (page: number, pageSize: number) => {
    const requestConfig = getFacilityAdmins();
    await fetchData({...requestConfig, url: `${requestConfig.url}?pageNumber=${page + 1}&pageSize=${pageSize}`});
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
              {title: 'Email', field: 'address'},
              {title: 'Phone', field: 'phone'},
            ]}
            data={admins}
            isLoading={loading}
            options={{
              actionsColumnIndex: -1,
            }}
            onChangePage={handlePageChange}
            page={(results?.currentPage || 1) - 1}
            totalCount={results.totalCount}
          />
        </Box>
        <AddAdminModal open={showAddAdminModal} onAddAdmin={handleAddFacilityAdmin} handleClose={() => setShowAddAdminModal(false)} />
      </Box>
    </AppAnimate>
  );
};

export default FacilityAdmin;
