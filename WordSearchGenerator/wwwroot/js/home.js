
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
            },
            userWords: {
                required:"Please enter a word."
            }
        }
    });

    $('#addToWordListButton').click(function (e) {
        e.preventDefault();
        const isValid = $('#createWordSearchForm').valid();
        const word = $('#userWords').val();
        const size = $('#gridSize').val();
        const myForm = $('#createWordSearchForm');
        if (isValid) {

            const wordLength = $('#userWords').val().length;
            if (wordLength <= size) {
                $('#wordList').append('<li><span id="wordSpan">' + word + '</span><button type="button" class="btn btn-outline-danger btn-sm mb-2 mt-2" id="deleteWordButton">X</button></li>');
                $('#userWords').val("");
                $('#gridSize').prop('disabled', true);
                $('#submitFormButton').prop('disabled', false);
                $('#wordInput p').empty();
                clearValidation(myForm);
            }
            else {
                const message = "That word is too long."
                $('<p>' + message + '</p>').appendTo('#wordInput');
            }
        }
        
    });

    function clearValidation(formElement){
        //Internal $.validator is exposed through $(form).validate()
        var validator = $(formElement).validate();
        //Iterate through named elements inside of the form, and mark them as error free
        $('[name]', formElement).each(function () {
            validator.successList.push(this);//mark as error free
            validator.showErrors();//remove error messages if present
        });
        validator.resetForm();//remove error class on name elements and clear history
        validator.reset();//remove all error and success data
    }

    $(document).on('click', "#deleteWordButton", function (e) {
        var entry = $(this).parent();
        entry.remove();  //remove entry from list

        var count = $('#wordList').children().length;
        if (count < 1) {
            $('#submitFormButton').prop('disabled', true);
        }

    });

    
    $('#submitFormButton').click(function () {

        const tempWordList = [];
        const tempGridSize = $('#gridSize').val();
        $('#wordList li span').each(function () {
            tempWordList.push($(this).text());
        });

        const payload = {
            gridSize: tempGridSize,
            wordList: tempWordList
        }
        
        $.ajax({
            type: "POST",
            url: "/api/userinput",            
            data: payload, 
            success: function () {
                window.location.href = "/grid";
            }
        });        
    });

    $('#resetButton').click(function () {
        $('#gridSize').prop('disabled', false);
        $('#submitFormButton').prop('disabled', true);
        $('#wordList').empty();
        $('#wordInput p').empty();
        $('#createWordSearchForm label.error ').empty();
    });

});

