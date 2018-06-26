$(function () {

    $('.datepicker').datepicker({
        //changeMonth: true,
        //changeYear: true,
        //changeDay: true,
        showButtonPanel: true,
        dateFormat: "dd/mm/yy",
        showOn: "both",
        buttonImage: "http://jqueryui.com/resources/demos/datepicker/images/calendar.gif",
        buttonImageOnly: true,
        onSelect: function (dateText) {

            var url = "/DailyRegister/LoadAttendance";

            dateText = $.datepicker.formatDate("yy-mm-dd", $(this).datepicker("getDate"));

            $.getJSON(url, { attendanceDate: dateText }, function (data) {

                //iteration through Json data
                $.each(data, function (i, details) {

                  
                    $("#LearnersDropDownList").val(details.Learner);

                    $("#Hour").val(details.Hours);

                    $("#MentorsDropDownList").val(details.Mentor);

                    $("#TrainingOn").val(details._TrainingOn);


                });

              

            });

        }


    }).datepicker("setDate", "0"); //set date to default


    //
    $.validator.methods.date = function (value, element) {

        return this.option(element) || moment(value, "DD.MM.YYYY", true).isValid();
    }


    $("#Hour").bind("input", function () {

        var Hours = $("#Hour").val();

         $(this).val = $(this).replaceWith(/[^0-9]/g, '');

        if (Hours < 1 || Hours > 24) {

            $(this).val($(this).val().substring(0, $(this).val().length - 1))

            $("#ErrorMessage").html("Must enter a time between 0-24 hours"); 


        }

        



    });


});