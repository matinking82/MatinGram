var messagesBox = document.querySelector(".chatroom-main .col-content");
messagesBox.scrollTo(0, 100000010000001000);

var chatroom = document.querySelector('.chatroom-main');
var chatHead = document.querySelector('.chatroom-main .col-head');
var chatList = document.querySelector('.main .col-left');
var BackButton = document.querySelector('.chatroom-main .col-head .back');
var btnSend = document.querySelector('.btn-send');
var btnCamera = document.querySelector('.btn-camera');
var ChtroomsList = document.querySelector('div.messages');
var ChatsBox = document.querySelector('.message .grid-message');
var ChatroomImage = document.querySelector('.col-head .prof');
var ChatroomName = document.querySelector('.col-head .name');

btnSend.addEventListener('click', btnSend_Click);
btnCamera.addEventListener('click', btnCamera_Click);

var ChatroomGuid;
var targetUsername;

GetList().then(
    function (value) {
    }, function (err) {
    }
)

async function closeChat() {
    chatList.style = '';
    chatroom.style = '';
    chatHead.style = '';
    BackButton.setAttribute('hidden', '');
    ChatroomGuid = null;
}

async function GetList() {
    $.ajax({
        contentType: 'application/x-www-form-urlencoded',
        type: "GET",
        url: "/Chatrooms/GetChatroomsList",
        success: function (data) {
            if (data.status == 0) {
                BuildChatroomList(data.data);
            }
            else {

                console.log('Failed To Get List!');
            }
        },
        error: function (request, status, error) {
            console.log('Failed To Get List!');
        }
    });
}

async function BuildChatroomList(data) {
    for (var i = 0; i < data.length; i++) {
        var item = data[i];

        AddChatroom(item);
    }
}

async function AddChatroom(item) {

    var li = document.createElement('li');
    var outdiv = document.createElement('div');
    var indiv = document.createElement('div');
    var h3 = document.createElement('h3');
    var p = document.createElement('p');
    var img = document.createElement('img');


    li.setAttribute("Guid", item.guid);
    outdiv.setAttribute("class", "avatar");
    indiv.setAttribute("class", "avatar-image");
    img.setAttribute("src", item.imageName);
    h3.innerHTML = item.chatroomName;
    p.innerHTML = item.lastMessage


    indiv.appendChild(img);
    outdiv.appendChild(indiv);
    li.appendChild(outdiv);
    li.appendChild(h3);
    li.appendChild(p);

    li.addEventListener('click', function () {
        var Guid = li.getAttribute('guid');
        openChat(Guid);
    });


    ChtroomsList.appendChild(li);
}

async function openChat(Guid) {

    if (window.window.innerWidth <= 768) {

        chatList.style = 'display:none;';

        chatroom.style = 'display: block;overflow-y:scroll;';
        chatHead.style = 'position:sticky;top:0px;z-index:10;';
        BackButton.addEventListener('click', closeChat);

        BackButton.removeAttribute('hidden');
    }
    if (Guid != null) {
        loadChatroom(Guid);
    }

}

async function loadChatroom(Guid) {

    $.ajax({
        contentType: 'application/x-www-form-urlencoded',
        type: "GET",
        url: "/OpenChat/" + Guid,
        success: function (data) {
            if (data.status == 0) {
                BuildChat(data.data);
            }
            else {

                console.log('Failed To Get List!');
            }
        },
        error: function (request, status, error) {
            console.log('Failed To Get List!');
        }
    });

}

async function BuildChat(data) {

    ChatroomGuid = data.chatroomGuid;
    ChatroomImage.setAttribute('src', data.imageName);
    ChatroomName.innerHTML = data.chatroomName

    for (var i = 0; i < data.messages.length; i++) {
        var item = data.messages[i];

        switch (data.type) {
            case 0:
                AddMessagePV(item);
                break;

            case 1:
                AddMessageChat(item);
                break;

            default:
                console.log("failed");
                break;
        }
    }
}

async function AddMessageChat(item) {

    var outdiv = document.createElement('div');
    var indiv = document.createElement('div');
    var inP = document.createElement('p');
    var img = document.createElement('img');
    var span = document.createElement('span');
    var hr = document.createElement('hr');

    if (item.isMe) {
        outdiv.setAttribute('class', 'col-message-sent');
        indiv.setAttribute('class', 'message-sent');
    } else {
        outdiv.setAttribute('class', 'col-message-received');
        indiv.setAttribute('class', 'message-received');
    }

    img.src = item.imageName;
    img.setAttribute('class', 'message-image');

    span.innerHTML = item.senderName;
    span.setAttribute('class', 'message-name');

    inP.innerHTML = item.text;

    indiv.appendChild(img);
    indiv.appendChild(span);
    indiv.appendChild(hr);
    indiv.appendChild(inP);
    outdiv.appendChild(indiv);

    ChatsBox.appendChild(CreateChatMessage(outdiv));
}

async function LoadPV(username) {
    $.ajax({
        contentType: 'application/x-www-form-urlencoded',
        type: "GET",
        url: "/OpenPV/" + username,
        success: function (data) {
            if (data.status == 0) {
                BuildPV(data.data);
                targetUsername = username;
            }
            else {

                console.log('Failed To Get List!');
            }
        },
        error: function (request, status, error) {
            console.log('Failed To Get List!');
        }
    });
}

async function BuildPV(data) {

    ChatroomGuid = data.chatroomGuid;
    ChatroomImage.setAttribute('src', data.imageName);
    ChatroomName.innerHTML = data.chatroomName

    if (data.messages != null) {
        for (var i = 0; i < data.messages.length; i++) {

            var item = data.messages[i];

            AddMessagePV(item);
        }
    }

}

async function AddMessagePV(item) {

    var outdiv = document.createElement('div');
    var indiv = document.createElement('div');
    var inP = document.createElement('p');

    if (item.isMe) {
        outdiv.setAttribute('class', 'col-message-sent');
        indiv.setAttribute('class', 'message-sent');
    } else {
        outdiv.setAttribute('class', 'col-message-received');
        indiv.setAttribute('class', 'message-received');
    }

    inP.innerHTML = item.text;


    indiv.appendChild(inP);
    outdiv.appendChild(indiv);

    ChatsBox.appendChild(outdiv);
}

async function btnSend_Click() {


    if ((ChatroomGuid == null || ChatroomGuid == "00000000-0000-0000-0000-000000000000") && targetUsername != null) {
        //Create Chatroom
        //Get Guid
        $.ajax({
            contentType: 'application/x-www-form-urlencoded',
            type: "POST",
            url: "/CreatePV/" + targetUsername,
            success: function (data) {
                ChatroomGuid = data.data;

                SendMessage()
            },
            error: function (request, status, error) {
                console.log('Failed To Get List!');
            }
        });
    } else {
        SendMessage();
    }

}

async function SendMessage() {
    alert(ChatroomGuid);

    var TextArea = document.querySelector('.txt-message');
    if (TextArea.innerHTML !== '') {

        var message = TextArea.innerHTML;
        TextArea.innerHTML = '';

        alert(message);
    }
}

async function btnCamera_Click() {
    alert("Clicked");
}