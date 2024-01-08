import React, { Component } from 'react';
import examImage from '../exam.jpg';

export class Home extends Component {
  static displayName = Home.name;

render() {
    return (
            <div className="container mt-5">
                <div className="row">
                    <div className="col-md-12 text-center">
                        <h1>Welcome to the Exam System</h1>
                        <p>
                            This system allows you to manage exams, view results, and much more.
                        </p>
                    <img src={examImage} className='img-fluid image' alt=''/>
                    </div>
                </div>
            </div>
        );
}
}
