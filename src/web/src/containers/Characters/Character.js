import React from 'react';

import { useToken } from '../../hooks/useToken';
import { getCharacter } from '../../services/CharacterService';

const Character = ({id}) => {

    const response = useToken(tk => getCharacter(tk, id));
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