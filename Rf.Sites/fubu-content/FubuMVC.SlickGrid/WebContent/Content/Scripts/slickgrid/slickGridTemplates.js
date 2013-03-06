_.templateSettings = {
    interpolate: /\{\{(.+?)\}\}/g
};

(function ($) {
    var templates = {};

    var getTemplate = function (subject) {
        if (templates[subject] == null) {
            var html = $('[data-subject=' + subject + ']').html();
            templates[subject] = _.template(html);
        }

        return templates[subject];
    }


    function UnderscoreTemplateFormatter(row, cell, value, columnDef, dataContext) {
        var subject = columnDef["displaySubject"];

        return Slick.Templates.apply(subject, dataContext);
    }

    $.extend(true, window, {
        "Slick": {
            "Formatters": {
                "Underscore": UnderscoreTemplateFormatter
            },
            "Templates": {
                apply: function (subject, data) {
                    return getTemplate(subject)(data);
                }
            }
        }
    });



})(jQuery);