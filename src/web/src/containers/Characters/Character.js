import React from 'react';

import { useToken } from '../../hooks/useToken';
import { getCharacter } from '../../services/CharacterService';

const Character = ({id}) => {

    const response = useToken(tk => getCharacter(tk, id));
    
    return (
    <>
        <h1>{ response ? response.data.name : null}</h1>
    </>
    );
};

export default Character;