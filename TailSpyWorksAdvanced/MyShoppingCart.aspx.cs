using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace TailSpyWorksAdvanced
{
    public partial class MyShoppingCart : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                DbOperations operations = new DbOperations();
                String cartId = operations.GetShoppingCartId();
                decimal totalBill = 0;

                MyList.DataSource = operations.ShowCartItems();
                MyList.DataBind();

                totalBill = operations.GetTotal(cartId);
                lblTotal.Text = String.Format("{0:c}", totalBill);
           }

        }
        public struct ShoppingCartUpdates {
            public int ProductId;
            public int PurchaseQuantity;
            public bool RemoveItem;
        }

        protected void UpdateBtn_Click(object sender, ImageClickEventArgs e)
        {
            DbOperations operations = new DbOperations();
            String cartID = operations.GetShoppingCartId();
            ShoppingCartUpdates[] updates = new ShoppingCartUpdates[MyList.Rows.Count];

            for (int i = 0; i < MyList.Rows.Count; i++) {
                IOrderedDictionary rowValues = new OrderedDictionary();
                rowValues = GetValues(MyList.Rows[i]);
                updates[i].ProductId = Convert.ToInt32(rowValues["ProductID"]);
                updates[i].PurchaseQuantity = Convert.ToInt32(rowValues["Quantity"]);

                CheckBox cbRemove = new CheckBox();
                cbRemove = (CheckBox)MyList.Rows[i].FindControl("Remove");
                updates[i].RemoveItem = cbRemove.Checked;

            }
            operations.UpdateShoppingCartData(cartID, updates);
            MyList.DataBind();
            lblTotal.Text = String.Format("{0:c}", operations.GetTotal(cartID));
            Response.Redirect("MyShoppingCart.aspx");
        }

        public static IOrderedDictionary GetValues(GridViewRow gridViewRow)
        {
            IOrderedDictionary values = new OrderedDictionary();
            foreach(DataControlFieldCell cell in gridViewRow.Cells)
            {
                if (cell.Visible)
                {
                    cell.ContainingField.ExtractValuesFromCell(values, cell, gridViewRow.RowState, true);
                }
            }
            return values;
        }

    }
}
