import React from 'react';
import Box from '@material-ui/core/Box';
import AppAnimate from '@crema/core/AppAnimate';

const Reports = () => (
    <AppAnimate animation='transition.slideUpIn' delay={200}>
      <Box>
        <Box component='h4' mb={3} fontSize={20}>
          Reports
        </Box>
        <Box component='p' fontSize={16}>
          You can start from here..
        </Box>
      </Box>
    </AppAnimate>
  );

export default Reports;
