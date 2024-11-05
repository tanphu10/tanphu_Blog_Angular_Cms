import { INavData } from '@coreui/angular';

export const navItems: INavData[] = [
  {
    name: 'Trang chủ',
    url: '/dashboard',
    iconComponent: { name: 'cil-speedometer' },
    badge: {
      color: 'info',
      text: 'NEW',
    },
    attributes: {
      policyName: 'Permissions.Dashboard.View',
    },
  },
  {
    name: 'Quản Lí Văn Phòng',
    url: '/content',
    iconComponent: { name: 'cil-pencil' },
    children: [
      {
        name: 'Dự Án',
        url: '/content/series',
        attributes: {
          policyName: 'Permissions.Series.View',
        },
      },
      {
        name: 'Danh mục',
        url: '/content/post-categories',
        attributes: {
          policyName: 'Permissions.PostCategories.View',
        },
      },
      {
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
        name: 'Tồn Kho',
        url: '/inventory/inventories',
        attributes: {
          policyName: 'Permissions.Inventories.View',
        },
      },
      {
        name: 'Sản Phẩm',
        url: '/inventory/products',
        attributes: {
          policyName: 'Permissions.Products.View',
        },
      },
      {
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
        name: 'Thống kê tháng',
        url: '/royalty/royalty-month',
        attributes: {
          policyName: 'Permissions.Royalty.View',
        },
      },
      {
        name: 'Thống kê tác giả',
        url: '/royalty/royalty-user',
        attributes: {
          policyName: 'Permissions.Royalty.View',
        },
      },
      {
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
    iconComponent: { name: 'cil-notes' },
    children: [
      {
        name: 'Quyền',
        url: '/system/roles',
        attributes: {
          policyName: 'Permissions.Roles.View',
        },
      },
      {
        name: 'Người dùng',
        url: '/system/users',
        attributes: {
          policyName: 'Permissions.Users.View',
        },
      },
    ],
  },

];
