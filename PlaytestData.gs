/*
 PlaytestData.gs
 Created by: Toast <sam@gib.games>
 Created on: 3/18/2024
 Licensable under MIT
*/

var ThisSheet = SpreadsheetApp.getActiveSpreadsheet();

function doGet(e) {
  var params = e.parameter;
  var sheet = ThisSheet.getSheetByName('Master');
  if(params.action == "post")
  {
    return postFormData(e);
  }
  else if (params.action == "retrieve")
  {
    return retrieveData(e);
  }
  else {
    return ContentService.createTextOutput("Invalid action");
  }
}

function postFormData(e) {
    var params = e.parameter;
    var sheet = ThisSheet.getSheetByName(params.version);

    if(sheet==null)
    {
      var sheet = ThisSheet.getSheetByName('Master');
    }

    // Fixed parameters
    var row = [params.timestamp, params.version, params.id];

    // Get all keys except for the fixed ones and 'action', then sort them
    var keys = Object.keys(params).filter(function(key) {
        return !['timestamp', 'version', 'id', 'action'].includes(key);
    }).sort(); // Sorting keys alphabetically

    // Append values in sorted order of keys
    keys.forEach(function(key) {
        row.push(params[key]);
    });

    sheet.appendRow(row);
    return ContentService.createTextOutput("Successfully added data");
}

function retrieveData(e) {
  var params = e.parameter;
  var result = "Not found";
      var sheet = ThisSheet.getSheetByName('Master');
      var rows = sheet.getDataRange().getValues(); // Get all data in the sheet

      var searchId = params.id; // Get the encodedId passed as parameter
      var valueIndex = params.index;

      for (var i = 0; i < rows.length; i++) {
        if (rows[i][2] == searchId) { // Assuming 'id' is in the third column (index 2)
        result = rows[i][valueIndex]; // Assuming 'fear' is in the seventh column (index 6)
          break;
        }
    }
    return ContentService.createTextOutput(result);
}