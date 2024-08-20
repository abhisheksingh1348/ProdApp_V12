using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class MasterPage : System.Web.UI.MasterPage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["userID"] != null)
        {
            lblUserName.Text = Session["username"].ToString();
        }
        //commented for testing
        //else
        //{
        //    Response.Redirect("LoginPage.aspx");
        //}
    }
}
