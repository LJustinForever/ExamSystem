import React, { Component } from 'react';
import { Route, Routes } from 'react-router-dom';
import { Layout } from './components/Layout';
import './custom.css';
import Register from './components/Register';
import Login from './components/Login';
import Exams from './components/Exams/Exams';
import Unauthorized from './components/Unauthorized'; 
import RequireAuth from './components/RequireAuth';
import { Home } from './components/Home';
import EditExam from './components/Exams/EditExam';
import CreateExam from './components/Exams/CreateExam';
import Questions from './components/Questions/Questions';
import CreateQuestion from './components/Questions/CreateQuestion';
import EditQuestion from './components/Questions/EditQuestion';
import Answers from './components/Answers/Answers';
import CreateAnswer from './components/Answers/CreateAnswer';
import EditAnswer from './components/Answers/EditAnswer';
import ExamLocations from './components/ExamLocations/ExamLocations';
import CreateExamLocation from './components/ExamLocations/CreateExamLocation';
import EditExamLocation from './components/ExamLocations/EditExamLocation';
import ExamTimes from './components/ExamTimes/ExamTimes';
import Logout from './components/Logout';
import CreateExamTime from './components/ExamTimes/CreateExamTime';
import EditExamTime from './components/ExamTimes/EditTimes';

const ROLES = {
  'User': "User",
  'Admin': "Admin"
}

export default class App extends Component {
  static displayName = App.name;

  render() {
    return (
      <Layout>
        <Routes>
          <Route path='/'>
            <Route path="unauthorized" element={<Unauthorized />} />
            <Route path='register' element={<Register />}/>
            <Route path='login' element={<Login />}/>
            <Route path='logout' element={<Logout />}/>
            <Route path='/' element={<Home />}/>
            <Route path='examtimes' element={<ExamTimes />}/>
            <Route element={<RequireAuth allowedRoles={[ROLES.Admin, ROLES.User]}/>}>
              <Route path='exams' element={<Exams />}/>
              <Route element={<RequireAuth allowedRoles={ROLES.Admin}/>}>
                {/* <Route path='exams/create' element={<Exams />}/> */}
                <Route path='exams/:id/edit' element={<EditExam />}/>
                <Route path='exams/new' element={<CreateExam />}/>
                <Route path='exams/:examId/questions' element={<Questions />}/>
                <Route path='exams/:examId/questions/new' element={<CreateQuestion />}/>
                <Route path='exams/:examId/questions/:id/edit' element={<EditQuestion />}/>
                <Route path='exams/:examId/questions/:questionId/answers' element={<Answers />}/>
                <Route path='exams/:examId/questions/:questionId/answers/new' element={<CreateAnswer />}/>
                <Route path='exams/:examId/questions/:questionId/answers/:id/edit' element={<EditAnswer />}/>

                <Route path='examlocations' element={<ExamLocations />}/>
                <Route path='examlocations/new' element={<CreateExamLocation />}/>
                <Route path='examlocations/:id/edit' element={<EditExamLocation />}/>

                <Route path='examtimes/new' element={<CreateExamTime/>}/>
                <Route path='examtimes/:id/edit' element={<EditExamTime/>}/>
              </Route>
            </Route>
          </Route>
        </Routes>
      </Layout>
    );
  }
}
