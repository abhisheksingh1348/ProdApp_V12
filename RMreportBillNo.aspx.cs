using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;
using System.IO;
using System.Text;
using System.Globalization;

public partial class RMreport : System.Web.UI.Page
{
    string constr = ConfigurationManager.ConnectionStrings["SONIC_PRDConnectionString"].ConnectionString;

    protected void Page_Load(object sender, EventArgs e)
    {
        if(!IsPostBack)
        {
            if (Session["userID"] == null)
            {
                Response.Redirect("LoginPage.aspx");
            }

        }

    }
    protected void Gv_RawMaterial_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        try
        {


            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                DataRow row = ((DataRowView)e.Row.DataItem).Row;
                if (row["Id"].ToString() == "1" || row["Id"].ToString() == "3")
                {
                    e.Row.DataItem = "test";
                    e.Row.BackColor = System.Drawing.Color.Green;
                    e.Row.ForeColor = System.Drawing.Color.White;
                }

                else
                {
                    e.Row.BackColor = System.Drawing.Color.Blue;
                    e.Row.ForeColor = System.Drawing.Color.White;
                }
            }
        }
        catch (Exception ex)
        {

        }
    }

    protected void btnexport_Click(object sender, EventArgs e)
    {
        try
        {
            btnexport.Visible = false;
            Response.Clear();
            Response.Buffer = true;
            Response.AddHeader("content-disposition", "attachment;filename=GridViewExport.xls");
            Response.Charset = "";
            Response.ContentType = "application/vnd.ms-excel";
            using (StringWriter sw = new StringWriter())
            {
                HtmlTextWriter hw = new HtmlTextWriter(sw);
                // To Export all pages
                //AbhiCode

                //
                Gv_RawMaterial.AllowPaging = false;
                //this.BindRMgrid(); commented for testing and its working w.o. this line
                Gv_RawMaterial.HeaderRow.BackColor = System.Drawing.Color.White;
                foreach (TableCell cell in Gv_RawMaterial.HeaderRow.Cells)
                {
                    cell.BackColor = Gv_RawMaterial.HeaderStyle.BackColor;
                }
                foreach (GridViewRow row in Gv_RawMaterial.Rows)
                {
                    row.BackColor = System.Drawing.Color.White;
                    foreach (TableCell cell in row.Cells)
                    {
                        //Abhi Code
                        cell.BackColor = System.Drawing.Color.White;
                        cell.ForeColor = System.Drawing.Color.Black;
                        //if (row.RowIndex == 3)
                        //{
                        //    cell.BackColor = System.Drawing.Color.Black;
                        //    cell.ForeColor = System.Drawing.Color.White;
                        //}
                        // Abhi Code
                        //if (row.RowIndex == 5)
                        //{
                        //    cell.BackColor = System.Drawing.Color.Green;
                        //    cell.ForeColor = System.Drawing.Color.White;
                        //}
                        //
                        //if (row.RowIndex % 2 == 0)
                        //{
                        //    cell.BackColor = System.Drawing.Color.Green;
                        //    cell.ForeColor = System.Drawing.Color.White;
                        //}
                        //else
                        //{
                        //    cell.BackColor = System.Drawing.Color.Blue;
                        //    cell.ForeColor = System.Drawing.Color.White;
                        //} Commented to remove color in alternate rows
                        cell.CssClass = "textmode";
                    }
                }
                //Gv_RawMaterial.RenderControl(hw);  //commented to append text above sheet data
                divExport.RenderControl(hw);
                // style to format numbers to string
                string style = "<style> .textmode { } </style>";
                Response.Write(style);
                //test code
                //StringBuilder buildHtml = new StringBuilder();
                //buildHtml.Append("<HTML>");
                //buildHtml.AppendLine("dfsfdsfdsfd");
                //buildHtml.Append("</HTML>");
                //string sHtml = buildHtml.ToString();
                //

                Response.Output.Write(sw.ToString());
                Response.Flush();
                Response.End();
            }
        }

        catch (Exception ex)
        {

        }
    }
    public override void VerifyRenderingInServerForm(Control control)
    {
        /* Verifies that the control is rendered */
    }

    protected void ToDate_TextChanged(object sender, EventArgs e)
    {
       
    }

    protected void txtBillNo_TextChanged(object sender, EventArgs e)
    {
        //BindRMgrid(ToDate.Text.Trim().ToString(), FrmDate.Text.Trim().ToString());
        if (txtBillNo.Text != string.Empty)
        {
            //DateTime fromdt = DateTime.ParseExact(FrmDate.Text, "dd-MM-yyyy", CultureInfo.InstalledUICulture);
            //DateTime Todt = DateTime.ParseExact(ToDate.Text, "dd-MM-yyyy", CultureInfo.InstalledUICulture);

            //var v11 = sonicPrd.SP_CMP_GET_MASTER_REQUEST_DETAILS_HOD(Session["userID"].ToString()).ToList().Where(x => x.CREATED_DATE >= fromdt && x.CREATED_DATE <= Todt).ToList();
            ////
            //var V12 = (from d in sonicPrd.T_RM_MASTERs.OrderBy(d => d.ID) select new { x => x.T_RMM_PDATE >= fromdt && x.T_RMM_PDATE <= Todt }).Distinct()
            //           .Where(x => x.T_RMM_PDATE >= fromdt && x.T_RMM_PDATE <= Todt)).ToList();
            //
            //
            try
            {
                using (SqlConnection con = new SqlConnection(constr))
                {
                    using (SqlCommand cmd = new SqlCommand("SELECT * FROM T_RM_MASTER where T_RMM_BILL = @Bill;"))
                    {
                        cmd.CommandType = CommandType.Text;
                        cmd.Parameters.AddWithValue("@Bill", txtBillNo.Text.Trim().ToString());
                        cmd.Connection = con;
                        con.Open();
                        SqlDataAdapter adp1 = new SqlDataAdapter(cmd);
                        DataTable dt1 = new DataTable();
                        adp1.Fill(dt1);
                        Gv_RawMaterial.DataSource = dt1;
                        Gv_RawMaterial.DataBind();
                        ViewState["RM"] = dt1;
                        con.Close();
                    }
                }
            }
            catch (Exception ex) { }
            //
        }
        else
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "Msg", "alert('Error: please enter Bill Num.');", true);
        }
    }
}