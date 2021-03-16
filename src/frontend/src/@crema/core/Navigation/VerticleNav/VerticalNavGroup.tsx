import React, { useContext, useMemo } from "react";
import { ListItem } from "@material-ui/core";
import clsx from "clsx";
import VerticalCollapse from "./VerticalCollapse";
import VerticalItem from "./VerticalItem";
import IntlMessages from "../../../utility/IntlMessages";
import useStyles from "./VerticalNavGroup.style";
import AppContext from "../../../utility/AppContext";
import { checkPermission } from "../../../utility/Utils";
import { NavItemProps } from "../../../../modules/routesConfig";
import AppContextPropsType from "../../../../types/AppContextPropsType";

interface VerticalNavGroupProps {
  item: NavItemProps;
  level: number;
}

const VerticalNavGroup: React.FC<VerticalNavGroupProps> = ({item, level}) => {
  const {themeMode} = useContext(AppContext);
  const classes = useStyles({level, themeMode});
  const {user} = useContext<AppContextPropsType>(AppContext);
  const hasPermission = useMemo(() => checkPermission(item.auth, user!.role), [
    item.auth,
    user,
  ]);
  if (!hasPermission) {
    return null;
  }
  return (
    <>
      <ListItem
        component='li'
        className={clsx(classes.navItem, 'nav-item nav-item-header')}>
        {<IntlMessages id={item.messageId} />}
      </ListItem>

      {item.children && Array.isArray(item.children) && (
        <>
          {item.children.map((item: any) => (
            <React.Fragment key={item.id}>
              {item.type === 'group' && (
                <NavVerticalGroup item={item} level={level} />
              )}

              {item.type === 'collapse' && (
                <VerticalCollapse item={item} level={level} />
              )}

              {item.type === 'item' && (
                <VerticalItem item={item} level={level} />
              )}
            </React.Fragment>
          ))}
        </>
      )}
    </>
  );
};

const NavVerticalGroup = VerticalNavGroup;

export default NavVerticalGroup;
