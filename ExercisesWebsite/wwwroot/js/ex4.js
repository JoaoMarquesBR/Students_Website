// Exercise #4 - a re-do of exercise #3 plus introduce session storage and
// allow the user to add a generated user


$(() => {

    const stringData =
        `[{ "id": 123, "firstname": "Gail", "lastname": "Storm" }, 
 { "id": 234, "firstname": "Donny", "lastname": "Brook" }, 
 { "id": 345, "firstname": "Chris", "lastname": "Cross" }]`;


    sessionStorage.getItem("studentData") === null
        ? sessionStorage.setItem("studentData", stringData)
        : null;


    let studentData = JSON.parse(sessionStorage.getItem("studentData"));
    $("#loadButton").click(() => {
        let html = "";
        html += `<h5 class="text-info">Select a Student</h5>`;
        studentData.forEach((student) => {
            html += `<div class="list-group-item" id="${student.id}">
 ${student.id},${student.firstname},${student.lastname}
 </div>`;
        });
        $("#studentList").html(html);
        $("#loadButton").hide();
        $("#addButton").show();

    }); // loadButton.click


    $("#studentList").click(e => {
        const myStudent = studentData.find(s => s.id === parseInt(e.target.id))

        let message;
        myStudent ? message = `you selected ${myStudent.firstname}` : `Something went wrong`

        $("#results").text(message)
    })

    $("#addButton").click(() => {
        // find the last student
        const student = studentData[studentData.length - 1];
        // add 101 to the id
        $("#results").text(`added student ${student.id + 101}`);
        // add a new student to the object array
        studentData.push({ "id": student.id + 101, "firstname": "New", "lastname": "Student" });
        // convert the object array back to a string and put it in session storage
        sessionStorage.setItem("studentData", JSON.stringify(studentData));
        let html = "";
        studentData.forEach(student => {
            html += `<div class="list-group-item" id="${student.id}" >
 ${student.id},${student.firstname},${student.lastname}
 </div>`;
        });
        $("#studentList").html(html);
    }); // addButton clicc

   

})

