using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Drawing;
using System.Globalization;
using static System.Runtime.CompilerServices.RuntimeHelpers;

public partial class ShearingEntry : System.Web.UI.Page
{
    string ShNo = "";
    string constr = ConfigurationManager.ConnectionStrings["SONIC_PRDConnectionString"].ConnectionString;
    protected void Page_Load(object sender, EventArgs e)
    {
        string constr = ConfigurationManager.ConnectionStrings["SONIC_PRDConnectionString"].ConnectionString;
        if (!IsPostBack)
        {
            try
            {
                ((Label)Master.FindControl("label1")).Text = "Shearing Entry";
                loadItemMaster();
                ShNo = LoadShNo();
                //BindShData(ShNo);
            }
            catch (Exception ex)
            {
            }

        }

    }
    protected string LoadShNo()
    {
        DataTable dt = new DataTable();
        string Sno = "";
        try
        {
            using (SqlConnection con = new SqlConnection(constr))
            {

                using (SqlCommand cmd = new SqlCommand("select max(T_SM_SHR_NO) + 1 as ShNo from sonic_prd.dbo.T_SHEARING_MASTER"))

                {
                    cmd.CommandType = CommandType.Text;
                    cmd.Connection = con;
                    con.Open();
                    SqlDataAdapter adp = new SqlDataAdapter(cmd);
                    adp.Fill(dt);
                    if (dt.Rows.Count > 0) {

                        txtShNo.Text = dt.Rows[0]["ShNo"].ToString();
                        Sno = txtShNo.Text;
                    }



                }
            }

        }
        catch (Exception ex)
        {
        }
        return Sno;
    }
    protected void loadItemMaster()
    {
        try
        {
            using (SqlConnection con = new SqlConnection(constr))
            {
                //using (SqlCommand cmd = new SqlCommand("select  T_IM_DESC +  ' '+'||' + ' '+ T_IM_SECTION +  ' '+'||' + ' '+ CONVERT(VARCHAR,T_IM_SHEARSIZE) +  ' '+'||' + ' '+ T_IM_LEAF_NO AS TEST from sonic_prd.dbo.T_ITEMMASTER order by T_IM_CODE  asc"))
                //    using (SqlCommand cmd = new SqlCommand("select * from sonic_prd.dbo.T_ITEMMASTER order by T_IM_CODE  asc"))

                //    {
                //        cmd.CommandType = CommandType.Text;
                //        cmd.Connection = con;
                //        con.Open();
                //        SqlDataAdapter adp = new SqlDataAdapter(cmd);
                //        DataSet ds = new DataSet();
                //        //DataTable dt = new DataTable();
                //        adp.Fill(ds);
                //        ds.Tables[0].Columns.Add("Description", typeof(string), "T_IM_DESC + '|' +T_IM_SECTION + '|' +  T_IM_SHEARSIZE + '|' + T_IM_LEAF_NO ");

                //        ddlDesc.DataValueField = "T_IM_CODE";
                //        ddlDesc.DataTextField = "Description";
                //        ddlDesc.DataSource = ds;
                //        ddlDesc.DataBind();


                //    }
                //}
                using (SqlCommand cmd = new SqlCommand("select * from sonic_prd.dbo.T_ITEMMASTER order by T_IM_CODE  asc"))

                {
                    cmd.CommandType = CommandType.Text;
                    cmd.Connection = con;
                    con.Open();
                    SqlDataAdapter adp = new SqlDataAdapter(cmd);
                    DataSet ds = new DataSet();
                    DataTable dt = new DataTable();
                    adp.Fill(dt);
                    
                    ddlDesc.DataValueField = "T_IM_CODE";
                    ddlDesc.DataTextField = "T_IM_DESCRIPTION";
                    ddlDesc.DataSource = dt;
                    ddlDesc.DataBind();


                }
            }
            ddlDesc.Items.Insert(0, new ListItem("--Select Item--", "0"));
        }
        catch (Exception ex)
        {
        }

    }
    protected void BindShData(string Sno)
    {
        DataTable dt = new DataTable();

        try
        {

            using (SqlConnection con = new SqlConnection(constr))
            {

                using (SqlCommand cmd = new SqlCommand("select T_SM_SHR_NO as ShNo, T_SM_STATUS as Status ,T_SM_CODE as Code , T_SM_DESC as Descrp,T_SM_SECTION as Section,T_SM_LEAF_NO as LeafNo,T_SM_QTY as Qty,T_SM_UNIT_WEIGHT as UnitWeight,T_SM_WEIGHT as Weight,T_SM_UNIT as Unit  from sonic_prd.dbo.T_SHEARING_MASTER where T_SM_SHR_NO = @ShNo "))
                //using (SqlCommand cmd = new SqlCommand("select *  from sonic_prd.dbo.T_SHEARING_MASTER where T_SM_SHR_NO = @ShNo "))
                {
                    cmd.Parameters.AddWithValue("@ShNo", Sno);
                    cmd.CommandType = CommandType.Text;
                    cmd.Connection = con;
                    con.Open();
                    SqlDataAdapter adp = new SqlDataAdapter(cmd);
                    adp.Fill(dt);
                    if (dt.Rows.Count > 0)
                    {
                        GridView1.DataSource = dt;
                        GridView1.DataBind();


                    }



                }
            }

        }
        catch (Exception ex)
        {
        }
    }
    protected void btnSave_Click(object sender, EventArgs e)
    {
        if (txtDate.Text != " " && txtShNo.Text != " " && ddlDesc.SelectedIndex != 0 && ddlUnit.SelectedIndex != 0)
        {
            try
            {
                //
                string Descrp = ddlDesc.SelectedItem.Text;
                char separator = '|'; // Space character.
                string[] words = Descrp.Split(separator); // returned array.
                DataTable dtItemMaster = GetDetails(words);
                CultureInfo provider = CultureInfo.InvariantCulture;
                String dateString = txtDate.Text;
                // Exception: String was not recognized as a valid DateTime because the day of week was incorrect.
                DateTime dateTime13 = DateTime.ParseExact(dateString, "dd-MM-yyyy", provider);

                //

                using (SqlConnection con = new SqlConnection(constr))
                {
                    con.Open();

                    using (SqlCommand cmd = new SqlCommand("INSERT INTO T_SHEARING_MASTER ([T_SM_SHR_NO]           ,[T_SM_STATUS]           ,[T_SM_CODE]           ,[T_SM_UNIT]           ,[T_SM_DATE]           ,[T_SM_DESC]           ,[T_SM_SHEAR_SIZE]           ,[T_SM_SECTION]           ,[T_SM_LEAF_NO]           ,[T_SM_QTY]      ,T_SM_UNIT_WEIGHT     ,[T_SM_WEIGHT]           ,[T_SM_CREATED_BY]           ,[T_SM_CREATED_DT]) VALUES  (@ShnNo,@Status,@ItemCode,@UnitNo,@ShDate,@Descr,@Ss,@Sec,@LeafNo,@Qty,@UnitWt,@Wt,@Cby,@Cdt)"))
                    {
                        cmd.Parameters.AddWithValue("@ShnNo", txtShNo.Text.ToString().Trim());
                        cmd.Parameters.AddWithValue("@Status", "ENTERED");
                        cmd.Parameters.AddWithValue("@ItemCode", dtItemMaster.Rows[0]["T_IM_CODE"].ToString().Trim());
                        cmd.Parameters.AddWithValue("@UnitNo", ddlUnit.SelectedValue);
                        cmd.Parameters.AddWithValue("@ShDate", dateTime13);
                        cmd.Parameters.AddWithValue("@Descr", words[0]);
                        cmd.Parameters.AddWithValue("@Ss", words[2]);
                        cmd.Parameters.AddWithValue("@Sec", words[1]);
                        cmd.Parameters.AddWithValue("@LeafNo", words[3]);
                        cmd.Parameters.AddWithValue("@Qty", txtQty.Text.Trim());
                        cmd.Parameters.AddWithValue("@UnitWt", dtItemMaster.Rows[0]["T_IM_LESSWEIGHT"].ToString().Trim());
                        cmd.Parameters.AddWithValue("@Wt", Convert.ToDouble(dtItemMaster.Rows[0]["T_IM_LESSWEIGHT"].ToString().Trim()) * Convert.ToDouble(txtQty.Text.Trim()));
                        cmd.Parameters.AddWithValue("@Cby", Session["userID"].ToString());
                        cmd.Parameters.AddWithValue("@Cdt", DateTime.Now);

                        cmd.Connection = con;
                        int Ins_RMM = 0;
                        try
                        {
                            Ins_RMM = cmd.ExecuteNonQuery();
                            if (Ins_RMM > 0)
                            {
                                string message = "alert('Shearing Entry Added !!!')";
                                ScriptManager.RegisterClientScriptBlock((sender as Control), this.GetType(), "alert", message, true);
                            }

                        }
                        catch
                        {

                        }







                    }
                }
                // BindRMgrid();
                resetControls();
            }
            catch (Exception ex)
            {

            }


        }
        else
        {

        }
    }
    protected void resetControls()
    {
        try
        {
            txtShNo.Enabled = false;
            txtDate.Text = string.Empty;
            txtQty.Text = string.Empty;

        }
        catch (Exception ex)
        {
        }
    }
    protected DataTable GetDetails(String[] words)
    {
        DataTable dt = new DataTable();
        try
        {
            using (SqlConnection con = new SqlConnection(constr))
            {

                using (SqlCommand cmd = new SqlCommand("select * from sonic_prd.dbo.T_ITEMMASTER where T_IM_DESC = @descr and T_IM_SECTION = @Sec and T_IM_LEAF_NO = @LeafNo"))

                {
                    cmd.Parameters.AddWithValue("@descr", words[0]);
                    cmd.Parameters.AddWithValue("@Sec", words[1]);
                    cmd.Parameters.AddWithValue("@LeafNo", words[3]);
                    cmd.CommandType = CommandType.Text;
                    cmd.Connection = con;
                    con.Open();
                    SqlDataAdapter adp = new SqlDataAdapter(cmd);
                    DataSet ds = new DataSet();
                    adp.Fill(dt);

                }
            }
        }
        catch (Exception ex)
        {
        }
        return dt;
    }

