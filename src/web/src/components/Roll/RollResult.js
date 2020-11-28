import React from 'react';

const rollResult = props => props.roll > 0 ? <h1>{props.roll}</h1> : <h1>0</h1>;

export default rollResult;