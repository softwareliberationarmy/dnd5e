import React from 'react';
import ReactDOM from 'react-dom';
import './index.css';
import App from './App';
import * as serviceWorker from './serviceWorker';
import {getAuthSettings} from './services/AuthService';

getAuthSettings()
  .then(settings => {
    ReactDOM.render(
      <React.StrictMode>
          <App authSettings={settings} />
      </React.StrictMode>,
      document.getElementById('root')
    );  
  })
  .catch(error => {
    console.log('Error encountered building app', error);
  })

// If you want your app to work offline and load faster, you can change
// unregister() to register() below. Note this comes with some pitfalls.
// Learn more about service workers: https://bit.ly/CRA-PWA
serviceWorker.unregister();
