$(document).ready(function () {
		$('input[type=file]').bootstrapFileInput();
		$('.file-inputs').bootstrapFileInput();
    $(".section-form-to-hide").hide();
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
 
    $("#show-form-create-section").on('click', function () {
        if ($(".section-form-to-hide").css('display') == 'none') {
            $(".section-form-to-hide").show("slow");
        }
        else {
            $(".section-form-to-hide").hide("slow");
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
        var answer = confirm('Вы действительно хотите удалить пользователя?');
        return answer;
    });
    
});

function OnSuccessCreateSection(data) {
    alert("Раздел был добавлен.");
}
function OnFailureCreateSection(request) {
    alert("Раздел не был добавлен.");
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