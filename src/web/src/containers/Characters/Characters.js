import React, { useEffect, useState } from 'react';
import { getMyCharacters } from '../../services/CharacterService';
import { useAuth0 } from '@auth0/auth0-react';

const Characters = () => {
    const [characterData, setCharacterData] = useState([]);

    const { getAccessTokenSilently } = useAuth0();    

    useEffect(() => {
        async function fetchData(){
            const token = await getAccessTokenSilently({
                audience: 'https://localhost:5001/api',
                scope: 'read:characters'
            });
            console.log('token', token);
            const response = await getMyCharacters(token);
            setCharacterData(response.data);    
        }
        fetchData();
    }, [getAccessTokenSilently]);

    return (
        <>
        <h1>Characters!</h1>
        {/* <p>{characterData}</p> */}
        </>
    );
};

export default Characters;
