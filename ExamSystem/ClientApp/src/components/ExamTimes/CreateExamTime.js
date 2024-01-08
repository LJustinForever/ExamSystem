import { faArrowLeft } from '@fortawesome/free-solid-svg-icons';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import {  useNavigate } from 'react-router-dom';
import { useEffect, useState } from 'react';
import useAxiosPrivate from "../../hooks/UseAxiosPrivate";
import { useLocation } from 'react-router-dom';
import DatePicker from "react-date-picker";

const CreateExamTime = () => {
    const navigate = useNavigate();
    const location = useLocation();
    const axiosPrivate = useAxiosPrivate();

    const [examData, setExamData] = useState({
        date: "",
        time: "",
        status: false,
        examId: "",
        examLocationId: ""
    });

    const [exams, setExams] = useState([]);
    const [examLocations, setExamLocations] = useState([]);

    useEffect(() => {
        const loadExams = async () => {
            try {
                const response = await axiosPrivate.get(`/exams`, {
                    params: { pageNumber : 1 }, // Pass the page number as a query parameter
                });
                setExams(response.data);
            } catch (err) {
                console.error(err);
                navigate('/login', { state: { from: location }, replace: true });
            }
        };
        loadExams();
    }, [axiosPrivate, navigate, location]);

    useEffect(() => {
        const loadExamLocations = async () => {
            try {
                const response = await axiosPrivate.get('/examlocations', {
                        params: { pageNumber : 1 }, // Pass the page number as a query parameter
                    });
                    setExamLocations(response.data);
                } catch (err) {
                    console.error(err);
                    navigate('/login', { state: { from: location }, replace: true });
                    return [];
                }
        };
        loadExamLocations();
    }, [axiosPrivate, navigate, location]);


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
        const updatedExamData = { ...examData, status: examData.status ? 1 : 2, date: examData.date + " " + examData.time, time: examData.date + " " + examData.time };
        await axiosPrivate.post(`/examtimes`, updatedExamData);
        handleGoBack();
      } catch (err) {
        console.error(err);
        navigate('/login', { state: { from: location }, replace: true });
      }
    };
    

    return (
      <section>
        <div className="container mt-4">
          <h2>Create Exam Time</h2>
          <button className="btn btn-primary" onClick={handleGoBack}><FontAwesomeIcon icon={faArrowLeft} /></button>
          <div className='d-flex justify-content-start'>
          <form onSubmit={handleSubmit}>
            <div className="mb-3 row">
              <label htmlFor="number" className="col-sm-2 col-form-label" style={{paddingRight:"15rem"}}>
                Date
              </label>
              <div className="col-sm-10">
                <input
                  type="date"
                  className="form-control"
                  onChange={handelInputChange}
                  id="date"
                  placeholder='Enter date'
                  value={examData.date}
                  required
                />
              </div>
            </div>
    
            <div className="mb-3 row">
              <label htmlFor="time" className="col-sm-2 col-form-label" style={{paddingRight:"15rem"}}>
                Time
              </label>
              <div className="col-sm-10">
                <input
                  type="time"
                  className="form-control"
                  id="time"
                  onChange={handelInputChange}
                  value={examData.time}
                  required
                />
              </div>
            </div>
            <div className="mb-3 row">
              <label htmlFor="status" className="col-sm-2 col-form-label" style={{paddingRight:"15rem"}}>
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
                <div className="mb-3 row">
                    <label htmlFor="examId" className="col-sm-2 col-form-label" style={{paddingRight:"15rem"}}>
                        Exam
                    </label>
                    <div className="col-sm-10">
                        <select
                        className="form-select"
                        id="examId"
                        onChange={handelInputChange}
                        def
                        value={examData.examId}
                        required
                        >
                        <option value="" disabled selected hidden>Select Exam</option>
                        {exams.map((exam) => (
                            <option key={exam.id} value={exam.id}>
                                {exam.name}
                            </option>
                        ))}
                        </select>
                    </div>
                    <div className="mb-3 row">
                        <label htmlFor="examLocationId" className="col-sm-2 col-form-label" style={{paddingRight:"15rem", whiteSpace:"nowrap"}}>
                            Exam Location
                        </label>
                        <div className="col-sm-10">
                            <select
                            className="form-select"
                            id="examLocationId"
                            onChange={handelInputChange}
                            def
                            value={examData.examLocationId}
                            required
                            >
                            <option value="" disabled selected hidden>Select Exam Location</option>
                            {examLocations.map((location) => (
                                <option key={location.id} value={location.id}>
                                    {location.name}
                                </option>
                            ))}
                            </select>
                        </div>
                    </div>
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
    
export default CreateExamTime;