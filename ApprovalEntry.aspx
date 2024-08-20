<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="ApprovalEntry.aspx.cs" Inherits="ApprovalEntry" EnableEventValidation="false" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <%-- test --%>
    <script src="js/jquery-1.12.4.js"></script>
    <script src="js/jquery-ui.js"></script>

   


    <link href="css/sweetalert.min.css" rel="stylesheet" />
    <script src="assets/javascripts/sweetalert.min.js"></script>
   

      <script language="javascript" type="text/javascript">
          function validate() {
              var summary = "";
              summary += isvalidRemark();
             

              if (summary != "") {
                  alert(summary);
                  return false;
              }
              else {
                  return true;
              }
          }
        
          function isvalidRemark() {
              var uid;
              var temp = document.getElementById("<%=txtRemarks.ClientID %>");
            uid = temp.value;
            if (uid == "") {
                return ("Please Enter Remarks" + "\n");
            }
            else {
                return "";
            }
        }
      
      </script>

    <style type="text/css">
        .clsTxt {
            width: 100%;
            width: 100%;
            min-height: 25px;
            max-height: 200px;
            resize: none;
        }
    </style>
    <style type="text/css">
        .clsTxt1 {
            width: 100%;
            width: 100%;
            min-height: 100px;
            max-height: 400px;
            resize: none;
        }
    </style>
            <form id="frm1" class="form-inline" runat="server">
        
<%--Put all dropdowns in a common panel--%>
        <asp:ScriptManager ID="scriptmanager1" runat="server"></asp:ScriptManager>
<asp:UpdatePanel ID="panel1" runat="server">
<ContentTemplate>

        <%--<asp:DropDownList ID="DropDownList1" runat="server" Width="90%" OnSelectedIndexChanged="ddlMaterialCode_SelectedIndexChanged" AutoPostBack="true"></asp:DropDownList>--%>
        <div class="row">
            <div class="col-md-12">
                
                <section class="panel">
                    
                    <div class="panel-body" runat="server" style="color:black">




                        <div class="row">
                            <div class="col-sm-2">
                                <div class="form-group">

                                    <label class="control-label">Entry Type</label>
                                    <br />
                                    <asp:DropDownList ID="ddlEntryType" runat="server"  AutoPostBack="true" OnSelectedIndexChanged="ddlEntryType_SelectedIndexChanged">
                                       <asp:ListItem Text="Select" Value="0">Select</asp:ListItem>
                                        <asp:ListItem Text="Purchase" Value="PURCHASE">PURCHASE</asp:ListItem>
                                        <asp:ListItem Text="Reduce" Value="REDUCE">REDUCE</asp:ListItem>
                                        <asp:ListItem Text="ReWork" Value="REWORK">REWORK</asp:ListItem>
                                        <asp:ListItem Text="Comp-ReWork" Value="CREWORK">Comp-ReWork</asp:ListItem>

                                    </asp:DropDownList>
                                 
                                    <br />

                                    
                                    


                                </div>
                            </div>

                            <div class="col-sm-2">
                                <div class="form-group">

                            <label class="control-label">Record Type</label>
                                 <br />

                            <asp:DropDownList ID="ddlRecordType" runat="server"   AutoPostBack="true">
                                                                       <asp:ListItem Text="Select" Value="0">Select</asp:ListItem>
                                <asp:ListItem Text="New" Value="ENTERED">New</asp:ListItem>
                                <asp:ListItem Text="Approved" Value="APPROVED">Approved</asp:ListItem>
                            </asp:DropDownList>
 
                                    <br />    

</div>
                            </div>
                            <div class="col-sm-2">
                                    <div class="form-group">
    <br />

<asp:Button ID="btn_show" Text="Show" class="btn btn-primary" runat="server" OnClick="btn_show_Click" />

                                        </div>
                            </div>
                               
                            <div class="col-sm-2">
                                <div class="form-group">
                                   
                                  <label class="control-label">Remarks</label>
     <br />
                                <asp:TextBox ID="txtRemarks" runat="server" CssClass="form-control"></asp:TextBox>
    </div>
</div>
    <div class="col-sm-2">
        <div class="form-group">
                                                           <br />

 <asp:Button ID="btnApprove" Text="Approve" class="btn btn-primary" runat="server" OnClick="btnApprove_Click" Enabled="False" />
                                    <br />
                                   <%-- <asp:CheckBox ID="approveAll" runat="server" />--%>
                                  

                                   
                                </div>
                            </div>
                               <div class="col-sm-2">
       <div class="form-group">
                                                          <br />

