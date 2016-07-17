using MessageSerializer;
using RabbitMQ.Client;
using RabbitMQAdapter;
using RabbitMQAdapter.Configuration;
using System;
using System.Windows.Forms;

namespace ClientApp
{
    public partial class Form1 : Form
    {
        private Sender _sender;
        private IModel _model;

        public Form1()
        {
            InitializeComponent();
            try
            {
                _model = new ConnectionAdapter(DemoConnection.HOST, DemoConnection.USER, DemoConnection.PW).GetModel();
                _sender = new Sender(_model, "OrderEngine", "");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Unable to connect with the system");
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            cmbxProduct.Items.Add("Product 1");
            cmbxProduct.Items.Add("Product 2");
            cmbxPaymentAccount.Items.Add("Cash");
            cmbxPaymentAccount.Items.Add("Credit Card");
        }

        private void btnOrder_Click(object sender, EventArgs e)
        {
            bool valid = ValidateForm();
            if (valid)
            {
                SubmitOrder();
            }
        }

        private bool ValidateForm()
        {
            bool result = false;
            if (cmbxPaymentAccount.SelectedItem == null || cmbxProduct.SelectedItem == null)
            {
                MessageBox.Show("Make sure to enter a product and payment");
            }
            else
            {
                result = true;
            }
            return result;
        }

        private void SubmitOrder()
        {
            var msg = new OrderRequest()
            {
                Customer = "Bill Bensing",
                Product = cmbxProduct.SelectedItem.ToString(),
                Payment = cmbxPaymentAccount.SelectedItem.ToString(),
            };
            byte[] buffer = new Serializer().UseObject(msg).SerializeAs(MessageType.JSON);

            _sender.Send(buffer);
        }
    }

    public class OrderRequest
    {
        public string Customer { get; set; }
        public string Product { get; set; }
        public string Payment { get; set; }
    }
}