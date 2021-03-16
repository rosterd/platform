import React, { useContext } from "react";
import PerfectScrollbar from "react-perfect-scrollbar";
import Drawer from "@material-ui/core/Drawer";
import Hidden from "@material-ui/core/Hidden";
import UserInfo from "../../../../shared/components/UserInfo";
import Navigation from "../../Navigation/VerticleNav";
import Box from "@material-ui/core/Box";
import useStyles from "./AppSidebar.style";
import AppContextPropsType from "../../../../types/AppContextPropsType";
import AppContext from "../../../utility/AppContext";
import { toggleNavCollapsed, useLayoutActionsContext, useLayoutContext } from "../LayoutContextProvider";

interface AppSidebarProps {
  position?: 'left' | 'bottom' | 'right' | 'top';
}

const AppSidebar: React.FC<AppSidebarProps> = ({position = 'left'}) => {

  const {navCollapsed} = useLayoutContext();
  const dispatch = useLayoutActionsContext()!;


  const {themeMode} = useContext<AppContextPropsType>(AppContext);

  const handleToggleDrawer = () => {
    dispatch(toggleNavCollapsed());
  };

  const classes = useStyles({themeMode});
  return (
    <Hidden lgUp>
      <Drawer
        anchor={position}
        open={navCollapsed}
        onClose={() => handleToggleDrawer()}
        style={{position: 'absolute'}}>
        <Box height='100%' className={classes.drawerContainer}>
          <Box
            height='100%'
            width='100%'
            color='primary.contrastText'
            className={classes.sidebarBg}>
            <UserInfo />
            <PerfectScrollbar className={classes.drawerScrollAppSidebar}>
              <Navigation />
            </PerfectScrollbar>
          </Box>
        </Box>
      </Drawer>
    </Hidden>
  );
};

export default AppSidebar;
