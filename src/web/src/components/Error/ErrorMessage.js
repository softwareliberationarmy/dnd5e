import React from 'react';
import {Form} from 'react-bootstrap';

const errorMessage = props => props.error ? <Form.Label>{props.error}</Form.Label> : null;

export default errorMessage;