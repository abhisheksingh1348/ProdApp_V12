
jQuery(document).ready(function() {

   
    /*
        Tooltips
    */
    $('.links a.home').tooltip();
    $('.links a.blog').tooltip();

    /*
        Form validation
    */
    $('.register #btnlogin').click(function () {

        $(".form").find("label[for='username']").html('Username');
        $(".form").find("label[for='password']").html('Password');
        ////
        var username = $(".form").find('input#username').val();
        var password = $(".form").find('input#password').val();        
        if(username == '') {
           /* $(this).find("label[for='username']").append("<span style='display:none' class='red'> - Please enter a valid username.</span>");
            $(this).find("label[for='username'] span").fadeIn('medium');*/
            alert("Please enter a valid username")
            return false;
        }      
        else if (password == '') {
            /* $(".form").find("label[for='password']").append("<span style='display:none' class='red'> - Please enter a valid password.</span>");
             $(".form").find("label[for='password'] span").fadeIn('medium');*/
            alert("Please enter a valid password.");
            return false;
        }
        else {
            
            $.ajax({
                type: "POST",
                url: "userLogin.asmx/checkUser",
                data: '{userId: "' + username + '",Password: "' + password + '" }',
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (response) {
                    if (response.d == null) {
                        // return;

                    }                    
                    if (response.d != false) {

                       // $.session.set("login", "ok");
                        window.location.href = "LiveImage.aspx";
                        /*if ($('#rememberMe').is(':checked')) {
                            // save username and password
                            localStorage.usrname = $('#username').val();
                            localStorage.pass = $('#password').val();
                            localStorage.chkbx = $('#rememberMe').val();
                            //window.location.href = "home.html";
                            window.location.href = "LiveImage.aspx";
                        } else {
                            localStorage.usrname = '';
                            localStorage.pass = '';
                            localStorage.chkbx = '';
                            //window.location.href = "home.html";
                            window.location.href = "LiveImage.aspx";
                        }*/

                    }
                    else {
                        alert("Please Invalid User ID Password.");
                       // $.notifyBar({ cssClass: "error", html: "PLEASE ENTER VALID ADID AND PASSWORD" });
                    }
                }
            });
            
        }
    });


    $('.register #btnGuest').click(function () {
        window.location.href = "LiveImage.aspx";
    });

});


