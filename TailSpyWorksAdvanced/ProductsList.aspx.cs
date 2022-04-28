using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace TailSpyWorksAdvanced
{
    public partial class ProductsList : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                DbOperations manager = new DbOperations();

                ListView_Products.DataSource = manager.ShowProductByCategory(Convert.ToInt32(Request.QueryString["CategoryID"]));
                ListView_Products.DataBind();

            }
        }
    }
}