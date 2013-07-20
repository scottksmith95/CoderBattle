$(document).ready(function () {
    var fightHubProxy = $.connection.fightHub;

    fightHubProxy.client.win = function () {
        console.log('you won');
    };

    fightHubProxy.client.lose = function () {
        console.log('you lost');
    };

    //Start the signalr hub
    $.connection.hub.start();

    $('#fight').on('click', function () {
        fightHubProxy.server.start();
    });
});