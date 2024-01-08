import { faArrowLeft } from '@fortawesome/free-solid-svg-icons';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import {  useNavigate } from 'react-router-dom';
import { useState } from 'react';
import useAxiosPrivate from "../../hooks/UseAxiosPrivate";
import { useLocation } from 'react-router-dom';
import { useParams } from 'react-router-dom';

const CreateAnswer = () => {
    const navigate = useNavigate();
    const location = useLocation();
    const axiosPrivate = useAxiosPrivate();
    const { examId, questionId } = useParams();

    const [answerData, setAnswerData] = useState({
        title: "",
        description: "",
        isCorrect: false
    });

    const handleGoBack = () => {
        navigate(-1); // This will navigate back one step in the history
    };

    const handelInputChange = (event) => {
        const { id, value } = event.target;
        setAnswerData({ ...answerData, [id]: value });
    };

    const handelCheckboxChange = (event) => {
        const { id, checked } = event.target;
        setAnswerData({ ...answerData, [id]: checked });

    }
    
    const handleSubmit = async (event) => {
      event.preventDefault();
      try {
        const updatedAnswerData = { ...answerData, status: answerData.status ? 1 : 2 };
        await axiosPrivate.post(`/exams/${examId}/questions/${questionId}/answers`, updatedAnswerData);
        handleGoBack();
      } catch (err) {
        console.error(err);
        navigate('/login', { state: { from: location }, replace: true });
      }
    };
    

    return (
      <section>
        <div className="container mt-4">
          <h2>Create Answer</h2>
          <button className="btn btn-primary" onClick={handleGoBack}><FontAwesomeIcon icon={faArrowLeft} /></button>
          <div className='d-flex justify-content-start'>
          <form onSubmit={handleSubmit}>
            <div className="mb-3 row">
              <label htmlFor="title" className="col-sm-2 col-form-label" style={{paddingRight:"5rem"}}>
                Title
              </label>
              <div className="col-sm-10">
                <input
                  type="text"
                  className="form-control"
                  onChange={handelInputChange}
                  id="title"
                  placeholder='Enter answer title'
                  value={answerData.title}
                />
              </div>
            </div>
    
            <div className="mb-3 row">
              <label htmlFor="description" className="col-sm-2 col-form-label" style={{paddingRight:"5rem"}}>
                Description
              </label>
              <div className="col-sm-10">
                <input
                  type="text"
                  className="form-control"
                  id="description"
                  onChange={handelInputChange}
                  placeholder="Enter answer description"
                  value={answerData.description}
                />
              </div>
            </div>
    
            <div className="mb-3 row">
              <label htmlFor="isCorrect" className="col-sm col-form-label" style={{paddingRight:"5rem"}}>
                Is Correct
              </label>
              <div className="col-sm-10">
                <div className="form-check">
                  <input
                    type="checkbox"
                    onChange={handelCheckboxChange}
                    className="form-check-input"
                    id="isCorrect"
                    checked={answerData.isCorrect}
                  />
                </div>
              </div>
            </div>
    
            <button type="submit" className="btn btn-primary">
              Save Changes
            </button>
          </form>
        </div>
        </div>
        </section>
);
};
    
export default CreateAnswer;