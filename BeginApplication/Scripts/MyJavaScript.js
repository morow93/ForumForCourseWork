$(document).ready(function () {

    $(".buttonShow").on('click', function () {
        $("#add-comment-form").show("slow");
        window.location.hash = "#add-comment-form";
    });

    $("#changeAvatarRef").on('click', function () {   
        if ($(".changeAvatar").css('display') == 'none') {
            $(".changeAvatar").show("slow");
        }
        else {
            $(".changeAvatar").hide("slow");
        }
    });

    $(".del-comment-ref").click(function () {
        var answer = confirm('Вы действительно хотите удалить сообщение?');
        return answer;        
    });

    $(".del-theme-ref").click(function () {
        var answer = confirm('Вы действительно хотите удалить тему?');
        return answer;
    });
    
    $(".del-ref").click(function () {
        var answer = confirm('Вы действительно хотите удалить пользователя?');
        return answer;
    });
});