
$(document).ready(function () {

    $('#rebuildGridButton').click(function () {
        const payload = {
            gridSize: window.gridObj.gridSize,
            wordList: window.gridObj.wordList
        }
        $.ajax({
            type: 'POST',
            url: '/GetPartial',            
            contentType: 'application/json; charset=utf-8',
            data: JSON.stringify(payload),  
            success: function(result) {
                $('#partial')
            }
        });  
    })

    $('#goBackButton').click(function () {
        window.location.href = '/Home';
    });

});