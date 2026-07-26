import { useState, useEffect } from 'react';
import UserForm from './UserForm';
import Login from './Login';
import AdminPanel from './AdminPanel';
import './App.css';

function App() {
  const [users, setUsers] = useState([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState(null);
  const [token, setToken] = useState(null);

  const fetchUsers = () => {
    setLoading(true);
    fetch('https://localhost:7263/api/users')
      .then((response) => {
        if (!response.ok) throw new Error('Failed to fetch users');
        return response.json();
      })
      .then((data) => {
        setUsers(data);
        setLoading(false);
      })
      .catch((err) => {
        setError(err.message);
        setLoading(false);
      });
  };

  useEffect(() => {
    fetchUsers();
  }, []);

  return (
    <div className="app">
      <div className="app-header">
        <h1>Banking Simulator</h1>
        <span className="tag">Ledger v1.0</span>
      </div>

      <div className="panel">
        <span className="panel-eyebrow">Registration</span>
        <UserForm onUserCreated={fetchUsers} />
      </div>

      <div className="panel">
        <span className="panel-eyebrow">Directory</span>
        <h2>Users</h2>
        {loading && <p className="empty-state">Loading users…</p>}
        {error && <p className="message error">{error}</p>}
        {!loading && !error && (
          users.length === 0 ? (
            <p className="empty-state">No users yet.</p>
          ) : (
            <ul className="user-list">
              {users.map((user) => (
                <li key={user.id}>
                  <span>{user.fullName}</span>
                  <span className="email">{user.email}</span>
                </li>
              ))}
            </ul>
          )
        )}
      </div>

      <div className="panel">
        <span className="panel-eyebrow">Fraud Review</span>
        {token ? <AdminPanel token={token} /> : <Login onLogin={setToken} />}
      </div>
    </div>
  );
}

export default App;