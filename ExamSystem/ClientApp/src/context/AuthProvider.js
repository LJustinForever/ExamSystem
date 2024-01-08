import { createContext, useEffect, useState } from "react";

const AuthContext = createContext({});

export const AuthProvider = ({ children }) => {
    const [auth, setAuth] = useState({});
    
    if(auth.refreshToken !== undefined) 
        localStorage.setItem('refreshToken', auth.refreshToken);
    if(auth.accessToken !== undefined) 
        localStorage.setItem('accessToken', auth.accessToken);
    if(auth.roles !== undefined) 
        localStorage.setItem('role', auth.roles);
    if(auth.id !== undefined) 
        localStorage.setItem('id', auth.id);
    if(auth.user !== undefined) 
        localStorage.setItem('user', auth.user);

    return (
        <AuthContext.Provider value={{ auth, setAuth }}>
            {children}
        </AuthContext.Provider>
    )
}

export default AuthContext;