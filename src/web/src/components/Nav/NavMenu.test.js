import React from 'react';
import { render, screen, cleanup, waitFor, act, getByText } from "@testing-library/react";
import { useAuth0 } from '@auth0/auth0-react';

import NavMenu from '../Nav/NavMenu';
import Login from '../Auth/Login';

jest.mock('@auth0/auth0-react');
jest.mock('../Auth/Login');

describe('Navigation menu', () => {
    afterEach(cleanup);

    it('should show high level link to main website', () => {
        useAuth0.mockReturnValue({});
        Login.mockReturnValue(null);
        render(<NavMenu />);
        expect(screen.getByText('My D&D App')).toBeDefined();
    });

    it('should show characters link if user is authenticated', () => {
        useAuth0.mockReturnValue({ isAuthenticated: true});
        Login.mockReturnValue(null);
        render(<NavMenu />);
        expect(screen.getByText('Characters')).toBeDefined();
    });

    it('should not show characters link if user is not authenticated', () => {
        useAuth0.mockReturnValue({ isAuthenticated: false});
        Login.mockReturnValue(null);
        render(<NavMenu />);
        expect(screen.queryByText('Characters')).toBeNull();
    });
});