<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="itemMaste_Searchable.aspx.cs" Inherits="itemMaste" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" Runat="Server">
     <%-- test --%>
 <script src="js/jquery-1.12.4.js"></script>
 <script src="js/jquery-ui.js"></script>
<style>
      table
  {
      border: 1px solid #ccc;
      width: 450px;
      margin-bottom: -1px;
  }
  table th
  {
      background-color: #F7F7F7;
      color: #333;
      font-weight: bold;
  }
  table th, table td
  {
      padding: 5px;
      border-color: #ccc;
  }
</style>
 <!-- Bootstrap -->

 <!-- Bootstrap DatePicker -->
 <link href="css/bootstrap-datepicker.css" rel="stylesheet" />
 <script src="js/bootstrap-datepicker.js" type="text/javascript"></script>
 <!-- Bootstrap DatePicker -->
 <%--    <script src="asset/js/jquery-3.5.1.slim.min.js"></script>--%>
 <script type="text/javascript">
     $(function () {
         $('[id*=txtDate]').datepicker({
             changeMonth: true,
             changeYear: true,
             format: "dd-mm-yyyy",
             language: "tr"
         });
     });
 </script>
 <%-- test --%>


 <link href="css/sweetalert.min.css" rel="stylesheet" />
 <script src="assets/javascripts/sweetalert.min.js"></script>
 <%-- commented to make the calendar work --%>
 <%--	<script src="assets/vendor/jquery/jquery.js"></script>--%>
 <%--    <script src="js/abhi.js"></script>--%>
 <%-- commented to make the calendar work --%>
 <script src="js/jquery.searchabledropdown-1.0.8.min.js"></script>
 <script type="text/javascript">
     $(document).ready(function () {
         $("select").searchable({
             maxListSize: 200, // if list size are less than maxListSize, show them all
             maxMultiMatch: 300, // how many matching entries should be displayed
             exactMatch: false, // Exact matching on search
             wildcards: true, // Support for wildcard characters (*, ?)
             ignoreCase: true, // Ignore case sensitivity
             latency: 200, // how many millis to wait until starting search
             warnMultiMatch: 'top {0} matches ...',
             warnNoMatch: 'no matches ...',
             zIndex: 'auto'
         });
     });

 </script>
     <form id="frm1" class="form-inline" runat="server">  
            <asp:ScriptManager ID="scriptmanager1" runat="server"></asp:ScriptManager>
<asp:UpdatePanel ID="panel1" runat="server">
<ContentTemplate>
            <div class="row">
            <div class="col-md-4">
             
                <section class="panel">
                   
                    <div class="panel-body">

                                <div class="form-group">
                                                 <asp:TextBox ID="txtCode" runat="server" CssClass="form-control" Enabled="false" ></asp:TextBox>

                                    <br />
                                    <%-- <label class="control-label">Party Name</label>
  <br />
  <asp:DropDownList ID="ddlPartyName" runat="server" Width="70%"  AutoPostBack="true"></asp:DropDownList>
                                    <br />--%>
                                    
                                    <label class="control-label">Leaf Number</label>
                                    <br />
                                    <asp:DropDownList ID="ddlLeaf" runat="server" Width="90%">
    
</asp:DropDownList>

<br />

                        
                                                                       
<br />


                            


                                   
                                </div>
                       
                                                                                
                                <div class="form-group">

                                   


                                    <label class="control-label">Shearing Size</label>
                                    <br />
                                    <asp:TextBox ID="txtSs" runat="server" CssClass="form-control" ></asp:TextBox>

                                    <br />

                                    <label class="control-label">Width</label>
                                    <br />
                                    <asp:TextBox ID="txtWidth" runat="server" CssClass="form-control"></asp:TextBox>
                               
                                    <br />

                                   <label class="control-label">Thickness</label>
<br />
<asp:TextBox ID="txtThickness" runat="server" CssClass="form-control"></asp:TextBox>
                                    <br />
                                    <label class="control-label">Less Percent</label>
<br />
<asp:TextBox ID="lessP" runat="server" CssClass="form-control" Text="3" Enabled="false"></asp:TextBox>
                                    <br />
                                                                     
                                </div>
                        
                                <div class="form-group">
                                                                                                          <label class="conrol-label">Model Description</label>
<br />
                                     <asp:TextBox ID="txtDesc" runat="server" CssClass="form-control" Rows="20" Columns="40"></asp:TextBox>
                                    

                                    <br />
                                                                                                            <label class="control-label">Category</label>
                                    <br />
<asp:DropDownList ID="ddlCat" runat="server" Width="60%">
        <asp:ListItem Value="0">Select Category</asp:ListItem>
    <asp:ListItem Value="MARKET">MARKET</asp:ListItem>
    <asp:ListItem Value="GENERAL">GENERAL</asp:ListItem>
   
</asp:DropDownList><br />
                                                                        <label class="control-label">Prab./Conv./Others</label>
<br />
                                    <asp:DropDownList ID="ddlType" runat="server" Width="60%">
         <asp:ListItem Value="0">Select Type</asp:ListItem>
 <asp:ListItem Value="PARAB">PARAB.</asp:ListItem>
 <asp:ListItem Value="CONVENTIONAL">CONV.</asp:ListItem>
                                        <asp:ListItem Value="OTHERS">OTHERS</asp:ListItem>
