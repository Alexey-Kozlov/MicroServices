import { Route, Routes, useNavigate } from 'react-router-dom';
import './App.css';
import LoginForm from './identity/LoginForm';

function App() {
  return (
      <Routes>
          <Route path="/login" element={<LoginForm />}></Route>
      </Routes>
  );
}

export default App;
