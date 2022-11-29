$(() => { // studentcrud.js
    const getAll = async (msg) => {
        try {
            $("#studentList").text("Finding Student Information...");
            let response = await fetch(`api/student`);
            if (response.ok) {
                let payload = await response.json(); // this returns a promise, so we await it
                buildStudentList(payload);
                msg === "" ? // are we appending to an existing message
                    $("#status").text("Students Loaded") : $("#status").text(`${msg} - Students Loaded`);
            } else if (response.status !== 404) { // probably some other client side error
                let problemJson = await response.json();
                errorRtn(problemJson, response.status);
            } else { // else 404 not found
                $("#status").text("no such path on server");
            } // else
          
        } catch (error) {
            $("#status").text(error.message);
        }
    }; // getAll


    $("#srch").keyup(() => {
        let alldata = JSON.parse(sessionStorage.getItem("allstudents"));
        let filtereddata = alldata.filter((stu) => stu.lastname.match(new RegExp($("#srch").val(), 'i')));
        buildStudentList(filtereddata, false);
    }); 



    const setupForUpdate = (id, data) => {

        $("#actionbutton").val("Update Grade");
        $("#modaltitle").html("<h4>update Student</h4>");
        clearModalFields();
        $('#deletealert').hide();
        $('#deleteprompt').show();
        data.forEach(student => {
            if (student.id === parseInt(id)) {
                $("#TextBoxTitle").val(student.title);
                $("#TextBoxFirstname").val(student.firstname);
                $("#TextBoxLastname").val(student.lastname);
                $("#TextBoxPhone").val(student.phoneno);
                $("#TextBoxEmail").val(student.email);

                loadDivisionDDL(student.id);
                sessionStorage.setItem("studentID", student.id);

                sessionStorage.setItem("id", student.id);
                sessionStorage.setItem("divisionId", student.divisionId);
                sessionStorage.setItem("timer", student.timer);
                $("#myModal").modal("toggle");
            } // if
        }); // data.forEach
    }; // setupForUpdate


    const loadDivisionDDL = async (studentID) => {
        $("#MarkRow").hide();
        $("#TextBoxMark").hide();
        $("#CommentsRow").hide();
        $("#actionbutton").hide();
        

        console.log("clicked on " + studentID);

        html = '';
        $('#ddlCourses').empty();

        let response = await fetch(`api/Course/${studentID}`);
        if (response.ok) {
            let divs = await response.json(); // this returns a promise, so we await it
            sessionStorage.setItem("allCourses", JSON.stringify(divs));
        } else if (response.status !== 404) { // probably some other client side error
            let problemJson = await response.json();
            errorRtn(problemJson, response.status);
        }
        let alldivisions = JSON.parse(sessionStorage.getItem('allCourses'));
        alldivisions.forEach((div) => { html += `<option value="${div.id}">${div.name}</option>` });
        $('#ddlCourses').append(html);
        $('#ddlCourses').val(studentID);
    }; // loadDivisionDDL


    $("#ddlCourses").change(async (e) => { // click on ddl
        //coding
        $("#CommentsRow").show();
        $("#actionbutton").show();        
        $("#MarkRow").show();
       $("#TextBoxMark").show();


        let courseSelectedID = $("#ddlCourses").val();
        console.log("Selected Course ID as " + courseSelectedID)
      

        let response = await fetch(`api/Grade/${sessionStorage.getItem("studentID")}`);

        if (response.ok) {
            let divs = await response.json(); // this returns a promise, so we await it
            sessionStorage.setItem("myGrades", JSON.stringify(divs));
            //console.log("Got " + JSON.stringify(divs))

            let allCourses = JSON.parse(sessionStorage.getItem('myGrades'));
            sessionStorage.setItem('courseID', courseSelectedID)

            allCourses.forEach((course) => {

                if (courseSelectedID == course.courseId) {
                    console.log(JSON.stringify(course))
                    $("#TextBoxMark").val(course.mark)
                    $("#TextBoxComments").val(course.comments)
                    sessionStorage.setItem('courseTimer',course.timer)
                    sessionStorage.setItem('id_course', course.id)

                } 

            });
        } else if (response.status !== 404) { // probably some other client side error
            let problemJson = await response.json();
            errorRtn(problemJson, response.status);
        }
    });


    const setupForAdd = () => {
        $("#actionbutton").val("add");
        $("#modaltitle").html("<h4>add student</h4>");
        $("#theModal").modal("toggle");
        $("#modalstatus").text("add new student");
        $("#myModalLabel").text("Add");
        $('#deletealert').hide();
        $('#deleteprompt').hide();
        clearModalFields();
    }; // setupForAdd
    const clearModalFields = () => {
        loadDivisionDDL(-1);

        $("#MarkRow").hide();
        $("#CommentsRow").hide();
        $("#actionbutton").hide();


        $("#TextBoxTitle").val("");
        $("#TextBoxFirstname").val("");
        $("#TextBoxLastname").val("");
        $("#TextBoxPhone").val("");
        $("#TextBoxEmail").val("");
        sessionStorage.removeItem("id");
        sessionStorage.removeItem("divisionId");
        sessionStorage.removeItem("timer");
        $("#myModal").modal("toggle");
    }; // clearModalFields

    const add = async () => {
        try {
            stu = new Object();
            stu.title = $("#TextBoxTitle").val();
            stu.firstname = $("#TextBoxFirstname").val();
            stu.lastname = $("#TextBoxLastname").val();
            stu.phoneno = $("#TextBoxPhone").val();
            stu.email = $("#TextBoxEmail").val();
            //stu.divisionId = 10; // hard code it for now, we"ll add a dropdown later
            stu.divisionId = parseInt($("#ddlCourses").val());
            stu.id = -1;
            stu.timer = null;
            stu.picture64 = null;
            // send the student info to the server asynchronously using POST
            let response = await fetch("api/student", {
                method: "POST",
                headers: {
                    "Content-Type": "application/json; charset=utf-8"
                },
                body: JSON.stringify(stu)
            });
            if (response.ok) // or check for response.status 
            {
                let data = await response.json();
                getAll(data.msg);
            } else if (response.status !== 404) { // probably some other client side error
                let problemJson = await response.json();
                errorRtn(problemJson, response.status);
            } else { // else 404 not found
                $("#status").text("no such path on server");
            } // else
        } catch (error) {
            $("#status").text(error.message);
        } // try/catch
        $("#myModal").modal("toggle");
    }; // add

    const _delete = async () => {
        try {
            let response = await fetch(`api/student/${sessionStorage.getItem('id')}`, {
                method: 'DELETE',
                headers: { 'Content-Type': 'application/json; charset=utf-8' }
            });
            if (response.ok) // or check for response.status 
            {
                let data = await response.json();
                getAll(data.msg);
            } else {
                $('#status').text(`Status - ${response.status}, Problem on delete server side, see server console`);
            } // else
            $('#myModal').modal('toggle');
        } catch (error) {
            $('#status').text(error.message);
        }
    }; // delete

    const update = async () => {
        try {
            // set up a new client side instance of Student
            stu = new Object();
            // pouplate the properties
            stu.title = $("#TextBoxTitle").val();
            stu.firstname = $("#TextBoxFirstname").val();
            stu.lastname = $("#TextBoxLastname").val();
            stu.phoneno = $("#TextBoxPhone").val();
            stu.email = $("#TextBoxEmail").val();
            // we stored these 3 earlier
            stu.id = parseInt(sessionStorage.getItem("id"));
            stu.divisionId = parseInt($("#ddlCourses").val());
            stu.timer = sessionStorage.getItem("timer");
            stu.picture64 = null;
            // send the updated back to the server asynchronously using PUT 
            let response = await fetch("api/student", {
                method: "PUT",
                headers: { "Content-Type": "application/json; charset=utf-8" },
                body: JSON.stringify(stu)
            });
            if (response.ok) // or check for response.status 
            {
                let data = await response.json();
                getAll(data.msg);
            } else if (response.status !== 404) { // probably some other client side error
                let problemJson = await response.json();
                errorRtn(problemJson, response.status);
            } else { // else 404 not found
                $("#status").text("no such path on server");
            } // else
        } catch (error) {
            $("#status").text(error.message);
        } // try/catch
        $("#myModal").modal("toggle");
    } // update

    $("#actionbutton").click(async (e) => {
        //coding
         let studentID = sessionStorage.getItem("studentID")
        let courseID = sessionStorage.getItem("courseID")
        let mark = $("#TextBoxMark").val()
        let comment = $("#TextBoxComments").val()
        let timer = sessionStorage.getItem("courseTimer")
        let id = sessionStorage.getItem("id_course")


        
        let grade = new Object();
        grade.studentID = studentID
        grade.courseID = courseID
        grade.mark = mark
        grade.comments = comment
        grade.timer = timer
        grade.id = id
        


        console.log("Sending "+ JSON.stringify(grade))
        let response = await fetch("api/Grade", {
            method: "PUT",
            headers: { "Content-Type": "application/json; charset=utf-8" },
            body: JSON.stringify(grade)
        });

        try { 
        if (response.ok) // or check for response.status 
        {
            let data = await response.json();
            $("#status").text("Grade for student "+ studentID +" was updated - Students Loaded");

         } else if (response.status !== 404) { // probably some other client side error
            let problemJson = await response.json();
            errorRtn(problemJson, response.status);
        } else { // else 404 not found
            $("#status").text("no such path on server");
        } // else
         } catch (error) {
        $("#status").text(error.message);
         } // try/catch




        $("#myModal").modal("toggle");
        $("")

    });


    $("#studentList").click((e) => {
        if (!e) e = window.event;
        let id = e.target.parentNode.id;
        if (id === "studentList" || id === "") {
            id = e.target.id;
        } // clicked on row somewhere else 
        if (id !== "status" && id !== "heading") {
            let data = JSON.parse(sessionStorage.getItem("allstudents"));
            id === "0" ? setupForAdd() : setupForUpdate(id, data);
        } else {
            return false; // ignore if they clicked on heading or status
        }
    }); // studentList click

    getAll(""); // first grab the data from the server


    const buildStudentList = (data, usealldata = true) => {
        $("#studentList").empty();
        div = $(`<div class="list-group-item text-white bg-secondary row d-flex" id="status">
 Student Info</div>
 <div class= "list-group-item row d-flex text-center" id="heading">
 <div class="col-4 h4">Title</div>
 <div class="col-4 h4">First</div>
 <div class="col-4 h4">Last</div>
 </div>`);
        div.appendTo($("#studentList"));
        usealldata ? sessionStorage.setItem("allstudents", JSON.stringify(data)) : null;
  
        data.forEach(stu => {
            btn = $(`<button class="list-group-item row d-flex" id="${stu.id}">`);
            btn.html(`<div class="col-4" id="studenttitle${stu.id}">${stu.title}</div>
 <div class="col-4" id="studentfname${stu.id}">${stu.firstname}</div>
 <div class="col-4" id="studentlastnam${stu.id}">${stu.lastname}</div>`
            );
            btn.appendTo($("#studentList"));
        }); // forEach
    }; // buildStudentList





    $('#deleteprompt').click((e) => {
        $('#deletealert').show();
    });
    $('#deletenobutton').click((e) => {
        $('#deletealert').hide();
    });
    $('#deletebutton').click(() => {
        _delete();
    });



}); // jQuery ready method



// server was reached but server had a problem with the call
const errorRtn = (problemJson, status) => {
    if (status > 499) {
        $("#status").text("Problem server side, see debug console");
    } else {
        let keys = Object.keys(problemJson.errors)
        problem = {
            status: status,
            statusText: problemJson.errors[keys[0]][0], // first error
        };
        $("#status").text("Problem client side, see browser console");
        console.log(problem);
    } // else

}

