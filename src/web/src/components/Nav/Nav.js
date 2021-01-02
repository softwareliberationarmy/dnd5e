import React from 'react';
import Login from '../Auth/Login';
import { Navbar, Nav } from 'react-bootstrap';

const nav = () => {
    return (
        <header>
            <Navbar bg="dark" variant="dark" expand="sm">
                <Navbar.Brand href="/">My D&amp;D App</Navbar.Brand>
                <Navbar.Toggle aria-controls="basic-navbar-nav" />
                <Navbar.Collapse id="basic-navbar-nav"> 
                    <Nav className="mr-auto">
                        <Nav.Link href="/">Home</Nav.Link>
                        <Nav.Link href="/roll">Free Roll</Nav.Link>                        
                    </Nav>
                    <Login />
                </Navbar.Collapse>
            </Navbar>
          </header>
    );
}

export default nav;