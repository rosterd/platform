import React, {useEffect, useState} from 'react';
import axios from 'axios';
import {BrowserRouter} from 'react-router-dom';
import AppLayout from '@crema/core/AppLayout';
import LocaleProvider from '@crema/utility/LocaleProvider';
import CremaThemeProvider from '@crema/utility/CremaThemeProvider';
import CremaStyleProvider from '@crema/utility/CremaStyleProvider';
import ContextProvider from '@crema/utility/ContextProvider';
import {Auth0Provider, AppState, Auth0ProviderOptions} from '@auth0/auth0-react';
import {components} from 'types/models';
import CssBaseline from '@material-ui/core/CssBaseline';
import InfoViewContextProvider from '@crema/core/InfoView/InfoViewContext';
import {getOrganization} from 'services';
import history from './utils/history';

type Organization = components['schemas']['OrganizationModel'];

const onRedirectCallback = (appState: AppState) => {
  history.push(appState && appState.returnTo ? appState.returnTo : window.location.pathname);
};

const providerConfig: Auth0ProviderOptions = {
  domain: process.env.REACT_APP_AUTH0_DOMAIN || '',
  clientId: process.env.REACT_APP_AUTH0_CLIENT_ID || '',
  ...(process.env.REACT_APP_AUTH0_AUDIENCE ? {audience: process.env.REACT_APP_AUTH0_AUDIENCE} : null),
  redirectUri: window.location.origin,
  organization: undefined,
  onRedirectCallback,
};

const App = (): JSX.Element => {
  const [config, setConfig] = useState(providerConfig);
  const [fetchOrganizationCompleted, setFetchOrganizationCompleted] = useState(false);

  useEffect(() => {
    (async () => {
      try {
        const hostName = window.location.host;
        const organizationName = hostName ? hostName.split('.')[0] : null;
        const requestConifg = getOrganization({organizationName, organizationId: null});
        const response = await axios.request<Organization>(requestConifg);
        const auth0OrganizationId = response.data?.auth0OrganizationId;
        if (auth0OrganizationId) {
          setConfig({...config, organization: auth0OrganizationId});
        }
      } catch (e) {
        console.error(e);
      }
      setFetchOrganizationCompleted(true);
    })();
  }, [setConfig]);

  if (!fetchOrganizationCompleted) {
    return <div>Loading ....</div>;
  }
  return (
    <Auth0Provider {...config}>
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
};

export default App;
