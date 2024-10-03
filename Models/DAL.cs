using Microsoft.Data.SqlClient;
using System.Data;

namespace EMedicineBE.Models
{
    public class DAL
    {
        public Response register(Users users, SqlConnection connection)
        {
            Response response = new Response();
            SqlCommand cmd = new SqlCommand("sp_register", connection);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@FirstName", users.FirstName);
            cmd.Parameters.AddWithValue("@LastName", users.LastName);
            cmd.Parameters.AddWithValue("@Password", users.Password);
            cmd.Parameters.AddWithValue("@Email", users.Email);
            cmd.Parameters.AddWithValue("@Fund", 0);
            cmd.Parameters.AddWithValue("@Type", "Users");
            cmd.Parameters.AddWithValue("@Status", "Pending");
            connection.Open();
            int i= cmd.ExecuteNonQuery();
            connection.Close();
            if(i> 0)
            {
                response.StatusCode = 200;
                response.StatusMessage = "User registered successfully";
            }
            else
            {
                response.StatusCode = 100;
                response.StatusMessage = "User registered failed";
            }
            return response;
        }
        public Response Login(Users users, SqlConnection connection)
        {
            SqlDataAdapter da = new SqlDataAdapter("sp_login", connection);
            da.SelectCommand.CommandType = CommandType.StoredProcedure;
            da.SelectCommand.Parameters.AddWithValue("Email", users.Email);
            da.SelectCommand.Parameters.AddWithValue("Password",users.Password);
            DataTable dt = new DataTable();
            da.Fill(dt);
            Response response = new Response();
            Users user = new Users();
            if(dt.Rows.Count>0)
            {
                user.ID = Convert.ToInt32(dt.Rows[0]["ID"]);
                user.FirstName = Convert.ToString(dt.Rows[0]["FirstName"]);
                user.LastName = Convert.ToString(dt.Rows[0]["LastName"]);
                user.Email = Convert.ToString(dt.Rows[0]["Email"]);
                user.Type = Convert.ToString(dt.Rows[0]["Type"]);
                response.StatusCode=200;
                response.StatusMessage = "user is valid";
                response.User = user;
            }
            else
            {
                response.StatusCode = 100;
                response.StatusMessage = "user is invalid";
                response.User = null;
            }
            return response;
        }

        public Response viewUser(Users users, SqlConnection connection)
        {
            SqlDataAdapter da =new SqlDataAdapter("p_viewUser", connection);
            da.SelectCommand.CommandType= CommandType.StoredProcedure;
            da.SelectCommand.Parameters.AddWithValue("@ID", users.ID);
            DataTable dt = new DataTable();
            da.Fill(dt);
            Response response = new Response();
            Users user = new Users();
            if (dt.Rows.Count > 0)
            {
                user.ID = Convert.ToInt32(dt.Rows[0]["ID"]);
                user.FirstName = Convert.ToString(dt.Rows[0]["FirstName"]);
                user.LastName = Convert.ToString(dt.Rows[0]["LastName"]);
                user.Email = Convert.ToString(dt.Rows[0]["Email"]);
                user.Type = Convert.ToString(dt.Rows[0]["Type"]);
                user.Fund = Convert.ToDecimal(dt.Rows[0]["Fund"]);
                user.CreateOn = Convert.ToDateTime(dt.Rows[0]["CreateOn"]);
                user.Password = Convert.ToString(dt.Rows[0]["Password"]);
                response.StatusCode = 200;
                response.StatusMessage = "user exists";
            }
            else
            {
                response.StatusCode = 100;
                response.StatusMessage = "user does not exist";
                response.User = users;
            }
            return response;
        }

        public Response updateProfile(Users users,SqlConnection connection)
        {
            Response response = new Response();
            SqlCommand cmd = new SqlCommand("sp_updateProfile",connection);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@FirstName", users.FirstName);
            cmd.Parameters.AddWithValue("@LastName", users.LastName);
            cmd.Parameters.AddWithValue("@Password", users.Password);
            cmd.Parameters.AddWithValue("@Email", users.Email);
            connection.Open();
            int i =cmd.ExecuteNonQuery();
            connection.Close();
            if(i > 0) { 
            response.StatusCode = 200;
            response.StatusMessage = "Record update successfully";
            }
            else
            {
                response.StatusCode = 100;
                response.StatusMessage = "some error occured . Try after sometime";
            }
            return response;
        }

        public Response addToCart(Cart cart ,SqlConnection connection)
        {
            Response response = new Response();
            SqlCommand cmd = new SqlCommand("sp_AddToCart",connection );
            cmd.CommandType= CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@UserId", cart.UserId);
            cmd.Parameters.AddWithValue("@UnitPrice", cart.UnitPrice);
            cmd.Parameters.AddWithValue("@Discount", cart.Discount);
            cmd.Parameters.AddWithValue("@Quantity", cart.Quantity);
            cmd.Parameters.AddWithValue("@TotalPrice", cart.TotalPrice);
            cmd.Parameters.AddWithValue("@MedicineID", cart.MedicineID);
            connection.Open();
            int i =cmd.ExecuteNonQuery();
            connection.Close();
            if(i > 0) {
                response.StatusCode = 200;
                response.StatusMessage= "Item addedd successfully";
            }
            else
            {
                response.StatusCode= 100;
                response.StatusMessage = "Item could not be add";
            }
            return response;

        }

