import { Button, Form } from "react-bootstrap";
import { shallow } from "enzyme";
import React from 'react';

import RollRequestor from "./RollRequestor";


describe('RollRequestor', () => {
    it('should return an input form and roll button', () => {
        const target = shallow(<RollRequestor />);
        expect(target.find(Button)).toHaveLength(1);
        expect(target.find(Form.Group)).toHaveLength(1);
        expect(target.find(Form.Group).first().children()).toHaveLength(2);
        expect(target.find(Form.Group).first().children().find(Form.Label)).toHaveLength(1);
        expect(target.find(Form.Group).first().children().find(Form.Control)).toHaveLength(1);
    });

    it('should pass the form value to the function on button click', () => {
        const expected = '2d4+2';
        const mocky = jest.fn();
        const target = shallow(<RollRequestor requested={mocky} />);
        target.find(Form.Group).first().children().find(Form.Control)
            .simulate('change', { target: {value: expected}});
        target.find(Button).simulate('click');
        expect(mocky).toHaveBeenCalledWith(expected);
    });

    it('should also pass the form value to the function on enter key press', () => {
        const expected = "4d4";
        const mocky = jest.fn();
        const target = shallow(<RollRequestor requested={mocky} />);
        target.find(Form.Control).first().simulate('change', {target: {value: expected}});
        target.find(Form.Control).first().simulate('keyPress', { key : 'Enter'});
        expect(mocky).toHaveBeenCalledWith(expected);
    })
});