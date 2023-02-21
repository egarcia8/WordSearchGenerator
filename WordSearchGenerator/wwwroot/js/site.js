// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

$(document).ready(function () {
    
    jQuery.validator.addMethod('lettersonly', function (value, element) {
        return this.optional(element) || /^[a-z]+$/i.test(value);
    }, "Letters only and no spaces please");

    $('#createWordSearchForm').validate({
        rules: {
            gridSize: {
                required: true,
                range:[5, 20]
            },
            userWords: {
                required: true,
                lettersonly: true

            }
        },
        messages: {
            gridSize: {
                required: "Please enter a size.",
                range: "Please enter a number from 5-20."
            }
        }
    });

    window.addToWordList = function() {
        const isValid = $('#createWordSearchForm').valid();
        const word = $('#userWords').val();
        const size = $('#gridSize').val();
        if (isValid) {
            
            const wordLength = $('#userWords').val().length;
            if (wordLength < size) {
                $('#wordList').append('<li>' + word + '</li>');
                $('#userWords').val("");
                $('#gridSize').prop('disabled', true);
                $('#submitFormButton').prop('disabled', false);
                $('#wordInput p').empty();
            }
            else {
                const message = "That word is too long."
                $('<p>' + message + '</p>').appendTo('#wordInput');
            }
        }
    }

   
});

