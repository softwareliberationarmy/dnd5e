import React from 'react';
import { useHistory } from 'react-router-dom';
import { useAuth0 } from '@auth0/auth0-react';
import { act } from 'react-dom/test-utils';
import { render, screen, cleanup, waitFor, getByText } from "@testing-library/react";

import Characters from './Characters';
import { getMyCharacters } from '../../services/CharacterService';
import userEvent from '@testing-library/user-event';

//NOTE: for testing useHistory: https://stackoverflow.com/questions/58524183/how-to-mock-history-push-with-the-new-react-router-hooks-using-jest/59451956
const mockHistoryPush = jest.fn();
jest.mock('react-router-dom', () => ({
  ...jest.requireActual('react-router-dom'),
  useHistory: () => ({
    push: mockHistoryPush,
  }),
}));

jest.mock('../../services/CharacterService');
jest.mock('@auth0/auth0-react');

describe('Characters container', () => {

    beforeEach(() => {
        mockHistoryPush.mockClear();
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

    it('should navigate to the specific character page when the user clicks on the character card', async () => {
        getMyCharacters.mockImplementation(() => {
            return Promise.resolve({
                data: [
                    { id: 31, name: 'Moonshine', level: 1, class: 'Cleric', race: 'Elf'}
                ]
            });
        } );

        await act(async () =>{ 
            await render(<Characters />);     
        });        
        await waitFor(() => {
            const characters = screen.getAllByTestId('character');
            expect(characters.length).toBe(1);
            userEvent.click(characters[0]);
        });

        await waitFor(() => {
            expect(mockHistoryPush).toBeCalledWith('/characters/31');
        });
    });
});