$(document).ready(function() {
    $("#toggle").click(function() {
        $(".sidebar").toggleClass("visible");
        $("body").toggleClass("opacity");
        $(".arrow").addClass("glyphicon glyphicon-chevron-down");
    });

    $(".collapse-btn").click(function() {
        $("#" + this.id + " > .arrow").toggleClass("glyphicon glyphicon-chevron-down");
        $("#" + this.id + " > .arrow").addClass("glyphicon glyphicon-chevron-up");
    });

    //$("#btnwhowearelink").click(function () {
    //    $("#btnevent1").toggleClass("display");
    //    $("#btnadminlink").toggleClass("display");
    //});
    //$("#btnevent1").click(function () {
    //    $("#btnadminlink").toggleClass("display");
    //});

    var num = 82; //number of pixels before modifying styles

    $(window).bind("scroll", function() {
        if ($(window).scrollTop() > num) {
            $("#nav-one").addClass("static");
            $("#nav-whoweare").addClass("nav-whoweare-fixed");
        } else {
            $("#nav-one").removeClass("static");
            $("#nav-whoweare").removeClass("nav-whoweare-fixed");
        }
    });

    //$("#carousel-example-generic > div.carousel-inner > div.item:first-of-type").addClass("active");

    $("ul.nav-tabs li a[href^='#']").click(function (e) {
        $("html, body").animate({ scrollTop: $(this.hash).offset().top }, 1000);
    });


    $('#calendar').fullCalendar({
        header: {
            left: 'prev,next today',
            center: 'title',
            //right: 'month,basicWeek,basicDay'
            right: 'month,basicWeek,basicDay'
        },
        defaultDate: new Date(),
        events: "/events/getevents",
        eventRender: function (event, element) {
            if (event.title != "") {
                if (event.start >= Date.now()) {
                    element.addClass("future");
                } else if (event.start == Date.now()) {
                    element.addClass("present");
                } else {
                    element.addClass("past");
                }
                element.mouseover(function () {
                    if (event.start >= Date.now()) {
                        element.css('color', '#333');
                    }else {
                        element.css('color', '#fff');
                    }
                    element.css('text-decoration', 'none');
                });
                element.mouseout(function () {
                    //element.css('color', 'currentColor');
                });
            }
        },
        eventMouseover: function (event, jsEvent, view) {
            $(this).popover({
                title: '<div style="font-size:16px; color:#cc0000;">' + event.title + '</div>',
                placement: 'top',
                trigger: 'manual',
                delay: { show: 200, hide: 100 },
                animation: true,
                container: '#calendar',
                html: true,
                content: '<div style="font-size:12px;"><p>' + event.StartMonth + ' ' + event.StartDay + ', ' + event.StartYear + ' @ ' + event.Venue + '</p><p>' + event.StartTime + ' ' + event.StartTimeMeridian + ' - ' + event.EndTime + ' ' + event.EndTimeMeridian + '</p><p>' + event.Description.substring(0, 250) + '...</p></div>'
                //content: '<div style="font-size:12px;"><p>' + event.start.toString("d") + ' @ ' + event.Venue + '</p><p>' + event.StartTime + ' ' + event.StartTimeMeridian + ' - ' + event.EndTime + ' ' + event.EndTimeMeridian + '</p><p>' + event.Description.substring(0, 250) + '...</p></div>'
            });

            $(this).popover('show');
        },

        eventMouseout: function (event, jsEvent, view) {
            $(this).popover("hide");
        }
    });

    $(".fancy-photo")
        .fancybox({
            padding : 0,
        cyclic: true,
        type: 'image',
        onUpdate: function () {
            $(".fancybox-nav span").css({
                visibility: "visible"
            });
            return;
        },
        afterClose: function () {
            $(".fancybox-nav span").css({
                visibility: "hidden"
            });
            $("#nav-one").removeClass("position");
            return;
        }
        });
    $(".fancy-photo").click(function () {
        $("#nav-one").addClass("position");
    });

    $("#Date").datepicker();
    $("#start").datepicker();
});

// check news display
function checknewsdisplay() {
    if ($('#Display').prop('checked')) {

        $.ajax({
            url: '/WhoWeAre/CheckNewsDisplay',
            type: "GET",
            dataType: "JSON",
            success: function (display) {
                if (display > 4 || display == 4) {
                    $("#displaylabel").text("Display has exceeded four");
                    $('#createNews').prop('disabled', true);
                }
            }
        });

    } else {
        $("#displaylabel").text("");
        $('#createNews').prop('disabled', false);
    }
};

// check article display
function checkarticledisplay() {
    if ($('#Display').prop('checked')) {

        $.ajax({
            url: '/WhoWeAre/CheckArticleDisplay',
            type: "GET",
            dataType: "JSON",
            success: function (display) {
                if (display > 3 || display == 3) {
                    $("#displaylabel").text("Display has exceeded four");
                    $('#createArticle').prop('disabled', true);
                }
            }
        });

    } else {
        $("#displaylabel").text("");
        $('#createArticle').prop('disabled', false);
    }
};


// LoadBatch
function LoadBatch() {
    var yearId = $("#YearId").val();
    $.ajax({
        url: "/WhoWeAre/FillBatch",
        type: "GET",
        dataType: "JSON",
        data: { id: yearId },
        success: function(batchs) {
            $("#BatchId").html(""); // clear before appending new list
            $.each(batchs, function(i, batch) {
                $("#BatchId").append(
                    $("<option></option>").val(batch.Id).html(batch.BatchName));
            });
        }
    });
}



// batch
function FillBatch() {
    var yearId = $("#year").val();
    $.ajax({
        url: "/WhoWeAre/Batch",
        type: "GET",
        dataType: "JSON",
        data: { year: yearId },
        success: function(batchs) {
            $("#batch").html(""); // clear before appending new list
            if (batchs == "") {
                $("#batch").append(
                    $("<option></option>").val("All").html("All"));
            } else {
                $("#batch").append(
                    $("<option></option>").val("All").html("All"));
                $.each(batchs, function(i, batch) {
                    $("#batch").append(
                        $("<option></option>").val(batch.BatchName).html(batch.BatchName));
                });
            }
        }
    });
}

// fiil album
function FillAlbum() {
    var subzoneId = $("#subzone").val();
    $.ajax({
        url: "/WhoWeAre/Album",
        type: "GET",
        dataType: "JSON",
        data: { subzone: subzoneId },
        success: function(albums) {
            $("#album").html(""); // clear before appending new list
            if (albums == "") {
                $("#album").append(
                    $("<option></option>").val("All").html("All"));
            } else {
                $("#album").append(
                    $("<option></option>").val("All").html("All"));
                $.each(albums, function(i, album) {
                    $("#album").append(
                        $("<option></option>").val(album.Name).html(album.Name));
                });
            }
        }
    });
}

// fill project
function FillProject() {
    var subzoneId = $("#subzone").val();
    $.ajax({
        url: "/WhoWeAre/ProjectName",
        type: "GET",
        dataType: "JSON",
        data: { subzone: subzoneId },
        success: function(projects) {
            $("#project").html(""); // clear before appending new list
            if (projects == "") {
                $("#project").append(
                    $("<option></option>").val("All").html("All"));
            } else {
                $("#project").append(
                    $("<option></option>").val("All").html("All"));
                $.each(projects, function(i, project) {
                    $("#project").append(
                        $("<option></option>").val(project.Name).html(project.Name));
                });
            }
        }
    });
}