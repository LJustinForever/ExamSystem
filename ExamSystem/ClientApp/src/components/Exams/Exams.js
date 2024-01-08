import { useState, useEffect, useCallback } from "react";
import useAxiosPrivate from "../../hooks/UseAxiosPrivate";
import { useNavigate, useLocation } from "react-router-dom";
import useAuth from "../../hooks/UseAuth";
import { Modal } from 'react-responsive-modal'
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { faTrash } from "@fortawesome/free-solid-svg-icons";

const Exams = () => {
    const axiosPrivate = useAxiosPrivate();
    const navigate = useNavigate();
    const location = useLocation();
    const { auth } = useAuth();

    const [exams, setExams] = useState([]);
    const [isLoading, setLoading] = useState(false);
    const [ setError] = useState(null);
    const canEdit = localStorage.getItem('role').includes('Admin');

    const getAllExams = useCallback(async (pageNumber) => {
        try {
        const response = await axiosPrivate.get('/exams', {
                params: { pageNumber : pageNumber }, // Pass the page number as a query parameter
            });
            return response.data;
        } catch (err) {
            console.error(err);
            navigate('/login', { state: { from: location }, replace: true });
            return [];
        }
    }, [axiosPrivate, navigate, location]);

    const loadExams = async () => { 
        try {
            if(isLoading) return;
            setLoading(true);
            const data = await getAllExams(1);
            setExams(data);
            setLoading(false);
        } catch (err) {
            setError(err);
        }
    };

    useEffect(() => {
        loadExams();
    }, []);

    const createExam = () => {
        navigate('/exams/new', { state: { from: location } });
    };

    const [open, setOpen] = useState(false);
    const [examToDelete, setExamToDelete] = useState("");

    const onOpenModal = (id) => {
        setExamToDelete(id);
        setOpen(true)
    };
    const onCloseModal = () => {
        setExamToDelete("");
        setOpen(false)
    };

    const deleteExam = async (id) => {
        try {
            await axiosPrivate.delete(`/exams/${id}`);
            loadExams();
            onCloseModal();
        } catch (err) {
            setError(err);
            onCloseModal();
        }
    };

    const editExam = async (id) => {
        navigate(`/exams/${id}/edit`, { state: { from: location } });
    }

    return(
        <article>
            <div className="container text-center">
                <h2>Exam List</h2>
                {canEdit && (<button className="btn btn-primary" style={{display:"flex",justifyContent: "flex-start"}} onClick={createExam}>Create Exam</button>)}
                {exams.length !== 0 ? (
                <table className="table">
                    <thead>
                        <tr>
                            <th scope="col">Number</th>
                            <th scope="col">Name</th>
                            <th scope="col">Status</th>
                            <th scope="col">Actions</th>
                        </tr>
                    </thead>
                    <tbody>
                        {exams.map((exam, index) => (
                            <tr key={index}>
                                <th >{exam.number}</th>
                                <td>{exam.name}</td>
                                <td>{exam.status == 1 ? ("Active") : ("Inactive")}</td>
                                {canEdit && (
                                    <td>
                                        <button className="btn btn-primary" style={{marginRight: "1rem"}} onClick={() => editExam(exam.id)}>Edit</button>
                                        <button className="btn btn-danger" onClick={() => onOpenModal(exam.id)}><FontAwesomeIcon icon={faTrash}/></button>
                                    </td>)
                                }
                            </tr>
                        ))}
                    </tbody>
                </table>
            ) : (<p>No exams to display</p>)}
            {isLoading ? (
                    <p>Loading...</p>
            ) : (
                <button onClick={loadExams} className="load-button-v1">Load</button>
            )}
            </div>
            <Modal open={open} onClose={onCloseModal} center>
                <div className="modal-content" style={{ paddingRight: "2rem", paddingBottom:"2rem" }}>
                    "Are you sure you want to delete this exam?"
                </div>
                <button className="btn btn-danger" style={{marginRight: "1rem"}} onClick={() => deleteExam(examToDelete)}>Yes</button>
                <button className="btn btn-secondary" onClick={onCloseModal}>No</button>
            </Modal>
        </article>
    )
}

export default Exams;
