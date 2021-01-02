import React, {Component} from 'react';
import { BrowserRouter, Route, Switch } from 'react-router-dom';

import FreeRoller from './containers/FreeRoller/FreeRoller';
import Home from './containers/Home/Home';

import './App.css';
import 'bootstrap/dist/css/bootstrap.min.css';
import Nav from './components/Nav/Nav';

class App extends Component {
  render() {
    return (
      <BrowserRouter>
        <div className="App">
          <Nav />
          <Switch>
            <Route path="/" exact component={ Home } />
            <Route path="/roll" component={ FreeRoller } />          
          </Switch>
        </div>
    </BrowserRouter>
    );
  }
}

export default App;
