$('#btnSubmit').click(function (e) {
    var url = $('#txtLongurl').val();
    shorten(url);
});

function shorten(url) {
    $('#error').hide();
    $('#progress').toggleClass("d-none")
    var ajaxRequest =  $.ajax({
        type: "POST",
        url: window._serviceAddress + "api/url/shorten",
        data: JSON.stringify({ LongUrl: url }),
        contentType: "application/json"
    });

    ajaxRequest.done(function (shortUrl) {
        var url = $(location).attr('protocol') + '//' + $(location).attr('host') + '/' + shortUrl;
        $('#ulShortUrl').append('<li><a target="_blank" href="' + url + '">' + url +'</a></li>')
        $('#progress').toggleClass("d-none")
    });

    ajaxRequest.fail(function (jqXHR, status) {
        $('#error').text(jqXHR.responseText);
        $('#error').show();
        $('#progress').toggleClass("d-none")
    });

}
