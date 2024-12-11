namespace TPBlog.Api.SignalR
{
    public class ConnectionMapping<T>
    {
        private readonly Dictionary<T, HashSet<string>> _connections = new Dictionary<T, HashSet<string>>();
        public int count
        {
            get
            {
                return _connections.Count;
            }

        }
        public void Add(T key, string connectionId)
        {
            lock (_connections)
            {
                if (!_connections.TryGetValue(key, out var connections))
                {
                    connections = new HashSet<string>();
                    _connections.Add(key, connections);
                }

                lock (connections)
                {
                    connections.Add(connectionId);
                }
            }
        }

        public IEnumerable<string> GetConnections(T key)
        {
            if (_connections.ContainsKey(key))
            {
                return _connections[key];
            }
            return Enumerable.Empty<string>();
        }
        // Thêm phương thức Where để lọc các kết nối theo điều kiện
        public IEnumerable<string> Where(Func<string, bool> predicate)
        {
            foreach (var connections in _connections.Values)
            {
                foreach (var connectionId in connections)
                {
                    if (predicate(connectionId))
                    {
                        yield return connectionId;
                    }
                }
            }
        }
        public IEnumerable<T> GetAllKeys()
        {
            return _connections.Keys;
        }
        public void Remove(T key, string connectionId)
        {
            lock (_connections)
            {
                HashSet<string> connections;
                if (!_connections.TryGetValue(key, out connections))
                {
                    return;
                }
                lock (connections)
                {
                    connections.Remove(connectionId);
                    if (connections.Count == 0)
                    {
                        _connections.Remove(key);
                    }
                }

            }


        }
    }
}
