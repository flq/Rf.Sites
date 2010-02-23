$.validator.setDefaults({
  submitHandler: function() {
    var f = $("#comment_post");
    var act = f.attr("action");
    var ser = f.serialize();
    $.post(act, ser, function() { $("#after_comment_post").fadeIn("slow"); });
    return false;
  }
});

jQuery(document).ready(function($) {
  // facebox support
  $('a[rel*=box]').facebox({
    loadingImage: '/files/images/facebox/loading.gif',
    closeImage: '/files/images/facebox/closelabel.gif'
  });
  //treeview support
  $('#archive').treeview({ url: "/chronics" });
  $("#comment_post").validate();
  // comment post fadeout on ajax Post
  jQuery().ajaxStart(function() {
    $("#comment_post").fadeOut("slow");
  });
  // tooltips wire-up for comment posting
  $('.tooltip').tooltip();
  // Highlight code stuff if there is any
  sh_highlightDocument();
});