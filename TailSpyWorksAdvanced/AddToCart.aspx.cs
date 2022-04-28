using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace TailSpyWorksAdvanced
{
    public partial class AddToCart : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            string rawId = Request.QueryString["ProductID"];
            int productId;
            if (!String.IsNullOrEmpty(rawId) && Int32.TryParse(rawId, out productId))
            {
                DbOperations usersShoppingCart = new DbOperations();
                String cartId = usersShoppingCart.GetShoppingCartId();
                usersShoppingCart.AddItem(cartId, productId, 1);
            }
            else
            {
                Debug.Fail("ERROR : We should never get to AddToCart.aspx without a ProductId.");
                throw new Exception("ERROR : It is illegal to load AddToCart.aspx without setting a ProductId.");
            }
            Response.Redirect("MyShoppingCart.aspx");
        }
    }
}