import React, {Component} from 'react';
import API from '../../services/api';
import {Button, Form} from 'react-bootstrap';
import RollResult from '../../components/Roll/RollResult';

class FreeRoller extends Component {
    constructor(props){
        super(props);
        this.state = {
            roll: 0,
            rollType: null,
            rollError: null
          };
          
          this.rollButtonHandler = this.rollButtonHandler.bind(this);
          this.handleChange = this.handleChange.bind(this);      
    }

    rollButtonHandler(){
        if(this.state.rollType){
          API.get('roll/' + this.state.rollType)
          .then(result => {
            console.log('api',result);
            const rollResult = result.data;
            this.setState({ roll: rollResult.result, rollError: null});
          })
          .catch(err => {
            console.log('kp-error', err.response);
            if(err.response && err.response.status === 400){
              //bad request
              this.setState({ rollError: 'You entered an invalid roll request.'})
            }
            else{
              this.setState({rollError: 'Error rolling dice. Please try again.'})
            }
          });  
        }
      }
    
      handleChange(event){
        const partialState = {};
        partialState[event.target.name] = event.target.value;
        this.setState(partialState);
      }
    
      handleEnter(event, onEnter){
        if(event.key === 'Enter'){
          onEnter();
        }
      }
    
      render(){
        return (
          <div className="App">
            <header className="App-header">
              <RollResult roll={this.state.roll} />
              <p>
                Roll your fate
              </p>
              {this.state.rollError ? <Form.Label>{this.state.rollError}</Form.Label> : null}          
              <Form.Group className="d-flex d-inline" controlId="frmRoll">
                            <Form.Label className="mr-3">Roll</Form.Label>
                            <Form.Control as="input" name="rollType"
                                onChange={this.handleChange} value={this.state.rollType}
                                onKeyPress={e => this.handleEnter(e,this.rollButtonHandler)} />
              </Form.Group>
              <Button size="lg" onClick={this.rollButtonHandler} >Roll</Button>
            </header>
          </div>
        );  
      }    
}

export default FreeRoller;