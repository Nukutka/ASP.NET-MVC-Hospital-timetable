$(document).ready(function () {
    $(":checkbox").click(function () {
        if ($(":checked").length > 6) {
            this.checked = false;
            alert('Ограничил шестью специалистами');
        }
    });
});




