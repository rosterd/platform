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
    auth: ['Manager'],
  },
  {
    id: 'skills',
    title: 'Skills',
    messageId: 'sidebar.skills',
    type: 'item',
    icon: 'directionsRun',
    url: '/skills',
    auth: ['Manager'],
  },
  {
    id: 'facilitiesManagement',
    title: 'Facilities Management',
    messageId: 'sidebar.facilitiesManagement',
    type: 'collapse',
    icon: 'business',
    auth: ['Admin'],
    children: [
      {
        id: 'facilities',
        title: 'Facilities',
        messageId: 'sidebar.facilitiesManagement.facilities',
        type: 'item',
        url: '/facilities',
      },
      {
        id: 'admin',
        title: 'Admins',
        messageId: 'sidebar.facilitiesManagement.admin',
        type: 'item',
        url: '/admins',
      },
    ],
  },
  {
    id: 'organizations',
    title: 'Organizations Management',
    messageId: 'sidebar.organizationManagement',
    type: 'collapse',
    icon: 'business',
    auth: ['RosterdAdmin'],
    children: [
      {
        id: 'organisations',
        title: 'Organisations',
        messageId: 'sidebar.organizationManagement.organizations',
        type: 'item',
        url: '/organizations',
      },
      {
        id: 'admin',
        title: 'Admins',
        messageId: 'sidebar.facilitiesManagement.admin',
        type: 'item',
        url: '/organization-admins',
      },
    ],
  },
  {
    id: 'jobs',
    title: 'Jobs',
    messageId: 'sidebar.jobs',
    type: 'item',
    icon: 'work',
    url: '/jobs',
    auth: ['Manager'],
  },
  {
    id: 'reports',
    title: 'Reports',
    messageId: 'sidebar.reports',
    type: 'item',
    icon: 'assessment',
    url: '/reports',
    auth: ['Manager'],
  },
];
export default routesConfig;
