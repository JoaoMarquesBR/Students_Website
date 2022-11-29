// Exercise #5 - a re-do of exercise #4 plus fetch JSON from GitHub and
// allow the user to add an inputted user


$(() => {

    const stringData =
        `[{ "id": 123, "firstname": "Gail", "lastname": "Storm" }, 
 { "id": 234, "firstname": "Donny", "lastname": "Brook" }, 
 { "id": 345, "firstname": "Chris", "lastname": "Cross" }]`;


    sessionStorage.getItem("studentData") === null
        ? sessionStorage.setItem("studentData", stringData)
        : null;


    let studentData = JSON.parse(sessionStorage.getItem("studentData"));
    // the event handler for a button with id attribute of loadButton
    $("#loadButton").click(async () => {
        if (sessionStorage.getItem("studentData") === null) { // if not loaded get data from GitHub
            // location of data
            const url = "https://raw.githubusercontent.com/elauersen/info3070/master/ex5.json";
            $('#results').text('Locating student data on GitHub, please wait..');
            try {
                let response = await fetch(url);
                if (!response.ok) // check response
                    throw new Error(`Status - ${response.status}, Text - ${response.statusText}`); // fires catch
                studentData = await response.json(); // this returns a promise, so we await it
                sessionStorage.setItem("studentData", JSON.stringify(studentData));
                $('#results').text('Student data on GitHub loaded!');
            } catch (error) {
                $("#results").text(error.message);
            }
        } else {
            // get the session data to an object format
            studentData = JSON.parse(sessionStorage.getItem("studentData"));
            $('#results').text('Student data from session storage loaded!');
        }
        let html = "";
   
        html += `<h5 class="text-info">Select a Student</h5>`;
        studentData.forEach((student) => {
            html += `<div class="list-group-item" id="${student.id}" >
 ${student.id},${student.firstname},${student.lastname}
 </div>`;
        });
        $("#studentList").html(html);
        $("#loadButton").hide();
        $("#addButton").show();
        $("#inputFields").show();
    }); // loadButton.click()


    $("#studentList").click(e => {
        const myStudent = studentData.find(s => s.id === parseInt(e.target.id))
        console.log('Clicked ' + myStudent.firstname)

        let message;
        myStudent ? message = `you selected ${myStudent.firstname}` : `Something went wrong`

        $("#results").text(message)
    })

    $("#addButton").click(() => {
        // data from textboxes
        const first = $("#txtFirstname").val();
        const last = $("#txtLastname").val();
        if (first.length > 0 && last.length > 0) { // only add if we have something
            if (studentData.length > 0) {
                const student = studentData[studentData.length - 1];
                studentData.push({ "id": student.id + 101, "firstname": first, "lastname": last });
                $("#results").text(`added student ${student.id + 101}`);
            } else {
                // if only student
                studentData.push({ "id": 101, "firstname": first, "lastname": last });
            }
            // clear out the textboxes
            $("#txtLastname").val("");
            $("#txtFirstname").val(""); 
            // convert the object array back to a string and put it in session storage
            sessionStorage.setItem("studentData", JSON.stringify(studentData));
            let html = "";
            studentData.forEach(student => {
                html += `<div class="list-group-item" id="${student.id}" >
 ${student.id},${student.firstname},${student.lastname}
 </div>`;
            });
            $("#studentList").html(html);
        }
    }); // addButton click

   

})

