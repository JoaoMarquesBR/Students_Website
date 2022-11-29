// Exercise #3 - a re-do of exercise #2 plus allow click on a student


$(() => {

    const studentData = JSON.parse(`[{ "id": 123, "firstname": "Gail", "lastname": "Storm" }, 
 { "id": 234, "firstname": "Donny", "lastname": "Brook" },
 { "id": 345, "firstname": "Chris", "lastname": "Cross" }]`);

    let html = "";

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
    }); // loadButton.click()


        $("#studentList").click(e => {
            const myStudent = studentData.find(s => s.id === parseInt(e.target.id))

            let message;
            myStudent ? message = `you selected ${myStudent.firstname}` : `Something went wrong`

            $("#results").text(message)
        })


   // studentList div click






})

