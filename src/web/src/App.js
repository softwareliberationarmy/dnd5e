import React, {Component} from 'react';
import axios from 'axios';
import {Button} from 'react-bootstrap';

import logo from './logo.svg';
import './App.css';
import 'bootstrap/dist/css/bootstrap.min.css';

class App extends Component {
  constructor(props){
    super(props);
    this.state = {
      roll: 0
    };
    
    this.rollButtonHandler = this.rollButtonHandler.bind(this);
  }

  componentDidMount(){

  }

  rollButtonHandler(){
    axios.get('https://localhost:5001/api/roll/2d4p2')
    .then(result => {
      const rollResult = result.data;
      this.setState({ roll: rollResult.result});
    });
  }

  render(){
    console.log('roll',this.state.roll);
    const rollResult = this.state.roll > 0 ? 
      <h1>{this.state.roll}</h1> :
      <img src={logo} className="App-logo" alt="logo" />;

    return (
      <div className="App">
        <header className="App-header">
          {rollResult}
          <p>
            Roll your fate
          </p>
          <Button size="lg" onClick={this.rollButtonHandler} >Roll</Button>
        </header>
      </div>
    );  
  }
}

export default App;
