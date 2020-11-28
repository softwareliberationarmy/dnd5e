import React, {useState } from 'react';
import {Form, Button} from 'react-bootstrap';


const RollRequestor = props => {
    const [rollType, setRollType] = useState('');

    const handleEnter = event => {
        if(event.key === 'Enter'){
             props.requested(rollType);
            }
      }

    return (<>
            <Form.Group className="d-flex d-inline" controlId="frmRoll">
                <Form.Label className="mr-3">Roll</Form.Label>
                <Form.Control as="input" name="rollType"
                    onChange={event => setRollType(event.target.value)} value={ rollType }
                    onKeyPress={handleEnter} />
            </Form.Group>
            <Button size="lg" onClick={() => props.requested(rollType)} >Roll</Button>
        </>);
}

export default RollRequestor;