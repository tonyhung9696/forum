(function($, h, c) {
	var a = $([]),
	e = $.resize = $.extend($.resize, {}),
	i,
	k = "setTimeout",
	j = "resize",
	d = j + "-special-event",
	b = "delay",
	f = "throttleWindow";
	e[b] = 250;
	e[f] = true;
	$.event.special[j] = {
		setup: function() {
			if (!e[f] && this[k]) {
				return false;
			}
			var l = $(this);
			a = a.add(l);
			$.data(this, d, {
				w: l.width(),
				h: l.height()
			});
			if (a.length === 1) {
				g();
			}
		},
		teardown: function() {
			if (!e[f] && this[k]) {
				return false;
			}
			var l = $(this);
			a = a.not(l);
			l.removeData(d);
			if (!a.length) {
				clearTimeout(i);
			}
		},
		add: function(l) {
			if (!e[f] && this[k]) {
				return false;
			}
			var n;
			function m(s, o, p) {
				var q = $(this),
				r = $.data(this, d);
				r.w = o !== c ? o: q.width();
				r.h = p !== c ? p: q.height();
				n.apply(this, arguments);
			}
			if ($.isFunction(l)) {
				n = l;
				return m;
			} else {
				n = l.handler;
				l.handler = m;
			}
		}
	};
	function g() {
		i = h[k](function() {
			a.each(function() {
				var n = $(this),
				m = n.width(),
				l = n.height(),
				o = $.data(this, d);
				if (m !== o.w || l !== o.h) {
					n.trigger(j, [o.w = m, o.h = l]);
				}
			});
			g();
		},
		e[b]);
	}
})(jQuery, this);


var tbody;
function table_merger()
{
    this.cell_class ='selected';
    this.table_id = tbody[0];
    merge(this.table_id,this.cell_class);
    
    function merge(table_id,cell_class)
    {
      var table_div = table_id;
      var hasMerged = true;
      while(hasMerged)
      {
        hasMerged = false;
        if ($(table_div).find("." + cell_class).length < 2)
          return;

        var table_rows = $(table_div).find("tr");
        for(var i=0; i < table_rows.length; i++)
        {
          var selected_cells_row = $(table_rows[i]).find("." + cell_class);
          if(selected_cells_row.length < 2) //less than two cells to merge
            continue;
          //CELLS
          for(var j=0; j < selected_cells_row.length; j++)
          {
            if(j >= selected_cells_row.length - 1) //last cell
              break;
            var current_cell = selected_cells_row[j];
            var next_cell = selected_cells_row[j+1];
            if(current_cell.cellIndex + current_cell.colSpan !== next_cell.cellIndex)
              continue; //if false continue
            if(current_cell.rowSpan !== next_cell.rowSpan)
              continue;

            $(current_cell).attr('colSpan',current_cell.colSpan + next_cell.colSpan);
            $(next_cell).removeClass(cell_class);
            $(next_cell).addClass('hidden');

            hasMerged = true;
            selected_cells_row = $(table_rows[i]).find("." + cell_class);
          }
        }

        var selected_cells = $(table_div).find("." + cell_class);
        if(selected_cells.length < 2) //no cells two merge
          return;
        for(var i=0; i < selected_cells.length; i++)
        {
          if(i >= selected_cells.length - 1) //last cell
            break;
          var current_cell = selected_cells[i];
          var cells = $(current_cell.parentElement).nextAll()[current_cell.rowSpan-1];
          if(!cells)
           continue;
          var next_cell = cells.children[current_cell.cellIndex];
          if(!next_cell) //no next_cell
            continue;
          if( !$(next_cell).hasClass(cell_class) )
            continue;
          if(current_cell.colSpan !== next_cell.colSpan)
            continue;
          $(current_cell).attr('rowSpan',current_cell.rowSpan + next_cell.rowSpan);
          $(next_cell).removeClass(cell_class);
          $(next_cell).addClass('hidden');
          hasMerged = true;
          selected_cells = $(table_div).find("." + cell_class);
        }
      }
    }
}

