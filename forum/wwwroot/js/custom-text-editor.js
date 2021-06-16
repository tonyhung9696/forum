var editorPos = 0;
function getCaretCharacterOffsetWithin(element) {
  var caretOffset = 0;
  var doc = element.ownerDocument || element.document;
  var win = doc.defaultView || doc.parentWindow;
  var sel;
  if (typeof win.getSelection != "undefined") {
    sel = win.getSelection();
    if (sel.rangeCount > 0) {
      var range = win.getSelection().getRangeAt(0);
      var preCaretRange = range.cloneRange();
      preCaretRange.selectNodeContents(element);
      preCaretRange.setEnd(range.endContainer, range.endOffset);
      caretOffset = preCaretRange.toString().length;
    }
  } else if ((sel = doc.selection) && sel.type != "Control") {
    var textRange = sel.createRange();
    var preCaretTextRange = doc.body.createTextRange();
    preCaretTextRange.moveToElementText(element);
    preCaretTextRange.setEndPoint("EndToEnd", textRange);
    caretOffset = preCaretTextRange.text.length;
  }
  editorPos=caretOffset;
  return caretOffset;
}
function setCurrentCursorPosition(chars) {
  if (chars >= 0) {
      var selection = window.getSelection();

      range = createRange(document.getElementById("editor1"), { count: chars });

      if (range) {
          range.collapse(false);
          selection.removeAllRanges();
          selection.addRange(range);
      }
  }
}
function createRange(node, chars, range) {
  if (!range) {
      range = document.createRange()
      range.selectNode(node);
      range.setStart(node, 0);
  }

  if (chars.count === 0) {
      range.setEnd(node, chars.count);
  } else if (node && chars.count >0) {
      if (node.nodeType === Node.TEXT_NODE) {
          if (node.textContent.length < chars.count) {
              chars.count -= node.textContent.length;
          } else {
               range.setEnd(node, chars.count);
               chars.count = 0;
          }
      } else {
          for (var lp = 0; lp < node.childNodes.length; lp++) {
              range = createRange(node.childNodes[lp], chars, range);

              if (chars.count === 0) {
                 break;
              }
          }
      }
 } 

 return range;
}
function getCaret() {
  var el = document.getElementById("editor1")
  var range = document.createRange()
  var sel = window.getSelection()
  GC=range.getCaret()
}

function setCaret() {
  var el = document.getElementById("editable")
  var range = document.createRange()
  var sel = window.getSelection()
  
  range.setStart(el.childNodes[2], 5)
  range.collapse(true)
  
  sel.removeAllRanges()
  sel.addRange(range)
}
function PrintElem()
{
  var elem=document.getElementById("editor1").innerHTML;

  var mywindow = window.open('', 'PRINT');

  mywindow.document.write('<html><head><title>' + document.title  + '</title>');
  mywindow.document.write('<script src="https://ajax.googleapis.com/ajax/libs/jquery/3.5.1/jquery.min.js"></script>');
  mywindow.document.write('<script type="text/javascript" src="custom-text-editor.js"></script>');
  mywindow.document.write('<script src="supporttable.js" charset="utf-8"></script>');
  mywindow.document.write('<script src="table-merger.js" charset="utf-8"></script>');
  mywindow.document.write('<script src="https://code.jquery.com/ui/1.12.1/jquery-ui.js"></script>');
  mywindow.document.write('<link rel="stylesheet" href="https://cdnjs.cloudflare.com/ajax/libs/font-awesome/4.7.0/css/font-awesome.min.css">');
  mywindow.document.write('<link type="text/css" rel="stylesheet" href="custom-text-editor.css"/>');
  mywindow.document.write('<link type="text/css" rel="stylesheet" href="table-merger.css"/>');
  mywindow.document.write('<link type="text/css" rel="stylesheet" href="supporttable.css">');
  mywindow.document.write('<link rel="stylesheet" href="https://code.jquery.com/ui/1.12.1/themes/base/jquery-ui.css">');
  mywindow.document.write('</head><body >');
  mywindow.document.write('<h1>' + document.title  + '</h1>');
  mywindow.document.write(elem);
  mywindow.document.write('</body></html>');

  mywindow.document.close(); // necessary for IE >= 10
  mywindow.focus(); // necessary for IE >= 10*/

  mywindow.print();
  mywindow.close();

  return true;
}
function preview(){
  $(".back").css("display","block");
  $(".CenterScreen").css("display","block");
  $(".c-preview").css("display","block");
  var doc = $("#editor1").html();
  $(".preview").html(doc);
}
function cimage(){
  var el = document.getElementById("editor1");
  getCaretCharacterOffsetWithin(el);
  $(".back").css("display","block");
  $(".CenterScreen").css("display","block");
  $(".c-image").css("display","block");
 
}
function chref(){
  var el = document.getElementById("editor1");
  getCaretCharacterOffsetWithin(el);
  $(".back").css("display","block");
  $(".CenterScreen").css("display","block");
  $(".c-href").css("display","block");
}

