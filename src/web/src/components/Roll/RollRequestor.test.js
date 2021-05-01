import React from 'react';
import userEvent from '@testing-library/user-event';

import RollRequestor from "./RollRequestor";
import { render, screen, cleanup } from "@testing-library/react";


describe('RollRequestor', () => {
    afterEach(cleanup);

    it('should return an input form and roll button', () => {
        render(<RollRequestor />);
        const inputTextbox = screen.getByLabelText('Roll');
        expect(inputTextbox).toBeDefined();
        expect(inputTextbox.textContext).toBeUndefined();
        expect(screen.getByRole('button').textContent).toBe('Roll');
    });

    it('should pass the form value to the function on button click', () => {
        const expected = '2d4+2';
        const mocky = jest.fn();
        render(<RollRequestor requested={mocky} />);
        userEvent.type(screen.getByLabelText('Roll'), expected);
        userEvent.click(screen.getByRole('button'));
        expect(mocky).toHaveBeenCalledWith(expected);
    });

    it('should also pass the form value to the function on enter key press', () => {
        const expected = "4d4{Enter}";
        const mocky = jest.fn();
        render(<RollRequestor requested={mocky} />);
        userEvent.type(screen.getByLabelText('Roll'), expected);
        expect(mocky).toHaveBeenCalledWith("4d4");
    })
});