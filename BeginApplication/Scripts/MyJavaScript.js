$(document).ready(function () {

    //$('input[type=file]').bootstrapFileInput();
    //$('.file-inputs').bootstrapFileInput();

    $(".to-form-add-comment").on('click', function () {
        if ($(".form-add-comment").css('display') == 'none') {
            $(".form-add-comment").show();
        }
        $('html, body').animate({
            scrollTop: $("#cur-form-add-comment").offset().top
        }, 800);
    });

    $("#changeAvatarRef").on('click', function () {   
        if ($(".changeAvatar").css('display') == 'none') {
            $(".changeAvatar").show("slow");
        }
        else {
            $(".changeAvatar").hide("slow");
        }
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

function OnFailureRenameSection(request) {
    alert("Раздел не был переименован.");
}
function OnFailureRemoveSection(request) {
    alert("Раздел не был удален.");
}
function OnFailureRemoveUser(request) {
    alert("Пользователь не был удален.");
}