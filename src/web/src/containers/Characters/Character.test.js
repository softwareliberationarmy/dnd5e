import { render, screen, waitFor } from "@testing-library/react";
import { act } from "react-dom/test-utils";

import Character from "./Character";
import { useToken } from "../../hooks/useToken";
import { getCharacter, makeAbilityRoll } from "../../services/CharacterService";
import { abilities } from '../../utilities/abilityConstants';
import userEvent from "@testing-library/user-event";

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

    makeAbilityRoll.mockImplementation((tk, id, ability) => {
      return { data: { result: abilities.indexOf(ability) + 15}};
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

  describe('ability scores', () => {
      it('should show character abilities', async () => {
        for (const ability of abilities) {
          await waitFor(() => {
            expect(screen.getByText(ability)).toBeDefined();
          });          
        }
      });

      it('should allow user to click on an ability to make a roll', async () => {
        for(const ability of abilities){
          await waitFor(() => {
            const abilityButton = screen.getByText(ability);
            userEvent.click(abilityButton);
          });

          await waitFor(() => {
            expect(makeAbilityRoll).toBeCalledWith(expect.anything(), 5, ability);
          });

          await waitFor(() => {
            expect(screen.getByText(`${ability} Roll`)).toBeDefined();
            expect((abilities.indexOf(ability) + 15).toString()).toBeDefined();
          });
        }
      });
  });
});
