import React, { useContext, useReducer } from "react";
import { contextReducer, InFoViewActions } from "./InfoViewReducer";
export interface InfoViewTypes {
  error: string;
  loading: boolean;
  message: string;
}

export const InfoViewState:InfoViewTypes = {
  loading: false,
  error: '',
  message: '',
};
export interface FetchStartAction {
  type: typeof InFoViewActions.FETCH_STARTS;
}

export interface FetchSuccessAction {
  type: typeof InFoViewActions.FETCH_SUCCESS;
}

export interface FetchErrorAction {
  type: typeof InFoViewActions.SET_ERROR;
  payload: string;
}

export interface ShowMessageAction {
  type: typeof InFoViewActions.SET_MESSAGE;
  payload: string;
}

export type InfoViewActionTypes =
  | FetchErrorAction
  | ShowMessageAction
  | FetchStartAction
  | FetchSuccessAction

export const fetchStart = ():InfoViewActionTypes => ({type: InFoViewActions.FETCH_STARTS})
export const fetchSuccess = ():InfoViewActionTypes =>     ({type: InFoViewActions.FETCH_SUCCESS});
export const fetchError = (error:string):InfoViewActionTypes =>   ({type: InFoViewActions.SET_ERROR, payload:error})
export const showMessage = (message:string):InfoViewActionTypes =>   ({type: InFoViewActions.SET_MESSAGE, payload:message})  ;

const InfoViewContext= React.createContext<InfoViewTypes>(InfoViewState);
const InfoViewActionsContext= React.createContext<React.Dispatch<InfoViewActionTypes>|undefined> (undefined);

export const useInfoViewContext = () => useContext(InfoViewContext);
export const useInfoViewActionsContext = () =>
  useContext(InfoViewActionsContext);


const InfoViewContextProvider : React.FC<React.ReactNode> = ({children}) => {
  const [state,dispatch] = useReducer(
    contextReducer,
    InfoViewState,
    () => InfoViewState,
  );

  return (
    <InfoViewContext.Provider value={state}>
      <InfoViewActionsContext.Provider
        value={dispatch}>
        {children}
      </InfoViewActionsContext.Provider>
    </InfoViewContext.Provider>
  );
};

export default InfoViewContextProvider;
