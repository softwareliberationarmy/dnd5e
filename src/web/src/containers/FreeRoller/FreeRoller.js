import React, {Component} from 'react';
import RollResult from '../../components/Roll/RollResult';
import ErrorMessage from '../../components/Error/ErrorMessage';
import RollRequestor from '../../components/Roll/RollRequestor';
import RollService from '../../services/RollService';

class FreeRoller extends Component {
    constructor(props){
        super(props);
        this.state = {
            roll: 0,
            rollType: null,
            rollError: null
          };
          
          this.makeRoll = this.makeRoll.bind(this);
    }

      makeRoll(rollType){
        if(rollType){
          RollService.rollDice(rollType)
            .then(result => {
              const rollResult = result.data;
              this.setState({ roll: rollResult.result, rollError: null});
            })
            .catch(err => {
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
    
      render(){
        return (
            <div className="Page-header">
              <h1>Roll your fate</h1>
              <RollResult roll={this.state.roll} />
              <ErrorMessage error={this.state.rollError} />              
              <RollRequestor requested={this.makeRoll} />
            </div>
        );  
      }    
}

export default FreeRoller;