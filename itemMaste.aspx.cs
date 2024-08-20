using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml.Linq;

public partial class itemMaste : System.Web.UI.Page
{
    string constr = ConfigurationManager.ConnectionStrings["SONIC_PRDConnectionString"].ConnectionString;
    private static int PageSize = 10;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!this.IsPostBack)
        {
            //this.BindGrid();
            try
            {
                LoadCode();
                LoadParty();
                LoadLeaf();
                BindGrid();

            }
            catch (Exception ex)
            {
            }
        }
    }
    private void BindGrid()
    {
        //string constr = ConfigurationManager.ConnectionStrings["SONIC_PRDConnectionString"].ConnectionString;
        using (SqlConnection con = new SqlConnection(constr))
        {
            //using (SqlCommand cmd = new SqlCommand("Customers_CRUD"))
            using (SqlCommand cmd = new SqlCommand("SELECT * FROM T_ITEMMASTER"))
            {
               // cmd.Parameters.AddWithValue("@Action", "SELECT");
                using (SqlDataAdapter sda = new SqlDataAdapter())
                {
                    //cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Connection = con;
                    sda.SelectCommand = cmd;
                    using (DataTable dt = new DataTable())
                    {
                        sda.Fill(dt);
                        gvItemMaster.DataSource = dt;
                        gvItemMaster.DataBind();
                    }
                }
            }
        }
    }

    protected void LoadCode()
    {
        try
        {
            using (SqlConnection con = new SqlConnection(constr))
            {
                using (SqlCommand cmd = new SqlCommand("select max(T_IM_CODE) + 1 from sonic_prd.dbo.t_itemmaster"))
                {
                    cmd.CommandType = CommandType.Text;
                    cmd.Connection = con;
                    con.Open();
                    SqlDataAdapter adp2 = new SqlDataAdapter(cmd);
                    DataTable dt2 = new DataTable();
                    adp2.Fill(dt2);
                    txtCode.Text = dt2.Rows[0][0].ToString();
                    con.Close();
                }
            }
        }
        catch (Exception ex)
        {
        }

    }
    protected void LoadParty()
    {
        try
        {
            using (SqlConnection con = new SqlConnection(constr))
            {
                using (SqlCommand cmd = new SqlCommand("select * from sonic_prd.dbo.T_ITEM_PARTY_MASTER order by ID desc"))
                {
                    cmd.CommandType = CommandType.Text;
                    cmd.Connection = con;
                    con.Open();
                    SqlDataAdapter adp = new SqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    adp.Fill(dt);
                    ddlPartyName.DataSource = dt;
                    ddlPartyName.DataTextField = "T_ITEM_PARTY_NAME";
                    ddlPartyName.DataValueField = "ID";
                    ddlPartyName.DataBind();


                }
            }
            ddlPartyName.Items.Insert(0, new ListItem("--Select Party Name--", "0"));
        }
        catch (Exception ex)
        {
        }

    }
    protected void LoadLeaf()
    {
        try
        {
            using (SqlConnection con = new SqlConnection(constr))
            {
                using (SqlCommand cmd = new SqlCommand("select * from sonic_prd.dbo.T_ITEM_LEAF_MASTER order by ID  asc"))
                {
                    cmd.CommandType = CommandType.Text;
                    cmd.Connection = con;
                    con.Open();
                    SqlDataAdapter adp = new SqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    adp.Fill(dt);
                    ddlLeaf.DataSource = dt;
                    ddlLeaf.DataTextField = "T_LM_LEAF_NO";
                    ddlLeaf.DataValueField = "ID";
                    ddlLeaf.DataBind();


                }
            }
            ddlLeaf.Items.Insert(0, new ListItem("--Select Leaf--", "0"));
        }
        catch (Exception ex)
        {
        }

    }

    protected void OnRowEditing(object sender, GridViewEditEventArgs e)
    {
        gvItemMaster.EditIndex = e.NewEditIndex;
        this.BindGrid();
        
    }

 

    

    protected void OnRowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        int customerId = Convert.ToInt32(gvItemMaster.DataKeys[e.RowIndex].Values[0]);
        string constr = ConfigurationManager.ConnectionStrings["SONIC_PRDConnectionString"].ConnectionString;
        using (SqlConnection con = new SqlConnection(constr))
        {
            using (SqlCommand cmd = new SqlCommand("Customers_CRUD"))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Action", "DELETE");
                cmd.Parameters.AddWithValue("@CustomerId", customerId);
                cmd.Connection = con;
                con.Open();
                cmd.ExecuteNonQuery();
                con.Close();
            }
        }
        this.BindGrid();
    }

    



    protected void gvItemMaster_SelectedIndexChanged(object sender, EventArgs e)
    {
        GridViewRow row = gvItemMaster.SelectedRow;
        txtSs.Text = (row.FindControl("lblName")as Label).Text;
        txtDesc.Text = (row.FindControl("lblCountry") as Label).Text;
    }

    protected void gvItemMaster_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        gvItemMaster.PageIndex = e.NewPageIndex;
        this.BindGrid();
    }
}