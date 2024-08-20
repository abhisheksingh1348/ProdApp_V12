using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Globalization;
using System.Drawing.Printing;
using System.Web.Services;
using System.Drawing;

public partial class itemMaster : System.Web.UI.Page
{
    string constr = ConfigurationManager.ConnectionStrings["SONIC_PRDConnectionString"].ConnectionString;
    private static int PageSize = 5000;

    protected void Page_Load(object sender, EventArgs e)
    {
       
        if (!IsPostBack)
        {
            try
            {
                LoadCode();
                LoadParty();
                LoadLeaf();
                LoadItemMaster();

            }
            catch (Exception ex)
            {
            }
        }
    }
    protected void LoadItemMaster()
    {
        DataTable ItemMasterData = new DataTable();
        //ItemMasterData.Columns.Add("T_IM_CODE");
        //ItemMasterData.Columns.Add("T_IM_DESC");
        //ItemMasterData.Columns.Add("T_IM_SECTION");
        //ItemMasterData.Columns.Add("T_IM_LEAF_NO");
        //ItemMasterData.Columns.Add("T_IM_WEIGHT");
        //ItemMasterData.Columns.Add("T_IM_LESSPER");
        //ItemMasterData.Columns.Add("T_IM_LESSWEIGHT");
        //ItemMasterData.Columns.Add("T_IM_THICKNESS");
        //ItemMasterData.Columns.Add("T_IM_WIDTH");
        //ItemMasterData.Columns.Add("T_IM_SHEARSIZE");
        //ItemMasterData.Columns.Add("T_IM_CAT");
        //ItemMasterData.Columns.Add("T_IM_TYPE");
        //ItemMasterData.Columns.Add("T_IM_CREATED_BY");
        //ItemMasterData.Columns.Add("T_IM_CREATED_DT");    
        ItemMasterData.Rows.Add();
        gvItemMaster.DataSource = ItemMasterData;
        gvItemMaster.DataBind();
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
                    txtCode.Text =  dt2.Rows[0][0].ToString();
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

    protected void btnSave_Click(object sender, EventArgs e)
    {
        bool Valid = CheckValidation();
        if (Valid)
        {
            try
            {
                String section = txtWidth.Text.ToString().Trim() + " " + "X" + " " + txtThickness.Text.ToString(); 
                double ShWt = (Convert.ToDouble(txtWidth.Text.ToString().Trim()) * Convert.ToDouble(txtThickness.Text.ToString().Trim()) * Convert.ToDouble(txtSs.Text.ToString().Trim() )* 7.85)/1000000;
                Double Lwt =  ((100 - Convert.ToDouble(lessP.Text.ToString().Trim()))/100) * ShWt;
               
                //
                

                //

                using (SqlConnection con = new SqlConnection(constr))
                {
                    con.Open();

                    using (SqlCommand cmd = new SqlCommand("INSERT INTO T_ITEMMASTER (T_IM_CODE,T_IM_DESC, T_IM_SECTION,T_IM_LEAF_NO,T_IM_LESSPER,T_IM_LESSWEIGHT,T_IM_THICKNESS,T_IM_WIDTH,T_IM_SHEARSIZE,T_IM_CAT,T_IM_TYPE,T_IM_CREATED_BY,T_IM_CREATED_DT) VALUES  (@Code,@Desc,@Sec,@LeafNo,@Lp,@Lwt,@Thick,@Width,@Ss,@Cat,@Type,@USER,@Cdt)"))
                    {
                        cmd.Parameters.AddWithValue("@Code", txtCode.Text.ToString().Trim());
                        cmd.Parameters.AddWithValue("@Desc", txtDesc.Text.ToString().Trim());
                        cmd.Parameters.AddWithValue("@Sec", section);
                        cmd.Parameters.AddWithValue("@LeafNo", ddlLeaf.SelectedValue);
                        //cmd.Parameters.AddWithValue("@Wt", (ShWt - Lwt).ToString());
                        cmd.Parameters.AddWithValue("@Lp", Convert.ToDouble(lessP.Text.ToString().Trim()));
                        cmd.Parameters.AddWithValue("@Lwt", Lwt);
                        cmd.Parameters.AddWithValue("@Thick", Convert.ToDouble( txtThickness.Text.ToString().Trim()));
                        cmd.Parameters.AddWithValue("@Width", Convert.ToDouble(txtWidth.Text.ToString().Trim()));
                        cmd.Parameters.AddWithValue("Ss",Convert.ToDouble(txtSs.Text.ToString().Trim()));
                        cmd.Parameters.AddWithValue("@Cat", ddlCat.SelectedValue);
                        cmd.Parameters.AddWithValue("@Type",ddlType.SelectedValue );
                        cmd.Parameters.AddWithValue("@USER", Session["userID"].ToString());
                        cmd.Parameters.AddWithValue("@Cdt", DateTime.Now);


                        cmd.Connection = con;
                        int Ins_IM = 0;
                        try
                        {
                            Ins_IM = cmd.ExecuteNonQuery();
                            if (Ins_IM > 0)
                            {
                              
                                string message = "alert('Item Added!!.')";
                                ScriptManager.RegisterClientScriptBlock((sender as Control), this.GetType(), "alert", message, true);
                            }
                            else
                            {
                                string message = "alert('Error in btnSave_Click. Please contact IT team!!')";
                                ScriptManager.RegisterClientScriptBlock((sender as Control), this.GetType(), "alert", message, true);
                            }
                        }
                        catch (Exception ex)
                        {


                            string message =  "alert('Error in CATCH BLOCK OF btnSave_Click. Please contact IT team!!')";
                            ScriptManager.RegisterClientScriptBlock((sender as Control), this.GetType(), "alert", message, true);
                        }







                    }
                }
                resetControls();

            }
            catch (Exception ex)
            {

            }
        }

    }
    protected void resetControls()
    {
        try
        {
           txtDesc.Text = string.Empty;
            txtSs.Text = string.Empty;
            txtThickness.Text = string.Empty;   
            txtWidth.Text = string.Empty;    
            txtDesc.Text = string.Empty;
            ddlPartyName.SelectedIndex = 0;
            ddlType.SelectedIndex = 0;
            ddlCat.SelectedIndex = 0;
            ddlLeaf.SelectedIndex = 0;
            LoadCode();
        }
        catch (Exception ex)
        {
        }
    }
    [WebMethod]
    public static string GetItems(string searchTerm, int pageIndex)
   {
        string query = "GetItemList";
        SqlCommand cmd = new SqlCommand(query);
        cmd.CommandType = CommandType.StoredProcedure;
        cmd.Parameters.AddWithValue("@SearchTerm", searchTerm);
      //  cmd.Parameters.AddWithValue("@PageIndex", pageIndex);
       // cmd.Parameters.AddWithValue("@PageSize", PageSize);
        cmd.Parameters.Add("@RecordCount", SqlDbType.Int, 4).Direction = ParameterDirection.Output;
        return GetData(cmd, pageIndex).GetXml();
    }

    private static DataSet GetData(SqlCommand cmd, int pageIndex)
   {
        string constr1 = ConfigurationManager.ConnectionStrings["SONIC_PRDConnectionString"].ConnectionString;
        using (SqlConnection con = new SqlConnection(constr1))
        {
            using (SqlDataAdapter sda = new SqlDataAdapter())
            {
                cmd.Connection = con;
                sda.SelectCommand = cmd;
                using (DataSet ds = new DataSet())
                {
                    sda.Fill(ds, "Items");
                    DataTable dt = new DataTable("Pager");
                   // dt.Columns.Add("PageIndex");
                  //  dt.Columns.Add("PageSize");
                    dt.Columns.Add("RecordCount");
                    dt.Rows.Add();
                   // dt.Rows[0]["PageIndex"] = pageIndex;
                   // dt.Rows[0]["PageSize"] = PageSize;
                    dt.Rows[0]["RecordCount"] = cmd.Parameters["@RecordCount"].Value;
                    ds.Tables.Add(dt);
                    return ds;
                }
            }
        }
    }
    protected bool CheckValidation()
    {
        bool valid  = false;
        try
        {
            if (txtCode.Text != " " && txtDesc.Text != " " && ddlCat.SelectedIndex != 0 && ddlLeaf.Text != " " && txtSs.Text != " " && txtThickness.Text != " " && ddlPartyName.SelectedIndex != 0 && txtWidth.Text != " " && lessP.Text !=" " && ddlCat.SelectedIndex != 0 && ddlType.SelectedIndex != 0)
            {
                valid = true;
            }
        }
        catch (Exception ex) 
        { 
            valid = false;
        }
        return valid;
    }

    protected void gvItemMaster_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            e.Row.Attributes["onclick"] = Page.ClientScript.GetPostBackClientHyperlink(gvItemMaster, "Select$" + e.Row.RowIndex);
            e.Row.ToolTip = "Click to select this row.";
        }
    }

   


    protected void gvItemMaster_SelectedIndexChanged(object sender, EventArgs e)
    {
        foreach (GridViewRow row in gvItemMaster.Rows)
        {
            if (row.RowIndex == gvItemMaster.SelectedIndex)
            {
                row.BackColor = ColorTranslator.FromHtml("#A1DCF2");
                row.ToolTip = string.Empty;
            }
            else
            {
                row.BackColor = ColorTranslator.FromHtml("#FFFFFF");
                row.ToolTip = "Click to select this row.";
            }
        }
    }
}