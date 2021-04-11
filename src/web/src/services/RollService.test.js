import RollService from './RollService';
import API from './dnd_api';

jest.mock('./dnd_api');

describe('Roll Service simple dice roll', () => {
    const testRollRequestMutation = async (input, expected) => {
        API.get.mockImplementation(() => Promise.resolve({ data: { result: 15}}));
        await RollService.rollDice(input);
        expect(API.get).toHaveBeenCalledWith(expected);
    }

    it('should replace + with p', async () => {
        await testRollRequestMutation('2d4+2', 'roll/2d4p2');
    });

    it('should replace - with m', async () => {
        await testRollRequestMutation('1d20-1', 'roll/1d20m1');
    });
});