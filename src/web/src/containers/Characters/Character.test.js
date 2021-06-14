import { render, screen, waitFor } from "@testing-library/react";
import { act } from 'react-dom/test-utils';

import Character from './Character';
import { useToken } from '../../hooks/useToken';

jest.mock('../../hooks/useToken');

const character = {
    name: 'Zook Dangleripple'            
};

describe('Character page', () => {

    beforeEach(async () => {
      useToken.mockImplementation(() => {
          return { data: character };
      });

      await act(async () => {
        await render(<Character id={5} />);
    }); 



    });

    describe('basic information', () => {

        it('should show the character name', async () => {
            await waitFor(() => {
                expect(screen.getByText(character.name)).toBeDefined();
            });
        });

    //should show the character class
    //should show the character level
    //should show the character race
    });



    //should show character abilities
    //should allow user to click on an ability to make a roll
    //should show the roll result when clicking an ability


});