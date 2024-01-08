import { faArrowLeft } from '@fortawesome/free-solid-svg-icons';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import {  useNavigate } from 'react-router-dom';
import { useState } from 'react';
import useAxiosPrivate from "../../hooks/UseAxiosPrivate";
import { useLocation } from 'react-router-dom';


const CreateExamLocation = () => {
    const navigate = useNavigate();
    const location = useLocation();
    const axiosPrivate = useAxiosPrivate();

    const [examLocationData, setExamLocationData] = useState({
        name: "",
        status: false
    });

    const handleGoBack = () => {
        navigate(-1); // This will navigate back one step in the history
    };

    const handelInputChange = (event) => {
        const { id, value } = event.target;
        setExamLocationData({ ...examLocationData, [id]: value });
    };

    const handelCheckboxChange = (event) => {
        const { id, checked } = event.target;
        setExamLocationData({ ...examLocationData, [id]: checked });
    }
    
    const handleSubmit = async (event) => {
      event.preventDefault();
      try {
        const updatedExamLocationData = { ...examLocationData, status: examLocationData.status ? 1 : 2 };
        await axiosPrivate.post(`/examlocations`, updatedExamLocationData);
        handleGoBack();
      } catch (err) {
        console.error(err);
        navigate('/login', { state: { from: location }, replace: true });
      }
    };
    
    return (
      <section>
        <div className="container mt-4">
          <h2>Create Exam Location</h2>
          <button className="btn btn-primary" onClick={handleGoBack}><FontAwesomeIcon icon={faArrowLeft} /></button>
          <div className='d-flex justify-content-start'>
          <form onSubmit={handleSubmit}>
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
                  placeholder="Enter exam location"
                  value={examLocationData.name}
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
                    checked={examLocationData.status}
                  />
                  <label className="form-check-label" htmlFor="status">
                    Active
                  </label>
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
    
export default CreateExamLocation;