    protected void GridView1_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            BindShData(txtShNo.Text.Trim());

            GridViewRow row = GridView1.SelectedRow;

            row.Font.Bold = true;

            row.BackColor = System.Drawing.Color.Yellow;
            row.ForeColor = System.Drawing.Color.Black;
            row.Font.Bold = true;
            txtShNo.Text = row.Cells[1].Text;
            txtQty.Text = row.Cells[7].Text;
            ddlUnit.SelectedValue = row.Cells[10].Text;
            ddlDesc.SelectedValue = row.Cells[3].Text;


            //
            //BindGrid_Search();
            //GridViewRow row = gvItemMaster.SelectedRow;
            //row.BackColor = System.Drawing.Color.Yellow;
            //row.ForeColor = System.Drawing.Color.Black;
            //row.Font.Bold = true;
            //txtCode.Text = row.Cells[0].Text;
            ////txtName.Text = row.Cells[1].Text;
            ////txtCountry.Text = (row.FindControl("lblCountry") as Label).Text;
            //ViewState["id"] = row.Cells[0].Text;
            //txtCode.Text = row.Cells[1].Text;
            ////ddlLeaf.SelectedValue = row.Cells[4].Text;
            //txtDesc.Text = row.Cells[2].Text;
            //ddlCat.SelectedValue = row.Cells[11].Text;
            //ddlType.SelectedValue = row.Cells[12].Text;
            //txtSs.Text = row.Cells[10].Text;
            //txtWidth.Text = row.Cells[9].Text;
            //txtThickness.Text = row.Cells[8].Text;
        }
        catch (System.Exception ex)
        {

        }

    }



    protected void btnNew_Click(object sender, EventArgs e)
    {
        Response.Redirect(Request.RawUrl);
    }

    protected void btnSearch_Click(object sender, EventArgs e)
    {
        BindShData(txtShNo.Text.Trim());

    }

    protected void btnUpdate_Click(object sender, EventArgs e)
    {

        if (txtDate.Text != " " && txtShNo.Text != " " && ddlDesc.SelectedIndex != 0 && ddlUnit.SelectedIndex != 0)
        {
            try
            {
                //

                string Descrp = ddlDesc.SelectedItem.Text;
                char separator = '|'; // Space character.
                string[] words = Descrp.Split(separator); // returned array.
                DataTable dtItemMaster = GetDetails(words);
                CultureInfo provider = CultureInfo.InvariantCulture;
                String dateString = txtDate.Text;
                // Exception: String was not recognized as a valid DateTime because the day of week was incorrect.
                DateTime dateTime13 = DateTime.ParseExact(dateString, "dd-MM-yyyy", provider);

                //

                using (SqlConnection con = new SqlConnection(constr))
                {
                    con.Open();

                    using (SqlCommand cmd = new SqlCommand("UPDATE T_SHEARING_MASTER SET [T_SM_SHR_NO] =  @ShnNo  ,[T_SM_UNIT] = @UnitNo,[T_SM_UPDATE_DATE] = @ShDate,[T_SM_DESC] = @Descr,          [T_SM_QTY] = @Qty ,T_SM_UPDATED_BY = @Uby , T_SM_UPDATD_DT = @Udt where [T_SM_SHR_NO]=@pShNo and [T_SM_CODE]=@pShCode and T_SM_DESC = @pShDesc and T_SM_SHEAR_SIZE = @pSs and T_SM_SECTION =@pSec and T_SM_LEAF_NO = @pLeafNo"))
                    {
                        cmd.Parameters.AddWithValue("@ShnNo", txtShNo.Text.ToString().Trim());
                        cmd.Parameters.AddWithValue("@UnitNo", ddlUnit.SelectedValue);
                        cmd.Parameters.AddWithValue("@ShDate", dateTime13);
                        cmd.Parameters.AddWithValue("@Descr", words[0]);
                        cmd.Parameters.AddWithValue("@Qty", txtQty.Text.Trim());
                        cmd.Parameters.AddWithValue("@Uby", Session["userID"].ToString());
                        cmd.Parameters.AddWithValue("@Udt", DateTime.Now);
                        //
                        cmd.Parameters.AddWithValue("@pShCode", dtItemMaster.Rows[0]["T_IM_CODE"].ToString().Trim());
                        cmd.Parameters.AddWithValue("@pShNo", txtShNo.Text.Trim());
                        cmd.Parameters.AddWithValue("@pShDesc", dtItemMaster.Rows[0]["T_IM_DESC"].ToString().Trim());
                        cmd.Parameters.AddWithValue("@pSs", dtItemMaster.Rows[0]["T_IM_SHEARSIZE"].ToString().Trim());
                        cmd.Parameters.AddWithValue("@pSec", dtItemMaster.Rows[0]["T_IM_SECTION"].ToString().Trim());
                        cmd.Parameters.AddWithValue("@pLeafNo",  dtItemMaster.Rows[0]["T_IM_LEAF_NO"].ToString().Trim());
                          cmd.Connection = con;
                        int Ins_RMM = 0;
                        try
                        {
                            Ins_RMM = cmd.ExecuteNonQuery();
                            if (Ins_RMM > 0)
                            {
                                string message = "alert('Shearing Entry Updated !!!')";
                                ScriptManager.RegisterClientScriptBlock((sender as Control), this.GetType(), "alert", message, true);
                            }

                        }
                        catch
                        {

                        }







                    }
                    // BindRMgrid();
                    resetControls();
                    Response.Redirect(Request.RawUrl);

                }
            }


            catch (Exception ex)
            {
            }
        }  
    
        else
        {

        }
    }
}