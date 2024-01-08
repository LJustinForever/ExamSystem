import { faArrowLeft } from '@fortawesome/free-solid-svg-icons';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import {  useNavigate } from 'react-router-dom';
import { useState } from 'react';
import useAxiosPrivate from "../../hooks/UseAxiosPrivate";
import { useLocation, useParams } from 'react-router-dom';


const CreateQuestion = () => {
    const navigate = useNavigate();
    const {examId} = useParams();
    const location = useLocation();
    const axiosPrivate = useAxiosPrivate();

    const [questionData, setQuestionData] = useState({
        number: "",
        description: "",
        examId: examId
    });

    const handleGoBack = () => {
        navigate(-1); // This will navigate back one step in the history
    };

    const handelInputChange = (event) => {
        const { id, value } = event.target;
        setQuestionData({ ...questionData, [id]: value });
    };
    
    const handleSubmit = async (event) => {
      event.preventDefault();
      try {
        await axiosPrivate.post(`/exams/${examId}/questions`, questionData);
        handleGoBack();
      } catch (err) {
        console.error(err);
        navigate('/login', { state: { from: location }, replace: true });
      }
    };
    

    return (
      <section>
        <div className="container mt-4">
          <h2>Create Question</h2>
          <button className="btn btn-primary" onClick={handleGoBack}><FontAwesomeIcon icon={faArrowLeft} /></button>
          <div className='d-flex justify-content-start'>
          <form onSubmit={handleSubmit}>
            <div className="mb-3 row">
              <label htmlFor="number" className="col-sm-2 col-form-label" style={{paddingRight:"5rem"}}>
                Number
              </label>
              <div className="col-sm-10">
                <input
                  type="text"
                  className="form-control"
                  onChange={handelInputChange}
                  id="number"
                  placeholder='Enter question number'
                  value={questionData.number}
                />
              </div>
            </div>
    
            <div className="mb-3 row">
              <label htmlFor="name" className="col-sm-2 col-form-label" style={{paddingRight:"5rem"}}>
                Name
              </label>
              <div className="col-sm-10">
                <input
                  type="text"
                  className="form-control"
                  id="description"
                  onChange={handelInputChange}
                  placeholder="Enter question description"
                  value={questionData.description}
                />
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
    
export default CreateQuestion;