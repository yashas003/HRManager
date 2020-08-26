//Clock implementation
function startTime() {
    var today = new Date();
    var h = today.getHours();
    var m = today.getMinutes();
    var s = today.getSeconds();
    m = checkTime(m);
    s = checkTime(s);

    var hour = h % 12 || 12;
    var ampm = (h < 12 || h === 24) ? "AM" : "PM";

    document.getElementById('clock').innerHTML =
        hour + ":" + m + ":" + s + " " + ampm;
    var t = setTimeout(startTime, 500);
}
function checkTime(i) {
    if (i < 10) { i = "0" + i };  // add zero in front of numbers < 10
    return i;
}

//Edit dialog implementation
var btn = document.getElementById("edit_btn");
var modal = document.getElementById("edit_dialog");

btn.onclick = function () {
    modal.style.display = "block";
}
window.onclick = function (event) {
    if (event.target == modal) {
        modal.style.display = "none";
    }
}

//Choose image implementation
$(document).ready(function () {
    $('.custom-file-input').on("change", function () {
        var fileName = $(this).val().split("\\").pop();
        $(this).next('.custom-file-label').html(fileName);
    });
});

//Validation for edit profile
(function () {
    'use strict';
    window.addEventListener('load', function () {
        var forms = document.getElementsByClassName('needs-validation');
        
        var validation = Array.prototype.filter.call(forms, function (form) {
            form.addEventListener('submit', function (event) {
                if (form.checkValidity() === false) {
                    event.preventDefault();
                    event.stopPropagation();
                }
                form.classList.add('was-validated');
            }, false);
        });
    }, false);
})();

//PunchIn PunchOut implementation
function hideCheckInBtn() {
    document.getElementById("check_out").style.display = "block";
    document.getElementById("check_in").style.display = "none";
}
function hideCheckOutBtn() {
    document.getElementById("check_out").style.display = "none";
    document.getElementById("check_in").style.display = "block";
}
function checkAttandance(isPunched) {
    if (isPunched == 1) {
        document.getElementById("check_out").style.display = "block";
        document.getElementById("check_in").style.display = "none";
    } else {
        document.getElementById("check_out").style.display = "none";
        document.getElementById("check_in").style.display = "block";
    }
}