import React, {Component} from 'react';

import FreeRoller from './containers/FreeRoller/FreeRoller';

import './App.css';
import 'bootstrap/dist/css/bootstrap.min.css';

class App extends Component {

  render() {
    return (
    <div className="App">
      <FreeRoller />
    </div>
    );
  }
}

export default App;
