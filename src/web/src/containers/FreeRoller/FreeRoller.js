import React, {useState} from 'react';
import RollResult from '../../components/Roll/RollResult';
import ErrorMessage from '../../components/Error/ErrorMessage';
import RollRequestor from '../../components/Roll/RollRequestor';
import RollService from '../../services/RollService';

const FreeRoller = props => {

  const [roll, setRoll] = useState({ result: 0, error: null});

  const makeRoll = rollType => {
        if(rollType){
          RollService.rollDice(rollType)
            .then(result => {
              const rollResult = result.data;
              setRoll({ result: rollResult.result, error: null});
            })
            .catch(err => {
              if(err.response && err.response.status === 400){
                //bad request
                setRoll({ result: 0, error: 'You entered an invalid roll request.'})
              }
              else{
                setRoll({ result: 0, error: 'Error rolling dice. Please try again.'})
              }
            });  
          }
        }
    
      return (
          <div className="Page-header">
            <h1>Roll your fate</h1>
            <RollResult roll={roll.result} />
            <ErrorMessage error={roll.error} />              
            <RollRequestor requested={makeRoll} />
          </div>
      );  
};

export default FreeRoller;