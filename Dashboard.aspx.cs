using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class DashBrd : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    
    {
  
        if (Session["userID"] == null)
        {
            Response.Redirect("LoginPage.aspx");
        }
       // ((Label)Master.FindControl("lblUserName")).Text = Session["username"].ToString();

    }
}