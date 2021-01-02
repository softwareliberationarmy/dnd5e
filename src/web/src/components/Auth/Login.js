import React from "react";
import { useAuth0 } from "@auth0/auth0-react";
import { Image, Nav, Navbar } from 'react-bootstrap';

const Login = () => {
  const { loginWithRedirect, user, isAuthenticated, isLoading } = useAuth0();

  const profileInfo = isLoading ? 
      (<Navbar.Text className="d-inline">Loading ...</Navbar.Text>) :
    isAuthenticated ? 
      (
        <Nav.Item className="align-items-center">
              <Image className="mr-1" src={user.picture} roundedCircle height="45" />
              <span className="align-items-center">
                {user.name}
              </span>
        </Nav.Item>
      ) : 
    null;

  return (  
    <Nav>
        {profileInfo}
      <Nav.Item>
        <Nav.Link className="text-nowrap align-middle" 
        onClick={() => loginWithRedirect('http://localhost:3000')}>
          Log In
        </Nav.Link>
      </Nav.Item>
      
    </Nav>  
  );
};

export default Login;