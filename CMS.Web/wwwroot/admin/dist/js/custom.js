function showPassword(iconId, inputName, lockClass, unclockClass) {
    $(`#${iconId}`).on('mouseup', function () {
        //$(this).attr('class', lockClass);
        $(this).children().attr('class', lockClass);
        $(`input[name="${inputName}"]`).attr('type', 'password');
    }).on('mousedown', function () {
        //$(this).attr('class', unclockClass);
        $(this).children().attr('class', unclockClass);
        $(`input[name="${inputName}"]`).attr('type', 'text');
    });
}