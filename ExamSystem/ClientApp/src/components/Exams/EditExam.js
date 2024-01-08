import { faArrowLeft } from '@fortawesome/free-solid-svg-icons';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { useParams, useNavigate } from 'react-router-dom';
import { useEffect, useState } from 'react';
import useAxiosPrivate from "../../hooks/UseAxiosPrivate";
import { useLocation } from 'react-router-dom';


const EditExam = () => {
    const { id } = useParams();
    const navigate = useNavigate();
    const location = useLocation();
    const axiosPrivate = useAxiosPrivate();

    const [examData, setExamData] = useState({
        number: 0,
        name: "",
        status: false
    });

    useEffect(() => {
        const loadExam = async () => {
            try {
                const response = await axiosPrivate.get(`/exams/${id}`);
                response.data.status = response.data.status === 1 ? true : false;
                setExamData(response.data);
            } catch (err) {
                console.error(err);
                navigate('/login', { state: { from: location }, replace: true });
            }
        };
        loadExam();
    }, [axiosPrivate, id, navigate, location])

    const handleGoBack = () => {
        navigate(-1); // This will navigate back one step in the history
    };

    const handelInputChange = (event) => {
        const { id, value } = event.target;
        setExamData({ ...examData, [id]: value });
    };

    const handelCheckboxChange = (event) => {
        const { id, checked } = event.target;
        setExamData({ ...examData, [id]: checked });

    }
    
    const handleSubmit = async (event) => {
      event.preventDefault();
      try {
        const updatedExamData = { ...examData, status: examData.status ? 1 : 2 };
        await axiosPrivate.put(`/exams/${id}`, updatedExamData);
        handleGoBack();
      } catch (err) {
        console.error(err);
        navigate('/login', { state: { from: location }, replace: true });
      }
    };
    
    const redirectToQuestions = () => {
        navigate(`/exams/${id}/questions`, { state: { from: location } });
    }

    return (
      <section>
        <div className="container mt-4">
          <h2>Edit Exam</h2>
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
                  value={examData.number}
                  readOnly
                  disabled={true}
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
                  id="name"
                  onChange={handelInputChange}
                  placeholder="Enter exam name"
                  value={examData.name}
                />
              </div>
            </div>
    
            <div className="mb-3 row">
              <label htmlFor="status" className="col-sm-2 col-form-label" style={{paddingRight:"5rem"}}>
                Status
              </label>
              <div className="col-sm-10">
                <div className="form-check">
                  <input
                    type="checkbox"
                    onChange={handelCheckboxChange}
                    className="form-check-input"
                    id="status"
                    checked={examData.status}
                  />
                  <label className="form-check-label" htmlFor="status">
                    Active
                  </label>
                </div>
                <button className="btn btn-primary" onClick={redirectToQuestions}>View Questions</button>
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
    
export default EditExam;