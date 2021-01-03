import React from 'react';
import Login from '../Auth/Login';
import { Navbar, Nav } from 'react-bootstrap';
import { useAuth0 } from '@auth0/auth0-react'

const NavMenu = () => {
    const { isAuthenticated } = useAuth0();

    return (
        <header>
            <Navbar bg="dark" variant="dark" expand="sm">
                <Navbar.Brand href="/">My D&amp;D App</Navbar.Brand>
                <Navbar.Toggle aria-controls="basic-navbar-nav" />
                <Navbar.Collapse id="basic-navbar-nav"> 
                    <Nav className="mr-auto">
                        <Nav.Link href="/">Home</Nav.Link>
                        <Nav.Link href="/roll">Free Roll</Nav.Link>   
                        {
                            isAuthenticated ? 
                            <Nav.Link href="/characters">Characters</Nav.Link> : 
                            null  
                        }                     
                    </Nav>
                    <Login />
                </Navbar.Collapse>
            </Navbar>
          </header>
    );
}

export default NavMenu;