import React from 'react';
import { render, screen, cleanup } from "@testing-library/react";

import Home from "./Home"


describe('Home page', () => {
    afterEach(cleanup);

    it('should show a welcome message', () => {
        render(<Home />);
        expect(screen.getByText('Welcome to my D&D app!')).toBeDefined();
    });

    it('should show my die image', () => {
        render(<Home />);
        expect(screen.getByAltText('dice')).toBeDefined();
    });
})