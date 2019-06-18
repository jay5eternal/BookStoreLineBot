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
    public class CustomersRepository
    {
        private static string connString;
        private SqlConnection conn;
        public CustomersRepository()
        {
            if (string.IsNullOrEmpty(connString))
            {
                connString = ConfigurationManager.ConnectionStrings["bsmobile"].ConnectionString;
            }

            conn = new SqlConnection(connString);
        }       
        
        public bool SelectCustomer(string CustomerAccount)
        {
            using (conn = new SqlConnection(connString))
            {
                string sql = "Select CustomerAccount From Customers Where CustomerAccount = '" + CustomerAccount + "'";
                var cust = conn.QueryFirstOrDefault<Customer>(sql);
                if(cust == null)
                {
                    return true;
                }
                else { return false; }
            }
        }

        public IEnumerable<Customer> ReadAllCustomer()
        {
            using (conn = new SqlConnection(connString))
            {
                string sql = "Select * From Customers";
                var cus = conn.Query<Customer>(sql);

                return cus;
            }
                
        }
        public string SelectCustomerEmail(int custid)
        {
            using (conn = new SqlConnection(connString))
            {
                string sql = "Select CustomerEmail From Customers Where CustomerID = @custid";
                string cust_email = conn.QueryFirstOrDefault<string>(sql,new { custid });
                return cust_email;
            }
        }

        public bool SelectCustomerEmail(string CustomerEmail)
        {
            using (conn = new SqlConnection(connString))
            {
                string sql = "Select CustomerEmail From Customers Where CustomerEmail = '" + CustomerEmail + "'";
                var cust = conn.QueryFirstOrDefault<Customer>(sql);
                if (cust == null)
                {
                    return true;
                }
                else { return false; }
            }
        }
        public Customer CustomerLogin(string account, string password)
        {
            using (conn = new SqlConnection(connString))
            {
                string sql = "Select * From Customers Where CustomerAccount= '" + account + "' and CustomerPassword= '" + password + "';";
                var cust = conn.QueryFirstOrDefault<Customer>(sql);
                return cust;

            }
        }
        public Customer CustomerLogin(string account)
        {
            using (conn = new SqlConnection(connString))
            {
                string sql = "Select * From Customers Where CustomerAccount= @account ;";
                var cust = conn.QueryFirstOrDefault<Customer>(sql,new { account});
                return cust;

            }
        }

        public CustomerViewModel SelectCustomerView(string account)
        {
            using (conn = new SqlConnection(connString))
            {
                string sql = "Select * From Customers Where CustomerAccount= '" + account + "'";
                var cust = conn.QueryFirstOrDefault<CustomerViewModel>(sql);
                return cust;

            }
        }

        public bool IsEmailConfirmed(string account)
        {
            using (conn = new SqlConnection(connString))
            {
                string sql = "Select EmailConfirmed From Customers Where CustomerAccount= '" + account + "' ;";
                var result = conn.QueryFirstOrDefault<bool>(sql);
                return result;

            }
        }



        public int GetCusromerID(string account)
        {
            using (conn = new SqlConnection(connString))
            {
                int customerId= conn.Query<int>("GetCustomerID",
                                new { customerAccount =account },
                                commandType: CommandType.StoredProcedure
                                ).SingleOrDefault();
                return customerId;
            }
        }

        public void UpdateCustomer(CustomerViewModel cust)
        {
            using (conn = new SqlConnection(connString))
            {
                string sql = "Update Customers Set CustomerName=@CustomerName, CustomerEmail=@CustomerEmail, CustomerPhone=@CustomerPhone, CustomerAddress=@CustomerAddress,EmailConfirmed=@EmailConfirmed WHERE CustomerAccount=@CustomerAccount";
                conn.Execute(sql, new {
                    CustomerName = cust.CustomerName,
                    CustomerAccount = cust.CustomerAccount,
                    CustomerEmail = cust.CustomerEmail,
                    CustomerPhone = cust.CustomerPhone,
                    CustomerAddress = cust.CustomerAddress,
                    EmailConfirmed = cust.EmailConfirmed
                });
            }

        }

        public void UpdateEmailConfirmed(int customerId,bool isConfirmed)
        {
            using (conn = new SqlConnection(connString))
            {
                string sql = "UPDATE Customers SET EmailConfirmed= @isConfirmed WHERE CustomerID=@customerId;";
                conn.Execute(sql, new { isConfirmed , customerId });
            }
        }






        public string SelectCustomerPassword(string account)
        {
            using (conn = new SqlConnection(connString))
            {
                string sql = "Select CustomerPassword From Customers Where CustomerAccount= '" + account + "'";
                var cust = conn.QueryFirstOrDefault<string>(sql);
                return cust;

            }
        }

        public List<Order> SelectOrders(string account)
        {
            List<Order> orders;
            using (conn = new SqlConnection(connString))
            {
                string sql =
                    "Select * from Orders As o Inner Join Customers As c On o.CustomerID = c.CustomerID Inner Join[Order Status] As os On o.OrderID = os.OrderID Where c.CustomerAccount = '" +
                    account + "'";
                orders = conn.Query<Order>(sql).ToList();
                return orders;
            }
        }

        public Order SelectOrder(int? orderId)
        {
            Order orders;
            using (conn = new SqlConnection(connString))
            {
                string sql = "Select * from Orders Where OrderID = " + orderId;
                orders = conn.QueryFirstOrDefault<Order>(sql);
                return orders;
            }
        }

        public List<OrderDetailModel> SelectOrderDetails(int? orderId)
        {
            List<OrderDetailModel> orderDetails;
            using (conn = new SqlConnection(connString))
            {
                string sql =
                    "Select * from [Order Detail] as od Inner join Books As b On od.BookID = b.BookID Where od.OrderID = " + orderId;
                orderDetails = conn.Query<OrderDetailModel>(sql).ToList();
                return orderDetails;
            }
        }

        public OrderStatusModel SelectOrderStatus(int? orderId)
        {
            OrderStatusModel orders;
            using (conn = new SqlConnection(connString))
            {
                string sql = "Select * from [Order Status] Where OrderID = " + orderId;
                orders = conn.QueryFirstOrDefault<OrderStatusModel>(sql);
                return orders;
            }
        }
        public List<string> SelectRoles(int userid)
        {
            using (conn = new SqlConnection(connString))
            {
                string sql = "select RolesID from UserRoles where UserID=@userid";
                string result = conn.QueryFirstOrDefault<string>(sql,new { userid });
                string[] rolesArr= result.Split(',');
                return rolesArr.ToList();
            }
        }
        public string SelectRolesName(string roleid)
        {
            using (conn = new SqlConnection(connString))
            {
                string sql = "select RoleName from Roles where RoleId=@roleid";
                string roleName = conn.QueryFirstOrDefault<string>(sql, new { roleid });
                return roleName;
            }
        }

        public Customer SelectCustomerDetail(string account)
        {
            using (conn = new SqlConnection(connString))
            {
                string sql = "Select * From Customers Where CustomerAccount= '" + account + "'";
                var cust = conn.QueryFirstOrDefault<Customer>(sql);
                return cust;

            }
        }

        public CustomerOrderCount SelectCustomerOrderCount(string account)
        {
            CustomerOrderCount orders;
            using (conn = new SqlConnection(connString))
            {
                string sql = "Select COUNT(*) As Count,SUM(o.TotalPrice) As Sum From Customers As c " +
                "Inner Join Orders As o On c.CustomerID = o.CustomerID " +
                "Where c.CustomerAccount = '" + account + "'" +
                "Group By c.CustomerAccount";
                orders = conn.QueryFirstOrDefault<CustomerOrderCount>(sql);
                return orders;
            }
        }
    }
}