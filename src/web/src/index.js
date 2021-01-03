import React from 'react';
import ReactDOM from 'react-dom';
import './index.css';
import App from './App';
import * as serviceWorker from './serviceWorker';
import { Auth0Provider } from '@auth0/auth0-react';
import {getAuthSettings} from './services/AuthService';

getAuthSettings()
  .then(settings => {
    ReactDOM.render(
      <React.StrictMode>
        <Auth0Provider domain={settings.data.domain}
                      clientId={settings.data.clientId}
                      redirectUri={window.location.origin} >
          <App />
        </Auth0Provider>
      </React.StrictMode>,
      document.getElementById('root')
    );  
  })
  .catch(error => {
    console.log(error);
  })

// If you want your app to work offline and load faster, you can change
// unregister() to register() below. Note this comes with some pitfalls.
// Learn more about service workers: https://bit.ly/CRA-PWA
serviceWorker.unregister();
