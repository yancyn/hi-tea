//  Declare SQL Query for SQLite
var db = openDatabase("pos", "1.0", "Hi Tea Pos", 4 * 1024 * 1024);
var updateStatement = "UPDATE OrderItem SET StatusId = 2 WHERE Id = ?";
var selectStatement = "SELECT Menu.Code, Menu.Name, OrderItem.*, 'Order'.* FROM OrderItem JOIN 'Order' ON OrderItem.ParentId = 'Order'.Id JOIN Menu ON OrderItem.MenuId = Menu.Id WHERE OrderItem.StatusId = 1 AND Menu.CategoryId IN(1,2,3) ORDER BY OrderItem.Id, 'Order'.Id;";
var dataset;

function markComplete(i) {
    //console.log("Updating " + i);
    var item = dataset.item(i);
    var id = item['Id'];
    db.transaction(function (tx) { tx.executeSql(updateStatement, [id], refresh, onError); });
}

function getTime(date) {
    return "[" + date.substring(11, 16) + "]";
}

/**
 * Called When Page is ready.
 */
 function initDatabase() {
 	console.log("initDatabase");

    try {
		// Check browser is supported SQLite or not.
        if (!window.openDatabase) {
            alert('Databases are not supported in this browser.');
        } else {
        	// If supported then call Function for create table in SQLite
        	refresh();
        }
    }
    catch (e) {    
    	// Version number mismatch. 
        if (e == 2) {
            console.log("Invalid database version.");
        } else {
            console.log("Unknown error " + e + ".");
        }

        return;
    }
}

function refresh() {
    // start loading database
    $("#table").html('');

    var line = "<tr><th>No.</th><th>Time</th><th style='text-align:left'>Item</th><th>Table</th><th>Out</th><th>Queue</th><th><span class='icon icon-refresh' onclick='refresh();' style='margin:0 0 0 10px;cursor:pointer;'></span></th></tr>";
    $("#table").append(line);
    
    console.log("start execute select query");
    db.transaction(function (tx) {
        tx.executeSql(selectStatement, {}, function (tx, result) {
            dataset = result.rows;
            for (var i = 0, item = null; i < result.rows.length; i++) {
                item = result.rows.item(i);
                console.log(item['Id']+": " + item['Name']);
                var menu = item['Code'] + " " + item['Name'];
                var table = item['TableNo'];
                var away = (item['OrderTypeId'] > 1) ? "<span class='icon icon-check'></span>" : "<span class='icon icon-check-empty'></span>";
                var line = "<tr><td>"+(i+1).toString()+".</td><td>"+getTime(item['Created'])+"</td><td>"+menu+"</td><td style='text-align:center'>"+table+"</td><td>"+away+"</td><td><span class='ball'>"+item['QueueNo']+"</span></td><td><input type='button' class='button' onclick='markComplete("+i+")' value='DONE' /></td></tr>";
                $("#table").append(line);
            }
            $("#total").html("Total: "+result.rows.length.toString());
        }, function(tx, error) {
        	alert(error.message);
        });
    });
}

function zeroPrefix(value, digit) {
    var output = "";
    for (var i = 0; i < digit - value.toString().length; i++) {
        output += "0";
    }
    output += value.toString();
    return output;
}

function displayLocalDateTime(date) {
    var output = "";
    var date = new Date();
    output = date.getDate() + "/" + (date.getMonth() + 1).toString() + "/" + (date.getYear() + 1900).toString();
    output += " " + zeroPrefix(date.getHours(), 2);
    output += ":" + zeroPrefix(date.getMinutes(), 2);
	//output += (date.getHours() > 11) ? "PM" : "AM";
    return output;
}

$(function () {
    $("#time").html(displayLocalDateTime());
    initDatabase();
});

/**
 * Handle error.
 */
function onError(tx, error) {
    alert(error.message);
}