<asp:Button ID="btnReject" Text="Reject" class="btn btn-primary" runat="server" OnClientClick="validate()" OnClick="btnReject_Click" Enabled="False" />
                                   <br />
                                  <%-- <asp:CheckBox ID="approveAll" runat="server" />--%>
                                 

                                  
                               </div>
                           </div>
                        </div>

                    </div>

                    <footer class="panel-footer">
                            <%--<asp:Button ID="btn_add" Text="Add" class="btn btn-primary" runat="server" OnClick="btn_add_Click" />

                       
                            <asp:Button ID="btnAddNew" Text="New Entry" class="btn btn-primary" runat="server" OnClick="btnAddNew_Click" />--%>
                    
                          
                    </footer>
                    
                </section>
            </div>
        </div>




        <div class="row">
            <div class="panel-body" style="overflow: scroll" runat="server" id="RawMat">

                <!-- Flot: Curves -->

                <table>
                    <tr>
                        <td>
                            
                                  <asp:GridView ID="Gv_RawMaterial" runat="server"
           AutoGenerateColumns="False" DataKeyNames="T_RMM_BILL"  OnSelectedIndexChanged="Gv_RawMaterial_SelectedIndexChanged"
           HeaderStyle-BackColor="#0078d4" HeaderStyle-ForeColor="AliceBlue" ForeColor="Black" >
                                      
                          
           <Columns>
               <asp:TemplateField HeaderText="Select">
            <ItemTemplate>
                <asp:CheckBox ID="chkselect" runat="server" />
            </ItemTemplate>
        </asp:TemplateField>
<%--                              <asp:ButtonField Text = "Select" CommandName = "Select" />--%>

              <%-- <asp:TemplateField HeaderText="Delete">
                   <ItemTemplate>
                       <asp:LinkButton ID="LinkApprove" runat="server"  CausesValidation="false">Delete</asp:LinkButton>
                   </ItemTemplate>
               </asp:TemplateField>--%>
               <asp:BoundField DataField="ID" HeaderText="Sno." ReadOnly="True" SortExpression="ID" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center" />
                <asp:BoundField DataField="T_RMM_DATAMODE" HeaderText="Data Mode" ReadOnly="True" SortExpression="T_RMM_DATAMODE" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center" />
               <asp:BoundField DataField="T_RMM_BILL" HeaderText="Bill No." ReadOnly="True" SortExpression="T_RMM_BILL" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center" />
               <asp:BoundField DataField="T_RMM_PDATE" HeaderText="Purchase Date" ReadOnly="True" SortExpression="T_RMM_PDATE" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center" />
               <asp:BoundField DataField="T_RMM_UNITNUM" HeaderText="Unit No." ReadOnly="True" SortExpression="T_RMM_UNITNUM" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center" />
               <asp:BoundField DataField="T_RMM_PARTY" HeaderText="Party Name" ReadOnly="True" SortExpression="T_RMM_PARTY" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center" />

               <asp:BoundField DataField="T_RMM_SECTION" HeaderText="Section" ReadOnly="True" SortExpression="T_RMM_SECTION" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center" />
               <asp:BoundField DataField="T_RMM_QUANTITY" HeaderText="Quantity" ReadOnly="True" SortExpression="T_RMM_QUANTITY" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center" />

               <asp:BoundField DataField="T_RMM_RATE" HeaderText="Rate" ReadOnly="True" SortExpression="T_RMM_RATE" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center" />
               <asp:BoundField DataField="T_RMM_TOTALAMNT" HeaderText="Total" ReadOnly="True" SortExpression="T_RMM_TOTALAMNT" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center" />

               <asp:BoundField DataField="T_RMM_EDATE" HeaderText="Entry Date" ReadOnly="True" SortExpression="T_RMM_EDATE" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center" />
               
           
           </Columns>

       </asp:GridView>
                        </td>
                    </tr>
                </table>
                <br />
                <footer class="panel-footer">
                   <%-- <asp:Button ID="btn_submit_req" Text="Submit" class="btn btn-primary" runat="server" OnClick="btn_submit_req_Click" />--%>

