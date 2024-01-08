import { useState, useEffect, useCallback } from "react";
import useAxiosPrivate from "../../hooks/UseAxiosPrivate";
import { useNavigate, useLocation } from "react-router-dom";
import useAuth from "../../hooks/UseAuth";
import { Modal } from 'react-responsive-modal'
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { faArrowLeft, faCheck, faPlus, faTrash, faX } from "@fortawesome/free-solid-svg-icons";
import { useParams } from 'react-router-dom';

const Answers = () => {
    const axiosPrivate = useAxiosPrivate();
    const navigate = useNavigate();
    const location = useLocation();
    const { auth } = useAuth();
    const { examId, questionId } = useParams();

    const [answers, setAnswers] = useState([]);
    const [isLoading, setLoading] = useState(false);
    const [error, setError] = useState(null);
    const canEdit = localStorage.getItem('role').includes('Admin');

    const getAllAnswers = useCallback(async (pageNumber) => {
        try {
        const response = await axiosPrivate.get(`/exams/${examId}/questions/${questionId}/answers`, {
                params: { pageNumber : pageNumber }, // Pass the page number as a query parameter
            });
            return response.data;
        } catch (err) {
            console.error(err);
            navigate('/login', { state: { from: location }, replace: true });
            return [];
        }
    }, [examId, questionId, axiosPrivate, navigate, location]);

    const loadAnswers = async () => { 
        try {
            if(isLoading) return;
            setLoading(true);
            const data = await getAllAnswers(1);
            setAnswers(data);
            setLoading(false);
        } catch (err) {
            setError(err);
        }
    };

    useEffect(() => {
        loadAnswers();
    }, []);

    const handleGoBack = () => {
        navigate(-1); // This will navigate back one step in the history
    };

    const createAnswer = () => {
        navigate(`/exams/${examId}/questions/${questionId}/answers/new`, { state: { from: location } });
    };

    const [open, setOpen] = useState(false);
    const [answerToDelete, setAnswerToDelete] = useState("");

    const onOpenModal = (id) => {
        setAnswerToDelete(id);
        setOpen(true)
    };
    const onCloseModal = () => {
        setAnswerToDelete("");
        setOpen(false)
    };

    const deleteAnswer = async (id) => {
        try {
            await axiosPrivate.delete(`/exams/${examId}/questions/${questionId}/answers/${id}`);
            loadAnswers();
            onCloseModal();
        } catch (err) {
            setError(err);
            onCloseModal();
        }
    };

    const editAnswer = async (id) => {
        navigate(`/exams/${examId}/questions/${questionId}/answers/${id}/edit`, { state: { from: location } });
    }

    return(
        <article>
            <div className="container text-center">
                <h2>Answer List</h2>
                <button className="btn btn-primary" onClick={handleGoBack} style={{display:"flex",justifyContent: "flex-start", marginBottom:"1rem"}}><FontAwesomeIcon icon={faArrowLeft} /></button>
                {canEdit && (<button className="btn btn-primary" style={{display:"flex",justifyContent: "flex-start"}} onClick={createAnswer}><FontAwesomeIcon icon={faPlus}/></button>)}
                {answers.length !== 0 ? (
                <table className="table">
                    <thead>
                        <tr>
                            <th scope="col">Title</th>
                            <th scope="col">Description</th>
                            <th scope="col">IsCorrect</th>
                            <th scope="col">Actions</th>
                        </tr>
                    </thead>
                    <tbody>
                        {answers.map((answer, index) => (
                            <tr key={index}>
                                <td>{answer.title}</td>
                                <td>{answer.description}</td>
                                <td>{answer.isCorrect === true ? (<FontAwesomeIcon icon={faCheck} style={{color: '#00eb10'}}/>) : (<FontAwesomeIcon icon={faX} style={{color: '#e80000'}}/>)}</td>
                                {canEdit && (
                                    <td>
                                        <button className="btn btn-primary" style={{marginRight: "1rem"}} onClick={() => editAnswer(answer.id)}>Edit</button>
                                        <button className="btn btn-danger" onClick={() => onOpenModal(answer.id)}><FontAwesomeIcon icon={faTrash}/></button>
                                    </td>)
                                }
                            </tr>
                        ))}
                    </tbody>
                </table>
            ) : (<p>No answers to display</p>)}
            {isLoading ? (
                    <p>Loading...</p>
            ) : (
                <button onClick={loadAnswers} className="load-button-v1">Load</button>
            )}
            </div>
            <Modal open={open} onClose={onCloseModal} center>
                <div className="modal-content" style={{ paddingRight: "2rem", paddingBottom:"2rem" }}>
                    "Are you sure you want to delete this answer?"
                </div>
                <button className="btn btn-danger" style={{marginRight: "1rem"}} onClick={() => deleteAnswer(answerToDelete)}>Yes</button>
                <button className="btn btn-secondary" onClick={onCloseModal}>No</button>
            </Modal>
        </article>
    )
}

export default Answers;
