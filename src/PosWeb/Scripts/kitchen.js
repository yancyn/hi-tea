﻿//  Declare SQL Query for SQLite
var db = openDatabase("pos", "1.0", "Hi Tea Pos", 4 * 1024 * 1024);
var updateStatement = "UPDATE OrderItem SET StatusId = 2 WHERE Id = ?";
var selectStatement = "SELECT Menu.Code, Menu.Name, OrderItem.*, 'Order'.* FROM OrderItem JOIN 'Order' ON OrderItem.ParentId = 'Order'.Id JOIN Menu ON OrderItem.MenuId = Menu.Id WHERE OrderItem.StatusId = 1 ORDER BY OrderItem.Id, 'Order'.Id;";
var dataset;

function markComplete(i) {
    console.log("Updating " + i);
    var item = dataset.item(i);
    var id = item['Id'];
    db.transaction(function (tx) { tx.executeSql(updateStatement, [id], refresh, onError); });
}

function getTime(date) {
    return "[" + date.substring(11, 16) + "]";
}

function refresh() {
    // start loading database
    $("#table").html('');

    var line = "<tr><th>No.</th><th>Time</th><th>Item</th><th>In</th><th>Out</th><th>Queue</th><th><input type='button' onclick='refresh();' value='REFRESH' /></th></tr>";
    $("#table").append(line);
    db.transaction(function (tx) {
        tx.executeSql(selectStatement, {}, function (tx, result) {
            dataset = result.rows;
            for (var i = 0, item = null; i < result.rows.length; i++) {
                item = result.rows.item(i);

                var menu = item['Code'] + ": " + item['Name'];
                var table = item['TableNo'];
                var away = (item['OrderTypeId'] > 1) ? "<input type='checkbox' checked onclick='return false;'></input>": "";
                var line = "<tr><td>"+(i+1).toString()+".</td><td>"+getTime(item['Created'])+"</td><td>"+menu+"</td><td>"+table+"</td><td>"+away+"</td><td><span class='ball'>"+item['QueueNo']+"</span></td><td><input type='button' onclick='markComplete("+i+")' value='DONE' /></td></tr>";
                $("#table").append(line);
            }
        });
    });
}

$(function () {
    refresh();
});

/**
 * Handle error.
 */
function onError(tx, error) {
    alert(error.message);
}
