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
    <div style={{ padding: '2rem', fontFamily: 'sans-serif' }}>
      <h1>Banking Simulator</h1>

      <UserForm onUserCreated={fetchUsers} />

      <h2>Users</h2>
      {loading && <p>Loading users...</p>}
      {error && <p>Error: {error}</p>}
      {!loading && !error && (
        users.length === 0 ? (
          <p>No users found.</p>
        ) : (
          <ul>
            {users.map((user) => (
              <li key={user.id}>
                {user.fullName} — {user.email}
              </li>
            ))}
          </ul>
        )
      )}

      <hr style={{ margin: '2rem 0' }} />

      {token ? (
        <AdminPanel token={token} />
      ) : (
        <Login onLogin={setToken} />
      )}
    </div>
  );
}

export default App;