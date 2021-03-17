export interface NavItemProps {
  id: string;
  messageId: string;
  title: string;
  icon?: string;
  exact?: boolean;
  url?: string;
  type?: string;
  count?: number;
  color?: string;
  auth?: string[];
  children?: NavItemProps[] | NavItemProps;
}

const routesConfig: NavItemProps[] = [
  {
    id: 'dashboard',
    title: 'Dashboard',
    messageId: 'sidebar.dashboard',
    type: 'item',
    icon: 'dashboard',
    url: '/dashboard',
  },
  {
    id: 'resources',
    title: 'Resources',
    messageId: 'sidebar.resources',
    type: 'item',
    icon: 'group',
    url: '/resources',
  },
  {
    id: 'skills',
    title: 'Skills',
    messageId: 'sidebar.skills',
    type: 'item',
    icon: 'directionsRun',
    url: '/skills',
  },
  {
    id: 'facilities',
    title: 'Facilities',
    messageId: 'sidebar.facilities',
    type: 'item',
    icon: 'business',
    url: '/facilities',
  },
  {
    id: 'jobs',
    title: 'Jobs',
    messageId: 'sidebar.jobs',
    type: 'item',
    icon: 'work',
    url: '/jobs',
  },
  {
    id: 'reports',
    title: 'Reports',
    messageId: 'sidebar.reports',
    type: 'item',
    icon: 'assessment',
    url: '/reports',
  },
];
export default routesConfig;
