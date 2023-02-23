
$(document).ready(function () {

    $('#rebuildGridButton').click(function () {
        const payload = {
            gridSize: window.gridObj.gridSize,
            wordList: window.gridObj.wordList
        }
        $.ajax({
            type: "POST",
            url: '/api/grid',            
            contentType: "application/json; charset=utf-8",
            data: JSON.stringify(payload),  
            dataType: "json",
            success: function(result) {
                createGrid(result.masterCoordinates);
                createWordFitList(result.wordFit);
                createWordNoFitList(result.wordNoFit);
            }
        });  
    })

    $('#goBackButton').click(function () {
        window.location.href = '/Home';
    });

    function createGrid(gridCoordinates) {
        $('#grid').empty();
        gridCoordinates.forEach(function (row) {
            const tr = $('<tr>');
            row.forEach(function (col) {
                $(tr).append('<td>' + col+ '</td>');  
            })
            $('#grid').append(tr)
        });
    }

    function createWordFitList(wordFitList) {
        $('#words').empty();
        wordFitList.forEach(function (word) {
            $('#words').append('<li>' + word + '</li>')
        });
    }

    function createWordNoFitList(wordNoFitList) {
        $('#wordNoFit').empty();
        wordNoFitList.forEach(function (word) {
            $('#wordNoFit').append('<li>' + word + '</li>')
        });
    }

    createGrid(window.gridObj.masterCoordinates);

    createWordFitList(window.gridObj.wordFit);
    createWordNoFitList(window.gridObj.wordNoFit);
})