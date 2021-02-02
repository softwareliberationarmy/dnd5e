import React, { useEffect, useState } from 'react';
import { Card, Container, Row, Col } from 'react-bootstrap';
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
            const response = await getMyCharacters(token);
            setCharacterData(response.data);    
        }
        fetchData();
    }, [getAccessTokenSilently]);

    return (
        <>
        <h1>Characters</h1>
        <Container className="m-5">
            <Row>
            {characterData.map(c => 
            (
            <Card as={Col} md="3" className="m-1" text="dark"  key={c.id}>
                <Card.Body>
                    <Card.Title>{c.name}</Card.Title>                
                    <Card.Subtitle>Level {c.level} {c.class} ({c.race})</Card.Subtitle>
                </Card.Body>
            </Card>
            )
        )} 
            </Row>
        </Container>
        </>
    );
};

export default Characters;
