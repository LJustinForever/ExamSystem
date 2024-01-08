import { useEffect } from 'react';

const Logout = () => {
    useEffect(() => {
        localStorage.removeItem('refreshToken');
        localStorage.removeItem('accessToken');
        localStorage.removeItem('user');
        localStorage.removeItem('role');
        localStorage.removeItem('id');
        window.location.href = '/login';
    }, [])
};

export default Logout;