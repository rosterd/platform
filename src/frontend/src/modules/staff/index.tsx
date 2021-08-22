import React, {useEffect, useState} from 'react';
import MaterialTable from 'material-table';
import Box from '@material-ui/core/Box';
import AppAnimate from '@crema/core/AppAnimate';
import IntlMessages from '@crema/utility/IntlMessages';
import {Fonts} from 'shared/constants/AppEnums';
import {makeStyles} from '@material-ui/core';
import {components} from 'types/models';
import {getStaff, deleteStaff, addStaff, updateStaff} from 'services';
import useRequest from 'shared/hooks/useRequest';
import {AxiosRequestConfig} from 'axios';

type GetStaffResponse = components['schemas']['StaffModelPagedList'];
type Staff = components['schemas']['StaffModel'];

const useStyles = makeStyles(() => ({
  materialTable: {
    '& .MuiTableCell-paddingCheckbox': {
      paddingLeft: 16,
    },
  },
}));

const initialState: Staff[] = [];

const Staff: React.FC = (): JSX.Element => {
  const classes = useStyles();
  const [results, setResults] = useState({} as GetStaffResponse);
  const [staff, setStaff] = useState(initialState);
  const [loading, setLoading] = useState(false);
  const {requestMaker} = useRequest();

  const fetchData = async (config: AxiosRequestConfig) => {
    setLoading(true);
    const staffResponse = await requestMaker<GetStaffResponse>(config);
    setLoading(false);
    setResults(staffResponse);
    setStaff(staffResponse.items || []);
  };

  useEffect(() => {
    (async () => {
      await fetchData(getStaff());
    })();
  }, []);

  const onAdd = async (staffToAdd: Staff) => {
    setLoading(true);
    const addedStaff = await requestMaker<Staff>(addStaff(staffToAdd));
    setLoading(false);
    setStaff([addedStaff, ...staff]);
  };

  const onUpdate = async (staffToUpdate: Staff) => {
    setLoading(true);
    const updatedSkill = await requestMaker<Staff>(updateStaff(staffToUpdate));
    setLoading(false);
  };

  const onDelete = async (deletedStaff: Staff) => {
    if (!deletedStaff?.staffId) return;
    setLoading(true);
    await requestMaker<GetStaffResponse>(deleteStaff(deletedStaff?.staffId));
    setLoading(false);
    setStaff(staff.filter((staffMember) => staffMember.staffId !== deletedStaff.staffId));
  };

  const handlePageChange = async (page: number, pageSize: number) => {
    const requestConfig = getStaff();
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
              {title: 'First Name', field: 'firstName'},
              {title: 'Last Name', field: 'lastName'},
              {title: 'Email', field: 'email'},
              {title: 'Mobile', field: 'mobilePhoneNumber'},
              {title: 'Job Title', field: 'jobTitle'},
            ]}
            data={staff}
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

export default Staff;
