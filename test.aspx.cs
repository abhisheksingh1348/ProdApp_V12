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


public partial class test : System.Web.UI.Page
{
    string constr = ConfigurationManager.ConnectionStrings["SONIC_PRDConnectionString"].ConnectionString;
    private static int PageSize = 5;
    protected void Page_Load(object sender, EventArgs e)
    {
        if(!IsPostBack)
        {
            LoadItemMaster();
        }
    }
    protected void LoadItemMaster()
    {
        DataTable ItemMasterData = new DataTable();
        ItemMasterData.Columns.Add("T_IM_CODE");
        ItemMasterData.Columns.Add("T_IM_DESC");
        ItemMasterData.Columns.Add("T_IM_SECTION");
        ItemMasterData.Columns.Add("T_IM_LEAF_NO");
        ItemMasterData.Columns.Add("T_IM_LESSPER");
        ItemMasterData.Columns.Add("T_IM_LESSWEIGHT");
        ItemMasterData.Columns.Add("T_IM_WEIGHT");
        ItemMasterData.Columns.Add("T_IM_THICKNESS");
        ItemMasterData.Columns.Add("T_IM_WIDTH");
        ItemMasterData.Columns.Add("T_IM_SHEARSIZE");
        ItemMasterData.Columns.Add("T_IM_CAT");
        ItemMasterData.Columns.Add("T_IM_TYPE");
        ItemMasterData.Columns.Add("T_IM_CREATED_BY");
        ItemMasterData.Columns.Add("T_IM_CREATED_DT");
        ItemMasterData.Rows.Add();
        gvItemMaster.DataSource = ItemMasterData;
        gvItemMaster.DataBind();
    }
    [WebMethod]
    public static string GetItems(string searchTerm, int pageIndex)
    {
        string query = "GetItemList";
        SqlCommand cmd = new SqlCommand(query);
        cmd.CommandType = CommandType.StoredProcedure;
        cmd.Parameters.AddWithValue("@SearchTerm", searchTerm);
        cmd.Parameters.AddWithValue("@PageIndex", pageIndex);
        cmd.Parameters.AddWithValue("@PageSize", PageSize);
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
                    dt.Columns.Add("PageIndex");
                    dt.Columns.Add("PageSize");
                    dt.Columns.Add("RecordCount");
                    dt.Rows.Add();
                    dt.Rows[0]["PageIndex"] = pageIndex;
                    dt.Rows[0]["PageSize"] = PageSize;
                    dt.Rows[0]["RecordCount"] = cmd.Parameters["@RecordCount"].Value;
                    ds.Tables.Add(dt);
                    return ds;
                }
            }
        }
    }
}