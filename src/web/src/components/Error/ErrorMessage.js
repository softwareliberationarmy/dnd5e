import React from 'react';
import {Alert} from 'react-bootstrap';

const errorMessage = props => 
    props.error ? 
    <Alert variant="danger">{props.error}</Alert> : 
    null;

export default errorMessage;