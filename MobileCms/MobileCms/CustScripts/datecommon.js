function formatterDate(val) {
    var date = new Date(val);

    var month = date.getMonth() + 1;

    var strMonth = "";

    if (month < 10) {
        strMonth = "0" + month.toString();
    } else {
        strMonth = month.toString();
    }

    return date.getFullYear() + '-' + strMonth + '-' + date.getDate();
}