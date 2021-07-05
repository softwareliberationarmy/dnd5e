import React, { useMemo } from 'react';

import { useToken } from '../../hooks/useToken';
import { getCharacter } from '../../services/CharacterService';

const Character = ({id}) => {
    const getCharacterFunc = useMemo(() => { return (tk) => getCharacter(tk, id)}, [id]);
    const response = useToken(getCharacterFunc);
    if(response && response.data){
        const character = response.data;
        return (
            <>
                <h1>{ character.name }</h1>
                <h3>{ `Level ${character.level} ${character.class} (${character.race})` }</h3>
            </>
            );
    }
    
    return null;
};

export default Character;