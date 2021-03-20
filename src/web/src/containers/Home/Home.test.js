import React from 'react';
import { shallow } from "enzyme"
import Home from "./Home"

describe('Home page', () => {
    let homePage;
    beforeEach(() => {
        homePage = shallow(<Home />);
    })

    it('should show a welcome message', () => {
        expect(homePage.find('h1').first().text())
        .toBe("Welcome to my D&D app!");
    });

    it('should show my die image', () => {
        expect(homePage.find('img[alt="dice"]').length).toBe(1);
    });
})