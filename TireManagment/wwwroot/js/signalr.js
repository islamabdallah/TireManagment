

var connection = new signalR.HubConnectionBuilder().withUrl("/questionHub").build();

connection.start().then(() => { //alert("connected")
}
);





//  alert(m.CustomerQuestion);
//var div = document.createElement('div');
//div.id = "content";
//div.textContent = m.CustomerQuestion;
//var main = document.getElementById("main");
//main.prepend(div);
connection.on("ReciveNewTransaction", function (message) {
  //  alert('ok');
    console.log(message);
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
        '<a href=/TireMovement/GetMovementDetail/' + message.id + '><li class="notification-item">' +
        '<i class="bi bi-info-circle text-primary" ></i >' +
        ' <div>' +
        '<h4>' + message.operation + '</h4>' +
        ' <p>' + message.trucknumber + '</p>' +
        '<p>' + d+ '</p>' +
        '</div>'+
        +'</li ></a> <li>'+
        '<hr class="dropdown-divider" >'+
                            '</li >';
    
    notificationpanel.insertAdjacentHTML('afterbegin', notificationItem);
  
    
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


    toast.success("New Question Posted ");
   
   

   


    //alert(notificationsCount);
  //  document.getElementById("notificationCount").innerHTML = notificationsCount;
    //document.getElementById('counter').innerHTML = notificationsCount;
    //alert(notificationsCount);
   
});