import React, { useContext, useReducer } from "react";
import { contextReducer, LayoutSetting } from "./ContextReducer";

export interface Layout {
  navCollapsed: boolean;
}

export const ContextState:Layout = {
  navCollapsed: true,
};
export interface NavCollapseAction {
  type: typeof LayoutSetting.TOGGLE_NAV_COLLAPSED;
}

const LayoutContext= React.createContext<Layout>(ContextState);
const LayoutActionsContext= React.createContext<React.Dispatch<NavCollapseAction>|undefined> (undefined);

export const useLayoutContext = () => useContext(LayoutContext);
export const useLayoutActionsContext = () =>  useContext(LayoutActionsContext);

export const toggleNavCollapsed = ():NavCollapseAction => ({type: LayoutSetting.TOGGLE_NAV_COLLAPSED})

const LayoutContextProvider: React.FC<React.ReactNode> = ({children}) => {
  const [state, dispatch] = useReducer(
    contextReducer,
    ContextState,
    () => ContextState,
  );

  return (
    <LayoutContext.Provider value={state}>
      <LayoutActionsContext.Provider
        value={dispatch}>
        {children}
      </LayoutActionsContext.Provider>
    </LayoutContext.Provider>
  );
};

export default LayoutContextProvider;