function table_split(table_id,merge_cell_class)
{
    this.cell_class ='selected';
    table_id = tbody[0];
    split(this.table_id,this.cell_class);
    
    function split(table_id,cell_class)
    {
      var table_div = table_id;
      var hasSplited = true;
      while(hasSplited)
      {
        hasSplited = false;
        if ($(table_div).find("." + cell_class).length < 1)
          return;
        var table_rows = $(table_div).find("tr");
        var getcolSpan=0;
        var getrowSpan=0;
        var getcol=0;
        var getrow=0;
        for(var i=0; i < table_rows.length; i++)
        {
          var selected_cells_row = $(table_rows[i]).find("." + cell_class);
          if(selected_cells_row.length < 1) //less than two cells to merge
            continue;
          var Ocurrent_cell = selected_cells_row[0];
          getcolSpan=parseInt($(Ocurrent_cell).attr('colSpan'));
          getrowSpan=parseInt($(Ocurrent_cell).attr('rowSpan'));
          if (getcolSpan<1 || isNaN(getcolSpan)){
            getcolSpan=1;
          }
          if (getrowSpan<1 ||isNaN(getrowSpan)){
            getrowSpan=1;
          }
          getcol=selected_cells_row.closest("td").index()+1;
          getrow=selected_cells_row.closest("tr").index()+1;
        
        }
        for(var i=0; i < getrowSpan; i++)
        {
          var table_td = $(table_rows[i+getrow-1]).find("td");
          for (var k=0; k<getcolSpan;k++)
          {
            var current_cell = table_td[k+getcol-1];
            //if ($(next_cell).attr('class')==="hidden")
            
              $(current_cell).removeAttr('colSpan');
              $(current_cell).removeAttr('rowSpan');
              if ($(current_cell).attr('class')==="hidden"){
                $(current_cell).removeAttr('class');
              }
          }
        
        
        }    
      }
    }
}
function table_Row(table_id,position)
{
  this.position = position === undefined ? 'before' : 'after';
    var cell_class = 'selected';
    this.table_id = tbody[0];
    Row(this.table_id,cell_class);
    
    function Row(table_id,cell_class)
    {
      var table_div = table_id;
      var hasSplited = true;
      while(hasSplited)
      {
        hasSplited = false;
        if ($(table_div).find("." + cell_class).length < 1)
          return;
        var table_rows = $(table_div).find("." + cell_class);
        var tdcount = $(table_div).find("tr:first td").length;
        if(table_rows.length < 1) //less than two cells to merge
            continue;
        var Ocurrent_cell = table_rows[table_rows.length-1];
        var addNewRow="<tr>"
        for (var i = 0; i < tdcount; i++)
        {
          //table += "<td contenteditable='true' id='grid_configurator_cell_"+i+j+"'></td>";
          addNewRow += "<td contenteditable='true' id='grid_configurator_cell'></td>";
        }
        addNewRow += "</tr>";
        if (position==="before"){
          $(Ocurrent_cell.parentElement).before(addNewRow);
        } else if (position==="after") {
          $(Ocurrent_cell.parentElement).after(addNewRow);
        } else if (position==="remove") {
          $(Ocurrent_cell.parentElement).remove();
        }
      }
    }
}
function table_Column(table_id,position)
{
  this.position = position === undefined ? 'before' : 'after';
    var cell_class = 'selected';
    this.table_id = tbody[0];
    Column(this.table_id,cell_class);
    
    function Column(table_id,cell_class)
    {
      var table_div = table_id;
      var hasSplited = true;
      while(hasSplited)
      {
        hasSplited = false;
        if ($(table_div).find("." + cell_class).length < 1)
          return;
        var table_rows = $(table_div).find("." + cell_class);
        var table_tr = $(table_div).find("tr");
        var getcol=$(table_rows[0]).closest("td").index();
        if(table_rows.length < 1) //less than two cells to merge
            continue;
        var Ocurrent_cell = table_rows[table_rows.length-1];
        var addNewTD="<td contenteditable='true' id='grid_configurator_cell'></td>";
        for(var i=0; i<table_tr.length; i++)
        {
          if (position==="before"){
            $(table_tr[i]).find("td").eq(getcol).before(addNewTD);
          } else if (position==="after") {
            $(table_tr[i]).find("td").eq(getcol).after(addNewTD);
          } else if (position==="remove") {
            $(table_tr[i]).find("td").eq(getcol).remove();
          }
        }
        
      }
    }
}
(function($) {
  $.fn.resizableColumns = function() {
    var isColResizing = false;
    var isRowResizing = false;
    var resizingPosX = 0;
    var resizingPosY = 0;
    var _editor1 = $("#editor1");

    var _tbody = $(_editor1).find('tbody')
      
    _tbody.find('td').each(function() {
      $(this).css('position', 'relative');
      })

    $(document).mouseup(function(e) {
      if (isColResizing) {
        _tbody.find('td').removeClass('resizing');
        isColResizing = false;
        e.stopPropagation();
      }
      else if (isRowResizing) {
        _tbody.find('tr').removeClass('resizing');
        isRowResizing = false;
        e.stopPropagation();
      }
    })
    _editor1.find("td").mouseup(function(e) {
      $("#editor1").attr("contenteditable", "true");
      $(this).focus();
    })
    _editor1.find('.resizerC').mousedown(function(e) {
      var name=$(this.parentElement)
      _tbody.find('td').removeClass('resizing');
      $(name).addClass('resizing');
      resizingPosX = e.pageX;
      isColResizing = true;
        e.stopPropagation();
        $(this).focus();
    })
    _editor1.find('.resizerR').mousedown(function(e) {
      var name=$(this.parentElement.parentElement)
      _tbody.find('tr').removeClass('resizing');
      $(name).addClass('resizing');
      resizingPosY = e.pageY;
      isRowResizing = true;
      e.stopPropagation();
      $('*').removeClass('selected');
      $(this).closest("td").addClass("selected");
   
    })
    $(".resizerR").click(function(){
      $(this.parentElement).focus();
    });
    $(".resizerR").mousedown(function () {
        $(this.parentElement).focus();
    });
    $(".resizerC").mousedown(function () {
        $(this.parentElement).focus();
    });
      $(".resizerR").mouseup(function () {
        $(this.parentElement).focus();
    });
      $(".resizerC").mouseup(function () {
        $(this.parentElement).focus();
    });
    _editor1.mousemove(function(e) {
      if (isColResizing) {
        var _resizing = _tbody.find('td.resizing');
        if (_resizing.length == 1) {
          var _nextRow = _tbody.find('td.resizing + td');
          var _pageX = e.pageX || 0;
          var _widthDiff = _pageX - resizingPosX;
          var _setWidth = _resizing.closest('td').innerWidth() + _widthDiff;
          var _nextRowWidth = _nextRow.innerWidth() - _widthDiff;
          if (resizingPosX != 0 && _widthDiff != 0 ) {
            _resizing.closest('td').innerWidth(_setWidth);
            resizingPosX = e.pageX;
            _nextRow.innerWidth(_nextRowWidth);
          }
        }
      }
      else if (isRowResizing) {
        var _resizing = _tbody.find('tr.resizing');
        if (_resizing.length == 1) {
          var _nextRow = _tbody.find('tr.resizing + tr');
          var _pageY = e.pageY || 0;
          var _HeightDiff = _pageY - resizingPosY;
          var _setHeight = _resizing.closest('tr').innerHeight() + _HeightDiff;
          var _nextRowHeight = _nextRow.innerHeight() - _HeightDiff;
          if (resizingPosY != 0 && _HeightDiff != 0) {
            _resizing.closest('tr').innerHeight(_setHeight);
            resizingPosY = e.pageY;
            if (_nextRow.length>0){
              _nextRow.innerHeight(_nextRowHeight);
            } else {
              var tablediv=$(_resizing[0]).parentsUntil($(".edittable"))
              $(tablediv[0].parentElement.parentElement).css("height","")
            }
          }
        }
      }
    })
  };
}
(jQuery))

