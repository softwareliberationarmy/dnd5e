import React, {Component} from 'react';
import { BrowserRouter, Route, Switch } from 'react-router-dom';

import FreeRoller from './containers/FreeRoller/FreeRoller';
import Home from './containers/Home/Home';
import Characters from './containers/Characters/Characters';

import './App.css';
import 'bootstrap/dist/css/bootstrap.min.css';
import NavMenu from './components/Nav/NavMenu';

class App extends Component {
  render() {
    return (
      <BrowserRouter>
        <div className="App">
          <NavMenu />
          <Switch>
            <Route path="/" exact component={ Home } />
            <Route path="/roll" component={ FreeRoller } />          
            <Route path="/characters" component={ Characters } />
          </Switch>
        </div>
    </BrowserRouter>
    );
  }
}

export default App;
