import { useAuth0 } from '@auth0/auth0-react';
import { useEffect, useState } from 'react';

const defaultOptions = {
    audience: 'https://localhost:5001/api',
    scope: 'read:characters'
};

export function useToken(tokenMethod, options) {

    const { getAccessTokenSilently } = useAuth0();    
    const [response, setResponse] = useState({});

    useEffect(() => {
        async function fetchData(){
            const token = await getAccessTokenSilently({...defaultOptions, ...options});
            const resp = await tokenMethod(token);
            setResponse(resp);
        };
        fetchData();
    }, [tokenMethod, options]);

    return response;
}