function cmedia(){
  var el = document.getElementById("editor1");
  getCaretCharacterOffsetWithin(el);
  $(".back").css("display","block");
  $(".CenterScreen").css("display","block");
  $(".c-media").css("display","block");
}
function ccode(){
  $(".back").css("display","block");
  var doc = $("#editor1").html();
  $(".CenterScreen").css("display","block");
  $(".c-code").css("display","block");
  $(".ccodeview").val(doc);
}
function editcode() {
    var el = document.getElementById("editor1");
    getCaretCharacterOffsetWithin(el);
    $(".back").css("display", "block");
    $(".CenterScreen").css("display", "block");
    $(".edit-code").css("display", "block");
}
function chooseColor(){
  var mycolor = document.getElementById("myColor").value;
  document.execCommand('foreColor', false, mycolor);
}
function chooseColor2(){
  var mycolor = document.getElementById("backColor").value;
  document.execCommand('backColor', false, mycolor);
}

function changeFont(){
  var myFont = document.getElementById("input-font").value;
  document.execCommand('fontName', false, myFont);
}

function changeSize(){
  var mysize = document.getElementById("fontSize").value;
  document.execCommand('fontSize', false, mysize);
}

function checkDiv(){
  var editorText = document.getElementById("editor1").innerHTML;
  if(editorText === ''){
    document.getElementById("editor1").style.border = '5px solid red';
  }
}
function getCaretPosition() {
  var caretPos = 0,
    sel, range;
  if (window.getSelection) {
    sel = window.getSelection();
    if (sel.rangeCount) {
      range = sel.getRangeAt(0);
      caretPos = range.startOffset;
    }
  }
  alert(caretPos);
  return caretPos;
}

