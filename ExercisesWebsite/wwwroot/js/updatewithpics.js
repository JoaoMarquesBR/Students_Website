$(() => { // update with pics
    $("#getbutton").click(async (e) => { // get button click event handler
        try {
            let lastname = $("#TextBoxFindLastname").val();
            $("#status").text("please wait...");
            $("#myModal").modal("toggle"); // pop the modal
            let response = await fetch(`api/student/${lastname}`);
            if (response.ok) {
                let data = await response.json(); // this returns a promise, so we await it
                if (data.lastname !== "not found") {
                    $("#TextBoxEmail").val(data.email);
                    $("#TextBoxTitle").val(data.title);
                    $("#TextBoxFirstname").val(data.firstname);
                    $("#TextBoxLastname").val(data.lastname);
                    $("#TextBoxPhone").val(data.phoneno);
                    $("#status").text("student found");
                    $("#ImageHolder").html(`<img height="120" width="110"
src="data:img/png;base64,${data.picture64}" />`);
                    // return these non-mutated values later
                    sessionStorage.setItem("id", data.id);
                    sessionStorage.setItem("divisionId", data.divisionId);
                    sessionStorage.setItem("timer", data.timer);
                    sessionStorage.setItem("picture", data.picture64);
                } else {
                    $("#TextBoxFirstname").val("not found");
                    $("#TextBoxLastname").val("");
                    $("#TextBoxEmail").val("");
                    $("#TextBoxTitle").val("");
                    $("#TextBoxPhone").val("");
                    $("#status").text("no such student");
                } // else
            } else if (response.status !== 404) { // probably some other client side error
                let problemJson = await response.json();
                errorRtn(problemJson, response.status);
            } else { // else 404 not found
                $("#status").text("no such path on server");
            } // else
        } catch (error) { // catastrophic
            $("#status").text(error.message);
        } // try/catch
    }); // get button click
    $("#updatebutton").click(async (e) => { // update button click event handler
        try {
            // set up a new client side instance of Student
            stu = new Object();
            // pouplate the properties
            stu.title = $("#TextBoxTitle").val();
            stu.firstname = $("#TextBoxFirstname").val();
            stu.lastname = $("#TextBoxLastname").val();
            stu.phoneno = $("#TextBoxPhone").val();
            stu.email = $("#TextBoxEmail").val();
            stu.divisionName = "";
            stu.picture64 = "";
            // stored earlier, numbers needed for Ids or http 401
            stu.id = parseInt(sessionStorage.getItem("id"));
            stu.divisionId = parseInt(sessionStorage.getItem("divisionId"));
            stu.timer = sessionStorage.getItem("timer");
            sessionStorage.getItem("picture")
                ? stu.picture64 = sessionStorage.getItem("picture")
                : stu.picture64 = null;
            // send the updated back to the server asynchronously using PUT
            let response = await fetch("api/student", {
                method: "PUT",
                headers: { "Content-Type": "application/json; charset=utf-8" },
                body: JSON.stringify(stu)
            });
            if (response.ok) // or check for response.status
            {
                let payload = await response.json();
                console.log("payload is " + JSON.stringify(payload))
                $("#status").text(payload.msg);
            } else if (response.status !== 404) { // probably some other client side error
                let problemJson = await response.json();
                errorRtn(problemJson, response.status);
            } else { // else 404 not found
                $("#status").text("no such path on server");
            } // else
        } catch (error) {
            $("#status").text(error.message);
            console.table(error);
        } // try/catch
        $("#myModal").modal("toggle");
    }); // update button click
    // do we have a picture?
    $("input:file").change(() => {
        const reader = new FileReader();
        const file = $("#uploader")[0].files[0];
        file ? reader.readAsBinaryString(file) : null;
        reader.onload = (readerEvt) => {
            // get binary data then convert to encoded string
            const binaryString = reader.result;
            const encodedString = btoa(binaryString);
            sessionStorage.setItem('picture', encodedString);
        };
    }); // input file change
}); // jQuery method
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