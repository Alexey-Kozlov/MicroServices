import { Route, Routes } from 'react-router-dom';
import './App.css';
import LoginForm from './identity/LoginForm';
import NewAccountForm from './identity/newAccountForm';

function App() {
  return (
      <Routes>
          <Route path="/login" element={<LoginForm />}></Route>
          <Route path="/newaccount" element={<NewAccountForm />}></Route>
      </Routes>
  );
}

export default App;
