function error(data) {
    if (data.message != "") {
        window.location.href = "/Home/Index";
    } else {
        toastr.error(data.message);
    }
}
function checkin(ids)
{
    if (ids==undefined) {
        $("[name=allcheck").each(function () {
            $(this).prop("checked", false);
        });
        return;
    }
    var idArrary = ids.split(',');
    $("[name=allcheck").each(function () {
        if (jQuery.inArray($(this).attr("id"),idArrary)!=-1) {
            $(this).prop("checked", true);
        } else {
            $(this).prop("checked",false);
        }
    });
}
function checkout() {
    var idArrary = new Array();
    $("[name=allcheck").each(function () {
        if ($(this).is(":checked")) {
            idArrary.push($(this).attr("id"));
        }
    });
    return idArrary.join(",");
}