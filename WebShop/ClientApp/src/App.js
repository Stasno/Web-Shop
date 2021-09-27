import { BrowserRouter as Router } from 'react-router-dom';
import { Header , Routing} from './components';
import { useState } from 'react';
import AuthService from './Services/AuthService';

import 'bootstrap/dist/css/bootstrap.min.css';
import './styles/App.scss'

function App() {
  const [isAutorized, setIsAutorized] = useState(!!localStorage.key("user"));

  AuthService.stateCallback = (e) => setIsAutorized(e);

  return (
    <Router>
      <div className="theme">
        <Header isAutorized={isAutorized}/>
        <Routing/>
      </div>
    </Router>
  );
}

export default App;
