import React from 'react';
import { Container, Nav, Navbar } from 'react-bootstrap';
import { Link } from 'react-router-dom';
import AuthService from '../Services/AuthService';

import SearchField from './SearchField';

import './styles/Header.scss'

export default function Header(props) 
{
    return (
        <Navbar expand="md" bg="dark" variant="dark">
            <Container>
                <Navbar.Brand>
                    <Link to="/" className="nav-link mainLink">
                        <img src="/img/main-flower.svg" alt="" height="30" width="30" className="d-inline-block align-top"></img>
                        FLOWERS
                    </Link>
                </Navbar.Brand>

                <Navbar.Collapse>
                    
                    <Nav className="me-auto">

                    </Nav>
                    <Nav>

                        {!props.isAutorized ?
                        <>
                            <Nav.Item className="item-padding-right">
                                <Link className="nav-link" to="/login">
                                    Login
                                </Link>
                            </Nav.Item>

                            <Nav.Item className="item-padding-right">
                                <Link className="nav-link" to="/Register">
                                    Register
                                </Link>
                            </Nav.Item>   
                        </> 
                        :
                        <>
                            <Nav.Item className="item-padding-right">
                                <Link className="nav-link" onClick={() => AuthService.logout()}>
                                    Logout
                                </Link>
                            </Nav.Item>

                            <Nav.Item className="item-padding-right">
                                <Link className="nav-link" to="/Orders">
                                    Orders
                                </Link>
                            </Nav.Item> 

                        </>
                        }

                        

                        <Nav.Item className="item-padding-right">
                            <Link className="nav-link" to={props.isAutorized ? "/cart" : "/cart"}>
                                <img src="/img/cart.svg" alt="cart" height="26" width="26" className="d-inline-block align-top invert"></img>
                            </Link>
                        </Nav.Item>
                    </Nav>

                </Navbar.Collapse>

                <SearchField/>

                <Navbar.Toggle>
                </Navbar.Toggle>

            </Container>
        </Navbar>
    );

}
