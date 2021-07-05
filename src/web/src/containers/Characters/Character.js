import React, { useMemo, useState } from "react";

import { useToken } from "../../hooks/useToken";
import { getCharacter, makeAbilityRoll } from "../../services/CharacterService";
import { abilities } from "../../utilities/abilityConstants";

const Character = ({ id }) => {
  const [selectedAbility, setSelectedAbility] = useState(null);

  const getCharacterFunc = useMemo(() => {
    return (tk) => getCharacter(tk, id);
  }, [id]);
  const makeRollFunc = useMemo(() => {
    return (tk) =>
      selectedAbility ? makeAbilityRoll(tk, id, selectedAbility) : null;
  }, [id,selectedAbility]);

  const response = useToken(getCharacterFunc);
  const abilityRoll = useToken(makeRollFunc);

  if (response && response.data) {
    const character = response.data;
    return (
      <>
        <h1>{character.name}</h1>
        <h3>{`Level ${character.level} ${character.class} (${character.race})`}</h3>

        {abilities.map((ab) => (
          <button key={ab} onClick={() => setSelectedAbility(ab)}>
            {ab}
          </button>
        ))}

        { abilityRoll ? (
            <>
            <h3>{selectedAbility} Roll</h3>
            <h4>{abilityRoll.data.result}</h4>
            </>
        ) : null}
      </>
    );
  }

  return null;
};

export default Character;
