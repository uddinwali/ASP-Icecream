using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Security.Principal;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using Microsoft.AspNet.Identity;

namespace TailSpyWorksAdvanced
{
    public partial class SiteMaster : MasterPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                DbOperations managers = new DbOperations();
                ListView_ProductsMenu.DataSource = managers.ShowCategory();
                ListView_ProductsMenu.DataBind();

            }

        }
    }

}