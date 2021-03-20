import React from 'react';
import { shallow } from "enzyme";
import FreeRoller from "./FreeRoller";
import RollResult from '../../components/Roll/RollResult';
import ErrorMessage from '../../components/Error/ErrorMessage';

describe("Free Roller page", () => {
    it ('should show default values on first render', () => {
        const freeRoller = shallow(<FreeRoller />);
        expect(freeRoller.find(RollResult).first().props().roll).toBe(0);
        expect(freeRoller.find(ErrorMessage).first().props().error).toBeNull();
    });

});