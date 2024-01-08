import { useLocation, Navigate, Outlet } from "react-router-dom";
import useAuth from "../hooks/UseAuth";

const RequireAuth = ({ allowedRoles }) => {
    const role = localStorage.getItem('role');
    const user = localStorage.getItem('user');
    const location = useLocation();
    const userRoles = typeof role === 'string' ? [role] : role;
    return (
        userRoles && (userRoles.includes(allowedRoles) || userRoles.some(role => allowedRoles.includes(role)))
            ? <Outlet />
            : user
                ? <Navigate to="/unauthorized" state={{ from: location }} replace />
                : <Navigate to="/login" state={{ from: location }} replace />
    );
}

export default RequireAuth;