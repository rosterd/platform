import React, {LazyExoticComponent} from 'react';
import {Redirect} from 'react-router-dom';

import {createRoutes} from '../@crema/utility/Utils';
import errorPagesConfigs from './errorPages';
import {authRouteConfig} from './auth';
import {initialUrl} from '../shared/constants/AppConst';

interface PageRouteConfig {
  path: string;
  component: LazyExoticComponent<any>;
}

const buildRouteConfigs = (routes: PageRouteConfig[]) =>
  routes.map((route) => {
    const {path, component} = route;
    return {
      auth: ['user'],
      routes: [{path, component}],
    };
  });

const pageRouteConfigs: PageRouteConfig[] = [
  {
    path: '/dashboard',
    component: React.lazy(() => import('./dashboard')),
  },
  {
    path: '/jobs',
    component: React.lazy(() => import('./jobs')),
  },
  {
    path: '/reports',
    component: React.lazy(() => import('./reports')),
  },
  {
    path: '/facilities',
    component: React.lazy(() => import('./facilities')),
  },
  {
    path: '/admins',
    component: React.lazy(() => import('./admins')),
  },
  {
    path: '/organizations',
    component: React.lazy(() => import('./organizations')),
  },
  {
    path: '/skills',
    component: React.lazy(() => import('./skills')),
  },
  {
    path: '/resources',
    component: React.lazy(() => import('./resources')),
  },
];

const pageRoutes = buildRouteConfigs(pageRouteConfigs);

const routeConfigs = [...pageRoutes, ...errorPagesConfigs, ...authRouteConfig];

const routes = [
  ...createRoutes(routeConfigs),
  {
    path: '/',
    exact: true,
    component: (): JSX.Element => <Redirect to={initialUrl} />,
  },
  {
    component: (): JSX.Element => <Redirect to='/error-pages/error-404' />,
  },
];

export default routes;
