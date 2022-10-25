var currentUser = localStorage.getItem('auth-user') ? JSON.parse(localStorage.getItem('auth-user')): {};

const USERNAME = currentUser.userName;
 const FULLNAME = currentUser.fullName;
 const USER_KY_SO = {userNameKySo:currentUser.user != null?currentUser.user.userNameKySo: "", passwordKySo: currentUser.user != null?currentUser.user.passwordKySo: ""}
const ROLES = currentUser.user != null? currentUser.user.roles : [];
export const CURRENT_USER = {
    USERNAME, FULLNAME, USER_KY_SO, ROLES
}