</asp:DropDownList>                                </div>
                      
                                                                                                        


                    </div>

                    <footer class="panel-footer">
                            <asp:Button ID="btnSave" Text="Save" class="btn btn-primary" runat="server" OnClick="btnSave_Click" />

                       
                            <asp:Button ID="btnUpdate" Text="Update" class="btn btn-primary " runat="server" onclick="btnUpdate_Click" />
                    
                          
                    </footer>
                    
                </section>
            </div>
                <div class="col-md-8">
                    <%--<asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="false" DataKeyNames="CustomerId"
    OnRowDataBound="OnRowDataBound" OnRowEditing="OnRowEditing" OnRowCancelingEdit="OnRowCancelingEdit"
    OnRowUpdating="OnRowUpdating" OnRowDeleting="OnRowDeleting" EmptyDataText="No records has been added." OnSelectedIndexChanged="GridView1_SelectedIndexChanged" >
    <Columns>
        <asp:TemplateField HeaderText="Name" ItemStyle-Width="150">
            <ItemTemplate>
                <asp:Label ID="lblName" runat="server" Text='<%# Eval("Name") %>'></asp:Label>
            </ItemTemplate>
            <EditItemTemplate>
                <asp:TextBox ID="txtName" runat="server" Text='<%# Eval("Name") %>'></asp:TextBox>
            </EditItemTemplate>
        </asp:TemplateField>
        <asp:TemplateField HeaderText="Country" ItemStyle-Width="150">
            <ItemTemplate>
                <asp:Label ID="lblCountry" runat="server" Text='<%# Eval("Country") %>'></asp:Label>
            </ItemTemplate>
            <EditItemTemplate>
                <asp:TextBox ID="txtCountry" runat="server" Text='<%# Eval("Country") %>'></asp:TextBox>
            </EditItemTemplate>
        </asp:TemplateField>
        <asp:CommandField ButtonType="Link" ShowEditButton="true" ShowDeleteButton="true"
            ItemStyle-Width="150" />
        <asp:ButtonField CommandName="Select" Text="Select" ButtonType="Button" />
       
        <asp:TemplateField>
            <ItemTemplate>
                <asp:LinkButton Text="Select" ID="lnkSelect" runat="server" CommandName="Select" />
            </ItemTemplate>
        </asp:TemplateField>
    </Columns>
</asp:GridView>--%>
                                    Search:
<asp:TextBox ID="txtSearch" runat="server" AutoPostBack="true" OnTextChanged="txtSearch_TextChanged" />
<hr />
                    <div style="overflow:auto; width:100%;">
                    <asp:GridView ID="gvItemMaster" runat="server"  AutoGenerateColumns="False"  AllowPaging="true" OnPageIndexChanging="gvItemMaster_PageIndexChanging" PageSize="10" OnSelectedIndexChanged="gvItemMaster_SelectedIndexChanged" scr >
    <Columns>
           <asp:TemplateField>
          <ItemTemplate>
              <asp:LinkButton Text="Select" ID="lnkSelect" runat="server" CommandName="Select" />
          </ItemTemplate>
      </asp:TemplateField>    
        <asp:BoundField HeaderStyle-Width="150px" DataField="T_IM_CODE" HeaderText="Code"
            ItemStyle-CssClass="ContactName" />
        <asp:BoundField HeaderStyle-Width="150px" DataField="T_IM_DESC" HeaderText="Desc" />
        <asp:BoundField HeaderStyle-Width="150px" DataField="T_IM_SECTION" HeaderText="Section" />
          <asp:BoundField HeaderStyle-Width="150px" DataField="T_IM_LEAF_NO" HeaderText="Leaf" />
          <asp:BoundField HeaderStyle-Width="150px" DataField="T_IM_WEIGHT" HeaderText="Weight" />
  <asp:BoundField HeaderStyle-Width="150px" DataField="T_IM_LESSPER" HeaderText="Less %" />
          <asp:BoundField HeaderStyle-Width="150px" DataField="T_IM_LESSWEIGHT" HeaderText="LessWeight" />
  <asp:BoundField HeaderStyle-Width="150px" DataField="T_IM_THICKNESS" HeaderText="Thickness" />
          <asp:BoundField HeaderStyle-Width="150px" DataField="T_IM_WIDTH" HeaderText="Width" />
  <asp:BoundField HeaderStyle-Width="150px" DataField="T_IM_SHEARSIZE" HeaderText="ShearSize" />
          <asp:BoundField HeaderStyle-Width="150px" DataField="T_IM_CAT" HeaderText="Cat" />
  <asp:BoundField HeaderStyle-Width="150px" DataField="T_IM_TYPE" HeaderText="Type" />
          <asp:BoundField HeaderStyle-Width="150px" DataField="T_IM_CREATED_BY" HeaderText="Created By" />
          <asp:BoundField HeaderStyle-Width="150px" DataField="T_IM_CREATED_DT" HeaderText="Created Dt" />
  
    </Columns>
</asp:GridView>.
</div>
                    
<%--                    <asp:SqlDataSource ID="SqlDataSource1" runat="server"></asp:SqlDataSource>--%>
                </div>
        </div>
    </ContentTemplate>
    </asp:UpdatePanel>
         </form>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
</asp:Content>

