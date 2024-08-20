using System;
using System.Activities.Expressions;
using System.Activities.Statements;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Globalization;
using System.IdentityModel.Protocols.WSTrust;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml.Linq;

public partial class MiascAddSub : System.Web.UI.Page
{
    DataClassesDataContext sonicoPrd = new DataClassesDataContext("SONIC_PRDConnectionString");
    string constr = ConfigurationManager.ConnectionStrings["SONIC_PRDConnectionString"].ConnectionString;

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {

            if (Session["userID"] == null)
            {
                Response.Redirect("LoginPage.aspx");
            }
            try
            {
                ((Label)Master.FindControl("label1")).Text = "Misc Add Sub Entry";

               
                    try
                    {
                        LoadSection();
                    
                        //BindRejgrid();

                    
                }
                    catch (Exception ex)
                    {
                    }

                

            }
            catch (Exception ex)
            {
            }
          
        }
    }

    
    protected void LoadSection()
    {
        try
        {
            using (SqlConnection con = new SqlConnection(constr))
            {
                using (SqlCommand cmd = new SqlCommand("SELECT *  FROM [SONIC_PRD].[dbo]. T_SECTION_MASTER"))
                {
                    cmd.CommandType = CommandType.Text;
                    cmd.Connection = con;
                    con.Open();
                    SqlDataAdapter adp2 = new SqlDataAdapter(cmd);
                    DataTable dt2 = new DataTable();
                    adp2.Fill(dt2);
                    ddlSection.DataSource = dt2;
                    ddlSection.DataTextField = "T_SECTIONM_DIMN";
                    ddlSection.DataValueField = "T_SECTIONM_DIMN";
                    ddlSection.DataBind();
                    con.Close();
                }
            }
            ddlSection.Items.Insert(0, new ListItem("--Select Section--", "0"));
        }
        catch (Exception ex)
        {
        }

    }

    protected void BindMisAddgrid()
    {
        try
        {
            using (SqlConnection con = new SqlConnection(constr))
            {
                using (SqlCommand cmd = new SqlCommand("SELECT T_RMMISCM_SECTION as Section,T_RMMISCM_OPERN as Operation,T_RMMISCM_QTY as Quantity,T_RMMISCM_REM as Remarks  FROM [SONIC_PRD].[dbo].[T_RM_MISC_ADD_SUB_MASTER] WHERE  MONTH(T_RMMISCM_CREATED_DT) = MONTH(GETDATE()) and T_RMMISCM_SECTION = @Sec and T_RMMISCM_OPERN = 'ADD' "))
                {
                    cmd.CommandType = CommandType.Text;
                    cmd.Connection = con;
                    cmd.Parameters.AddWithValue("@Sec", ddlSection.SelectedValue);
                    con.Open();
                    SqlDataAdapter adp1 = new SqlDataAdapter(cmd);
                    DataTable dt1 = new DataTable();
                    adp1.Fill(dt1);
                    Gv_MiscAddSub.DataSource = dt1;
                    Gv_MiscAddSub.DataBind();
                    ViewState["MiscAddSub"] = dt1;
                    con.Close();
                }
            }
        }
        catch (Exception ex) { }

    }

    
