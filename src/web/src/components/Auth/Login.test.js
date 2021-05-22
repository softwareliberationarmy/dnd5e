import React from 'react';
import Login from './Login'
import { render, screen, cleanup, fireEvent } from "@testing-library/react";
import { useAuth0 } from '@auth0/auth0-react';
import '@testing-library/jest-dom';

jest.mock('@auth0/auth0-react');

describe('Login component', () => {
    afterEach(cleanup);

    describe('when the component loads',() => {
        it('should show Log In button when user is not authenticated', () => {
            useAuth0.mockReturnValue({});
            render(<Login />);
            expect(screen.getByRole('button')).toHaveTextContent('Log In');
        });
    
        it('should show Log Out button when user is authenticated', () => {
            useAuth0.mockReturnValue({ isAuthenticated: true, user: {} });
            render(<Login />);
            expect(screen.getByRole('button')).toHaveTextContent('Log Out');
        });
    
        it('should show Loading link when user info is loading', () => {
            useAuth0.mockReturnValue({ isLoading: true});
            render(<Login />);
            expect(screen.getByText('Loading ...')).toBeDefined();
        });
    
        it('should show user info when not loading and user is authenticated', () => {
            useAuth0.mockReturnValue({ isAuthenticated: true, user: { name: 'Bruce Wayne' }});
            render(<Login />);
            expect(screen.getByText('Bruce Wayne')).toBeDefined();
        });
    
        it('should show error if error encountered', () => {
            const expected = 'Danger, Will Robinson!';
            useAuth0.mockReturnValue({ error: expected, isLoading: true });
            render(<Login />);
            expect(screen.getByText(expected)).toBeDefined();
        });    
    });

    describe('interactions', () => {
        it('should call logout function when clicking Log Out button', () => {
            const logout = jest.fn();
            useAuth0.mockReturnValue({ logout: logout, isAuthenticated: true, user: {} });
            render(<Login />);
            fireEvent.click(screen.getByText('Log Out'));
            expect(logout.mock.calls.length).toBe(1);
        });

        it('should call login function when clicking Log In button', () => {
            const login = jest.fn();
            useAuth0.mockReturnValue({loginWithRedirect: login });
            render(<Login />);
            fireEvent.click(screen.getByText('Log In'));
            expect(login.mock.calls.length).toBe(1);
        });
    });
});