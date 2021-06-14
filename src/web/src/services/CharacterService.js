import API from './dnd_api';

export const getMyCharacters = (token) => {    
    return API.get('/characters', { 
        headers: {Authorization: 'Bearer ' + token}
    });
};

export const getCharacter = (token, id) => {    
    return API.get(`/character/${id}`, { 
        headers: {Authorization: 'Bearer ' + token}
    });
};