protected void BindMisSubgrid()
    {
        try
        {
            using (SqlConnection con = new SqlConnection(constr))
            {
                using (SqlCommand cmd = new SqlCommand("SELECT T_RMMISCM_SECTION as Section,T_RMMISCM_OPERN as Operation,T_RMMISCM_QTY as Quantity,T_RMMISCM_REM as Remarks  FROM [SONIC_PRD].[dbo].[T_RM_MISC_ADD_SUB_MASTER] WHERE  MONTH(T_RMMISCM_CREATED_DT) = MONTH(GETDATE()) and T_RMMISCM_SECTION = @Sec and T_RMMISCM_OPERN = 'SUB' "))
                {
                    cmd.CommandType = CommandType.Text;
                    cmd.Connection = con;
                    cmd.Parameters.AddWithValue("@Sec", ddlSection.SelectedValue);
                    con.Open();
                    SqlDataAdapter adp1 = new SqlDataAdapter(cmd);
                    DataTable dt1 = new DataTable();
                    adp1.Fill(dt1);
                    Gv_MiscAddSub.DataSource = dt1;
                    Gv_MiscAddSub.DataBind();
                    ViewState["MiscAddSub"] = dt1;
                    con.Close();
                }
            }
        }
        catch (Exception ex) { }

    }


    protected void btnAddNew_Click(object sender, EventArgs e)
    {
        try
        {
            Response.Redirect(Request.RawUrl);
        }
        
        catch (Exception ex)
        {

        }
    }

    protected void btn_rej_Click(object sender, EventArgs e)
    {
        int UpdSM = 0;
        if (txtDate.Text != " " && txtQty.Text != " " && ddlSection.SelectedIndex != 0)
        {
            try
            {
                //
                CultureInfo provider = CultureInfo.InvariantCulture;
                String dateString = txtDate.Text;
                // Exception: String was not recognized as a valid DateTime because the day of week was incorrect.
                DateTime Edate = DateTime.ParseExact(dateString, "dd-MM-yyyy", provider);

                //

                using (SqlConnection con = new SqlConnection(constr))
                {
                    DataTable dtRej = new DataTable();
                    dtRej = GetRecord();
                    con.Open();

                    using (SqlCommand cmd = new SqlCommand("INSERT INTO [SONIC_PRD].[dbo].[T_RM_REJ_MASTER] ([T_RMREJM_SECTION] ,[T_RMREJM_QTY],[T_RMREJM_STATUS],[T_RMREJM_ENTRY_DT],[T_RMREJM_CREATED_BY] ,[T_RMREJM_CREATED_DT]) VALUES  (@Section          ,@Quantity    ,@Status      ,@Edate,@USER,@Cdate)"))
                    {

                        cmd.Parameters.AddWithValue("@Edate", DateTime.ParseExact(txtDate.Text, "dd-MM-yyyy", provider)); cmd.Parameters.AddWithValue("@Section", ddlSection.SelectedValue);
                        cmd.Parameters.AddWithValue("@Quantity", txtQty.Text.ToString().Trim());
                        cmd.Parameters.AddWithValue("@Status", "ENTERED");
                        cmd.Parameters.AddWithValue("@Cdate", DateTime.Now);
                        cmd.Parameters.AddWithValue("@USER", Session["userID"].ToString());

                        cmd.Connection = con;
                        int Ins_RMM = 0;
                        try
                        {
                            Ins_RMM = cmd.ExecuteNonQuery();
                            if (Ins_RMM > 0)
                            {
                                string message = "alert('Rejection Entered!!')";
                                ScriptManager.RegisterClientScriptBlock((sender as Control), this.GetType(), "alert", message, true);
                                //ScriptManager.RegisterStartupScript(this, GetType(), "showalert", "alert('Only alert Message');", true);
                                //  ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);



                            }
                        }
                        catch (Exception ex)
                        {
                        }
                        //    using (SqlCommand cmd1 = new SqlCommand("UPDATE[dbo].[T_RM_STOCK_MASTER] SET T_RMSM_REJECTION = @Rej, T_RMSM_CL_QTY = @clQty,T_RMSM_UPDATED_BY = @Uby , T_RMSM_UPDATED_DT = @Udt where ID = @id "))
                        //{
                        //    cmd1.Parameters.Clear();
                        //    cmd1.Parameters.AddWithValue("@id", dtRej.Rows[0]["ID"].ToString());
                        //    cmd1.Parameters.AddWithValue("@Rej", txtQty.Text);
                        //    if(!string.IsNullOrEmpty(dtRej.Rows[0]["T_RMSM_CL_QTY"].ToString()))
                        //    {
                        //        cmd1.Parameters.AddWithValue("@clQty", Convert.ToInt32(dtRej.Rows[0]["T_RMSM_CL_QTY"].ToString()) - (Convert.ToInt32(txtQty.Text)));

                        //    }
                        //    else
                        //    {
                        //        cmd1.Parameters.AddWithValue("@clQty", 0 - (Convert.ToInt32(txtQty.Text)));
                        //    }

                        //    cmd1.Parameters.AddWithValue("@Uby", Session["userID"].ToString());
                        //    cmd1.Parameters.AddWithValue("@Udt", DateTime.Now);


                        //    cmd1.Connection = con;

                        //    try
                        //    {
                        //        Ins_RMM = cmd.ExecuteNonQuery();

                        //        if (Ins_RMM > 0)
                        //        {
                        //            UpdSM = cmd1.ExecuteNonQuery();
                        //        }
                        //        if (Ins_RMM > 0 && UpdSM > 0)
                        //        {
                        //            trans.Commit();
                        //        }
                        //        else
                        //        {
                        //            trans.Rollback();
                        //            string message = "alert('Error  Please contact IT team!!')";
                        //            ScriptManager.RegisterClientScriptBlock((sender as Control), this.GetType(), "alert", message, true);
                        //        }


                        //    }

                        //    catch
                        //    {
                        //        string message = "alert('Error in  btn_rej_Click. Please contact IT team!!')";
                        //        ScriptManager.RegisterClientScriptBlock((sender as Control), this.GetType(), "alert", message, true);
                        //    }







                        //    BindRejgrid();
                        //}
                     
                        resetControls();
                    }
                }
            }
            catch (Exception ex)
            {
                string message = "alert('Error in  btn_rej_Click. Please contact IT team!!')";
                ScriptManager.RegisterClientScriptBlock((sender as Control), this.GetType(), "alert", message, true);
            }
        }
    }


    protected DataTable GetRecord()
    {
        DataTable dt = new DataTable();
        try
        {
            using (SqlConnection con = new SqlConnection(constr))
            {
                using (SqlCommand cmd = new SqlCommand("SELECT *  FROM [SONIC_PRD].[dbo].[T_RM_STOCK_MASTER] WHERE  MONTH(T_RMSM_CREATED_DT) = MONTH(GETDATE()) and T_RMSM_SECTION = @Sec"))
                {
                    cmd.CommandType = CommandType.Text;
                    cmd.Connection = con;
                    cmd.Parameters.AddWithValue("@Sec", ddlSection.SelectedItem.Value.ToString());
                    con.Open();
                    SqlDataAdapter adp1 = new SqlDataAdapter(cmd);
                    adp1.Fill(dt);

                    con.Close();
                }
            }
        }
        catch (Exception ex) { }
        return dt;  
    }
    protected void resetControls()
    {
        try
        {
            LoadSection();
            txtDate.Text = string.Empty;
            txtQty.Text = string.Empty;

        }
        catch (Exception ex)
        {
        }
    }
    protected int MiscAddition (String Dat , string Qty , string rem , string section,SqlConnection con, SqlTransaction trans )
    {
        int MisAdd = 0;

        if (!string.IsNullOrEmpty(txtDate.Text) && !string.IsNullOrEmpty(txtQty.Text) && !string.IsNullOrEmpty(txtRem.Text) && ddlSection.SelectedIndex != 0)
        {
            
                try
                {
                CultureInfo provider = CultureInfo.InvariantCulture;
               // using (SqlConnection con = new SqlConnection(constr))
                    //{

                        //con.Open();

                        using (SqlCommand cmd = new SqlCommand("INSERT INTO [dbo].[T_RM_MISC_ADD_SUB_MASTER]          ([T_RMMISCM_SECTION],T_RMMISCM_OPERN,[T_RMMISCM_QTY],[T_RMMISCM_REM],[T_RMMISCM_CREATED_BY],[T_RMMISCM_CREATED_DT]) VALUES  (@Sec,@Opn,@Qty,@Rem,@Cby,@Cdt)  ", con,trans)) 
                        {
                            cmd.Parameters.Clear();
                            cmd.Parameters.AddWithValue("@Sec", ddlSection.SelectedValue);
                        cmd.Parameters.AddWithValue("@Opn", "ADD");

                        cmd.Parameters.AddWithValue("@Qty", txtQty.Text.ToString().Trim());
                            cmd.Parameters.AddWithValue("@Rem", txtRem.Text.ToString().Trim());
                            cmd.Parameters.AddWithValue("@Date", DateTime.ParseExact(txtDate.Text, "dd-MM-yyyy", provider));
                            cmd.Parameters.AddWithValue("@Cby", Session["userID"].ToString());
                            cmd.Parameters.AddWithValue("@Cdt", DateTime.Now);


                            cmd.Connection = con;
                            try
                            {
                                 MisAdd = cmd.ExecuteNonQuery();
                            }
                            catch (Exception ex)
                            {

                            }



                        }



                        
                    //}

                }
                catch (Exception ex)
                {
                }     
            //
        }
        return MisAdd;
    }


    protected int MisSub(String Dat, string Qty, string rem, string section, SqlConnection con, SqlTransaction trans)
    {
        int MisSub = 0;

        if (!string.IsNullOrEmpty(txtDate.Text) && !string.IsNullOrEmpty(txtQty.Text) && !string.IsNullOrEmpty(txtRem.Text) && ddlSection.SelectedIndex != 0)
        {

            try
            {
                CultureInfo provider = CultureInfo.InvariantCulture;
                // using (SqlConnection con = new SqlConnection(constr))
                //{

                //con.Open();

                using (SqlCommand cmd = new SqlCommand("INSERT INTO [dbo].[T_RM_MISC_ADD_SUB_MASTER]          ([T_RMMISCM_SECTION],T_RMMISCM_OPERN,[T_RMMISCM_QTY],[T_RMMISCM_REM],[T_RMMISCM_CREATED_BY],[T_RMMISCM_CREATED_DT]) VALUES  (@Sec,@Opn,@Qty,@Rem,@Cby,@Cdt)  ", con, trans))
                {
                    cmd.Parameters.Clear();
                    cmd.Parameters.AddWithValue("@Sec", ddlSection.SelectedValue);
                    cmd.Parameters.AddWithValue("@Opn", "SUB");

                    cmd.Parameters.AddWithValue("@Qty", txtQty.Text.ToString().Trim());
                    cmd.Parameters.AddWithValue("@Rem", txtRem.Text.ToString().Trim());
                    cmd.Parameters.AddWithValue("@Date", DateTime.ParseExact(txtDate.Text, "dd-MM-yyyy", provider));
                    cmd.Parameters.AddWithValue("@Cby", Session["userID"].ToString());
                    cmd.Parameters.AddWithValue("@Cdt", DateTime.Now);


                    cmd.Connection = con;
                    try
                    {
                        MisSub = cmd.ExecuteNonQuery();
                    }
                    catch (Exception ex)
                    {

                    }



                }




                //}

            }
            catch (Exception ex)
            {
            }
            //
        }
        return MisSub;
    }
    protected DataTable CheckCurrentRecordStatusMiscAddSUB(String section,SqlConnection con,SqlTransaction trans)
    {
        DataTable dtCCR = new DataTable();
        try
        {
            //using (SqlConnection con = new SqlConnection(constr))
           // {

                using (SqlCommand cmd = new SqlCommand("select * from T_RM_STOCK_MASTER where  T_RMSM_SECTION = @SECTION  AND  MONTH(T_RMSM_CREATED_DT) = @MONTH", con,trans))
                {
                    //con.Open();
                    cmd.CommandType = CommandType.Text;
                    cmd.Connection = con;
                    cmd.Parameters.AddWithValue("@MONTH", DateTime.Now.Month);
                    cmd.Parameters.AddWithValue("@SECTION", section);
                    //con.Open();
                    SqlDataAdapter adp1 = new SqlDataAdapter(cmd);
                    adp1.Fill(dtCCR);

                    ViewState["CCRMA"] = dtCCR;
                    //con.Close();
                }
           // }
        }
        catch (Exception ex) { }
        return dtCCR;
    }
    protected void btn_Add_Click(object sender, EventArgs e)
    {
        //
       if(!string.IsNullOrEmpty(txtDate.Text) && !string.IsNullOrEmpty(txtQty.Text) && !string.IsNullOrEmpty(txtRem.Text) && ddlSection.SelectedIndex != 0)
        {
            using (SqlConnection con = new SqlConnection(constr))
            {
                con.Open();
                SqlTransaction trans = con.BeginTransaction();

            int UpdMiscTable = MiscAddition(txtDate.Text.ToString().Trim(), txtQty.Text.ToString().Trim(), txtRem.Text.ToString(), ddlSection.SelectedValue,con,trans);
           
            int clnQty = 0;
            int MiscAdd = 0;
            //con.Open();
            int InsAdd = 0;
            DataTable CheckRecord = CheckCurrentRecordStatusMiscAddSUB(ddlSection.SelectedValue,con,trans); //check whether record present for present month or not ?
            if (CheckRecord.Rows.Count > 0)
            {
                if (!string.IsNullOrEmpty(CheckRecord.Rows[0]["T_RMSM_CL_QTY"].ToString()))
            {
                if ((Convert.ToInt32(CheckRecord.Rows[0]["T_RMSM_CL_QTY"].ToString())) > 0)
                {
                    clnQty = Convert.ToInt32(CheckRecord.Rows[0]["T_RMSM_CL_QTY"].ToString()) + Convert.ToInt32(txtQty.Text.ToString().Trim());

                }
            }
           
            if (!string.IsNullOrEmpty(CheckRecord.Rows[0]["T_RMSM_MISC_ADD"].ToString()))
            {


                MiscAdd = Convert.ToInt32(CheckRecord.Rows[0]["T_RMSM_MISC_ADD"].ToString()) + Convert.ToInt32(txtQty.Text.ToString().Trim());
            }
            else
            {
                MiscAdd = Convert.ToInt32(txtQty.Text.ToString().Trim());
            }


           
                try
                {
                   // using (SqlConnection con = new SqlConnection(constr))
                   // {

                        //con.Open();

                        using (SqlCommand cmd = new SqlCommand("UPDATE [dbo].[T_RM_STOCK_MASTER] SET T_RMSM_MISC_ADD = @MisAdd, T_RMSM_UPDATED_BY= @Uby , T_RMSM_UPDATED_DT = @Udt ,T_RMSM_CL_QTY = @clQty where ID = @id  ", con, trans))
                    {
                        cmd.Parameters.Clear();
                        cmd.Parameters.AddWithValue("@id", CheckRecord.Rows[0]["ID"].ToString());
                        cmd.Parameters.AddWithValue("@MisAdd", MiscAdd);
                        cmd.Parameters.AddWithValue("@clQty", clnQty);
                        cmd.Parameters.AddWithValue("@Uby", Session["userID"].ToString());
                        cmd.Parameters.AddWithValue("@Udt", DateTime.Now);


                        cmd.Connection = con;
                        try
                        {
                                InsAdd = cmd.ExecuteNonQuery();
                        }
                        catch (Exception ex)
                        {

                        }



                    }



                        if (InsAdd > 0 && UpdMiscTable >0)
                        {
                            trans.Commit();
                            string message = "alert('Misc. Addition Completed!!.')";
                            ScriptManager.RegisterClientScriptBlock((sender as Control), this.GetType(), "alert", message, true);
                        }
                        else
                        {
                            trans.Rollback();
                            string message = "alert('Error in btnAdd_click (MiscSub Add)')";
                            ScriptManager.RegisterClientScriptBlock((sender as Control), this.GetType(), "alert", message, true);
                        }
                        con.Close();

                  //  }
                    
                }
                catch (Exception ex) 
                { 
                }
            }


            BindMisAddgrid();
            resetcontrols();
            }
        }
    }
    protected void btn_Sub_Click(object sender, EventArgs e)
    {
        if (!string.IsNullOrEmpty(txtDate.Text) && !string.IsNullOrEmpty(txtQty.Text) && !string.IsNullOrEmpty(txtRem.Text) && ddlSection.SelectedIndex != 0)
        {
            using (SqlConnection con = new SqlConnection(constr))
            {
                con.Open();
                SqlTransaction trans = con.BeginTransaction();
                int UpdMiscTable = MisSub(txtDate.Text.ToString().Trim(), txtQty.Text.ToString().Trim(), txtRem.Text.ToString(), ddlSection.SelectedValue, con, trans);


                int clnQty = 0;
                int MiscSub = 0;
                //con.Open();
                int InsSub = 0;
                DataTable CheckRecord = CheckCurrentRecordStatusMiscAddSUB(ddlSection.SelectedValue, con, trans); //check whether record present for present month or not ?
                if (CheckRecord.Rows.Count > 0)
                {
                    if (!string.IsNullOrEmpty(CheckRecord.Rows[0]["T_RMSM_CL_QTY"].ToString()))
                    {
                        if ((Convert.ToInt32(CheckRecord.Rows[0]["T_RMSM_CL_QTY"].ToString())) > 0)
                        {
                            clnQty = Convert.ToInt32(CheckRecord.Rows[0]["T_RMSM_CL_QTY"].ToString()) - Convert.ToInt32(txtQty.Text.ToString().Trim());

                        }
                    }

                    if (!string.IsNullOrEmpty(CheckRecord.Rows[0]["T_RMSM_MISC_SUB"].ToString()))
                    {


                        MiscSub = Convert.ToInt32(CheckRecord.Rows[0]["T_RMSM_MISC_SUB"].ToString()) - Convert.ToInt32(txtQty.Text.ToString().Trim());
                    }
                    else
                    {
                        MiscSub = Convert.ToInt32(txtQty.Text.ToString().Trim());
                    }



                    try
                    {
                        // using (SqlConnection con = new SqlConnection(constr))
                        // {

                        //con.Open();

                        using (SqlCommand cmd = new SqlCommand("UPDATE [dbo].[T_RM_STOCK_MASTER] SET T_RMSM_MISC_SUB = @MisSub, T_RMSM_UPDATED_BY= @Uby , T_RMSM_UPDATED_DT = @Udt ,T_RMSM_CL_QTY = @clQty where ID = @id  ", con, trans))
                        {
                            cmd.Parameters.Clear();
                            cmd.Parameters.AddWithValue("@id", CheckRecord.Rows[0]["ID"].ToString());
                            cmd.Parameters.AddWithValue("@MisSub", MiscSub);
                            cmd.Parameters.AddWithValue("@clQty", clnQty);
                            cmd.Parameters.AddWithValue("@Uby", Session["userID"].ToString());
                            cmd.Parameters.AddWithValue("@Udt", DateTime.Now);


                            cmd.Connection = con;
                            try
                            {
                                InsSub = cmd.ExecuteNonQuery();
                            }
                            catch (Exception ex)
                            {

                            }



                        }



                        if (InsSub > 0 && UpdMiscTable > 0)
                        {
                            trans.Commit();
                            string message = "alert('Misc. Substraction Completed!!.')";
                            ScriptManager.RegisterClientScriptBlock((sender as Control), this.GetType(), "alert", message, true);
                        }
                        else
                        {
                            trans.Rollback();
                            string message = "alert('Error in btnSub_click (MiscSub Add)')";
                            ScriptManager.RegisterClientScriptBlock((sender as Control), this.GetType(), "alert", message, true);
                        }
                        con.Close();

                        //  }

                    }
                    catch (Exception ex)
                    {
                    }
                }


                BindMisSubgrid();
                resetcontrols();
            }
        }
    }
    protected void resetcontrols()
    {
        try
        {
            //LoadSection();
            ddlSection.ClearSelection();
            txtQty.Text = string.Empty;
            txtRem.Text = string.Empty;
            txtDate.Text = "";
        }
        catch (Exception ex)
        {
        }
    }

}