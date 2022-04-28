using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.SessionState;
using TailSpyWorksAdvanced;

namespace TailSpyWorksAdvanced
{

    public partial class DbOperations
    {
        String connString;
        SqlConnection conn;
        SqlDataAdapter adapter;

        public DbOperations()
        {
            connString = ConfigurationManager.ConnectionStrings["connection"].ConnectionString;
            conn = new SqlConnection(connString);
            conn.Open();

        }

        public void insertData(String _ProductName, String _Price, String Quantity)
        {
            adapter = new SqlDataAdapter();
            adapter.InsertCommand = new SqlCommand("sp_insert", conn);

            adapter.InsertCommand.CommandType = CommandType.StoredProcedure;
            adapter.InsertCommand.Parameters.AddWithValue("@ProductName", _ProductName);
            adapter.InsertCommand.Parameters.AddWithValue("@Price", _Price);
            adapter.InsertCommand.Parameters.AddWithValue("@Quantity", Quantity);

            adapter.InsertCommand.ExecuteNonQuery();

        }
        public void UpdateData(int Id, String _ProductName, String _Price, String Quantity)
        {
            adapter = new SqlDataAdapter();
            adapter.UpdateCommand = new SqlCommand("sp_update", conn);

            adapter.UpdateCommand.CommandType = CommandType.StoredProcedure;
            adapter.UpdateCommand.Parameters.AddWithValue("@Id", Id);
            adapter.UpdateCommand.Parameters.AddWithValue("@ProductName", _ProductName);
            adapter.UpdateCommand.Parameters.AddWithValue("@Price", _Price);
            adapter.UpdateCommand.Parameters.AddWithValue("@Quantity", Quantity);

            adapter.UpdateCommand.ExecuteNonQuery();
        }

        public void UpdateShoppingCartData(string cartID, MyShoppingCart.ShoppingCartUpdates[] updates)
        {
            using (SqlConnection sqlCon = new SqlConnection(connString))
            {
                int cartItemCount = updates.Count();
                adapter = new SqlDataAdapter("Select * from ShoppingCart sh inner join Products pr on sh.ProductID = pr.ProductID where sh.CartID='"+cartID+"' ", sqlCon);
                DataSet ds = new DataSet();
                adapter.Fill(ds);

                foreach (DataRow MatchRecord in ds.Tables[0].Rows)
                {
                    for (int i = 0; i < cartItemCount; i++)
                    {
                        if (MatchRecord["ProductID"].ToString() == updates[i].ProductId.ToString())
                        {
                            if (updates[i].RemoveItem == true)
                            {
                                RemoveItem(cartID, MatchRecord["ProductID"].ToString());
                                
                            }
                            else {
                                UpdateItem(cartID, MatchRecord["ProductID"].ToString(), updates[i].PurchaseQuantity);
                            }
                        }
                    }
                }
            }


        }

        private void UpdateItem(string cartID, string productID, int purchaseQuantity)
        {
            using (SqlConnection sqlCon = new SqlConnection(connString))
            {
                try
                {
                    adapter = new SqlDataAdapter("Select * from ShoppingCart where CartID = '" + cartID + "' AND ProductID = '" + productID + "'", sqlCon);
                    DataSet ds = new DataSet();
                    adapter.Fill(ds);
                    int rows = ds.Tables[0].Rows.Count;
                    if (rows > 0)
                    {
                        adapter = new SqlDataAdapter("Update ShoppingCart set Quantity = '"+purchaseQuantity+"' where CartID= '" + cartID + "' AND ProductID = '" + productID + "'", sqlCon);
                        ds = new DataSet();
                        adapter.Fill(ds);
                        ds = new DataSet();
                        adapter = new SqlDataAdapter("Select * from ShoppingCart where CartID = '" + cartID + "' AND ProductID = '" + productID + "'", sqlCon);
                        adapter.Fill(ds);
                    }
                }
                catch (Exception exp) { throw new Exception("ERROR: Unable to Update Cart Item - " + exp.Message.ToString(), exp); }
            }
        }

        private void RemoveItem(string cartID, string productID)
        {
            using (SqlConnection sqlCon = new SqlConnection(connString)) {
                try {
                    adapter = new SqlDataAdapter("Select * from ShoppingCart where CartID = '" + cartID + "' AND ProductID = '" + productID + "'", sqlCon);
                    DataSet ds = new DataSet();
                    adapter.Fill(ds);
                    int rows = ds.Tables[0].Rows.Count;
                    if (rows > 0) {
                        adapter = new SqlDataAdapter("Delete from ShoppingCart where CartID= '" + cartID + "' AND ProductID = '" + productID + "'", sqlCon);
                        ds = new DataSet();
                        adapter.Fill(ds);
                    }
                } catch (Exception exp) { throw new Exception("ERROR: Unable to Remove Cart Item - " + exp.Message.ToString(), exp); } }
        }

