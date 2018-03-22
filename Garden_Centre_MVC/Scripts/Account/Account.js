$(document).ready(function() {

    $("#ForgottonPasswordLink").click(function() {

        $.ajax({
            url: "/Account/ForgotPassword",
            success: function (content) {
                //alert(content);
                $("#forgotPasswordModalBody").html(content);
            }

        });

        $("#ForgottenPasswordModal").modal();

        e.preventDefault();

    });


//    $("#loginForm").submit(function() {
//        var form = $("#loginForm").serialize();

//        alert("form submitted");


//        $.ajax({
//            url: "/Account/Login",
//            data: form,
//            success: function (data) {
//                alert(data);
//            }
//        });

//        e.preventDefault();


//    });


//    $("#click").click(function() {

//        alert("button clicked");

//        $.ajax({
//            url: "/Account/Send",
//            success: function(data) {
//                $(".wrapper").innerHTML = data;
//            }
//        });

//        e.preventDefault();
//    });


});