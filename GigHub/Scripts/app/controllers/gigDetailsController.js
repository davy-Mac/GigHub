var GigDetailsController = function(followingService) { /*this module depends on followingService module*/
    var followButton; /*this is the follow button in the details page*/

    var init = function() {
        $(".js-toggle-follow").click(toggleFollowing); /*selects the js-toggle-follow object click event*/
    };

    var toggleFollowing = function(e) { /*this is the click handler for the button with class js-toggle-follow*/
        followButton = $(e.target); /*gets the reference to the button that was clicked*/
        var followeeId = followButton.attr("data-user-id"); /*gets the followeeId*/

        if (followButton.hasClass("btn-default"))
            followingService.createFollowing(followeeId,done,fail); /*if the button has a btn-default class, creates a following object*/
        else
            followingService.deleteFollowing(followeeId,done,fail); /*if the button has a btn-info class, deletes the following object*/
    };

    var done = function() { /*this is the done method that changes the button's style and text*/
        var text = (followButton.text() == "Follow") ? "Following" : "Follow";
        followButton.toggleClass("btn-info").toggleClass("btn-default").text(text);
    };

    var fail = function() {
        alert("Something failed");
    };

    return {
        init: init
    }
}(FollowingService);