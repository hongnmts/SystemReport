import store from '@/state/store'

export default [
    {
        path: '/',
        meta: {
            // authRequired: true
        },
        name: 'home',
        component: () => import('../pages/trangchu/index'),
    },
    {
        path: '/bang-dieu-khien',
        meta: {
            authRequired: true
        },
        name: 'dashboard',
        component: () => import('../pages/dashboard/index'),
    },
    {
        path: '/thong-tin-xuat-ban',
        meta: {
            authRequired: true
        },
        name: 'xuatban',
        component: () => import('../pages/baocao/xuatBan'),
    },
    {
        path: '/thong-ke',
        meta: {
            authRequired: true
        },
        name: 'thongke',
        component: () => import('../pages/thongKe'),
    },
    {
        path: '/bang-bieu-thong-ke/:id?',
        meta: {
            authRequired: true
        },
        name: 'bangBieuThongKe',
        component: () => import('../pages/thongKe/detailThongKe'),
    },
    {
        path: '/dang-nhap',
        name: 'login',
        component: () => import('../pages/auth/auth'),
        meta: {
            beforeResolve(routeTo, routeFrom, next) {
                // If the user is already logged in
                const loggeduser = localStorage.getItem('user-token');
                if (loggeduser) {
                    // Redirect to the home page instead
                    window.location.href = '/bang-dieu-khien'
                } else {
                    // Continue to the login page
                    next()
                }
            },
        },
    },
    {
        path: '/pages/404',
        name: 'Page-404',
        component: () => import('./views/pages/error-404'),
        meta: {
            authRequired: true,
        }
    },
    {
        path: '/pages/500',
        name: 'Page-500',
        component: () => import('./views/pages/error-500'),
        meta: {
            authRequired: true,
        }
    },
    {
        path: '/linh-vuc',
        name: 'LinhVuc',
        component: () => import('../pages/linhvuc'),
        meta: {
            authRequired: true,
        }
    },
    {
        path: "/nhom-quyen",
        name: "NhomQuyen",
        meta: {
            authRequired: true,
        },
        component: () => import("../pages/module"),
    },
    {
        path: "/nhom-quyen/action/:id?",
        name: "Hành động",
        // meta: {},
        component: () => import("../pages/module/action"),
    },
    {
        path: "/menu",
        name: "Menu",
         meta: {          authRequired: true, },
        component: () => import("../pages/menu"),
    },
    {
        path: "/vai-tro",
        name: "Quyền",
        meta: {can: 'viewpage-roles'},
        component: () => import("../pages/role"),
    },
    {
        path: "/vai-tro/thiet-lap-quyen/:id?",
        name: "Vai trò",
        meta: {can: 'viewpage-roles' },
        component: () => import("../pages/role/addPermissions01"),
    },
    {
        path: "/add-permissions",
        name: "Test ",
        component: () => import("../pages/role/addPermissions"),
    },
    {
        path: "/tai-khoan",
        name: "Tài khoản",
        meta: {
            can: 'viewpage-user',
        },
        component: () => import("../pages/user"),
    },
    {
        path: "/tai-khoan/doi-mat-khau",
        name: "Đổi mật khẩu",
        meta: {can: 'viewpage-user', },
        component: () => import("../pages/user/ChangePass"),
    },
    {
        path: "/don-vi",
        name: "Đơn vị",
        meta: { },
        component: () => import("../pages/donVi"),
    },
    {
        path: "/loggings",
        name: "Loggings",
        meta: {},
        component: () => import("../pages/loggings"),
    },
    {
        path: "/thong-tin-ca-nhan",
        name: "Thông tin cá nhân",
        // meta: {},
        component: () => import("../pages/auth/profile"),
    },
    {
        path: "/chuc-vu",
        name: "Chức vụ",
        // meta: {},
        component: () => import("../pages/chucVu"),
    },
    {
        path: "/trang-thai",
        name: "Trạng thái",
        // meta: {},
        component: () => import("../pages/trangThai"),
    },
    {
        path: "/co-quan",
        name: "Cơ quan",
        // meta: {},
        component: () => import("../pages/coQuan"),
    },
    {
        path: "/thong-bao",
        name: " Thông báo",
        // meta: {},
        component: () => import("../pages/thongBao"),
    },
    {
        path: "/loai-trang-thai",
        name: " Loại trạng thái",
        // meta: {},
        component: () => import("../pages/loaiTrangThai"),
    },
    {
        path: "/trang-chu",
        name: "trang-chu",
        component: () => import("../pages/trangchu"),
    },
    {
        path: "/cong-bo",
        name: "cong-bo",
        component: () => import("../pages/congbo"),
    },
    {
        path: "/ky-bao-cao",
        name: "ky-bao-cao",
        component: () => import("../pages/kybaocao"),
    },
    {
        path: "/don-vi-tinh",
        name: "don-vi-tinh",
        component: () => import("../pages/donViTinh"),
    },
    {
        path: "/loai-mau-bieu",
        name: "loai-mau-bieu",
        component: () => import("../pages/loaiMauBieu"),
    },
    {
        path: "/thiet-ke-mau-bieu",
        name: "thiet-ke-mau-bieu",
        component: () => import("../pages/thietKeMauBieu"),
    },
    {
        path: "/thiet-ke-mau-bieu/thiet-ke/:id?",
        name: "thiet-ke-mau-bieu-thiet-ke",
        component: () => import("../pages/thietKeMauBieu/thietKeMauBieu"),
    },
    {
        path: "/nhap-lieu",
        name: "don-vi-tinh",
        component: () => import("../pages/baocao/nhaplieu"),
    },
    {
        path: "/thuc-hien-nhap-lieu/:id?",
        name: "thuc-hien-nhap-lieu",
        component: () => import("../pages/baocao/inputBangBieu"),
    },
    {
        path: "/kiem-tra",
        name: "kiem-tra",
        component: () => import("../pages/baocao/kiemtra"),
    },
    {
        path: "/tong-hop",
        name: "tong-hop",
        component: () => import("../pages/baocao/tonghop"),
    },
    {
        path: "/xuat-ban",
        name: "han-hanh",
        component: () => import("../pages/baocao/banhanh"),
    },
    {
        path: "/huong-dan-su-dung",
        name: "/huong-dan-su-dung",
        component: () => import("../pages/huongDanSuDung"),
    },
    {
        path: "/ho-tro-doanh-nghiep",
        name: "/ho-tro-doanh-nghiep",
        component: () => import("../pages/hoTroDoanhNghiep"),
    },
    {
        path: "/ho-tro-doanh-nghiep/create/:id?",
        name: "/ho-tro-doanh-nghiep - craete",
        component: () => import("../pages/hoTroDoanhNghiep/create"),
    },
    {
        path: "/ho-tro-doanh-nghiep/thong-ke",
        name: "/ho-tro-doanh-nghiep - thong-ke",
        component: () => import("../pages/hoTroDoanhNghiep/thongke"),
    },
    {
        path: "/quan-ly-de-tai",
        name: "/quan-ly-khoa-hoc",
        component: () => import("../pages/quanLyKhoaHoc"),
    },
    {
        path: "/quan-ly-de-tai/create/:id?",
        name: "/ho-tro-doanh-nghiep - craete",
        component: () => import("../pages/quanLyKhoaHoc/create"),
    },
    {
        path: "/quan-ly-de-tai/thong-ke",
        name: "/ho-tro-doanh-nghiep - thong-ke",
        component: () => import("../pages/quanLyKhoaHoc/thongKe"),
    },
    {
        path: "/tong-hop-thong-ke",
        name: "/quan-ly-khoa-hoc",
        component: () => import("../pages/baocao/thongKe"),
    },
]
