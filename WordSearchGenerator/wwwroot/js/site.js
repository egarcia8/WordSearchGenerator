// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

$(document).ready(function () {
    var word = $('#userWords').val();
    var size = $('#gridSize').val();

    $('#createForm').validate({
        rules: {
            gridSize: {
                required: true,
                range:[5, 20]
            }
        },
        messages: {
            gridSize: {
                required: "Please enter a size.",
                range: "Please enter a number from 5-20."
            }
        }
    });

    window.addToWordList = function () {
        const isValid = $('#createForm').valid();

        if (isValid) {
            alert("Good");
        }
        else {
            alert("Not Good")
        }



        $('#wordList').append('<li>' + word + '</li>');
    }
});

