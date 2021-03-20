import React from 'react';
import { shallow } from "enzyme";
import FreeRoller from "./FreeRoller";
import RollResult from '../../components/Roll/RollResult';

describe("Free Roller page", () => {
    it ('should show roll result of 0 on first render', () => {
        const freeRoller = shallow(<FreeRoller />);
        expect(freeRoller.find(RollResult).first().props().roll).toBe(0);
    });

});