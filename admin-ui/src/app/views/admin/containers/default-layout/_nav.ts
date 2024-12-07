import { INavData } from '@coreui/angular';

export const navItems: INavData[] = [
  {
    name: 'Trang chủ',
    url: '/dashboard',
    iconComponent: { name: 'cilChart' },
    badge: {
      color: 'info',
      text: 'NEW',
    },
    attributes: {
      policyName: 'Permissions.Dashboard.View',
    },
  },
  {
    name: 'Quản Lí Dự Án',
    url: '/project',
    iconComponent: { name: 'cilLayers' },
    children: [
      {
        iconComponent: { name: 'cilArrowRight' },
        name: 'Dự Án',
        url: '/project',
        attributes: {
          policyName: 'Permissions.Projects.View',
        },
      },
      {
        iconComponent: { name: 'cilArrowRight' },
        name: 'Nhóm Dự Án',
        url: '/project/groups',
        attributes: {
          policyName: 'Permissions.Projects.View',
        },
      },
      {
        iconComponent: { name: 'cilArrowRight' },
        name: 'Báo Cáo Tiến Độ',
        url: '/project/report',
        attributes: {
          policyName: 'Permissions.Projects.View',
        },
      },
    ],
  },
  {
    name: 'Quản Lí Văn Phòng',
    url: '/content',
    iconComponent: { name: 'cilFolderOpen' },
    children: [
      {
        iconComponent: { name: 'cilArrowRight' },
        name: 'Loạt Bài',
        url: '/content/series',
        attributes: {
          policyName: 'Permissions.Series.View',
        },
      },
      {
        iconComponent: { name: 'cilArrowRight' },
        name: 'Danh mục',
        url: '/content/post-categories',
        attributes: {
          policyName: 'Permissions.PostCategories.View',
        },
      },
      {
        iconComponent: { name: 'cilArrowRight' },
        name: 'Bài viết',
        url: '/content/posts',
        attributes: {
          policyName: 'Permissions.Posts.View',
        },
      },
    ],
  },
  {
    name: 'Quản Lí Kho',
    url: '/inventory',
    iconComponent: { name: 'cilMenu' },
    children: [
      {
        iconComponent: { name: 'cilArrowRight' },
        name: 'Tồn Kho',
        url: '/inventory/stocks/:slug',
        attributes: {
          policyName: 'Permissions.Inventories.View',
        },
      },
      {
        iconComponent: { name: 'cilArrowRight' },
        name: 'Sản Phẩm',
        url: '/inventory/products',
        attributes: {
          policyName: 'Permissions.Products.View',
        },
      },
      {
        iconComponent: { name: 'cilArrowRight' },
        name: 'Danh Mục Sản Phẩm',
        url: '/inventory/product-categories',
        attributes: {
          policyName: 'Permissions.ProductCategories.View',
        },
      },
    ],
  },
  {
    name: 'Nhuận bút',
    url: '/royalty',
    iconComponent: { name: 'cilDollar' },
    children: [
      {
        iconComponent: { name: 'cilArrowRight' },
        name: 'Thống kê tháng',
        url: '/royalty/royalty-month',
        attributes: {
          policyName: 'Permissions.Royalty.View',
        },
      },
      {
        iconComponent: { name: 'cilArrowRight' },
        name: 'Thống kê tác giả',
        url: '/royalty/royalty-user',
        attributes: {
          policyName: 'Permissions.Royalty.View',
        },
      },
      {
        iconComponent: { name: 'cilArrowRight' },
        name: 'Giao dịch',
        url: '/royalty/transactions',
        attributes: {
          policyName: 'Permissions.Royalty.View',
        },
      },
    ],
  },
  {
    name: 'Hệ thống',
    url: '/system',
    iconComponent: { name: 'cilSettings' },
    children: [
      {
        iconComponent: { name: 'cilArrowRight' },
        name: 'Quyền',
        url: '/system/roles',
        attributes: {
          policyName: 'Permissions.Roles.View',
        },
      },
      {
        iconComponent: { name: 'cilArrowRight' },
        name: 'Người dùng',
        url: '/system/users',
        attributes: {
          policyName: 'Permissions.Users.View',
        },
      },
      {
        iconComponent: { name: 'cilArrowRight' },
        name: 'Thông Báo',
        url: '/system/announcements',
        attributes: {
          policyName: 'Permissions.Announcements.View',
        },
      },
    ],
  },
  {
    name: 'Quản Lí Task',
    url: '/task',
    iconComponent: { name: 'cilDescription' },
    children: [
      {
        iconComponent: { name: 'cilArrowRight' },
        name: 'Dashboard',
        url: '/task/task-dashboard',
        attributes: {
          policyName: 'Permissions.Roles.View',
        },
      },
      {
        iconComponent: { name: 'cilArrowRight' },
        name: 'Nhiệm Vụ',
        url: '/task/management',
        attributes: {
          policyName: 'Permissions.Roles.View',
        },
      },
    ],
  },
];
