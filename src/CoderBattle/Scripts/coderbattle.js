$(document).ready(function () {
    var fightHubProxy = $.connection.fightHub;

    fightHubProxy.client.boutComplete = function (boutResult) {
        console.log(boutResult);
    };

    //Start the signalr hub
    $.connection.hub.start();

    $('#fight').on('click', function () {
        fightHubProxy.server.start('scott', 'thabo');
        return false;
    });
});