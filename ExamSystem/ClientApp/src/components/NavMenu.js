import React, { Component } from 'react';
import { Collapse, Navbar, NavbarBrand, NavbarToggler, NavItem, NavLink } from 'reactstrap';
import { Link } from 'react-router-dom';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import './NavMenu.css';
import { faUser } from '@fortawesome/free-solid-svg-icons';
import useAuth from '../hooks/UseAuth';

export class NavMenu extends Component {
  static displayName = NavMenu.name;
  canEdit = false;
  isUser = false;

  constructor (props) {
    super(props);
    this.canEdit = localStorage.getItem('role')?.includes('Admin');
    this.isUser = localStorage.getItem('role')?.includes('User');
    this.toggleNavbar = this.toggleNavbar.bind(this);
    this.state = {
      collapsed: true
    };
  }


  toggleNavbar () {
    this.setState({
      collapsed: !this.state.collapsed
    });
  }


  render() {
    return (
      <header>
        <Navbar className="navbar-expand-sm navbar-toggleable-sm ng-white border-bottom box-shadow mb-3" container light>
          <NavbarBrand tag={Link} to="/">ExamSystem</NavbarBrand>
          <NavbarToggler onClick={this.toggleNavbar} className="mr-2" />
          <Collapse className="d-sm-inline-flex flex-sm-row-reverse" isOpen={!this.state.collapsed} navbar>
            <ul className="navbar-nav flex-grow">
                <NavItem>
                  <NavLink tag={Link} className="text-dark" to="/examtimes">Times</NavLink>
                </NavItem>
              {this.canEdit && (
                <NavItem>
                  <NavLink tag={Link} className="text-dark" to="/examlocations">Locations</NavLink>
                </NavItem>
              )}
              {(this.canEdit || this.isUser) && (
                <NavItem>
                  <NavLink tag={Link} className="text-dark" to="/exams">Exams</NavLink>
                </NavItem>
              )}
              {(this.canEdit || this.isUser) ? (
                <NavItem>
                  <NavLink tag={Link} className="text-dark" to="/logout">Logout</NavLink>
                </NavItem>
              ) :(<NavItem>
                <NavLink tag={Link} className="text-dark" to="/login"><FontAwesomeIcon icon={faUser}/></NavLink>
              </NavItem>)
              }
            </ul>
          </Collapse>
        </Navbar>
      </header>
    );
  }
}