var FollowingService = function() {  /*this module is responsible for calling the api*/
    var createFollowing = function(followeeId, done, fail) {
        $.post("/api/followings", { followeeId: followeeId }) /*jquery ajax method to call the endpoint*/
            .done(done)
            .fail(fail);
    };

    var deleteFollowing = function(followeeId, done, fail) {
        $.ajax({                                        /*jquery ajax is used to call the api endpoint*/
                url: "/api/followings/" + followeeId,
                method: "DELETE"
            })
            .done(done)
            .fail(fail);
    };

    return {
        createFollowing: createFollowing,
        deleteFollowing: deleteFollowing
    }
}(); /*this makes this module an Immediately Invoked Function Expression*/