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
  $('a[rel*=box]').facebox({
    loadingImage: '/files/images/facebox/loading.gif',
    closeImage: '/files/images/facebox/closelabel.gif'
  });
  $('#archive').treeview({ url: "/chronics" });
  $("#comment_post").validate();
  jQuery().ajaxStart(function() {
    $("#comment_post").fadeOut("slow");
  });
});