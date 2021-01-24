import API from './dnd_api';

export const getMyCharacters = (token) => {    
    return API.get('/characters', { 
        headers: {Authorization: 'Bearer ' + token}
    });
};