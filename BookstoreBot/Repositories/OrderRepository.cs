using Dapper;
using BookstoreBot.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace BookstoreBot.Repositories
{
    public class OrderRepository
    {
        private static string connString;
        private SqlConnection conn;
        public OrderRepository()
        {
            if (string.IsNullOrEmpty(connString))
            {
                connString = ConfigurationManager.ConnectionStrings["bsmobile"].ConnectionString;
            }

            conn = new SqlConnection(connString);
        }

        public string GetMaxNo()
        {
            using (conn = new SqlConnection(connString))
            {
                string sql = "SELECT MAX(OrderNO) FROM Orders";
                var maxNo = conn.QueryFirstOrDefault<string>(sql);
                return maxNo;

            }
        }


        //加訂單寫在這之下

        public void CreateOrder(Order model)
        {
            using (conn = new SqlConnection(connString))
            {
                string sql = "insert into Orders (OrderNo,CustomerID,OrderDate,PayWay,DeliveryMethod,TotalPrice,Recipient,RecipientPhone,RecipientEmail,RecipientAddress,ShippingRate) values(@OrderNo,@CustomerID,@OrderData,@PayWay,@DeliveryMethod,@TotalPrice,@Recipient,@RecipientPhone,@RecipientEmail,@RecipientAddress,@ShippingRate)";
                conn.Execute(sql, new {

                    OrderNo = model.OrderNo,
                    CustomerID = model.CustomerID,
                    OrderData = model.OrderDate,
                    PayWay = model.PayWay,
                    DeliveryMethod = model.DeliveryMethod,

                    TotalPrice = model.TotalPrice,
                    Recipient = model.Recipient,
                    RecipientPhone = model.RecipientPhone,
                    RecipientEmail = model.RecipientEmail,
                    RecipientAddress = model.RecipientAddress,
                    
                    ShippingRate = model.ShippingRate
                });
            }
        }

        public void CreateOrderStatus(int orderID,Order model)
        {
            using (conn = new SqlConnection(connString))
            {
                string sql = "insert into[dbo].[Order Status](OrderID,SetUp) values(@OrderID,@SetUp)";
                conn.Execute(sql, new {

                    OrderID =orderID,
                    SetUp =model.SetUp

                });

            }
        }



        public Order GetOrderFromOrderNo(string orderNo)
        {
            Order result;
            using (conn = new SqlConnection(connString))
            {
                string sql = "select * from[dbo].[Orders] where OrderNo ='" + orderNo + "'";
                result = conn.QueryFirstOrDefault<Order>(sql);
            }
            return result;
        }

        public IEnumerable<OrderDetailModel> GetOrderDetail()
        {
            using (conn = new SqlConnection(connString))
            {
                string sql = "select * from[dbo].[Order Detail] od INNER JOIN Books b ON b.BookID = od.BookID";
                var result = conn.Query<OrderDetailModel>(sql);
                return result;
            }
            
        }

        public IEnumerable<Order> GetOrder()
        {
            using (conn = new SqlConnection(connString))
            {
                string sql = "select * from Orders";
                var result = conn.Query<Order>(sql);
                return result;
            }
        }

        public List<Order> GetAllOrders()
        {
            List<Order> allOrders;
            using (conn = new SqlConnection(connString))
            {
                string sql = "select * from Orders As o Inner Join [Order Status] As os On o.OrderID = os.OrderID Inner Join Customers As c On o.CustomerID = c.CustomerID";
                allOrders = conn.Query<Order>(sql).ToList();
                return allOrders;
            }
        }

        public OrderStatusModel GetOrderStatus(string orderId)
        {
            using (conn = new SqlConnection(connString))
            {
                string sql = "select * from [Order Status] As od Inner Join Orders As o On od.OrderID = o.OrderID Where od.OrderID = " + orderId + ";";
                var orderStatus = conn.QueryFirstOrDefault<OrderStatusModel>(sql);
                return orderStatus;
            }
        }
        public OrderStatusModel GetOrderStatusByNo(string orderNo)
        {
            using (conn = new SqlConnection(connString))
            {
                string sql = "select * from [Order Status] As od Inner Join Orders As o On od.OrderID = o.OrderID Where o.OrderNo=@orderNo;";
                var orderStatus = conn.QueryFirstOrDefault<OrderStatusModel>(sql,new { orderNo });
                return orderStatus;
            }
        }
        public List<OrderStatusModel> GetOrderStatusByNotop4(string customerAccount)
        {
            List<OrderStatusModel> orders;
            using (conn = new SqlConnection(connString))
            {
                string sql = "select TOP 3 * from [Order Status] As od Inner Join Orders As o On od.OrderID = o.OrderID inner join Customers as c on c.CustomerID=o.CustomerID where od.TransactionComplete is null and c.CustomerAccount='" + customerAccount + "' ORDER BY od.OrderID desc;";
                orders = conn.Query<OrderStatusModel>(sql).ToList();
                return orders;
            }
        }
    }
}