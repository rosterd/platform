import React from 'react';
import Divider from '@material-ui/core/Divider';
import List from '@material-ui/core/List';
import Box from '@material-ui/core/Box';
import HorizontalGroup from './HorizontalGroup';
import HorizontalCollapse from './HorizontalCollapse';
import HorizontalItem from './HorizontalItem';

import routesConfig from '../../../../modules/routesConfig';

interface HorizontalNavProps {}

const HorizontalNav: React.FC<HorizontalNavProps> = () => (
    <List className='navbarNav'>
      {routesConfig.map((item: any) => (
        <React.Fragment key={item.id}>
          {item.type === 'group' && (
            <HorizontalGroup item={item} nestedLevel={0} />
          )}

          {item.type === 'collapse' && (
            <HorizontalCollapse item={item} nestedLevel={0} />
          )}

          {item.type === 'item' && <HorizontalItem item={item} />}

          {item.type === 'divider' && (
            <Box my={5} clone>
              <Divider />
            </Box>
          )}
        </React.Fragment>
      ))}
    </List>
  );

export default HorizontalNav;
