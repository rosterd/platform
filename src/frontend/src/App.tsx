import React from 'react';
import {BrowserRouter} from 'react-router-dom';
import AppLayout from '@crema/core/AppLayout';
import LocaleProvider from '@crema/utility/LocaleProvider';
import CremaThemeProvider from '@crema/utility/CremaThemeProvider';
import CremaStyleProvider from '@crema/utility/CremaStyleProvider';
import ContextProvider from '@crema/utility/ContextProvider';

import CssBaseline from '@material-ui/core/CssBaseline';
import InfoViewContextProvider from '@crema/core/InfoView/InfoViewContext';

const App = () => (
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
);

export default App;
