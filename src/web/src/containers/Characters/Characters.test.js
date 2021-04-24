import React from 'react';
import { Row, Card } from 'react-bootstrap';
import { useAuth0 } from '@auth0/auth0-react';
import { act } from 'react-dom/test-utils';

import Characters from './Characters';
import { getMyCharacters } from '../../services/CharacterService';
import { render, fireEvent, screen, getByText, cleanup, wait } from "@testing-library/react";

jest.mock('../../services/CharacterService');
jest.mock('@auth0/auth0-react');

describe('Characters container', () => {
    beforeEach(() => {
        useAuth0.mockImplementation(() => {
            return {
                getAccessTokenSilently: () => Promise.resolve({})
            };
        });
        getMyCharacters.mockImplementation(() => {
            return Promise.resolve({ data: [] });
        });
    });

    afterEach(cleanup);

    it('should render an empty row if no characters', async () => {
        await act(async () =>{ 
            render(<Characters />);     
            screen.debug();                   
            expect(screen.getByText("Characters")).toBeDefined();
        });        
    });
    
    it('should return a character card for each character returned', async () => {
        getMyCharacters.mockImplementation(() => {
            return Promise.resolve({
                data: [
                    { id: 1, name: 'Bob', level: 1, class: 'Fighter', race: 'Human'},
                    { id: 2, name: 'Bilbo', level: 1, class: 'Rogue', race: 'Halfling'},
                    { id: 3, name: 'Wensleydale', level: 1, class: 'Cleric', race: 'Elf'}
                ]
            });
        } );

        await act(async () =>{ 
            await render(<Characters />);     
        });        
        expect(screen.getByText("Characters")).toBeDefined();
        await wait(() => expect((screen.getAllByTestId('character')).length).toBe(3));
        screen.debug();                   
    });
});