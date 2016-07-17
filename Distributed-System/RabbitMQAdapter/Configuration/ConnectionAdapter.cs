using RabbitMQ.Client;
using RabbitMQ.Client.Exceptions;
using System;

namespace RabbitMQAdapter.Configuration
{
    public class ConnectionAdapter
    {
        private ConnectionFactory _connectionFactory;
        private static IConnection _connection;
        private static ConnectionAdapter instance;

        public ConnectionAdapter(string Host, string Username, string Password)
        {
            _connectionFactory = new ConnectionFactory()
            {
                HostName = Host,
                UserName = Username,
                Password = Password
            };
            try
            {
                _connection = this._connectionFactory.CreateConnection();
            }
            catch (Exception ex)
            {
                if (ex.GetType() == typeof(BrokerUnreachableException))
                {
                    throw new Exception("Not able to establsih a connection with the RabbitMQ");
                }
            }

            instance = this;
        }

        public IModel GetModel()
        {
            ValidateInstanceExists();
            return _connection.CreateModel();
        }

        public IConnection GetConnection()
        {
            ValidateInstanceExists();
            return _connection;
        }

        private void ValidateInstanceExists()
        {
            if (instance == null)
            {
                throw new NullReferenceException("An instance of a RabbitMQ Connection does not exist; please configure the RabbitMQConfiguration object at app startup");
            }
        }
    }
}