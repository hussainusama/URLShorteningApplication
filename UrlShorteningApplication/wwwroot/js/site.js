$('#btnSubmit').click(function (e) {
    var url = $('#txtLongurl').val();
    shorten(url);
});

function shorten(url) {
    $('#error').hide();
    $('#progress').toggleClass("d-none");
    var ajaxRequest =  $.ajax({
        type: "POST",
        url: window._serviceAddress + "api/url/shorten",
        data: JSON.stringify({ LongUrl: url }),
        contentType: "application/json"
    });

    ajaxRequest.done(function (shortUrl) {
        var url = $(location).attr('protocol') + '//' + $(location).attr('host') + '/' + shortUrl;
        $('#ulShortUrl').append('<li><a target="_blank" href="' + url + '">' + url + '</a></li>');
        $('#progress').toggleClass("d-none");
    });

    ajaxRequest.fail(function (jqXHR, status) {
        if (jqXHR.status == 400) {
            $('#error').text(jqXHR.responseText);
        }
        else (jqXHR.status == 500)
        {
            $('#error').text("Something unexpected occured. Please try again.");
        }
        $('#error').show();
        $('#progress').toggleClass("d-none");
    });

}