        public Response placeOrder(Users users ,SqlConnection connection)
        {
            Response response = new Response();
            SqlCommand cmd = new SqlCommand("sp_PlaceOrder", connection);
            cmd.CommandType= CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@ID", users.ID);
            connection.Open() ;
            int i =cmd.ExecuteNonQuery();
            connection.Close();
            if(i > 0)
            {
                response.StatusCode = 200;
                response.StatusMessage = "Order has been placed successfully";
            }
            else
            {
                response.StatusCode= 100;
                response.StatusMessage = "Order could not be placed";
            }
            return response;
        }
        public Response orderList(Users users ,SqlConnection connection)
        {
            Response response = new Response();
            List<Orders> listOrder = new List<Orders>();
            SqlDataAdapter da = new SqlDataAdapter("sp_OrderList",connection);
            da.SelectCommand.CommandType = CommandType.StoredProcedure;
            da.SelectCommand.Parameters.AddWithValue("@Type", users.Type);
            da.SelectCommand.Parameters.AddWithValue("@ID", users.ID);
            DataTable dt = new DataTable();
            da.Fill(dt);
            if(dt.Rows.Count > 0)
            {
                for(int i =0;i< dt.Rows.Count;i++)
                {
                    Orders o = new Orders();
                    o.ID = Convert.ToInt32(dt.Rows[i]["ID"]);
                    o.OrderNo = Convert.ToString(dt.Rows[i]["OrderNo"]);
                    o.OrderTotal = Convert.ToDecimal(dt.Rows[i]["OrderTotal"]);
                    o.OrderStatus = Convert.ToString(dt.Rows[i]["OrderStatus"]);
                    listOrder.Add(o);
                }if(listOrder.Count > 0)
                {
                    response.StatusCode = 200;
                    response.StatusMessage = "Order details fetched";
                    response.ListOrders = listOrder;
                }
                else
                {
                    response.StatusCode = 100;
                    response.StatusMessage = "Order details are not available";
                    response.ListOrders = null;
                }
            }
            else
            {
                response.StatusCode = 100;
                response.StatusMessage = "Order details are not available";
                response.ListOrders = null;
            }
            return response;

        }

        public Response addUpdateMedicine(Medicines medicines,SqlConnection connection)
        {
            Response response = new Response();
            SqlCommand cmd = new SqlCommand("sp_AddUpdateMedicine", connection);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@Name", medicines.Name);
            cmd.Parameters.AddWithValue("@Manufacturer", medicines.Manufacturer);
            cmd.Parameters.AddWithValue("@UnitPrice", medicines.UnitPrice);
            cmd.Parameters.AddWithValue("@Discount", medicines.Discount);
            cmd.Parameters.AddWithValue("@Quantity", medicines.Quantity);
            cmd.Parameters.AddWithValue("@ExpDate", medicines.ExpDate);
            cmd.Parameters.AddWithValue("@ImageUrl", medicines.ImageUrl);
            cmd.Parameters.AddWithValue("@Status", medicines.Status);
            cmd.Parameters.AddWithValue("@Type", medicines.Type);
            connection.Open();
            int i = cmd.ExecuteNonQuery();
            connection.Close();
            if (i > 0)
            {
                response.StatusCode = 200;
                response.StatusMessage = "Medicine inserted successfully";
            }
            else
            {
                response.StatusCode = 100;
                response.StatusMessage = "Medicine did not save. try again.";
            }
            return response;
        }
        public Response userList( SqlConnection connection)
        {
            Response response = new Response();
            List<Users> listUser = new List<Users>();
            SqlDataAdapter da = new SqlDataAdapter("sp_UserList", connection);
            da.SelectCommand.CommandType = CommandType.StoredProcedure;
            DataTable dt = new DataTable();
            da.Fill(dt);
            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    Users u = new Users();
                    u.ID = Convert.ToInt32(dt.Rows[i]["ID"]);
                    u.FirstName = Convert.ToString(dt.Rows[i]["FirstName"]);
                    u.LastName = Convert.ToString(dt.Rows[i]["LastName"]);
                    u.Password = Convert.ToString(dt.Rows[i]["Password"]);
                    u.Email = Convert.ToString(dt.Rows[i]["Email"]);
                    u.Fund = Convert.ToDecimal(dt.Rows[i]["Fund"]);
                    u.Status = Convert.ToInt32(dt.Rows[i]["Status"]);
                    u.CreateOn = Convert.ToDateTime(dt.Rows[i]["CreateOn"]);
                    listUser.Add(u);
                }
                if (listUser.Count > 0)
                {
                    response.StatusCode = 200;
                    response.StatusMessage = "User details fetched";
                    response.ListUsers = listUser;
                }
                else
                {
                    response.StatusCode = 100;
                    response.StatusMessage = "User details are not available";
                    response.ListUsers = null;
                }
            }
            else
            {
                response.StatusCode = 100;
                response.StatusMessage = "User details are not available";
                response.ListOrders = null;
            }
            return response;

        }
    }
}
