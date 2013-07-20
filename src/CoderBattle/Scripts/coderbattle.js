$(document).ready(function () {
    var fightHubProxy = $.connection.fightHub;
    fightHubProxy.client.win = function () {
        
    };
    fightHubProxy.client.lose = function () {

    };
    $.connection.hub.start().done(function () {
    });
});