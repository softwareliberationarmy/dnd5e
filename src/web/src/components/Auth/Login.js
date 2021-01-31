import React from "react";
import { useAuth0 } from "@auth0/auth0-react";
import { Image, Nav, Navbar } from 'react-bootstrap';

const Login = () => {
  const { loginWithRedirect, logout, user, isAuthenticated, isLoading, error } = useAuth0();

  const profileInfo = isLoading ? (<Navbar.Text className="d-inline">Loading ...</Navbar.Text>) :
    isAuthenticated ? (
        <Nav.Item className="align-items-center">
              <Image className="mr-1" src={user.picture} roundedCircle height="45" />
              <span className="align-items-center">
                {user.name}
              </span>
        </Nav.Item>
      ) : 
    null;

    const loginLogoutButton = isAuthenticated ? 
        (
          <Nav.Link className="text-nowrap align-middle" 
        onClick={() => logout({ returnTo: window.location.origin})}>
          Log Out
        </Nav.Link>
        ):
        (
          <Nav.Link className="text-nowrap align-middle" 
        onClick={() => loginWithRedirect(window.location.origin)}>
          Log In
        </Nav.Link>
        );

  return error ? error : (  
    <Nav>
        {profileInfo}
      <Nav.Item>
        {loginLogoutButton}
      </Nav.Item>      
    </Nav>  
  );
};

export default Login;