import { render, screen, waitFor } from "@testing-library/react";
import { act } from "react-dom/test-utils";

import Character from "./Character";
import { useToken } from "../../hooks/useToken";
import { getCharacter } from "../../services/CharacterService";

jest.mock("../../hooks/useToken");
jest.mock("../../services/CharacterService");

const character = {
  name: "Zook Dangleripple",
  level: 1,
  class: "Fighter",
  race: "Gnome",
};

describe("Character page", () => {
  beforeEach(async () => {
    useToken.mockImplementation((func) => {
      return func({});
    });

    getCharacter.mockImplementation(() => {
      return { data: character };
    });

    await act(async () => {
      await render(<Character id={5} />);
    });
  });

  describe("basic information", () => {
    it("should show the character name", async () => {
      await waitFor(() => {
        expect(screen.getByText(character.name)).toBeDefined();
      });
    });

    it("should show the character class, level, and race", async () => {
      await waitFor(() => {
        expect(screen.getByText("Level 1 Fighter (Gnome)")).toBeDefined();
      });
    });
  });

  // describe('ability scores', () => {
  //     it('should show character abilities', () => {

  //     });
  // });

  //should show character abilities
  //should allow user to click on an ability to make a roll
  //should show the roll result when clicking an ability
});
