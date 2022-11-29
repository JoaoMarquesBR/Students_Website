
const stringData = `[{ "id": 123, "firstname": "Gail", "lastname": "Storm" }, 
 { "id": 234, "firstname": "Donny", "lastname": "Brook" }, 
 { "id": 345, "firstname": "Chris", "lastname": "Cross" }]`


const loadData = (e) => { 
    let list = document.getElementById("studentList");
  
    const studentData = JSON.parse(stringData);

    let html = "";

    studentData.forEach((student) => {
        html += `<div class="list-group-item">
 ${student.id},${student.firstname},${student.lastname}
 </div>`;
    });

    list.insertAdjacentHTML("afterbegin", html);
    document.getElementById("loadButton").style.visibility = "hidden";
}; // loadData

//action listener to run loadData 
document.getElementById("loadButton").addEventListener("click", loadData);



