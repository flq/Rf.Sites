
function init_pagination(opts) 
{
    var total_count = opts.total_count;
    var element = opts.element;
    var url = opts.url_template;

    delete opts.element;
    delete opts.total_count;

    opts["callback"] = function (new_page_index, pagination_container) { return true; }

    $('#pagination').pagination(total_count, opts);
}

function turn_on_code_highlight() {
    dp.SyntaxHighlighter.HighlightAll('code');
}