<%--                    <asp:Button ID="btn_draft" Text="Save as Draft" class="btn btn-primary" runat="server" />--%>
                </footer>


            </div>
        </div>

            <div class="row">
            <div class="panel-body" style="overflow: scroll" runat="server" id="Rej">

                <!-- Flot: Curves -->

                <table>
                    <tr>
                        <td>
                            
                                  <asp:GridView ID="GV_Rej" runat="server"
           AutoGenerateColumns="False" DataKeyNames="ID" 
           HeaderStyle-BackColor="#0078d4" HeaderStyle-ForeColor="AliceBlue" ForeColor="Black" >
                                      
                          
           <Columns>
               <asp:TemplateField HeaderText="Select">
            <ItemTemplate>
                <asp:CheckBox ID="chkselect" runat="server" />
            </ItemTemplate>
        </asp:TemplateField>

               <asp:BoundField DataField="ID" HeaderText="ID" ReadOnly="True" SortExpression="ID" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center" />
                
               <asp:BoundField DataField="T_RMREJM_SECTION" HeaderText="Section" ReadOnly="True" SortExpression="T_RMREJM_SECTION" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center" />
               
               <asp:BoundField DataField="T_RMREJM_QTY" HeaderText="Quantity" ReadOnly="True" SortExpression="T_RMREJM_QTY" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center" />
               <asp:BoundField DataField="T_RMREJM_STATUS" HeaderText="Status" ReadOnly="True" SortExpression="T_RMREJM_STATUS" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center" />
               <asp:boundfield datafield="T_RMREJM_ENTRY_DT" HeaderText="Date" dataformatstring="{0:dd/MM/yyyy}" />

              <%-- <asp:BoundField DataField="T_RMREJM_ENTRY_DT" HeaderText="Entry Date" ReadOnly="True" SortExpression="T_RMREJM_ENTRY_DT" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center" />--%>
             
               
           
           </Columns>

       </asp:GridView>
                        </td>
                    </tr>
                </table>
                <br />
                <footer class="panel-footer">
                   <%-- <asp:Button ID="btn_submit_req" Text="Submit" class="btn btn-primary" runat="server" OnClick="btn_submit_req_Click" />--%>

<%--                    <asp:Button ID="btn_draft" Text="Save as Draft" class="btn btn-primary" runat="server" />--%>
                </footer>


            </div>
                            <div class="panel-body" style="overflow: scroll" runat="server" id="Rew">

                <!-- Flot: Curves -->

                <table>
                    <tr>
                        <td>
                            
                                  <asp:GridView ID="Gv_Rew" runat="server"
           AutoGenerateColumns="False" DataKeyNames="ID" 
           HeaderStyle-BackColor="#0078d4" HeaderStyle-ForeColor="AliceBlue" ForeColor="Black" >
                                      
                          
           <Columns>
               <asp:TemplateField HeaderText="Select">
            <ItemTemplate>
                <asp:CheckBox ID="chkselect" runat="server" />
            </ItemTemplate>
        </asp:TemplateField>

               <asp:BoundField DataField="ID" HeaderText="ID" ReadOnly="True" SortExpression="ID" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center" />
                
               <asp:BoundField DataField="T_RMREWM_SECTION" HeaderText="Section" ReadOnly="True" SortExpression="T_RMREWM_SECTION" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center" />
               
               <asp:BoundField DataField="T_RMREWM_QTY" HeaderText="Quantity" ReadOnly="True" SortExpression="T_RMREWM_QTY" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center" />
               <asp:BoundField DataField="T_RMREWM_STATUS" HeaderText="Status" ReadOnly="True" SortExpression="T_RMREWM_STATUS" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center" />
               <asp:boundfield datafield="T_RMREWM_ENTRY_DT" HeaderText="Date" dataformatstring="{0:dd/MM/yyyy}" />

              <%-- <asp:BoundField DataField="T_RMREJM_ENTRY_DT" HeaderText="Entry Date" ReadOnly="True" SortExpression="T_RMREJM_ENTRY_DT" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center" />--%>
             
               
           
           </Columns>

       </asp:GridView>
                        </td>
                    </tr>
                </table>
                <br />
                <footer class="panel-footer">
                   <%-- <asp:Button ID="btn_submit_req" Text="Submit" class="btn btn-primary" runat="server" OnClick="btn_submit_req_Click" />--%>

