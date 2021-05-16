import React from 'react';
import Login from './Login'
import { render, screen, cleanup } from "@testing-library/react";
import { useAuth0 } from '@auth0/auth0-react';

jest.mock('@auth0/auth0-react');

describe('Login component', () => {
    afterEach(cleanup);

    it('should show Log In button when user is not authenticated', () => {
        useAuth0.mockReturnValue({});
        render(<Login />);
        expect(screen.getByText('Log In')).toBeDefined();
    });
});