function removeBorder(){
  document.getElementById("editor1").style.border = '1px solid transparent';
}
var menushow =false;
$(document).ready(function () {
    var canvas;
    function loadImage(input) {
        var isImage = input.value;
        isImage = isImage.split('.');
        isImage = isImage[isImage.length - 1];
        if (isImage === 'png' || isImage === 'jpg' || isImage === 'jpeg') {
            var fd = new FormData();
            var files = $('.imgupload')[0].files;
            if (files.length > 0) {
                fd.append('file', files[0]);
                $.ajax({
                    type: "POST",
                    url: `${location.protocol}//${window.location.host}/Article/UploadFilesWihtLocation`,
                    contentType: false,
                    processData: false,
                    data: fd,
                    success: function (path) {
                        $("#imgsource").attr("value", path.toString())
                        var img = new Image();
                        img.onload = function () {
                            $("#imagex").attr("value", this.width)
                            $("#imagey").attr("value", this.height)
                        }
                        img.src = path;
                    }
                });
            }
        }
        else {
            alert("something wrong.");
        }
    }
    $(".imgupload").change(function () {
        loadImage(this);
    });
  $("#bimage").click(function(){
    var imagesource=$("#imgsource").val();
    var imagedesc=$("#imagedesc").val();
    var imagex=$("#imagex").val();
    var imagey=$("#imagey").val();
    $(".back").css("display","none");
    $(".CenterScreen").css("display","none");
    $(".c-image").css("display","none");
    $("#imgInp").val('');
    $("#imgsource").attr("value", "");
    $("#imagedesc").attr("value", "");
    $("#imagex").attr("value", "");
    $("#imagey").attr("value", "");
    setCurrentCursorPosition(editorPos);
    document.execCommand('insertHTML', 0, '<img src="' + imagesource + '" alt="' + imagedesc + '" width="' + imagex + '" height="' + imagey + '" />');
  })
  $("#bhref").click(function(){
    var linkURL=$("#URLhref").val();
    var sText=$("#URLtitle").val();
    $(".back").css("display","none");
    $(".CenterScreen").css("display","none");
    $(".c-href").css("display","none");
    $("#URLhref").val("");
    $("#URLtitle").val("");
    setCurrentCursorPosition(editorPos);
    document.execCommand('insertHTML', false, '<a href="' + linkURL + '" target="_blank">' + sText + '</a>');

  })
  $("#bmedia").click(function(){
    var mediahref=$("#mediahref").val();
    var res = mediahref.split("?v=");
    if (res.length>1){
      mediahref="https://www.youtube.com/embed/" + res[1];
      
    }
    $(".back").css("display","none");
    $(".CenterScreen").css("display","none");
    $(".c-media").css("display", "none");
    $("#mediahref").val("");
    setCurrentCursorPosition(editorPos);
    document.execCommand('insertHTML', 0, ' <iframe src="' + mediahref + '" width="560" height="314" allowfullscreen="allowfullscreen"></iframe>');
  })
$("#beditcode").click(function () {
    var languagesname = $("#languagesname").val();
    var codesource = $("#codesource").val();
    $(".back").css("display", "none");
    $(".CenterScreen").css("display", "none");
    $(".edit-code").css("display", "none");
    $("#codesource").val("");
    setCurrentCursorPosition(editorPos);
    var code = ' <br><pre class="line-numbers language-"><code class="language- language-' + languagesname + '"> ' + codesource + ' </code></pre><p><div><br></div>';
    document.execCommand('insertHTML', 0, code);
})
$("#submitCode").click(function () {
    var content = $("#editor1").html();
    $("#content").val(content);
})

$(".c-close").click(function () {
    $("#imgInp").val('');
    $("#imgsource").attr("value","");
    $("#imagedesc").attr("value", "");
    $("#imagex").attr("value", "");
    $("#imagey").attr("value", "");
    $("#URLhref").val("");
    $("#URLtitle").val("");
    $("#mediahref").val("");
    $("#codesource").val("");
    $(".edit-code").css("display", "none");
    $(".back").css("display", "none");
    $(".CenterScreen").css('display', 'none');
    $(this.parentElement.parentElement).find(".c-body>div").each(function() {
        
        $(this).css('display', 'none');
    })
})

  $(".dropbtncustom").click(function(){
    var el = document.getElementById("editor1");
    getCaretCharacterOffsetWithin(el);
    if (menushow===true)
    {
      menushow=false;
    }
    else
    {
      menushow=true;
    }
    if (menushow===true)
    {
      $(".dropdowncustom-content").css("display", "none");
      $(this).parent().find(".dropdowncustom-content").css("display", "block");
    }
    else
    {
      $(".dropdowncustom-content").css("display", "none");
    }
    $("#editor1").focus();
  });
  $(".editorImageDetail").click(function(){
    $(this).parent(".dropdowncustom-content").css("display", "none");
    menushow=false;
  });
  $(".dropbtncustom").hover(function(){
    if (menushow===true)
    {
      $(".dropdowncustom-content").css("display", "none");
      $(this).parent().find(".dropdowncustom-content").css("display", "block");

      $(".editorImageDetail").hover(function(){
          $(".droprightcustom-content").css("display", "none");
          $(this).find(".droprightcustom-content").css("display", "block");
      });
    }
  });
  $(".c-topbar div").hover(function(){
    $(this).css("cursor","default");
  })
  $(".dropdowncustom-content a").hover(function(){
    $(".droprightcustom-content").css("display", "none");
  });

  $(".resizable").css("resize","none");
  
});
$(document).click(function(event) {
  if (!$(event.target).closest(".resizerR").length) {
    $("#editor1").attr("contenteditable", "true");
  } else {
    $("#editor1").attr("contenteditable", "false");
  }
  if (!$(event.target).closest(".dropbtncustom").length) {
    menushow=false;
    $(".dropdowncustom-content").css("display", "none");
  }

  if (!$(event.target).closest("table").length) {
    if (!$(event.target).closest("button").length) {
      $('#editor1 table tr').children('td').each(function () 
      {
        if ($(this).attr("class")==="selected")
        {
          $(this).attr("class", "");
        }
      });
    }
  }
});
function talberesizealbe() {
    $("#editor1 #edittable").find('td').each(function () {
        $(this).css('position', 'relative');
        $(this).append("<div class='resizerR' contenteditable='false' style='position:absolute;left:-3px;bottom:0px;width:100%;height:5px;z-index:999;background:transparent;cursor:row-resize'></div>");
        if ($(this).is(':not(:last-child)')) $(this).append("<div class='resizerC' contenteditable='false' style='position:absolute;top:0px;right:-3px;bottom:0px;width:6px;z-index:999;background:transparent;cursor:col-resize'></div>");
    })
    $("#editor1 #edittable").attr("id", "edittableed")
}
var dashboard_configurator = 
{	
	on_grid_selector_hover: function(element)  
	{
		var x = parseInt(element.getAttribute("data-x"));
		var y = parseInt(element.getAttribute("data-y"));
		dashboard_configurator.grid_choser_parameters = {x:x, y:y};
		$('#grid_chooser').children('div').each(function () 
		{
			if(this.dataset.x <= dashboard_configurator.grid_choser_parameters.x 
					&& this.dataset.y <= dashboard_configurator.grid_choser_parameters.y)
				this.classList.add("chosen");
			else
				this.classList.remove("chosen");
		});
	},
	on_grid_selector_click: function(element)
	{
		var x = element.getAttribute("data-x");
		var y = element.getAttribute("data-y");
		
		var container = $("#editor1");
        var table = '<div class="resizable"><table class="edittable" id="edittable"><tbody>';
		var widthAll =$(container).innerWidth()-20;
		var width=widthAll/x;
		for(var i=0; i<x; i++)
		{
			table += "<tr>";
			for(var j=0; j<y; j++)
			{
                table += "<td contenteditable='true' id='grid_configurator_cell_" + i + j + "'>&nbsp;</td>";
				//table += "<td contenteditable='true' id='grid_configurator_cell' style='position: relative;width: "+ width +"px;'><div class='resizerR' contenteditable='false' style='position:absolute;left:-3px;bottom:0px;width:100%;height:5px;z-index:999;background:transparent;cursor:row-resize'></div><div class='resizerC' contenteditable='false' style='position:absolute;top:0px;right:-3px;bottom:0px;width:6px;z-index:999;background:transparent;cursor:col-resize'></div></td>";
			}
			table += "</tr>";
		}
		table += "</tbody></table></div>";
		//$("#editor1").append(table);
    setCurrentCursorPosition(editorPos);
        document.execCommand('insertHTML', false, table);
        talberesizealbe();
		$(".edittable").resizableColumns();
		//container.fadeOut(1,function(){container.html(table);});
		//container.fadeIn(400);
		
	}
}
