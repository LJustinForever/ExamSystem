import { useState, useEffect } from 'react';
import useAuth from '../hooks/UseAuth';
import { Link, useNavigate, useLocation } from 'react-router-dom';
import { Modal } from 'react-responsive-modal'
import axios from '../api/axios';
import { jwtDecode } from 'jwt-decode';

const Login = () => {
    const { setAuth, setAuthToLocalStorage }= useAuth();

    const navigate = useNavigate();
    const location = useLocation();
    const from = location.state?.from?.pathname || "/";

    // const userRef = useRef();
    // const errRef = useRef();

    const [user, setUser] = useState('');
    const [pwd, setPwd] = useState('');
    // const [errMsg, setErrMsg] = useState('');

    const [open, setOpen] = useState(false);

    const onOpenModal = () => setOpen(true);
    const onCloseModal = () => setOpen(false);

    const [errorContent, setErrorContent] = useState('');

    useEffect(() => {
        setAuth({});
        localStorage.removeItem('refreshToken');
        localStorage.removeItem('accessToken');
        localStorage.removeItem('user');
        localStorage.removeItem('role');
        localStorage.removeItem('id');
            //userRef.current.focus();
    }, [])

    // useEffect(() => {
    //     setErrMsg('');
    // }, [user, pwd])

    const handleSubmit = async (e) => {
        e.preventDefault();

        try {
            const response = await axios.post('/login',
                JSON.stringify({ username : user, password : pwd }),
                {
                    headers: { 'Content-Type': 'application/json' },
                    withCredentials: true,
                    method: "POST"
                }
            );
            const accessToken = response?.data?.accessToken;
            const refreshToken = response?.data?.refreshToken;
            const decodedToken = jwtDecode(accessToken);
            const roles = decodedToken['http://schemas.microsoft.com/ws/2008/06/identity/claims/role'];
            const id = decodedToken.sub;
            setAuth({ user, roles, id, accessToken, refreshToken });
            setUser('');
            setPwd('');
            navigate(from, { replace: true });
            window.location.reload(false);
        } catch (err) {
            if (!err?.response) {
                console.log(err);
                // Handle no server response error by setting error content for modal
                setErrorContent('No Server Response');
            } else if (err.response?.status === 400) {
                // Handle 400 error
                setErrorContent('Missing Username or Password');
            } else if (err.response?.status === 401) {
                // Handle 401 error
                setErrorContent('Unauthorized');
            } else if (err.response?.status === 422) {
                // Handle 422 error
                if (err.response.data) {
                setErrorContent(err.response.data);
                } else {
                setErrorContent('Unprocessable Entity');
                }
            } else {
                // Handle other errors
                setErrorContent('Login Failed');
            }
            //errRef.current.focus();
            onOpenModal();
        }
    }

    return (
        <section>
            <div className="container">
                <div className="row justify-content-center">
                    <div className="col-md-6 ">
                            <h1>Sign In</h1>
                            <form onSubmit={handleSubmit} className="login-form">
                                <div className="form-group">
                                    <label htmlFor="username">Username</label>
                                    <input
                                        type="text"
                                        id="username"
                                        autoComplete="off"
                                        onChange={(e) => setUser(e.target.value)}
                                        value={user}
                                        className="form-control"
                                        required
                                    />
                                </div>
                                <div className="form-group">
                                    <label htmlFor="password">Password</label>
                                    <input
                                        type="password"
                                        id="password"
                                        onChange={(e) => setPwd(e.target.value)}
                                        value={pwd}
                                        className="form-control"
                                        required
                                    />
                                </div>
                                <button type="submit" className="btn btn-primary">Sign In</button>
                            </form>
                        <p className="account-section">
                            Need an Account?<br />
                            <span className="line">
                                <Link to="/register" className="signup-link">Sign Up</Link>
                            </span>
                        </p>
                        <div className="modal-container">
                            <Modal open={open} onClose={onCloseModal} center>
                                <div className="modal-content" style={{ paddingRight: "2rem" }}>
                                    {errorContent}
                                </div>
                            </Modal>
                        </div>
                    </div>
                </div>
            </div>
        </section>
    )
}

export default Login