using RabbitMQ.Client;
using System;
using System.Collections.Generic;

namespace RabbitMQAdapter
{
    public class Sender : ISender
    {
        public string Queue { get; private set; }
        public string Exchange { get; private set; }
        public Dictionary<string, string> Headers { get; private set; }
        private IModel _model;
        private IBasicProperties _basicProperties;

        public Sender(IModel Model, string Queue, string Exchange)
        {
            this.Queue = Queue;
            this.Exchange = Exchange;
            this._model = Model;
            this.Headers = new Dictionary<string, string>();

            this._basicProperties = this._model.CreateBasicProperties();
            this.ConfigureBasicProperties();
        }

        public void Send(byte[] MessageBuffer)
        {
            try
            {
                ConfigureMessageHeaders();
                this._model.BasicPublish(this.Exchange, this.Queue, this._basicProperties, MessageBuffer);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Was unable to send message.\n{0}", ex.ToString());
            }
        }

        public void AddHeader(string key, string value)
        {
            try
            {
                if (this.Headers.ContainsKey(key))
                {
                    this.Headers[key] = value;
                }
                else
                {
                    this.Headers.Add(key, value);
                }
            }
            catch (Exception ex)
            {
                throw new ArgumentNullException(string.Format("Cannot add header, an argument is null.  Key: {0} | Value: {1} \n\n", key, value), ex.ToString());
            }
        }

        public virtual void ConfigureBasicProperties()
        {
            this._basicProperties.Persistent = true;
        }

        private void ConfigureMessageHeaders()
        {
            if (this.Headers.Count > 0)
            {
                PopulateBasicPropertyHeaders();
            }
        }

        private void PopulateBasicPropertyHeaders()
        {
            this._basicProperties.Headers = new Dictionary<string, object>();
            foreach (var h in this.Headers)
            {
                var obj = (object)h.Value;
                this._basicProperties.Headers.Add(h.Key, obj);
            }
        }
    }
}