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

// brawler init
$('.brawler .init').on('keyup', function (e) {
	if (e.keyCode != 13) return;
	var el = $(this),
		name = el.val();

	trigger(el, 'b_setName', name)
	//el.closest('.brawler').trigger();
});

$('.brawler').on({
	b_getGravatar: function (e, hash, link) {
		$b = $(this);
		var url = 'http://www.gravatar.com/avatar/' + hash + '?d=mm&s=256';
		console.log(url);

		$b.html('<a target=_blank href="' + link + '"><img src="' + url + '"></a>')
	},
	b_setProfile: function (e, profile) {
		$b = $(this);
		$b.data(profile);
		trigger($b, 'b_getGravatar', profile.gravatar_hash, profile.link);
		setPageHash();
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
		})
	}
});

$(document).ready(function () {
	var fightHubProxy = $.connection.fightHub;

    fightHubProxy.client.boutComplete = function (boutResult) {
        console.log(boutResult);
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