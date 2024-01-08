import { useState, useEffect, useCallback } from "react";
import useAxiosPrivate from "../../hooks/UseAxiosPrivate";
import { useNavigate, useLocation } from "react-router-dom";
import useAuth from "../../hooks/UseAuth";
import { Modal } from 'react-responsive-modal'
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { faTrash } from "@fortawesome/free-solid-svg-icons";

const ExamLocations = () => {
    const axiosPrivate = useAxiosPrivate();
    const navigate = useNavigate();
    const location = useLocation();
    const { auth } = useAuth();

    const [examLocations, setExamLocations] = useState([]);
    const [isLoading, setLoading] = useState(false);
    const [ setError] = useState(null);
    const canEdit = localStorage.getItem('role').includes('Admin');

    const getAllExamLocations = useCallback(async (pageNumber) => {
        try {
        const response = await axiosPrivate.get('/examlocations', {
                params: { pageNumber : pageNumber }, // Pass the page number as a query parameter
            });
            return response.data;
        } catch (err) {
            console.error(err);
            navigate('/login', { state: { from: location }, replace: true });
            return [];
        }
    }, [axiosPrivate, navigate, location]);

    const loadExamLocations = useCallback(async () => { 
        try {
            if(isLoading) return;
            setLoading(true);
            const data = await getAllExamLocations(1);
            setExamLocations(data);
            setLoading(false);
        } catch (err) {
            setError(err);
        }
    }, [isLoading, setError, getAllExamLocations]);

    useEffect(() => {
        loadExamLocations();
    }, []);

    const createExam = () => {
        navigate('/examlocations/new', { state: { from: location } });
    };

    const [open, setOpen] = useState(false);
    const [examToDelete, setExamLocationToDelete] = useState("");

    const onOpenModal = (id) => {
        setExamLocationToDelete(id);
        setOpen(true)
    };
    const onCloseModal = () => {
        setExamLocationToDelete("");
        setOpen(false)
    };

    const deleteExamLocation = async (id) => {
        try {
            await axiosPrivate.delete(`/examlocations/${id}`);
            loadExamLocations();
            onCloseModal();
        } catch (err) {
            setError(err);
            onCloseModal();
        }
    };

    const editExam = async (id) => {
        navigate(`/examlocations/${id}/edit`, { state: { from: location } });
    }

    return(
        <article>
            <div className="container text-center">
                <h2>Exam Location List</h2>
                {canEdit && (<button className="btn btn-primary" style={{display:"flex",justifyContent: "flex-start"}} onClick={createExam}>Create Exam Location</button>)}
                {examLocations.length !== 0 ? (
                <table className="table">
                    <thead>
                        <tr>
                            <th scope="col">Name</th>
                            <th scope="col">Status</th>
                            <th scope="col">Actions</th>
                        </tr>
                    </thead>
                    <tbody>
                        {examLocations.map((exam, index) => (
                            <tr key={index}>
                                <td>{exam.name}</td>
                                <td>{exam.status === 1 ? ("Active") : ("Inactive")}</td>
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
            ) : (<p>No examLocations to display</p>)}
            {isLoading ? (
                    <p>Loading...</p>
            ) : (
                <button onClick={loadExamLocations} className="load-button-v1">Load</button>
            )}
            </div>
            <Modal open={open} onClose={onCloseModal} center>
                <div className="modal-content" style={{ paddingRight: "2rem", paddingBottom:"2rem" }}>
                    "Are you sure you want to delete this exam location?"
                </div>
                <button className="btn btn-danger" style={{marginRight: "1rem"}} onClick={() => deleteExamLocation(examToDelete)}>Yes</button>
                <button className="btn btn-secondary" onClick={onCloseModal}>No</button>
            </Modal>
        </article>
    )
}

export default ExamLocations;
