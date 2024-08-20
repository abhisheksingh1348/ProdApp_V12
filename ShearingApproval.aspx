
<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="ShearingApproval.aspx.cs" Inherits="ShearingApproval" %>

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
 <div class="row">
     <div class="col-md-4" draggable="true">
     <asp:DropDownList ID="ddlType" runat="server" Width="30%" CssClass="panel-footer" OnSelectedIndexChanged="ddlType_SelectedIndexChanged" AutoPostBack="true">
    <asp:ListItem Text="Select Data Type" Value="0"></asp:ListItem>
                                            <asp:ListItem Text="New" Value="ENTERED"></asp:ListItem>
    <asp:ListItem Text="Approved" Value="APPROVED"></asp:ListItem>

</asp:DropDownList>
     </div>
     <div class="col-md-6">

     </div>
      <asp:Button ID="btnApprove" Text="Approve" class="btn btn-primary" runat="server" OnClick="btnApprove_Click"  />
 </div>                                                                       
         
<%--Put all dropdowns in a common panel--%>
            <asp:ScriptManager ID="scriptmanager1" runat="server"></asp:ScriptManager>
<asp:UpdatePanel ID="panel1" runat="server">
<ContentTemplate>
            <div class="row">
            <div class="col-md-4">
             
                <section class="panel">
                   
                    <div class="panel-body" style="overflow:scroll">

                        <asp:GridView ID="gv_Data" runat="server" OnSelectedIndexChanged="gv_Data_SelectedIndexChanged" AllowPaging="true" PageSize="10">
                                                          <Columns>
                                       <asp:TemplateField>
    <ItemTemplate>
        <asp:LinkButton Text="Select" ID="lnkSelect" runat="server" CommandName="Select" />
    </ItemTemplate>
</asp:TemplateField>  
                                              
                              </Columns>
                        </asp:GridView>

                    <footer class="panel-footer">
                           
                    
                          
                    </footer>
                    
                </section>
            </div>
                <div class="col-md-8">
                    
                                 
                    
                    <div style="overflow:scroll">
                          <asp:GridView ID="GridView1" runat="server">
                              
                          </asp:GridView>
                    </div>
                  
                </div>
        </div>
    </ContentTemplate>
    </asp:UpdatePanel>
         </form>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
</asp:Content>


