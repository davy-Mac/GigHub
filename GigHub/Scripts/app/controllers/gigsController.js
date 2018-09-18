var GigsController = function (attendanceService) {
    var button;

    var init = function () {
        $(".js-toggle-attendance").click(toggleAttendance);
    };

    var toggleAttendance = function (e) { /*here is where the button click event is handled*/
        button = $(e.target);

        var gigId = button.attr("data-gig-id"); /*stores the data attaced to the button*/

        if (button.hasClass("btn-default"))
            attendanceService.createAttendance(gigId, done, fail);
        else
            attendanceService.deleteAttendance(gigId, done, fail);
    };


    var done = function () {
        var text = (button.text() == "Going") ? "Going?" : "Going";
        button.toggleClass("btn-info").toggleClass("btn-default").text(text);
    };

    var fail = function () {
        alert("Something failed");
    };

    return {
        init: init
    }
}(AttendanceService);