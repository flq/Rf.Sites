$.validator.setDefaults({
  submitHandler: function() {
    var f = $("#comment_post");
    var act = f.attr("action");
    var ser = f.serialize();
    f.fadeOut("slow");
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
  //validation support
  $("#comment_post").validate();
  // tooltips wire-up for comment posting
  $('.tooltip').tooltip();
  // Highlight code stuff if there is any
  sh_highlightDocument();
});