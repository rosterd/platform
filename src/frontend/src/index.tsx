import 'react-app-polyfill/ie11';
import React from 'react';
import ReactDOM from 'react-dom';
import 'react-perfect-scrollbar/dist/css/styles.css';
import 'slick-carousel/slick/slick.css';
import 'slick-carousel/slick/slick-theme.css';
import {Auth0Provider, AppState} from '@auth0/auth0-react';

import './shared/styles/index.css';
import App from './App';
import reportWebVitals from './reportWebVitals';
import getConfig from './config';
import history from './utils/history';

const onRedirectCallback = (appState: AppState) => {
  history.push(
    appState && appState.returnTo
      ? appState.returnTo
      : window.location.pathname,
  );
};

const config = getConfig();

const providerConfig = {
  domain: config.domain,
  clientId: config.clientId,
  ...(config.audience ? {audience: config.audience} : null),
  redirectUri: window.location.origin,
  onRedirectCallback,
};

ReactDOM.render(
  <Auth0Provider {...providerConfig}>
    <App />
  </Auth0Provider>,
  document.getElementById('root'),
);

// If you want to start measuring performance in your app, pass a function
// to log results (for example: reportWebVitals(console.log))
// or send to an analytics endpoint. Learn more: https://bit.ly/CRA-vitals
reportWebVitals();
