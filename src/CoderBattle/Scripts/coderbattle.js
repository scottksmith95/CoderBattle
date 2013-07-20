// helper to trigger an event on brawler with arguments
function trigger($el, eventName) {
    var args = _.toArray(arguments);
    args.shift(); // remove $el
    args.shift(); // remove eventName

    $el.closest('.brawler').trigger(eventName, args);
}

function setPageHash() {
    var b1 = $('#brawler1').data('username') || '-';
    var b2 = $('#brawler2').data('username') || '-';

    location.hash = '#' + b1 + '/' + b2;

    if (b1 != '-' && b2 != '-') {
        $('#fight').prop('disabled', false);
    }
}

function getBits(number, reverse) {
    var s = number.toString(2);

    if (reverse) {
        s = s.split('').reverse().join('');
    }

    return s.replace(/\d/g, function (flag) {
        return '<span class=bit' + flag + '></span>';
    });
}

function showBout($header, items) {

    items.forEach(function (item) {
        var className,
            value,
            $cell,
            $row = $(
                '<div class="row">' +
                    '<div class="Fighter1Hits span3"></div>' +
                    '<div class="Message span2"></div>' +
                    '<div class="Fighter2Hits span3"></div>' +
                '</div>');

        $header.after($row);

        // dirty - let the classnames match
        for (className in item) {
            value = item[className];
            //if (value === 0) continue;

            $cell = $row.find('.' + className);

            if (className == 'Message') {
                $cell.text(value);
            } else {
                $cell.html(getBits(value, className == 'Fighter2Hits'));
            }
        }
         
    });
}


// brawler init
$('.brawler .init').on('keyup', function (e) {
    if (e.keyCode != 13) return;
    var el = $(this),
        name = el.val();

    trigger(el, 'b_setName', name);
    //el.closest('.brawler').trigger();
});

$('.brawler').on({
    b_getGravatar: function (e, hash, link) {
        $b = $(this);
        var url = 'http://www.gravatar.com/avatar/' + hash + '?d=mm&s=512';
        console.log(url);

        $b.html('<a target=_blank href="' + link + '"><img src="' + url + '" width="256" height="256"></a>');
    },
    b_setProfile: function (e, profile) {
        $b = $(this);
        $b.data(profile);

        var gravatar = profile.gravatar_hash,
            link = profile.link;

        if (gravatar && link) {
            trigger($b, 'b_getGravatar', gravatar, link);
            setPageHash();
        } else {
            $b.find('label').append('<div class=error>not found</div>');
        }
    },
    b_setName: function (e, name) {
        console.log(e.name, name);

        if (!name || name == '-') {
            console.warn('ignoring empty name');
            return;
        }

        var $b = $(this);

        $.getJSON('https://coderbits.com/' + name + '.json?callback=?').then(function (data) {
            //		console.log(data);
            trigger($b, 'b_setProfile', data);
        });
    }
});

$(document).ready(function () {
    var fightHubProxy = $.connection.fightHub;

    fightHubProxy.client.boutComplete = function (boutResult) {
        console.log(boutResult);
        var category = boutResult.Category,
            items = boutResult.Results,
            header = $('<h3>' + _.escape(category) + '</h3>');

        // change `after` to `before` to have newest at the bottom
        $('#results').before(header);

        showBout(header, items);
    };

    //Start the signalr hub
    $.connection.hub.start().done(function () {
        var hash = location.hash;
        if (hash && (hash = hash.substr(1))) {
            var names = hash.split('/'),
                bawler1 = names[0],
                bawler2 = names[1];

            trigger($('#brawler1'), 'b_setName', bawler1);
            trigger($('#brawler2'), 'b_setName', bawler2);
        }
    });

    $('#fight').on('click', function () {
        fightHubProxy.server.start('scott', 'thabo');
        return false;
    });
});