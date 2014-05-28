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
    
    $(".show-hide-form-create-section").on('click', function () {
        if ($(".form-create-section").css('display') == 'none') {
            $(".form-create-section").show("slow");
        }
        else {
            $(".form-create-section").hide("slow");
        }
    });

    $(".admit-comment-ref").click(function () {
        var answer = confirm('Вы действительно хотите допустить сообщение?');
        return answer;
    });

    $(".del-comment-ref").click(function () {
        var answer = confirm('Вы действительно хотите удалить сообщение?');
        return answer;        
    });

    $(".del-theme-ref").click(function () {
        var answer = confirm('Вы действительно хотите удалить тему?');
        return answer;
    });
    
    $(".del-section-ref").click(function () {
        var answer = confirm('Вы действительно хотите удалить раздел?');
        return answer;
    });

    $(".del-user-ref").click(function () {
        var answer = confirm('Вы действительно хотите заблокировать пользователя?');
        return answer;
    });
    
    $(".rec-user-ref").click(function () {
        var answer = confirm('Вы действительно хотите восстановить пользователя?');
        return answer;
    });
});

function OnSuccessCreateSection(data) {
    setTimeout(function () {
        alert("Раздел был добавлен.");
    }, 500);    
}
function OnFailureCreateSection(request) {
    setTimeout(function () {
        alert("Раздел не был добавлен.");
    }, 500);
}
function OnFailureRenameSection(request) {
    alert("Раздел не был переименован.");
}
function OnFailureRemoveSection(request) {
    alert("Раздел не был удален.");
}
function OnFailureRemoveUser(request) {
    alert("Пользователь не был удален.");
}
function OnFailureRecoveryUser(request) {
    alert("Пользователь не был восстановлен.");
}