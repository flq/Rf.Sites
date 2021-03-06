﻿
function init_pagination(opts) 
{
    var total_count = opts.total_count;
    var element = opts.element;
    var url = opts.url_template;

    delete opts.element;
    delete opts.url_template;
    delete opts.total_count;

    var handlers = {
      on_error: function (x, e) {
        alert("Something bad happened, the server returned" + x.status + ", sorry!"); 
      },
      on_data: function (data) {
        $("#page").fadeOut(100, function () { $(this).html(data); }).fadeIn(400);
      }
    };

    opts["callback"] = function (new_page_index, pagination_container) {
        $.get(url + new_page_index, handlers.on_data).error(onerror);
        return false;
    }

    $('#pagination').pagination(total_count, opts);
}

function init_searchbar(search_element, url) {
    // working with the following structure
    // [{linktext: "bla", link: "http:..." }, ...]

    var render = function (ul, value) {
        var link = "<a href=\"" + value.link + "\">" + value.linktext + "</a>";
        return $("<li></li>")
				.data("item.autocomplete", value)
				.append(link)
				.appendTo(ul);
    };

    search_element.autocomplete({
        source: url,
        select: function (event, ui) {
            window.location = ui.item.link;
            return true;
        }
    })
    .data("autocomplete")._renderItem = render;
}

function turn_on_code_highlight() {
    dp.SyntaxHighlighter.HighlightAll('code');
}