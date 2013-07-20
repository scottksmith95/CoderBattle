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

    var $holder = $('<div></div>');

    $header.after($holder);

    items.forEach(function (item) {
        var className,
            value,
            $cell,
            $row = $(
                '<div class="row">' +
                    '<div class="Fighter1Hits span3" title="Dec: ' + item.Fighter1Hits + ' Hex: ' + item.Fighter1Hits.toString(16) + ' Bin: ' + item.Fighter1Hits.toString(2) + '"></div>' +
                    '<div class="Message span2"></div>' +
                    '<div class="Fighter2Hits span3" title="Dec: ' + item.Fighter2Hits + ' Hex: ' + item.Fighter2Hits.toString(16) + ' Bin: ' + item.Fighter2Hits.toString(2) + '"></div>' +
                '</div>');

        $holder.before($row);

        // dirty - let the classnames match
        for (className in item) {
            value = item[className];
            if (value === 0) continue;

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
}).on('blur', function (e) {
    var el = $(this),
        name = el.val();

    trigger(el, 'b_setName', name);
});

$('.brawler').on({
    b_getGravatar: function (e, hash, link, name) {
        $b = $(this);
        var url = 'http://www.gravatar.com/avatar/' + hash + '?d=mm&s=1024';

        $b.html('<a target=_blank href="' + link + '"><img src="' + url + '" width="256" height="256" class="animated"></a><h3>' + name + '</h3>');
    },
    b_setProfile: function (e, profile) {
        $b = $(this);
        $b.data(profile);

        var gravatar = profile.gravatar_hash,
            link = profile.link;

        if (gravatar && link) {
            trigger($b, 'b_getGravatar', gravatar, link, profile.name);
            setPageHash();
        } else {
            $b.find('label').append('<div class=error>not found</div>');
        }
    },
    b_setName: function (e, name) {
        if (!name || name == '-') {
            return;
        }

        var $b = $(this);

        $.getJSON('https://coderbits.com/' + name + '.json?callback=?').then(function (data) {
            trigger($b, 'b_setProfile', data);
        });
    }
});

$(document).ready(function () {
    var fightHubProxy = $.connection.fightHub;
    var fighter1Losses = 0;
    var fighter2Losses = 0;

    var effectsArray = ['flash', '', 'shake', '', 'bounce', '', 'swing', '', 'wobble'];

    fightHubProxy.client.boutComplete = function (boutResult) {
        $('#results-wrapper').show();
        $('#fight-again').show();
        $('#fight-again').prop('disabled', false);

        console.log(boutResult);

        if (boutResult.Fighter1Won) {
            $('#brawler2 img').addClass(effectsArray[fighter2Losses]).addClass('faded' + fighter2Losses);
            fighter2Losses++;
        } else if (boutResult.Fighter2Won) {
            $('#brawler1 img').addClass(effectsArray[fighter1Losses]).addClass('faded' + fighter1Losses);
            fighter1Losses++;
        }

        var category = boutResult.Category,
            items = boutResult.Results,
            header = $('<h3>' + _.escape(category) + '</h3>');

        // change `after` to `before` to have newest at the bottom
        $('#results').before(header);

        showBout(header, items);

        if (fighter1Losses + fighter2Losses == 9) {
            if (fighter1Losses < fighter2Losses) {
                $('#brawler1').addClass('winner');
                $('#brawler1 h3').text($('#brawler1 h3').text() + ' is the Winner!');
                $('#brawler2 img').addClass('hinge');
                $('#brawler2 h3').hide();
            }
            else {
                $('#brawler2').addClass('winner');
                $('#brawler2 h3').text($('#brawler2 h3').text() + ' is the Winner!');
                $('#brawler1 img').addClass('hinge');
                $('#brawler1 h3').hide();
            }
        }
    };

    var getBrawlers = function () {
        var hash = location.hash;
        if (hash && (hash = hash.substr(1))) {
            return hash.split('/');
        }

        return null;
    };

    var getBrawler1 = function() {
        var brawlers = getBrawlers();
        return brawlers[0];
    };

    var getBrawler2 = function () {
        var brawlers = getBrawlers();
        return brawlers[1];
    };

    //Start the signalr hub
    $.connection.hub.start().done(function () {
        var brawlers = getBrawlers();
        if (brawlers && brawlers.length == 2) {
            trigger($('#brawler1'), 'b_setName', brawlers[0]);
            trigger($('#brawler2'), 'b_setName', brawlers[1]);
        }
    });

    $('#fight').on('click', function () {
        fightHubProxy.server.start(getBrawler1(), getBrawler2());
        $('#fight').hide();
        return false;
    });

    $('#fight-again').on('click', function () {
        window.location.href = '/';
    });
});