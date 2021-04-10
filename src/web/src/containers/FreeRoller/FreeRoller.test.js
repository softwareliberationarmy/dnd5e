import React from 'react';
import { shallow } from "enzyme";
import FreeRoller, { reducer, makeRoll } from "./FreeRoller";
import RollResult from '../../components/Roll/RollResult';
import ErrorMessage from '../../components/Error/ErrorMessage';
import RollRequestor from '../../components/Roll/RollRequestor';
import RollService from '../../services/RollService';

jest.mock('../../services/RollService');

describe("Free Roller component", () => {
    it ('should show default values on first render', () => {
        const freeRoller = shallow(<FreeRoller />);
        expect(freeRoller.find(RollResult).first().props().roll).toBeNull();
        expect(freeRoller.find(ErrorMessage).first().props().error).toBeNull();
        expect(freeRoller.find(RollRequestor).first().props().requested).toBeTruthy();
    });
});

describe('Free Roller reducer', () => {
    it('should set loading to true for load', () => {
        const state = reducer({}, {type: 'loading'});
        expect(state.loading).toBe(true);
    });    

    it('should set loading to false on success', () => {
        const state = reducer({}, {type: 'success', payload: 12});
        expect(state.loading).toBe(false);
    });

    it('should set loading to false on error', () => {
        const state = reducer({}, {type: 'error', payload: Error('something went wrong')})
        expect(state.loading).toBe(false);
    });

    it('should set roll on success', () => {
        const state = reducer({}, {type: 'success', payload: 12});
        expect(state.roll).toBe(12);
    });

    it('should set roll to null on load', () => {
        const state = reducer({ roll: 15}, {type: 'loading'});
        expect(state.roll).toBeNull();
    });

    it('should set roll to null on error', () => {
        const state = reducer({ roll: 15}, {type: 'error', payload: Error('timeout')});
        expect(state.roll).toBeNull();
    });

    it('should set error to null on load', () => {
        const state = reducer({ error: Error('something')}, { type: 'loading'});
        expect(state.error).toBeNull();
    });

    it('should set error to null on success', () => {
        const state = reducer({ error: Error('old error')}, {type: 'success', payload: 15});
        expect(state.error).toBeNull();
    });

    it('should set error on error', () => {
        const state = reducer({}, {type: 'error', payload: Error('some error')});
        expect(state.error).toEqual(Error('some error'))
    });
});

describe('FreeRoller makeRoll logic', () => {
    let dispatch;

    beforeEach(() => {
        dispatch = jest.fn();
    });

    it('should not dispatch anything if no rollType', () => {
        makeRoll("", dispatch);
        expect(dispatch).not.toHaveBeenCalled();
     });

     it('should always dispatch loading first', () => {
        RollService.rollDice.mockImplementation(() =>  Promise.resolve({ data: { result: 20}}));
        makeRoll("1d20", dispatch);
         expect(dispatch).toHaveBeenCalledWith({type: 'loading'});
     });

     it('should dispatch success with the roll result when successful call', async () => {
         RollService.rollDice.mockImplementation(() =>  Promise.resolve({ data: { result: 20}}));
         await makeRoll('1d20', dispatch);   
         expect(dispatch).toHaveBeenCalledTimes(2);
         expect(dispatch).toHaveBeenCalledWith({type: 'loading'});
         expect(dispatch).toHaveBeenCalledWith({type: 'success', payload: 20});
     });

     
})