import { Layout, NavCollapseAction } from "./index";

export const LayoutSetting = {
  TOGGLE_NAV_COLLAPSED: 'TOGGLE_NAV_COLLAPSED',
};

export function contextReducer(state:Layout, action:NavCollapseAction) {
  switch (action.type) {
    case LayoutSetting.TOGGLE_NAV_COLLAPSED: {
      return {
        ...state,
        navCollapsed: !state.navCollapsed,
      };
    }
    default:
      return state;
  }
}
