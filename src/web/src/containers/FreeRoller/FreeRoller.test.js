import React from 'react';
import { shallow } from "enzyme";
import FreeRoller, { reducer } from "./FreeRoller";
import RollResult from '../../components/Roll/RollResult';
import ErrorMessage from '../../components/Error/ErrorMessage';

describe("Free Roller page", () => {
    it ('should show default values on first render', () => {
        const freeRoller = shallow(<FreeRoller />);
        expect(freeRoller.find(RollResult).first().props().roll).toBe(0);
        expect(freeRoller.find(ErrorMessage).first().props().error).toBeNull();
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