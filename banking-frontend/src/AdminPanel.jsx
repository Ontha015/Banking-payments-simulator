import { useState, useEffect } from 'react';

function AdminPanel({ token }) {
  const [flagged, setFlagged] = useState([]);
  const [message, setMessage] = useState('');

  const fetchFlagged = () => {
    fetch('https://localhost:7263/api/transactions/flagged', {
      headers: { Authorization: `Bearer ${token}` },
    })
      .then((res) => res.json())
      .then(setFlagged)
      .catch((err) => setMessage(`Error: ${err.message}`));
  };

  useEffect(() => {
    fetchFlagged();
  }, []);

  const handleReview = async (id, decision) => {
    try {
      const res = await fetch(
        `https://localhost:7263/api/transactions/${id}/review?decision=${decision}`,
        {
          method: 'PUT',
          headers: { Authorization: `Bearer ${token}` },
        }
      );

      if (!res.ok) throw new Error('Review action failed');

      setMessage(`Transaction ${id} ${decision}d.`);
      fetchFlagged();
    } catch (err) {
      setMessage(`Error: ${err.message}`);
    }
  };

  return (
    <div>
      <h2>Flagged Transactions</h2>
      {message && <p>{message}</p>}
      {flagged.length === 0 ? (
        <p>No flagged transactions.</p>
      ) : (
        <table border="1" cellPadding="8" style={{ borderCollapse: 'collapse' }}>
          <thead>
            <tr>
              <th>ID</th>
              <th>Type</th>
              <th>Amount</th>
              <th>Account ID</th>
              <th>Timestamp</th>
              <th>Actions</th>
            </tr>
          </thead>
          <tbody>
            {flagged.map((t) => (
              <tr key={t.id}>
                <td>{t.id}</td>
                <td>{t.type}</td>
                <td>{t.amount}</td>
                <td>{t.accountId}</td>
                <td>{new Date(t.timestamp).toLocaleString()}</td>
                <td>
                  <button onClick={() => handleReview(t.id, 'Approve')}>
                    Approve
                  </button>{' '}
                  <button onClick={() => handleReview(t.id, 'Block')}>
                    Block
                  </button>
                </td>
              </tr>
            ))}
          </tbody>
        </table>
      )}
    </div>
  );
}

export default AdminPanel;