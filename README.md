# Distributed-Messaging
A demo project which implements RabbitMQ and distributed services.  The goal is to demonstrate error recovery and system stability when issues arise among the integrations

##Setup
Before this demo works, you must set up a RabbitMQ.  When your instance is ready, add the (3) three following queues; make sure each queue is set to Durable and Auto-delete is off.
- OrderEngine
- InventoryMgt.FillOrder
- DeliveryMgt.NewDelivery
When all your queues are set up, pull this repo and navigate to the [RabbitMQAdapter.DemoConnection](https://github.com/BillBensing/Distributed-Messaging/blob/master/Distributed-System/RabbitMQAdapter/DemoConnection.cs) file.  You will see the following lines which need your connection information:

````
public static readonly string HOST = "Queue-IP-Address"
public static readonly string USER = "Your-Username";
public static readonly string PW = "Your-Password";
````

With all the above complete, you are ready to run the Demo.  Run the ClientApp, OrderEngine, InventoryMgt and DeliveryMgt applications.  Fill out the form with your product and payment type, then click "Submit"
