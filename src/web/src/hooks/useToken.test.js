import React, { useState } from 'react';
import { render, screen, cleanup, waitFor, act } from "@testing-library/react";
import { useAuth0 } from '@auth0/auth0-react';

import { useToken } from './useToken';

jest.mock('@auth0/auth0-react');

const tokenFunc = tk => { return { token: tk };};   //for some reason, when this is declared inside TokenConsumer, our test fails
//we cannot test the react hook directly, so we create a dummy react component to test it
const TokenConsumer = () => {
    const response = useToken(tokenFunc);
    return response ? (<button>{response.token}</button>) : null;
}

describe('useToken react hook', () => {

    afterEach(cleanup);

    it('should make a call using a token created by auth0', async () => {        
        const token = '123abc';
        useAuth0.mockReturnValue({
            getAccessTokenSilently: () => Promise.resolve(token) 
        });

        await act(async () => {
            await render(<TokenConsumer />);
        });
        await waitFor(() => {
            const btn = screen.getByRole('button');  
            expect(btn.textContent).toBe('123abc');
        });
    });
});