$(document).ready(function() {
  $(".edittable").resizableColumns();
});

var tableselect=false;
var clickx=0;
var clicky=0;
$(document).click(function(event){
  if (!$(event.target).closest(".edittable td").length) {
    $(".resizable").css("resize","none");
  } 
  $(".edittable td").mouseup(function(){
    if (tableselect===true)
    {
      tableselect=false;
    }
  });
  $(".edittable td").mousedown(function(){
    //clickx = parseInt($(this).attr("data-x"));
    //clicky = parseInt($(this).attr("data-y"));
    $(".resizable").css("resize","both");
    clickx=$(this).closest("td").index()+1;
    clicky=$(this).closest("tr").index()+1;
    tbody=$(this).parentsUntil($("table"),"tbody")
    $('td').each(function(index, item) {
      if ($(item).hasClass("selected"))
      {
        $(item).removeClass("selected");
      }
  });
    tableselect=true;
    if (tableselect===true)
    {
      $(this).addClass("selected");
    }
  });
  $(".edittable td").hover(function(){
    if (tableselect===true)
    {
      $('td').each(function(index, item) {
        if ($(item).hasClass("selected"))
        {
          $(item).removeClass("selected");
        }
    });
      //var x = parseInt($(this).attr("data-x"));
	  	//var y = parseInt($(this).attr("data-y"));
      var x = $(this).closest("td").index()+1;
	  	var y = $(this).closest("tr").index()+1;
      window.getSelection().removeAllRanges();
       $(tbody[0]).find("tr").children("td").each(function () 
       {
         var datasetx=$(this).closest("td").index()+1
         var datasety=$(this).closest("tr").index()+1
         if (x>=clickx && y>=clicky){
           if( x >= datasetx && datasetx>=clickx && y >=datasety && datasety>=clicky)
           test($(this));
         }
         else if (x<=clickx && y<=clicky){
           if( x <= datasetx && datasetx<=clickx && y <=datasety && datasety<=clicky)
           test($(this));
         }
         else if (x>=clickx && y<=clicky){
           if( x >= datasetx && datasetx>=clickx && y <=datasety && datasety<=clicky)
           test($(this));
         }
         else if (x<=clickx && y>=clicky){
           if( x <= datasetx && datasetx<=clickx && y >=datasety && datasety>=clicky)
           test($(this));
         }
         
      });
      $(this).addClass("selected");
    }
    function test(object)
    {
      if (!object.hasClass("hidden"))
      $(object).addClass("selected");
    }
  });
  
});

