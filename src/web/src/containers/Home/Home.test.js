import React from 'react';
import { shallow } from "enzyme"
import Home from "./Home"

describe('Home page', () => {
    it('should show a welcome message', () => {
        const homePage = shallow(<Home />);
        const welcomeMsg = homePage.find('h1').first();
        console.log(welcomeMsg.text);
        expect(welcomeMsg.text()).toBe("Welcome to my D&D app!");
    })
})