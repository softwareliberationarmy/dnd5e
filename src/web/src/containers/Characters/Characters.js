import React, { useEffect, useState } from 'react';
import { Card, Container, Row, Col } from 'react-bootstrap';
import { useAuth0 } from '@auth0/auth0-react';

import { useToken } from '../../hooks/useToken';
import { getMyCharacters } from '../../services/CharacterService';

const Characters = () => {

    const response = useToken(getMyCharacters);

    console.log(response);

    return (
        <>
        <h1>Characters</h1>
        <Container className="m-5">
            <Row>
            { (response && response.data) ? response.data.map(c => 
                (
                <Card as={Col} data-testid="character" md="3" className="m-1" text="dark"  key={c.id}>
                    <Card.Body>
                        <Card.Title>{c.name}</Card.Title>                
                        <Card.Subtitle>Level {c.level} {c.class} ({c.race})</Card.Subtitle>
                    </Card.Body>
                </Card>
                )
            ) : null} 
            </Row>
        </Container>
        </>
    );
};

export default Characters;
