import React from 'react';
import {BrowserRouter} from 'react-router-dom';
import AppLayout from '@crema/core/AppLayout';
import LocaleProvider from '@crema/utility/LocaleProvider';
import CremaThemeProvider from '@crema/utility/CremaThemeProvider';
import CremaStyleProvider from '@crema/utility/CremaStyleProvider';
import ContextProvider from '@crema/utility/ContextProvider';
import {Auth0Provider, AppState} from '@auth0/auth0-react';

import CssBaseline from '@material-ui/core/CssBaseline';
import InfoViewContextProvider from '@crema/core/InfoView/InfoViewContext';
import history from './utils/history';

const onRedirectCallback = (appState: AppState) => {
  history.push(appState && appState.returnTo ? appState.returnTo : window.location.pathname);
};

const providerConfig = {
  domain: process.env.REACT_APP_AUTH0_DOMAIN || '',
  clientId: process.env.REACT_APP_AUTH0_CLIENT_ID || '',
  ...(process.env.REACT_APP_AUTH0_AUDIENCE ? {audience: process.env.REACT_APP_AUTH0_AUDIENCE} : null),
  redirectUri: window.location.origin,
  onRedirectCallback,
};

const App = () => (
  <Auth0Provider {...providerConfig}>
    <ContextProvider>
      <InfoViewContextProvider>
        <CremaThemeProvider>
          <CremaStyleProvider>
            <LocaleProvider>
              <BrowserRouter>
                <CssBaseline />
                <AppLayout />
              </BrowserRouter>
            </LocaleProvider>
          </CremaStyleProvider>
        </CremaThemeProvider>
      </InfoViewContextProvider>
    </ContextProvider>
  </Auth0Provider>
);

export default App;
