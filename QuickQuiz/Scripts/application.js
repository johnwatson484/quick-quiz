$(document).on('change', ':file', function () {
    var input = $(this),
        numFiles = input.get(0).files ? input.get(0).files.length : 1,
        label = input.val().replace(/\\/g, '/').replace(/.*\//, '');
    input.trigger('fileselect', [numFiles, label]);
});

$(document).ready(function () {
    $(':file').on('fileselect', function (event, numFiles, label) {

        var input = $(this).parents('.input-group').find(':text'),
            log = numFiles > 1 ? numFiles + ' files selected' : label;

        if (input.length) {
            input.val(log);
        } else {
            if (log) alert(log);
        }

    });
});

$(document).ready(function () {
    $(':radio').change(function () {
        if ($(this).val() !== null) {
            $('#btn-submit').attr('disabled', false);
        }
    });

    $('#btn-submit').click(function () {
        $(':radio').attr('disabled', true);
        $('#btn-submit').hide();
        var questionId = $('input[name="selected-answer"]:checked').data('question');
        var answerId = $('input[name="selected-answer"]:checked').data('answer');         
        $.ajax({
            type: "POST",
            url: "Submit",
            data: { questionId: questionId, answerId: answerId },
            success: function (data) {
                if (data.redirect) {
                    location.href = 'Expired';
                }
                else {
                    $('#correct-answer-' + data.correctAnswerId).show();
                    if (data.correctAnswerId !== answerId) {
                        $('#incorrect-answer-' + answerId).show();
                    }
                    $('#btn-next').show();
                }
            }
        });
    });
});