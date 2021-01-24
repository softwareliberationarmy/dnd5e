import React, {Component} from 'react';
import { Router, Route, Switch } from 'react-router-dom';
import { Auth0Provider } from '@auth0/auth0-react';

import FreeRoller from './containers/FreeRoller/FreeRoller';
import Home from './containers/Home/Home';
import Characters from './containers/Characters/Characters';
import PrivateRoute from './components/Auth/PrivateRoute';

import './App.css';
import 'bootstrap/dist/css/bootstrap.min.css';
import NavMenu from './components/Nav/NavMenu';
import { createBrowserHistory } from 'history';

export const history = createBrowserHistory();

const onRedirectCallback = (appState) => {
  history.replace(appState?.returnTo || window.location.pathname);
};

class App extends Component {
  render() {
    return (
      <Auth0Provider 
        domain={this.props.authSettings.data.domain}
        clientId={this.props.authSettings.data.clientId}
        redirectUri={window.location.origin} 
        onRedirectCallback={onRedirectCallback}
        audience={this.props.authSettings.data.audience}
        scope="read:characters" >
        <Router history={history} >
          <div className="App">
            <NavMenu />
            <Switch>
              <Route path="/" exact component={ Home } />
              <Route path="/roll" component={ FreeRoller } />          
              <PrivateRoute path="/characters" component={ Characters } />
            </Switch>
          </div>
      </Router>
    </Auth0Provider>
    );
  }
}

export default App;
