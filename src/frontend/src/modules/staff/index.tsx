import React, {useEffect, useState} from 'react';
import MaterialTable from 'material-table';
import Box from '@material-ui/core/Box';
import AppAnimate from '@crema/core/AppAnimate';
import IntlMessages from '@crema/utility/IntlMessages';
import {Fonts} from 'shared/constants/AppEnums';
import {Button, Grid, makeStyles} from '@material-ui/core';
import {components} from 'types/models';
import {getStaff, deleteStaff, addStaff, updateStaff, getSkills} from 'services';
import useRequest from 'shared/hooks/useRequest';
import {AxiosRequestConfig} from 'axios';
import AddIcon from '@material-ui/icons/Add';
import AddStaffModal from './components/AddStaffModal';
import UpdateStaffModal from './components/UpdateStaffModal';

type GetStaffResponse = components['schemas']['StaffModelPagedList'];
type GetSkillsResponse = components['schemas']['SkillModelPagedList'];
type Staff = components['schemas']['StaffModel'];
type Skill = components['schemas']['SkillModel'];

const useStyles = makeStyles(() => ({
  materialTable: {
    '& .MuiTableCell-paddingCheckbox': {
      paddingLeft: 16,
    },
  },
}));

const initialState: Staff[] = [];
const initialSkillsState: Skill[] = [];

const Staff: React.FC = (): JSX.Element => {
  const classes = useStyles();
  const [results, setResults] = useState({} as GetStaffResponse);
  const [staff, setStaff] = useState(initialState);
  const [skills, setSkills] = useState(initialSkillsState);
  const [staffMember, setStaffMember] = useState<Staff | undefined>(undefined);
  const [loading, setLoading] = useState(false);
  const {requestMaker} = useRequest();
  const [showStaffModal, setShowStaffModal] = useState(false);

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

  useEffect(() => {
    (async () => {
      const {url} = getSkills();
      const skillsResponse = await requestMaker<GetSkillsResponse>({url: `${url}?pageSize=100`});
      setSkills(skillsResponse.items || []);
    })();
  }, []);

  const handleClose = () => {
    setLoading(false);
    setStaffMember(undefined);
    setShowStaffModal(false);
  };

  const handleAddStaff = async (values: Staff) => {
    setLoading(true);
    const addedStaff = await requestMaker<Staff>(addStaff(values));
    setLoading(false);
    if (addedStaff) {
      setStaff([addedStaff, ...staff]);
    }
  };

  const onAddButtonClick = () => {
    setStaffMember(undefined);
    setShowStaffModal(true);
  };

  const onRowClick = async (_: any, staffToUpdate?: Staff) => {
    setShowStaffModal(true);
    setStaffMember(staffToUpdate);
  };

  const handleUpdateStaff = async (values: Staff) => {
    setLoading(true);
    const updatedStaff = await requestMaker<Staff>(updateStaff(values));
    setStaff(staff.map((member) => (member.staffId === updatedStaff.staffId ? updatedStaff : member)));
    setStaffMember(undefined);
    setShowStaffModal(false);
    setLoading(false);
  };

  const onDelete = async (deletedStaff: Staff) => {
    if (!deletedStaff?.staffId) return;
    setLoading(true);
    await requestMaker<GetStaffResponse>(deleteStaff(deletedStaff?.staffId));
    setLoading(false);
    setStaff(staff.filter((member) => member.staffId !== deletedStaff.staffId));
  };

  const handlePageChange = async (page: number, pageSize: number) => {
    const requestConfig = getStaff();
    await fetchData({...requestConfig, url: `${requestConfig.url}?pageNumber=${page + 1}&pageSize=${pageSize}`});
  };

  return (
    <AppAnimate animation='transition.slideUpIn' delay={200}>
      <Box>
        <Box mb={{xs: 4, sm: 4, xl: 6}}>
          <Grid container direction='row' justify='space-between' alignItems='center'>
            <Grid item xs={6}>
              <Box component='h2' color='text.primary' fontSize={16} fontWeight={Fonts.BOLD}>
                <IntlMessages id='staff.heading' />
              </Box>
            </Grid>
            <Grid item xs={6}>
              <Box textAlign='right'>
                <Button variant='contained' color='primary' startIcon={<AddIcon />} onClick={onAddButtonClick}>
                  Add Staff
                </Button>
              </Box>
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
              {title: 'Mobile', field: 'mobilePhoneNumber'},
              {title: 'Job Title', field: 'jobTitle'},
            ]}
            data={staff}
            isLoading={loading}
            onRowClick={onRowClick}
            editable={{
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
        {staffMember ? (
          <UpdateStaffModal skills={skills} staffMember={staffMember} open={showStaffModal} onUpdateStaff={handleUpdateStaff} handleClose={handleClose} />
        ) : (
          <AddStaffModal skills={skills} open={showStaffModal} onAddStaff={handleAddStaff} handleClose={handleClose} />
        )}
      </Box>
    </AppAnimate>
  );
};

export default Staff;
