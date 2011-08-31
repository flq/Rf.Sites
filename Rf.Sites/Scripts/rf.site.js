
function init_pagination(opts) 
{
    var total_count = opts.total_count;
    var element = opts.element;
    var url = opts.url_template;

    delete opts.element;
    delete opts.url_template;
    delete opts.total_count;

    opts["callback"] = function (new_page_index, pagination_container) {
        $.get(url + new_page_index, function (data) {
            $("#page").fadeOut(100, function () { $(this).html(data); }).fadeIn(400);
        }).error(function (x, e) { alert("Something bad happened, the server returned" + x.status + ", sorry!"); });
        return false;
    }

    $('#pagination').pagination(total_count, opts);
}

function init_searchbar(search_element) {
    search_element.autocomplete("options", "source", "/lookup");
}

function turn_on_code_highlight() {
    dp.SyntaxHighlighter.HighlightAll('code');
}