<%--                    <asp:Button ID="btn_draft" Text="Save as Draft" class="btn btn-primary" runat="server" />--%>
                </footer>


            </div>
                                            <div class="panel-body" style="overflow: scroll" runat="server" id="CompRew">

                <!-- Flot: Curves -->

                <table>
                    <tr>
                        <td>
                            
                                  <asp:GridView ID="gv_Rewrk" runat="server"
           AutoGenerateColumns="False" DataKeyNames="ID" 
           HeaderStyle-BackColor="#0078d4" HeaderStyle-ForeColor="AliceBlue" ForeColor="Black" >
                                      
                          
           <Columns>
               <asp:TemplateField HeaderText="Select">
            <ItemTemplate>
                <asp:CheckBox ID="chkselect" runat="server" />
            </ItemTemplate>
        </asp:TemplateField>

               <asp:BoundField DataField="ID" HeaderText="ID" ReadOnly="True" SortExpression="ID" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center" />
                
               <asp:BoundField DataField="T_RMREWRK_FRMSECTION" HeaderText="Frm-Section" ReadOnly="True" SortExpression="T_RMREWRK_FRMSECTION" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center" />
                         <asp:BoundField DataField="T_RMREWRK_TOSECTION" HeaderText="To-Section" ReadOnly="True" SortExpression="T_RMREWRK_TOSECTION" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center" />
               <asp:BoundField DataField="T_RMREWRK_QTY" HeaderText="Quantity" ReadOnly="True" SortExpression="T_RMREWRK_QTY" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center" />
               <asp:BoundField DataField="T_RMREWRK_STATUS" HeaderText="Status" ReadOnly="True" SortExpression="T_RMREWRK_STATUS" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center" />
               <asp:boundfield datafield="T_RMREWRK_ENTRY_DT" HeaderText="Date" dataformatstring="{0:dd/MM/yyyy}" />

              <%-- <asp:BoundField DataField="T_RMREJM_ENTRY_DT" HeaderText="Entry Date" ReadOnly="True" SortExpression="T_RMREJM_ENTRY_DT" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center" />--%>
             
               
           
           </Columns>

       </asp:GridView>
                        </td>
                    </tr>
                </table>
                <br />
                <footer class="panel-footer">
                   <%-- <asp:Button ID="btn_submit_req" Text="Submit" class="btn btn-primary" runat="server" OnClick="btn_submit_req_Click" />--%>

<%--                    <asp:Button ID="btn_draft" Text="Save as Draft" class="btn btn-primary" runat="server" />--%>
                </footer>


            </div>
        </div>

                                    </ContentTemplate>
<Triggers>
<%--<asp:AsyncPostBackTrigger ControlID ="ddlSection"/>--%>

</Triggers>
</asp:UpdatePanel>
                               

        <script type="text/javascript">
            $(function () {
                SetDatePicker();
            });

            //On UpdatePanel Refresh.
            var prm = Sys.WebForms.PageRequestManager.getInstance();
            if (prm != null) {
                prm.add_endRequest(function (sender, e) {
                    if (sender._postBackSettings.panelsToUpdate != null) {
                        SetDatePicker();
                    }
                });
            };

            function SetDatePicker() {
                $('#txtDate').datepicker({ minDate: 0 });
            }
        </script>

         <script type="text/javascript">
             //On Page Load.
             $(function () {
                 SetDatePicker();
             });

             //On UpdatePanel Refresh.
             var prm = Sys.WebForms.PageRequestManager.getInstance();
             if (prm != null) {
                 prm.add_endRequest(function (sender, e) {
                     if (sender._postBackSettings.panelsToUpdate != null) {
                         SetDatePicker();
                         $(".datepicker-orient-bottom").hide();
                     }
                 });
             };

             function SetDatePicker() {
                 $("#datepicker").datepicker();
                 if ($("#txtBookDate").val() == "") {
                     var dateNow = new Date();
                     $('#datepicker').datepicker("setDate", dateNow);
                 }
             }
         </script>
                           <asp:GridView ID="GridView1" HeaderStyle-BackColor="#3AC0F2" HeaderStyle-ForeColor="White"
    runat="server" AutoGenerateColumns="false">
    <Columns>
        <asp:BoundField DataField="Name" HeaderText="Name" ItemStyle-Width="150" />
        <asp:BoundField DataField="Country" HeaderText="Country" ItemStyle-Width="150" />
    </Columns>
</asp:GridView>
<asp:LinkButton ID="lnkDummy" runat="server"></asp:LinkButton>
    </form>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
</asp:Content>

