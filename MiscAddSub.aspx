<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="MiscAddSub.aspx.cs" Inherits="MiascAddSub"  EnableEventValidation="false" %>


<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <%-- test --%>
    <script src="js/jquery-1.12.4.js"></script>
    <script src="js/jquery-ui.js"></script>

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
    <script type="text/javascript">
        function Showalert() {
            alert('Call JavaScript function from codebehind');
        }
    </script>

   <%-- <link href="css/sweetalert.min.css" rel="stylesheet" />
    <script src="assets/javascripts/sweetalert.min.js"></script>--%>
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
<%--    <script type="text/javascript" src="http://ajax.googleapis.com/ajax/libs/jquery/1.8.3/jquery.min.js"></script>--%>
    
    <form id="frm1" class="form-inline" runat="server">

                           <asp:TextBox ID="txtDate" runat="server" CssClass="form-control" autocomplete="off" placeholder="Enter Date"></asp:TextBox>               
<%--Put all dropdowns in a common panel--%>
        <asp:ScriptManager ID="scriptmanager1" runat="server"></asp:ScriptManager>
<asp:UpdatePanel ID="panel1" runat="server">
<ContentTemplate>

       
        <div class="row">
            <div class="col-md-12">
              
                <section class="panel">
                   
                    <div class="panel-body">




                     
                          
                              <div class="row ">
                                  <div class="col-md-12">
                                                                        <div class="col-md-4">
                                                                        <label class="control-label">Quantity</label>
<br />
<asp:TextBox ID="txtQty" runat="server" CssClass="form-control"></asp:TextBox>
                                  </div>
                                  
                                  <div class="col-md-4">
                                                                                                          <label class="control-label">Section</label>
                                    <br />
<asp:DropDownList ID="ddlSection" runat="server" Width="90%"></asp:DropDownList>
</div>
                                                                        <div class="col-md-4">
                                                                                                          <label class="control-label">Remarks</label>
                                    <br />
<asp:TextBox ID="txtRem" runat="server" CssClass="form-control" Columns="22" Rows="2"></asp:TextBox>

</div>
                                  </div>
                                 
                              </div>
                          

                        

                    </div>

                    <footer class="panel-footer">
                        <div class="row">
                            <div class="col-md-2">
                                
                                                            <asp:Button ID="btn_Add" Text="Add" class="btn btn-primary" runat="server" OnClick="btn_Add_Click"   />

                            </div>
                            <div class="col-md-9">

                            </div>
 <div class="col-md-1">
                                <asp:Button ID="btn_Sub" Text="Sub" class="btn btn-primary text-right" runat="server" OnClick="btn_Sub_Click"  />


 </div>
                       

                        </div>
                            
                    
                          
                    </footer>
                    
                </section>
            </div>
        </div>




        <div class="row">
            <div class="panel-body" style="overflow: scroll">

                <!-- Flot: Curves -->

                <table>
                    <tr>
                        <td>
                            
                                  <asp:GridView ID="Gv_MiscAddSub" runat="server"
           HeaderStyle-BackColor="#0078d4" HeaderStyle-ForeColor="AliceBlue">
                                      
                          
           <%--<Columns>
              
               <asp:BoundField DataField="ID" HeaderText="Sno." ReadOnly="True" SortExpression="ID" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center" />
               

               <asp:BoundField DataField="T_RMREJM_SECTION" HeaderText="Section" ReadOnly="True" SortExpression="T_RMM_SECTION" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center" />
               <asp:BoundField DataField="T_RMREJM_QTY" HeaderText="Quantity" ReadOnly="True" SortExpression="T_RMM_QUANTITY" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center" />   
           </Columns>--%>

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
<asp:AsyncPostBackTrigger ControlID ="ddlSection"/>

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

    </form>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
</asp:Content>


