<%@ Page Language="C#" AutoEventWireup="true" CodeFile="LoginPage.aspx.cs" Inherits="LoginPage" %>


<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    
    <title>Production Application</title>
    <meta name="viewport" content="width=device-width, initial-scale=1.0"/>
    <link rel="stylesheet" href="home_asset/bootstrap/css/bootstrap.min.css"/>
    <link rel="stylesheet" href="home_asset/css/style1.css"/>
    <!-- HTML5 shim, for IE6-8 support of HTML5 elements -->
    <!--[if lt IE 9]>
            <script src="http://html5shim.googlecode.com/svn/trunk/html5.js"></script>
        <![endif]-->

    <style>
        .pnlmodel {
            width: 200px;
            background: #fff;
            border-radius: 2px;
            -moz-border-radius: 2px;
            -webkit-border-radius: 2px;
            border-top: 3px solid #00c0ef;
        }

        .modelback {
            -webkit-transition: opacity .3s 0s, visibility 0s .3s;
            -moz-transition: opacity .3s 0s, visibility 0s .3s;
            transition: opacity .3s 0s, visibility 0s .3s;
            filter: alpha(opacity=80);
            opacity: 0.8;
            background: #000;
        }
        .butn {
            color:#fff;
        }
        .h1{
            color:aliceblue;
        }
        
    </style>
</head>
    <script type="text/javascript">
        window.onsubmit = function () {
            if (Page_IsValid) {
                var updateProgress = $find("<%= UpdateProgress1.ClientID %>");
                window.setTimeout(function () {
                    updateProgress.set_visible(true);
                }, 100);
            }
        }
    </script>
<body>
        <div class="register-container container" style="width:fit-content">

<%--    <div class="header" style="padding-bottom:0px">--%>
<%--        <div class="container-fluid" style="background-color:transparent" >--%>
<%--            <div class="row" style="background-color:#307FE2">--%>
              
    <div class="col-sm-3">

    
<%--       <div class="logo span2">--%>
                    <img src="img/logo-new.png"  />
<%--                </div>--%>
        </div>
                <div class="col-sm-6">
                    <div class="logo span10 ">
                    <h1><a style="color:aliceblue"></a></h1>
                </div>
                </div>
             
                 <div class="col-sm-3">
               <%-- <div class="logo span1">--%>
<%--                 <img src="home_asset/img/T.png" style="margin-top:9px; float:right; padding:0px;"/>--%>
                      <%--<img src="home_asset/img/3189134-200.png"/>--%>
                <%--</div>--%>
                     </div>
              
<%--          </div>--%>
                <%--<div class="logo span3 ">
                    <h1 style="color:white"><a>Smart Logistics</a></h1>
                </div>--%>
                <%--<div class="links span2">
                    <img src="home_asset/img/t.png" />
                </div>--%>
                
<%--            </div>--%>
<%--        </div>--%>
   

    
          

    

  

<%--    <div class="register-container container">--%>
        <div class="row">
            <div class="iphone span8">
            </div>
            <div class="register span4">
                <form id="frm1" class="form" runat="server">
                    <asp:ScriptManager ID="ScriptManager1" runat="server">
                    </asp:ScriptManager>
                    <asp:UpdateProgress ID="UpdateProgress1" runat="server" AssociatedUpdatePanelID="UpdatePanel1">
<ProgressTemplate>
    <div class="modal" style="height:100px;">
        <div class="center" style="position:relative;">
             <img src="home_asset/img/Fidget-spinner.gif" style="height:100px;width:100px" /> 
        </div>
    </div>
</ProgressTemplate>
</asp:UpdateProgress>
                    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                        <ContentTemplate>
                            <h2>User Login </h2>
                            <label for="username">Username</label>
                            <asp:TextBox runat="server" ID="username" placeholder="Please enter Your Username" autocomplete="off" />
                            <label for="password">Password</label>
                            <asp:TextBox runat="server" ID="password" TextMode="Password" placeholder="Please Enter Your Password" autocomplete="off" />
                            <asp:Button ID="btnlogin" runat="server" Text="LOGIN"  CssClass="butn" ForeColor="WhiteSmoke"  OnClick="btnlogin_Click" />
                            <div id="loading" runat="server" visible="false">

                            </div>
                        </ContentTemplate>

                    </asp:UpdatePanel>
                    

                    <asp:LinkButton ID="LinkButton2" runat="server" Style="display: none" />
              
                  <%--  <script type="text/javascript">

                        Sys.WebForms.PageRequestManager.getInstance().add_beginRequest(BeginRequestHandler);

                        Sys.WebForms.PageRequestManager.getInstance().add_endRequest(EndRequestHandler)

                        function BeginRequestHandler(sender, args) {

                            $find("mpp").show();

                        }

                        function EndRequestHandler(sender, args) {

                            $find("mpp").hide();

                        }

                    </script>--%>
                </form>

            </div>
        </div>
    </div>

    <%--<footer class="footer">
        <div class="container-fluid">
            <div class="row">
                <div class="links span1 pull-left">
                </div>
                <div class="logo span10">
                    <h4>Release version:1.0.0, Release Date:27-12-2017</h4>
                </div>
                <div class="links span2 pull-right">
                    <img src="home_asset/img/auto_logo_small.png" />
                </div>


            </div>
        </div>
    </footer>--%>


    <!-- Javascript -->
    <script src="home_asset/js/jquery-1.8.2.min.js"></script>
    <script src="home_asset/bootstrap/js/bootstrap.min.js"></script>
    <script src="home_asset/js/jquery.backstretch.min.js"></script>
    <%-- <script src="home_asset/js/scripts.js"></script>--%>
    <script>
        jQuery(document).ready(function () {

            /*
                Background slideshow
            */
            $.backstretch([
              "home_asset/img/backgrounds/about-machine-infrastructure.jpg"
                , "home_asset/img/backgrounds/about-machinery.jpg"
                ,"home_asset/img/backgrounds/about-worker.jpg"
                , "home_asset/img/backgrounds/induction-furnace.jpg"
                , "home_asset/img/backgrounds/painting.jpg"
                , "home_asset/img/backgrounds/robotic-heat-treatment.jpg"
            //, "home_asset/img/backgrounds/7.jpg"
            //, "home_asset/img/backgrounds/8.jpg"
            
            ], { duration: 3000, fade: 750 });
        });
    </script>
</body>
</html>
