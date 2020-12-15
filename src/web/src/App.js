import React, {Component} from 'react';
import { BrowserRouter, Route, NavLink } from 'react-router-dom';

import FreeRoller from './containers/FreeRoller/FreeRoller';
import Home from './containers/Home/Home';
import Profile from './components/Auth/Profile';

import './App.css';
import 'bootstrap/dist/css/bootstrap.min.css';
import LoginButton from './components/Auth/Login';

class App extends Component {

  render() {
    return (
      <BrowserRouter>
        <div className="App">
          <header>
            <nav>
              <ul>
                <li><NavLink to="/" exact>Home</NavLink></li>
                <li><NavLink to="/roll">Free Roll</NavLink></li>
                <LoginButton></LoginButton>
                <Profile></Profile>
              </ul>
            </nav>
          </header>
          <Route path="/" exact component={ Home } />
          <Route path="/roll" component={ FreeRoller } />          
        </div>
    </BrowserRouter>
    );
  }
}

export default App;