        public void DelData(int Id)
        {
            adapter = new SqlDataAdapter();
            adapter.DeleteCommand = new SqlCommand("sp_delete", conn);

            adapter.DeleteCommand.CommandType = CommandType.StoredProcedure;
            adapter.DeleteCommand.Parameters.AddWithValue("@Id", Id);


            adapter.DeleteCommand.ExecuteNonQuery();
        }

        public DataSet FetchData()
        {
            adapter = new SqlDataAdapter("sp_select", conn);
            DataSet ds = new DataSet();
            adapter.Fill(ds);

            return ds;

        }

        public DataSet FetchDataById(int Id)
        {

            adapter = new SqlDataAdapter();
            adapter.SelectCommand = new SqlCommand("sp_selectById", conn);
            adapter.SelectCommand.CommandType = CommandType.StoredProcedure;
            adapter.SelectCommand.Parameters.AddWithValue("@Id", Id);
            adapter.SelectCommand.ExecuteNonQuery();

            DataSet ds = new DataSet();
            adapter.Fill(ds);
            return ds;

        }

        public DataSet ShowCategory()
        {
            adapter = new SqlDataAdapter("Select * from Categories", conn);
            DataSet ds = new DataSet();
            adapter.Fill(ds);

            return ds;
        }

        public DataSet ShowProductByCategory(int selectedCategory)
        {
            adapter = new SqlDataAdapter("Select CategoryID, ProductID, ProductName, ProductImage, UnitCost from Products where CategoryID = '" + selectedCategory + "'", conn);
            DataSet ds = new DataSet();
            adapter.Fill(ds);
            return ds;

        }
        const string CartId = "TailSpinSpyWorks_CartID";
        //------------------------------------------------------------------------------------------------------------------------------------------+
        public String GetShoppingCartId()
        {
            if (System.Web.HttpContext.Current.Session[CartId] == null)
            {
                System.Web.HttpContext.Current.Session[CartId] = System.Web.HttpContext.Current.Request.IsAuthenticated ? System.Web.HttpContext.Current.User.Identity.Name : Guid.NewGuid().ToString();
            }
            return System.Web.HttpContext.Current.Session[CartId].ToString();
        }

        //------------------------------------------------------------------------------------------------------------------------------------------+
        public void AddItem(string cartID, int productID, int quantity)
        {
            using (SqlConnection conn = new SqlConnection(connString))
            {
                try
                {
                    conn.Open();
                    adapter = new SqlDataAdapter("Select * from ShoppingCart where CartID = '" + cartID + "' AND ProductID = '" + productID + "' ", conn);
                    DataSet ds = new DataSet();
                    adapter.Fill(ds);

                    int rows = ds.Tables[0].Rows.Count;

                    if (rows == 0)
                    {
                        ds = new DataSet();
                        adapter = new SqlDataAdapter("Insert into ShoppingCart(CartID,Quantity,ProductID,DateCreated) values ('" + cartID + "','" + quantity + "','" + productID + "','" + DateTime.Now + "')", conn);
                        adapter.Fill(ds);
                    }
                    else if (rows > 0)
                    {
                        String innerQuery = "Select Quantity from ShoppingCart where ProductID = '" + productID + "'";
                        //int increment = Convert.ToInt32(innerQuery) + 1;

                        //ds = new DataSet();
                        adapter = new SqlDataAdapter("Update ShoppingCart Set Quantity=(Select Quantity from Products where productID='" + productID + "') + 1 where CartID ='" + cartID + "' AND ProductID='" + productID + "'", conn);
                        adapter.Fill(ds);
                    }
                }
                catch (Exception exp)
                {
                    throw new Exception("ERROR: Unable to Add Item to Cart - " + exp.Message.ToString(), exp);
                }

            }

        }

        public DataSet ShowCartItems()
        {

            adapter = new SqlDataAdapter("Select * from ShoppingCart sh inner join Products pr on sh.ProductID=pr.ProductID where sh.CartID = '" + System.Web.HttpContext.Current.Session[CartId].ToString() + "'", conn);
            DataSet ds = new DataSet();
            adapter.Fill(ds);
            return ds;

        }
        public decimal GetTotal(string cartID)
        {
            using (SqlConnection conn = new SqlConnection(connString))
            {
                decimal totalBill = 0;
                try
                {
                    conn.Open();
                    adapter = new SqlDataAdapter("Select pr.UnitCost, sh.Quantity from ShoppingCart sh inner join Products pr on sh.ProductID = pr.ProductID where sh.CartID = '" + System.Web.HttpContext.Current.Session[CartId].ToString() + "'", conn);
                    DataSet ds = new DataSet();
                    adapter.Fill(ds);

                    ds.Tables[0].Columns.Add("TotalBilling", typeof(double), "Quantity * UnitCost");

                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        totalBill = Convert.ToDecimal(ds.Tables[0].Compute("Sum([TotalBilling])", ""));
                    }
                }
                catch (Exception exp)
                {
                    throw new Exception("ERROR: Unable to Add Item to Cart - " + exp.Message.ToString(), exp);
                }
                return totalBill;
            }
        }
    }



}
