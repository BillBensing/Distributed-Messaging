using OrderEngine.Component.DeliveryMgt.Service;
using OrderEngine.Component.InventoryMgt.Service;
using OrderEngine.Component.OrderMgt.Service;
using OrderEngine.Component.OrderMgt.Service.OrderHandler;
using OrderEngine.Component.Payment.Service;
using OrderEngine.Component.Payment.Service.OrderHandler;
using OrderEngine.Core.Model;
using OrderEngine.Core.Model.Request;
using OrderEngine.Core.Service.OrderHandler;
using System;

namespace OrderEngine.Component.OrderEngine.Service
{
    /// <summary>
    /// Business logic starting point for processing all orders
    /// </summary>
    public class OrderEngineService : IOrderEngineService
    {
        private IInventoryMgtService _inventorySvc;
        private IDeliveryMgtService _deliverySvc;

        public OrderEngineService(
            IInventoryMgtService InventoryMgtService,
            IDeliveryMgtService DeliveryMgtService)
        {
            this._inventorySvc = InventoryMgtService;
            this._deliverySvc = DeliveryMgtService;
        }

        public void ProcessOrder(OrderRequest request)
        {
            Guid transId = Guid.NewGuid();
            var id = transId.ToString();
            Console.WriteLine("{0} | Order Recieved: {1} buying {2} with {3}", id, request.Customer, request.Product, request.Payment);
            Console.WriteLine("{0} | Order Orchestration Start...", id);

            //Get approval to process order
            OrderState approval = this.RequestOrderApproval(request);

            //Process order basedon approval
            this.ProcessOrderApproval(approval, request, transId);
        }

        public OrderState RequestOrderApproval(OrderRequest request)
        {
            // Declare approval chains for Chain of Responsibility (CoR)
            OrderHandler PaymentHandler = new OrderHandler(new PaymentApprover());
            OrderHandler OrderMgtHandler = new OrderHandler(new OrderMgtApprover());

            //Estabish CoR
            PaymentHandler.RegisterNext(OrderMgtHandler);

            //Execute CoR
            return PaymentHandler.Process(request);
        }

        public void ProcessOrderApproval(OrderState state, OrderRequest request, Guid TransactionId)
        {
            if (state == OrderState.Invalid)
            {
                this.ProcessInvalidOrder(request, TransactionId);
            }
            else
            {
                this.ProcessValidOrder(request, TransactionId);
            }
        }

        public void ProcessValidOrder(OrderRequest request, Guid TransactionId)
        {
            //Update Inventory
            _inventorySvc.NewOrder(request, TransactionId);

            // Schedule Delivery
            _deliverySvc.NewDelivery(request, TransactionId);

            var id = TransactionId.ToString();
            Console.WriteLine("{0} | Order Orchestration End...", id);
            Console.WriteLine("{0} | Order succesfully placed!", id);
        }

        public void ProcessInvalidOrder(OrderRequest reqeust, Guid TransactionId)
        {
            var id = TransactionId.ToString();
            Console.WriteLine("{0} | --------- ! Cannont Complete Order ! ---------", id);
            Console.WriteLine("{0} | Order Orchestration End...", id);
        }
    }
}