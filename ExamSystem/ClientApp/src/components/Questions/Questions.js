import { useState, useEffect, useCallback } from "react";
import useAxiosPrivate from "../../hooks/UseAxiosPrivate";
import { useNavigate, useLocation } from "react-router-dom";
import useAuth from "../../hooks/UseAuth";
import { Modal } from 'react-responsive-modal'
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { faArrowLeft, faPlus, faTrash } from "@fortawesome/free-solid-svg-icons";
import { useParams } from 'react-router-dom';

const Questions = () => {
    const axiosPrivate = useAxiosPrivate();
    const navigate = useNavigate();
    const location = useLocation();
    const { auth } = useAuth();
    const { examId } = useParams();

    const [questions, setQuestions] = useState([]);
    const [isLoading, setLoading] = useState(false);
    const [error, setError] = useState(null);
    const canEdit = localStorage.getItem('role').includes('Admin');

    const getAllQuestions = useCallback(async (pageNumber) => {
        try {
        const response = await axiosPrivate.get(`/exams/${examId}/questions`, {
                params: { pageNumber : pageNumber }, // Pass the page number as a query parameter
            });
            return response.data;
        } catch (err) {
            console.error(err);
            navigate('/login', { state: { from: location }, replace: true });
            return [];
        }
    }, [examId, axiosPrivate, navigate, location]);

    const loadQuestions = async () => { 
        try {
            if(isLoading) return;
            setLoading(true);
            const data = await getAllQuestions(1);
            setQuestions(data);
            setLoading(false);
        } catch (err) {
            setError(err);
        }
    };

    useEffect(() => {
        loadQuestions();
    }, []);

    const handleGoBack = () => {
        navigate(-1); // This will navigate back one step in the history
    };

    const createQuestion = () => {
        navigate(`/exams/${examId}/questions/new`, { state: { from: location } });
    };

    const [open, setOpen] = useState(false);
    const [questionToDelete, setQuestionToDelete] = useState("");

    const onOpenModal = (id) => {
        setQuestionToDelete(id);
        setOpen(true)
    };
    const onCloseModal = () => {
        setQuestionToDelete("");
        setOpen(false)
    };

    const deleteQuestion = async (id) => {
        try {
            await axiosPrivate.delete(`/exams/${examId}/questions/${id}`);
            loadQuestions();
            onCloseModal();
        } catch (err) {
            setError(err);
            onCloseModal();
        }
    };

    const editQuestion = async (id) => {
        navigate(`/exams/${examId}/questions/${id}/edit`, { state: { from: location } });
    }

    return(
        <article>
            <div className="container text-center">
                <h2>Question List</h2>
                <button className="btn btn-primary" onClick={handleGoBack} style={{display:"flex",justifyContent: "flex-start", marginBottom:"1rem"}}><FontAwesomeIcon icon={faArrowLeft} /></button>
                {canEdit && (<button className="btn btn-primary" style={{display:"flex",justifyContent: "flex-start"}} onClick={createQuestion}><FontAwesomeIcon icon={faPlus}/></button>)}
                {questions.length !== 0 ? (
                <table className="table">
                    <thead>
                        <tr>
                            <th scope="col">Number</th>
                            <th scope="col">Description</th>
                            <th scope="col">Actions</th>
                        </tr>
                    </thead>
                    <tbody>
                        {questions.map((question, index) => (
                            <tr key={index}>
                                <td>{question.number}</td>
                                <td>{question.description}</td>
                                {canEdit && (
                                    <td>
                                        <button className="btn btn-primary" style={{marginRight: "1rem"}} onClick={() => editQuestion(question.id)}>Edit</button>
                                        <button className="btn btn-danger" onClick={() => onOpenModal(question.id)}><FontAwesomeIcon icon={faTrash}/></button>
                                    </td>)
                                }
                            </tr>
                        ))}
                    </tbody>
                </table>
            ) : (<p>No questions to display</p>)}
            {isLoading ? (
                    <p>Loading...</p>
            ) : (
                <button onClick={loadQuestions} className="load-button-v1">Load</button>
            )}
            </div>
            <Modal open={open} onClose={onCloseModal} center>
                <div className="modal-content" style={{ paddingRight: "2rem", paddingBottom:"2rem" }}>
                    "Are you sure you want to delete this question?"
                </div>
                <button className="btn btn-danger" style={{marginRight: "1rem"}} onClick={() => deleteQuestion(questionToDelete)}>Yes</button>
                <button className="btn btn-secondary" onClick={onCloseModal}>No</button>
            </Modal>
        </article>
    )
}

export default Questions;
