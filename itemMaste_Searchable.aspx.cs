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
using System.Activities.Expressions;
using System.Globalization;

public partial class itemMaste : System.Web.UI.Page
{
    string constr = ConfigurationManager.ConnectionStrings["SONIC_PRDConnectionString"].ConnectionString;
    private static int PageSize = 500;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!this.IsPostBack)
        {

            if (Session["userID"] == null)
            {
                Response.Redirect("LoginPage.aspx");
            }
            //this.BindGrid();
            try
            {
                LoadCode();
               // LoadParty();
                LoadLeaf();
                //BindGrid();

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
    private void BindGrid_Search()
    {
        if (txtSearch.Text.Trim().Length > 0)
        {
            using (SqlConnection con = new SqlConnection(constr))
            {
                //using (SqlCommand cmd = new SqlCommand("Customers_CRUD"))
                using (SqlCommand cmd = new SqlCommand("SELECT * FROM T_ITEMMASTER where t_im_desc like '%' + @descrip + '%'"))
                {
                    // cmd.Parameters.AddWithValue("@Action", "SELECT");
                    cmd.Parameters.AddWithValue("@descrip",txtSearch.Text.ToString().Trim());
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
        //string constr = ConfigurationManager.ConnectionStrings["SONIC_PRDConnectionString"].ConnectionString;
        else
        {
            BindGrid();
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
    //protected void LoadParty()
    //{
    //    try
    //    {
    //        using (SqlConnection con = new SqlConnection(constr))
    //        {
    //            using (SqlCommand cmd = new SqlCommand("select * from sonic_prd.dbo.T_ITEM_PARTY_MASTER order by ID desc"))
    //            {
    //                cmd.CommandType = CommandType.Text;
    //                cmd.Connection = con;
    //                con.Open();
    //                SqlDataAdapter adp = new SqlDataAdapter(cmd);
    //                DataTable dt = new DataTable();
    //                adp.Fill(dt);
    //                ddlPartyName.DataSource = dt;
    //                ddlPartyName.DataTextField = "T_ITEM_PARTY_NAME";
    //                ddlPartyName.DataValueField = "ID";
    //                ddlPartyName.DataBind();


    //            }
    //        }
    //        ddlPartyName.Items.Insert(0, new ListItem("--Select Party Name--", "0"));
    //    }
    //    catch (Exception ex)
    //    {
    //    }

    //}
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

   

 

    

   
    



   

    protected void gvItemMaster_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        gvItemMaster.PageIndex = e.NewPageIndex;
        this.BindGrid_Search();
    }

    protected void txtSearch_TextChanged(object sender, EventArgs e)
    {
        BindGrid_Search();
    }

    protected void chkSelect_CheckedChanged(object sender, EventArgs e)
    {
        
    }

    protected void gvItemMaster_SelectedIndexChanged(object sender, EventArgs e)
    {
        BindGrid_Search();
        GridViewRow row = gvItemMaster.SelectedRow;
        row.BackColor=System.Drawing.Color.Yellow;
       row.ForeColor=System.Drawing.Color.Black;
        row.Font.Bold=true;
        txtCode.Text = row.Cells[0].Text;
        //txtName.Text = row.Cells[1].Text;
        //txtCountry.Text = (row.FindControl("lblCountry") as Label).Text;
        ViewState["id"] = row.Cells[0].Text;
        txtCode.Text = row.Cells[1].Text;
        //ddlLeaf.SelectedValue = row.Cells[4].Text;
        txtDesc.Text = row.Cells[2].Text;
        ddlCat.SelectedValue = row.Cells[11].Text;
        ddlType.SelectedValue = row.Cells[12].Text;
        txtSs.Text = row.Cells[10].Text;
        txtWidth.Text = row.Cells[9].Text;
        txtThickness.Text = row.Cells[8].Text;
    }

    protected void btnSave_Click(object sender, EventArgs e)
    {
        if (!string.IsNullOrEmpty(txtCode.Text) && ddlLeaf.SelectedIndex != 0 && !string.IsNullOrEmpty(txtDesc.Text) && ddlCat.SelectedIndex != 0 && ddlType.SelectedIndex != 0 && !string.IsNullOrEmpty(txtSs.Text) && !string.IsNullOrEmpty(txtWidth.Text) && !string.IsNullOrEmpty(txtThickness.Text))
        {
            {
                try
                {
                    //
                    Double weight = 0;
                    Double weit = (Convert.ToDouble(txtThickness.Text.ToString().Trim()) * Convert.ToDouble(txtWidth.Text.ToString().Trim()) * Convert.ToDouble(txtSs.Text.ToString().Trim()) * 7.85) / 1000000;
                    Double lessWt = (weit * 3) / 100;
                    weight  =  weit - lessWt;
                    string section = txtWidth.Text.ToString().Trim() + "X" + txtThickness.Text.ToString().Trim();

                    //

                    using (SqlConnection con = new SqlConnection(constr))
                    {
                        con.Open();
                        
                        using (SqlCommand cmd = new SqlCommand("INSERT INTO [dbo].[T_ITEMMASTER]      ([T_IM_CODE],[T_IM_DESC],[T_IM_SECTION],[T_IM_LEAF_NO],[T_IM_NEW_CODE],[T_IM_LESSPER],[T_IM_WEIGHT],[T_IM_LESSWEIGHT],[T_IM_THICKNESS],[T_IM_WIDTH],[T_IM_SHEARSIZE],[T_IM_DESCRIPTION],[T_IM_CAT],[T_IM_TYPE],[T_IM_CREATED_BY],[T_IM_CREATED_DT])   VALUES       (@IMCode,@decsr,@sec,@leafNo,           @NewCode  ,  @Lp ,  @Wt       ,@LWt          ,@Thick           ,@Width           ,@Ss ,@Description,@Cat   ,@Type   ,@Cby   ,@Cdt)"))

                        {
                            cmd.Parameters.AddWithValue("@IMCode", txtCode.Text.ToString().Trim());
                            cmd.Parameters.AddWithValue("@decsr", txtDesc.Text.ToUpper().ToString().Trim());
                            cmd.Parameters.AddWithValue("@sec",section);
                            cmd.Parameters.AddWithValue("@leafNo", ddlLeaf.SelectedValue.ToUpper());
                            cmd.Parameters.AddWithValue("@NewCode", "");
                            cmd.Parameters.AddWithValue("@Wt", weight);
                            cmd.Parameters.AddWithValue("@Lp", lessP.Text.ToString());
                            cmd.Parameters.AddWithValue("@LWt", lessWt);
                            cmd.Parameters.AddWithValue("@Thick", txtThickness.Text.ToString().Trim());
                            cmd.Parameters.AddWithValue("@Width", txtWidth.Text.ToString().Trim());
                            //cmd.Parameters.AddWithValue("@TotalAmnt", (float)Convert.ToDouble(txtQty.Text) * (float)Convert.ToDouble(txtRate.Text));
                            cmd.Parameters.AddWithValue("@Ss", txtSs.Text.ToString().Trim());
                            cmd.Parameters.AddWithValue("@Description", txtDesc.Text.ToUpper().ToString().Trim() + '|' + section + '|' + txtSs.Text.ToString().Trim() + '|' + ddlLeaf.SelectedValue.ToUpper());
                            cmd.Parameters.AddWithValue("@Cat", ddlCat.SelectedValue);
                            cmd.Parameters.AddWithValue("@Type", ddlType.SelectedValue);
                            cmd.Parameters.AddWithValue("@Cby", Session["userID"].ToString().ToUpper());
                            cmd.Parameters.AddWithValue("@Cdt", DateTime.Now);

                            cmd.Connection = con;
                            int Ins_RMM = 0;
                            try
                            {
                                Ins_RMM = cmd.ExecuteNonQuery();
                                if (Ins_RMM > 0)
                                {
                                    string message = "alert('Item Added')";
                                    ScriptManager.RegisterClientScriptBlock((sender as Control), this.GetType(), "alert", message, true);
                                }

                            }
                            catch (Exception ex) 
                            {
                               

                            }







                        }
                    }

                    //resetControls();
                }
                catch (Exception ex)
                {

                }


            }
        }
        else
        {
            string message = "alert('Please fill all the details')";
            ScriptManager.RegisterClientScriptBlock((sender as Control), this.GetType(), "alert", message, true);
        }
    }

    protected void btnUpdate_Click(object sender, EventArgs e)
    {
        if (!string.IsNullOrEmpty(txtCode.Text) && ddlLeaf.SelectedIndex != 0 && !string.IsNullOrEmpty(txtDesc.Text) && ddlCat.SelectedIndex != 0 && ddlType.SelectedIndex != 0 && !string.IsNullOrEmpty(txtSs.Text) && !string.IsNullOrEmpty(txtWidth.Text) && !string.IsNullOrEmpty(txtThickness.Text))
        {
            {
                try
                {
                    //
                    Double weight = 0;
                    Double weit = (Convert.ToDouble(txtThickness.Text.ToString().Trim()) * Convert.ToDouble(txtWidth.Text.ToString().Trim()) * Convert.ToDouble(txtSs.Text.ToString().Trim()) * 7.85) / 1000000;
                    Double lessWt = (weit * 3) / 100;
                    weight = weit - lessWt;

                    //

                    using (SqlConnection con = new SqlConnection(constr))
                    {
                        con.Open();

                        using (SqlCommand cmd = new SqlCommand("UPDATE [dbo].[T_ITEMMASTER] SET [T_IM_DESC] = @decsr ,[T_IM_SECTION] = @sec ,[T_IM_LEAF_NO] = @leafNo ,[T_IM_NEW_CODE] = @NewCode ,[T_IM_LESSPER] = @Lp ,[T_IM_WEIGHT] = @Wt ,[T_IM_LESSWEIGHT] = @LWt,[T_IM_THICKNESS] = @Thick ,[T_IM_WIDTH] = @Width  ,[T_IM_SHEARSIZE] = @Ss,[T_IM_CAT] = @Cat ,[T_IM_TYPE] = @Type,[T_IM_CREATED_BY] = @Cby ,[T_IM_CREATED_DT] = @Cdt  WHERE T_IM_CODE = @IMCode"))

                        {
                            cmd.Parameters.AddWithValue("@IMCode", txtCode.Text.ToString().Trim());
                            cmd.Parameters.AddWithValue("@decsr", txtDesc.Text.ToUpper().ToString().Trim());
                            cmd.Parameters.AddWithValue("@sec", txtWidth.Text.ToString().Trim() + "X" + txtThickness.Text.ToString().Trim());
                            cmd.Parameters.AddWithValue("@leafNo", ddlLeaf.SelectedValue.ToUpper());
                            cmd.Parameters.AddWithValue("@NewCode", "");
                            cmd.Parameters.AddWithValue("@Wt", weight);
                            cmd.Parameters.AddWithValue("@Lp", lessP.Text.ToString());
                            cmd.Parameters.AddWithValue("@LWt", lessWt);
                            cmd.Parameters.AddWithValue("@Thick", txtThickness.Text.ToString().Trim());
                            cmd.Parameters.AddWithValue("@Width", txtWidth.Text.ToString().Trim());
                            //cmd.Parameters.AddWithValue("@TotalAmnt", (float)Convert.ToDouble(txtQty.Text) * (float)Convert.ToDouble(txtRate.Text));
                            cmd.Parameters.AddWithValue("@Ss", txtSs.Text.ToString().Trim());
                            cmd.Parameters.AddWithValue("@Cat", ddlCat.SelectedValue);
                            cmd.Parameters.AddWithValue("@Type", ddlType.SelectedValue);
                            cmd.Parameters.AddWithValue("@Cby", Session["userID"].ToString().ToUpper());
                            cmd.Parameters.AddWithValue("@Cdt", DateTime.Now);

                            cmd.Connection = con;
                            int Ins_RMM = 0;
                            try
                            {
                                Ins_RMM = cmd.ExecuteNonQuery();
                                if (Ins_RMM > 0)
                                {
                                    string message = "alert('Item Added')";
                                    ScriptManager.RegisterClientScriptBlock((sender as Control), this.GetType(), "alert", message, true);
                                }

                            }
                            catch
                            {

                            }







                        }
                    }

                    //resetControls();
                }
                catch (Exception ex)
                {

                }


            }
        }
        else
        {

        }
    }
}
