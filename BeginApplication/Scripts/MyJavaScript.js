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
    });//не менять

    $("#changeAvatarRef").on('click', function () {   
        if ($(".changeAvatar").css('display') == 'none') {
            $(".changeAvatar").show("slow");
        }
        else {
            $(".changeAvatar").hide("slow");
        }
    });//не менять
 
    $(".show-hide-form-create-section").on('click', function () {
        
        if ($(".form-create-section").css('display') == 'none') {
            $(".form-create-section").show("slow");
        }
        else {
            $(".form-create-section").hide("slow");
        }
        //можно попробовать здесь очистить поля формы
    });//не менять
   
    $(".del-section-ref").click(function () {
        var answer = confirm('Вы действительно хотите удалить раздел?');
        return answer;
    });//не менять

    $(".del-user-ref").click(function () {
        var answer = confirm('Вы действительно хотите заблокировать пользователя?');
        return answer;
    });//не менять

    $(".rec-user-ref").click(function () {
        var answer = confirm('Вы действительно хотите восстановить пользователя?');
        return answer;
    });//не менять
    
});

//раздел
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
//раздел

function OnFailureRemoveUser(request) {
    alert("Пользователь не был удален.");
}