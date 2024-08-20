
<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="ShearingEntry.aspx.cs" Inherits="ShearingEntry" %>

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
                                    <asp:TextBox ID="txtDate" runat="server" CssClass="form-control" autocomplete="off" placeholder="Shearing Date"></asp:TextBox>               
<%--Put all dropdowns in a common panel--%>
            <asp:ScriptManager ID="scriptmanager1" runat="server"></asp:ScriptManager>
<asp:UpdatePanel ID="panel1" runat="server">
<ContentTemplate>
            <div class="row">
            <div class="col-md-4">
             
                <section class="panel">
                   
                    <div class="panel-body">

                                <div class="form-group">
                                    <label class="control-label">Shear No.</label>
<br />
                                                 <asp:TextBox ID="txtShNo" runat="server" CssClass="form-control" placeholder="ShNo." ></asp:TextBox>
                                    <br />
                                    <label class="control-label">Qty.</label>
                                    <br />
                                      <asp:TextBox ID="txtQty" runat="server" CssClass="form-control" placeholder="Qty"  ></asp:TextBox>
                                    <br />
                                    
                                    
                                    <label class="control-label">Unit Number</label>
                                    <br />
                                    <asp:DropDownList ID="ddlUnit" runat="server" Width="90%">
    <asp:ListItem Text="Select Unit" Value="0"></asp:ListItem>
                                            <asp:ListItem Text="Unit1" Value="UNIT1"></asp:ListItem>
    <asp:ListItem Text="Unit2" Value="UNIT2"></asp:ListItem>
    <asp:ListItem Text="Unit3" Value="UNIT3"></asp:ListItem>

</asp:DropDownList>

<br />

                        
                                                                       
<br />


                            


                                   
                                </div>
                       
                                                                                
                                <div class="form-group">

                                   

                                    <br />
                                  <br />
                                     <asp:Button ID="btnNew" Text="New" class="btn btn-primary" runat="server" OnClick="btnNew_Click"  />
                                     <asp:Button ID="btnSearch" Text="Search" class="btn btn-primary" runat="server" OnClick="btnSearch_Click"  />
                                    <br />
                        
                                </div>
                        
                                <div class="form-group">
                                                                                                                                                                                                                      <label class="control-label" >Description</label>
                                    <br />
<asp:DropDownList ID="ddlDesc" runat="server" Width="100%">
       
   
</asp:DropDownList><br />
                                                                        
                      
                                                                                                        


                    </div>

                    <footer class="panel-footer">
                            <asp:Button ID="btnSave" Text="Save" class="btn btn-primary" runat="server" OnClick="btnSave_Click"  />

                       
                            <asp:Button ID="btnUpdate" Text="Update" class="btn btn-primary " runat="server" OnClick="btnUpdate_Click"  />
                    
                          
                    </footer>
                    
                </section>
            </div>
                <div class="col-md-8">
                    
                                 
                    <%--<div style="overflow:auto; width:100%;">
                    <asp:GridView ID="gvShMaster" runat="server"  AutoGenerateColumns="False" Visible="false" >
    <%--<Columns>
           <asp:TemplateField>
          <ItemTemplate>
              <asp:LinkButton Text="Select" ID="lnkSelect" runat="server" CommandName="Select" />
          </ItemTemplate>
      </asp:TemplateField>    
        <asp:BoundField HeaderStyle-Width="150px" DataField="T_SM_SHR_NO" HeaderText="Sl No."
            ItemStyle-CssClass="ContactName" />
        <asp:BoundField HeaderStyle-Width="150px" DataField="T_SM_CODE" HeaderText="Code" />
        <asp:BoundField HeaderStyle-Width="150px" DataField="T_SM_DESC" HeaderText="Description" />
          <asp:BoundField HeaderStyle-Width="150px" DataField="T_SM_SECTION" HeaderText="Section" />
          <asp:BoundField HeaderStyle-Width="150px" DataField="T_SM_LEAF_NO" HeaderText="Leaf No" />
  <asp:BoundField HeaderStyle-Width="150px" DataField="T_SM_QTY" HeaderText="Qty" />
         <asp:BoundField HeaderStyle-Width="150px" DataField="T_SM_UNIT_WEIGHT" HeaderText="Unit Weight" />
          <asp:BoundField HeaderStyle-Width="150px" DataField="T_SM_WEIGHT" HeaderText="Weight" />
  
          <asp:BoundField HeaderStyle-Width="150px" DataField="T_IM_CREATED_BY" HeaderText="Created By" />
          <asp:BoundField HeaderStyle-Width="150px" DataField="T_IM_CREATED_DT" HeaderText="Created Dt" />
  
    </Columns>
</asp:GridView>.
</div>--%>
                    <div style="overflow:scroll">
                          <asp:GridView ID="GridView1" runat="server" AllowPaging="true" PageSize="10" OnSelectedIndexChanged="GridView1_SelectedIndexChanged" >
                              <Columns>
                                       <asp:TemplateField>
    <ItemTemplate>
        <asp:LinkButton Text="Select" ID="lnkSelect" runat="server" CommandName="Select" />
    </ItemTemplate>
</asp:TemplateField>  
                                       <%-- <asp:BoundField HeaderStyle-Width="150px" DataField="T_SM_SHR_NO" HeaderText="ShNO"
          ItemStyle-CssClass="ContactName" />
      <asp:BoundField HeaderStyle-Width="150px" DataField="T_SM_STATUS" HeaderText="Status" />
      <asp:BoundField HeaderStyle-Width="150px" DataField="T_SM_CODE" HeaderText="Code" />
        <asp:BoundField HeaderStyle-Width="150px" DataField="T_SM_DESC" HeaderText="Desc" />
        <asp:BoundField HeaderStyle-Width="150px" DataField="T_SM_SECTION" HeaderText="Section" />
<asp:BoundField HeaderStyle-Width="150px" DataField="T_SM_LEAF_NO" HeaderText="LeafNo" />
        <asp:BoundField HeaderStyle-Width="150px" DataField="T_SM_QTY" HeaderText="Qty" />
<asp:BoundField HeaderStyle-Width="150px" DataField="T_IM_THICKNESS" HeaderText="Thickness" />
        <asp:BoundField HeaderStyle-Width="150px" DataField="T_SM_UNIT_WEIGHT" HeaderText="Unit Weight" />
<asp:BoundField HeaderStyle-Width="150px" DataField="T_SM_WEIGHT" HeaderText="Weight" />--%>
       
                              </Columns>
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


