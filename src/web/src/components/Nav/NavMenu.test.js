import React from 'react';
import { render, screen, cleanup } from "@testing-library/react";
import { useAuth0 } from '@auth0/auth0-react';

import NavMenu from './NavMenu';
import Login from '../Auth/Login';

jest.mock('@auth0/auth0-react');
jest.mock('../Auth/Login');

describe('Navigation menu', () => {
    beforeEach(() => {
        Login.mockReturnValue(null);
        useAuth0.mockReturnValue({});
    });

    afterEach(cleanup);

    it('should show high level link to main website every time', () => {
        render(<NavMenu />);
        expect(screen.getByText('My D&D App')).toBeDefined();
    });

    it('should always show Home link', () => {
        render(<NavMenu />);
        expect(screen.getByText('Home')).toBeDefined();
    });

    it('should always show Free Roll link', () => {
        render(<NavMenu />);
        expect(screen.getByText('Free Roll')).toBeDefined();
    });

    it('should show characters link if user is authenticated', () => {
        useAuth0.mockReturnValue({ isAuthenticated: true});
        render(<NavMenu />);
        expect(screen.getByText('Characters')).toBeDefined();
    });

    it('should not show characters link if user is not authenticated', () => {
        render(<NavMenu />);
        expect(screen.queryByText('Characters')).toBeNull();
    });
});