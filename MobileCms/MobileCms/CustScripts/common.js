/*两边去逗号*/
function trimComma(str) {
    return str.replace(/(^\,*)|(\,*$)/g, "");
}

function initWarningDialog() {
    $("#warningDialog").dialog({
        modal: true,
        autoOpen: false,
        draggable: false,
        resizable: false
    });
}

function openWarningDialog(message) {
    initWarningDialog();

    $("#warningDialog_Message").text("");
    $("#warningDialog_Message").text(message);

    $("#warningDialog").dialog("open");
}

function closeWarningDialog() {
    $("#warningDialog").dialog("close");
}

function initMessageDialog() {
    $("#messageDialog").dialog({
        modal: true,
        autoOpen: false,
        draggable: false,
        resizable: false
    });
}

function openMessageDialog(message) {
    initMessageDialog();

    $("#messageDialog_Message").text("");
    $("#messageDialog_Message").text(message);

    $("#messageDialog").dialog("open");
}

function closeMessageDialog() {
    $("#messageDialog").dialog("close");
}

function initLoadingDialog() {
    $("#loadingDialog").dialog({
        modal: true,
        autoOpen: false,
        draggable: false,
        resizable: false,
        dialogClass: "my-dialog",
        title: "Loading..."
    });
}

function openLoadingDialog() {
    initLoadingDialog();
    $("#loadingDialog").dialog("open");
}

function closeLoadingDialog() {
    $("#loadingDialog").dialog("close");
}

function initConfirmDialog(option, id) {
    $("#confirmDialog").dialog({
        modal: true,
        autoOpen: false,
        draggable: false,
        resizable: false,
        buttons: {
            "确认": function () {
                if (option == "delete") {
                    deleteById(id);
                    $(this).dialog("close");
                } else if (option == "batchdelete") {
                    batchDelete(id);
                    $(this).dialog("close");
                } else if (option == "resetpassword") {
                    resetPassword(id);
                    $(this).dialog("close");
                } else if (option == "lock") {
                    lockById(id);
                    $(this).dialog("close");
                } else if (option == "clear") {
                    clearById(id);
                    $(this).dialog("close");
                } else if (option == "sendmessage") {
                    sendMessageById(id);
                    $(this).dialog("close");
                } else if (option == "poweroff") {
                    powerOffById(id);
                    $(this).dialog("close");
                } else if (option == "reportlocation") {
                    reportLocationById(id);
                    $(this).dialog("close");
                } else if (option == "listening") {
                    listeningById(id);
                    $(this).dialog("close");
                }
            },
            "关闭": function () {
                $(this).dialog("close");
            }
        }
    });
}

function openConfirmDialog(option, id) {
    initConfirmDialog(option, id);

    $("#confirmDialog").dialog("open");
}

function initConfirmDialogByResetPassword(option, id) {
    $("#confirmDialog_ResetPassword").dialog({
        modal: true,
        autoOpen: false,
        draggable: false,
        resizable: false,
        buttons: {
            "确认": function () {
                if (option == "resetpassword") {
                    resetPassword(id);
                    $(this).dialog("close");
                }
            },
            "关闭": function () {
                $(this).dialog("close");
            }
        }
    });
}

function openConfirmDialogByResetPassword(option, id) {
    initConfirmDialogByResetPassword(option, id);

    $("#confirmDialog_ResetPassword").dialog("open");
}

function trimDoubleQuotation(str) {
    return str.replace(/(^\"*)|(\"*$)/g, "");
}