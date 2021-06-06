import React from 'react';
import { useAuth0 } from '@auth0/auth0-react';
import { act } from 'react-dom/test-utils';
import { render, screen, cleanup, waitFor, getByText } from "@testing-library/react";

import Characters from './Characters';
import { getMyCharacters } from '../../services/CharacterService';

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
            expect(screen.getByText("Characters")).toBeDefined();
        });        
    });
    
    it('should return a character card for each character returned', async () => {
        getMyCharacters.mockImplementation(() => {
            return Promise.resolve({
                data: [
                    { id: 7, name: 'Bob', level: 1, class: 'Fighter', race: 'Human'},
                    { id: 11, name: 'Bilbo', level: 1, class: 'Rogue', race: 'Halfling'},
                    { id: 17, name: 'Wensleydale', level: 1, class: 'Cleric', race: 'Elf'}
                ]
            });
        } );

        await act(async () =>{ 
            await render(<Characters />);     
        });        
        expect(screen.getByText("Characters")).toBeDefined();
        await waitFor(() => {
            const characters = screen.getAllByTestId('character');
            expect(characters.length).toBe(3);
            expect(characters[0].querySelector('.card-title').textContent).toBe('Bob');
            expect(characters[2].querySelector('.card-subtitle').textContent).toBe('Level 1 Cleric (Elf)');
        });
    });
});