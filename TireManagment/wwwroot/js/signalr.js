

var connection = new signalR.HubConnectionBuilder().withUrl("/Tire/qustionHub").build();

connection.start().then(() => { alert("connected")
}
);





//  alert(m.CustomerQuestion);
//var div = document.createElement('div');
//div.id = "content";
//div.textContent = m.CustomerQuestion;
//var main = document.getElementById("main");
//main.prepend(div);
/*alltires, newtires, runningtires, damagedtires, retreadtires,*/
connection.on("ReciveNewTransaction", function ( message) {
  //  alert('ok');
   

   
    applyNotificationSound();
    var count = parseInt(document.getElementById("count").innerHTML);
    var header = document.getElementById('head');
    document.getElementById("count").innerHTML = count + 1;
    header.innerHTML = count + 1;
   // header.innerHTML = count + 1;
            console.log(new Date(message.movmentdate));
    var d= new Date(message.movmentdate).toLocaleString();
    var notificationpanel = document.getElementById('notificationspanel');
    var notificationsDetail = "/TireMovement/GetMovementDetail/" + message.Id;

    var notificationItem ='<hr class="dropdown-divider">'+
        '<a href=/TireMovement/GetMovementDetail/' + message.id +'><li class="notification-item">' +
        '<i class="bi bi-info-circle text-primary" ></i >' +
        ' <div>'+
        '<h4>' + message.operation + '</h4>' +
        '<p>' + message.trucknumber + '</p>' +
        '<p>' +d+'</p>' +
        '</div>'+
        '</li ></a> <li>'+
        '<hr class="dropdown-divider" >'+
        '</li >';
    notificationpanel.insertAdjacentHTML('afterbegin', notificationItem);
    console.log(message);
    var alltires = document.getElementById("alltires");
    var newandrunning = document.getElementById("newandrunning");
    var newonly = document.getElementById("new");
    var runningonly = document.getElementById("running");
    var retreadanddamaged = document.getElementById("retreadanddamaged");
    var retreadonly = document.getElementById("retread");
    var damagedonly = document.getElementById("damaged");

    alltires.innerHTML = message.alltires;
    newandrunning.innerHTML = message.newtires + message.runningtires;
    retreadanddamaged.innerHTML = message.damagedtires + message.retreadtires;
    newonly.innerHTML = message.newtires;
    runningonly.innerHTML = message.runningtires;
    retreadonly.innerHTML = message.retreadtires;
    //alert(message.damagedtires);
    damagedonly.innerHTML = message.damagedtires;
  
  ////table insertion/
    var table = document.getElementById("movetable");
    var row = table.insertRow(1);
    row.style.backgroundColor = "#FFFF00";
    //this adds row in 0 index i.e. first place
   
    row.innerHTML = "<th scope='row'>" + message.id + "</th><td>" + message.tireman + "</td><td>" + d + "</td><td>" + message.trucknumber + "</td><td><span class='badge bg-success'>"+message.operation+"</span></td>";
  
    

   
});
function applyNotificationSound() {

    var options = {
        autoClose: false,
        progressBar: true,
        enableSounds: true,

        sounds: {

            info: "https://res.cloudinary.com/dxfq3iotg/video/upload/v1557233294/info.mp3",
            // path to sound for successfull message:
            success: "https://res.cloudinary.com/dxfq3iotg/video/upload/v1557233524/success.mp3",
            // path to sound for warn message:
            warning: "https://res.cloudinary.com/dxfq3iotg/video/upload/v1557233563/warning.mp3",
            // path to sound for error message:
            error: "https://res.cloudinary.com/dxfq3iotg/video/upload/v1557233574/error.mp3",
        },
    };

    var toast = new Toasty(options);
    toast.configure(options);


    toast.success("New  Operation");
}