import API from './dnd_api';

export const getAuthSettings = () => {
    return API.get('auth');
}