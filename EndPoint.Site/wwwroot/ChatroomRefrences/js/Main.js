var messagesBox = document.querySelector(".chatroom-main .col-content");

var chatroom = document.querySelector('.chatroom-main');
var chatHead = document.querySelector('.chatroom-main .col-head');
var chatList = document.querySelector('.main .col-left');
var BackButton = document.querySelector('.chatroom-main .col-head .back');
var btnSend = document.querySelector('.btn-send');
var btnCamera = document.querySelector('.btn-camera');
var ChtroomsList = document.querySelector('div.messages');
var ChatsBox = document.querySelector('.message-out .grid-message');
var ChatroomImage = document.querySelector('.col-head .prof');
var ChatroomName = document.querySelector('.col-head .name');
var btnAddNewChat = document.querySelector('.add-new-chat');
var ModalContent = document.querySelector('#main-modal-content');


btnSend.addEventListener('click', btnSend_Click);
btnCamera.addEventListener('click', btnCamera_Click);
btnAddNewChat.addEventListener('click', btnAddNewChat_Click);
ChatroomImage.addEventListener('click', OpenProfile);


var ChatroomGuid;
var targetUsername;

GetList().then(
    function (value) {
    }, function (err) {
    }
)

ShowModal();

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
        btnAddNewChat.style = 'display:none;';

        chatroom.style = 'display: block;overflow-y:scroll;';
        chatHead.style = 'position:sticky;top:0px;z-index:10;';
        BackButton.addEventListener('click', closeChat);

        BackButton.removeAttribute('hidden');
    }
    if (Guid != null && Guid != ChatroomGuid) {
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
    ClearChats();
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
    messagesBox.scrollTo(0, 100000010000001000);

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

        img.src = item.imageName;
        img.setAttribute('class', 'message-image');

        span.innerHTML = item.senderName;
        span.setAttribute('class', 'message-name');

        indiv.appendChild(img);
        indiv.appendChild(span);
        indiv.appendChild(hr);
    }

    inP.innerHTML = item.text;


    indiv.appendChild(inP);
    outdiv.appendChild(indiv);

    ChatsBox.appendChild(outdiv);
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
    ClearChats();
    ChatroomGuid = data.chatroomGuid;
    ChatroomImage.setAttribute('src', data.imageName);
    ChatroomName.innerHTML = data.chatroomName

    if (data.messages != null) {
        for (var i = 0; i < data.messages.length; i++) {

            var item = data.messages[i];

            AddMessagePV(item);
        }
    }
    messagesBox.scrollTo(0, 100000010000001000);

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
    } else if (!(ChatroomGuid == null || ChatroomGuid == "00000000-0000-0000-0000-000000000000")) {
        SendMessage();
    }

}

async function SendMessage() {

    var TextArea = document.querySelector('.txt-message');
    if (TextArea.value !== '' && !(ChatroomGuid == null || ChatroomGuid == "00000000-0000-0000-0000-000000000000")) {

        var sendMessage = TextArea.value;
        TextArea.value = '';

        //var message = new FormData();

        //message.append('Text', sendMessage);
        //message.append('Guid', String.toString(ChatroomGuid))

        var message = {
            'text': sendMessage,
            'guid': ChatroomGuid
        }
        //TODO
        $.ajax({
            contentType: 'application/x-www-form-urlencoded',
            type: "POST",
            url: "/Messages/AddMessage",
            data: message,
            success: function (data) {
                if (data.status == 0) {
                    debugger;
                    var item = {
                        "text": sendMessage,
                        "isMe": true
                    }
                    AddMessagePV(item);
                    messagesBox.scrollTo(0, 100000010000001000);
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
}

async function btnCamera_Click() {
    alert("Clicked");
}

async function ClearChats() {
    debugger;
    var AllMessages = document.querySelectorAll('.message-out .grid-message div.col-message-received,.message-out .grid-message div.col-message-sent');

    if (AllMessages != null) {
        for (var i = 0; i < AllMessages.length; i++) {
            var item = AllMessages[i];

            ChatsBox.removeChild(item);
        }
    }

}

async function btnAddNewChat_Click() {

    var btnGroup = document.createElement('button');
    btnGroup.classList.add("btn", "btn-primary");
    btnGroup.innerHTML = "گروه جدید +";
    btnGroup.type = "button";

    var btnChannel = document.createElement('button');
    btnChannel.classList.add("btn", "btn-primary");
    btnChannel.innerHTML = "کانال جدید +";
    btnChannel.type = "button";

    var btnContact = document.createElement('button');
    btnContact.classList.add("btn", "btn-primary");
    btnContact.innerHTML = "پیدا کردن با شماره تلفن";
    btnContact.type = "button";


    btnGroup.addEventListener('click', btnAddNewGroup_Clicked);
    btnChannel.addEventListener('click', btnAddNewChannel_Clicked);
    btnContact.addEventListener('click', btnAddNewContact_Clicked);

    ModalContent.innerHTML = '';

    ModalContent.style = 'text-align:center;';
    ModalContent.appendChild(btnGroup);
    ModalContent.appendChild(document.createElement('hr'));
    ModalContent.appendChild(btnChannel);
    ModalContent.appendChild(document.createElement('hr'));
    ModalContent.appendChild(btnContact);
    ModalContent.appendChild(document.createElement('hr'));


    ShowModal();
}

async function ShowModal() {
    $('#myModal').modal('show');
}

async function CloseModal() {
    ModalContent.innerHTML = '';
    $('#myModal').modal('hide');
}

async function btnAddNewGroup_Clicked() {
    ModalContent.innerHTML = '';


    var inputGroupName = document.createElement('input');
    inputGroupName.type = 'text';
    inputGroupName.classList.add('form-control', 'col-8', 'offset-2');
    inputGroupName.placeholder = 'نام گروه را وارد کنید';
    inputGroupName.style = 'text-align:right;';
    inputGroupName.id = 'AddNewGroupName';


    var inputGroupImage = document.createElement('input');
    inputGroupImage.type = 'file';
    inputGroupImage.classList.add('col-4', 'offset-4');
    inputGroupImage.placeholder = 'انتخاب عکس';
    inputGroupImage.style = 'text-align:right;';
    inputGroupImage.id = 'AddNewGroupImage';


    var GroupName = document.createElement('div');
    GroupName.classList.add('form-group');

    var GroupImage = document.createElement('div');
    GroupImage.classList.add('form-group');

    GroupName.appendChild(inputGroupName);
    GroupImage.appendChild(inputGroupImage);

    ModalContent.appendChild(GroupName);
    ModalContent.appendChild(GroupImage);
    ModalContent.appendChild(document.createElement('hr'));

    var btnSubmit = document.createElement('button');
    btnSubmit.classList.add('btn', 'btn-success', 'btn-lg');
    btnSubmit.innerHTML = 'ایجاد';
    btnSubmit.addEventListener('click', btnAddNewGroup_Submit);

    ModalContent.appendChild(btnSubmit);

}

async function btnAddNewGroup_Submit() {
    debugger;

    var GroupName = document.querySelector('#AddNewGroupName');
    var imageFile = $('#AddNewGroupImage').prop('files')[0];
    //var GroupImage = document.querySelector('#AddNewGroupImage');

    ModalContent.innerHTML = '<div class="text-black-50">Sending To Server...</div>';

    //var Data = {
    //    groupName: GroupName.value,
    //    imageFile: GroupImage.value
    //};

    debugger;
    var Data = new FormData();

    Data.append('groupName', GroupName.value);
    Data.append('imageFile', imageFile);

    $.ajax({
        url: '/CreateGroup', // point to server-side PHP script 
        dataType: 'json',  // what to expect back from the PHP script, if anything
        cache: false,
        contentType: false,
        processData: false,
        data: Data,
        type: 'POST',
        success: function (data) {
            ModalContent.innerHTML = '';
            if (data.status == 0) {
                ModalContent.innerHTML = '<div class="text-black-50">گروه شما ایجاد شد</div>';
            }
            else {
                ModalContent.innerHTML = '<div class="text-black-50">مشکلی پیش آمد</div>';
            }
        },
        error: function (request, status, error) {
            console.log('Failed To Get List!');
        }
    });

    //$.ajax({
    //    contentType: 'application/x-www-form-urlencoded',
    //    type: "POST",
    //    url: "/CreateGroup",
    //    data: Data,
    //    success: function (data) {
    //        ModalContent.innerHTML = '';
    //        if (data.status == 0) {
    //            ModalContent.innerHTML = '<div class="text-black-50">گروه شما ایجاد شد</div>';
    //        }
    //        else {
    //            ModalContent.innerHTML = '<div class="text-black-50">مشکلی پیش آمد</div>';
    //        }
    //    },
    //    error: function (request, status, error) {
    //        console.log('Failed To Get List!');
    //    }
    //});
}

async function btnAddNewChannel_Clicked() {
    ModalContent.innerHTML = '<div class="text-black-50">این قسمت بزودی تکمیل خواهد شد</div>';

}

async function btnAddNewContact_Clicked() {
    ModalContent.innerHTML = '<div class="text-black-50">این قسمت بزودی تکمیل خواهد شد</div>';
}

async function OpenProfile() {
    $.ajax({
        contentType: 'application/x-www-form-urlencoded',
        type: "GET",
        url: "/Chatrooms/GetProfile?ChatroomGuid=" + ChatroomGuid,
        success: function (data) {

            if (data.status == 0) {
                debugger;
                switch (data.type) {
                    case 0:
                        BuildAndShowPvProfile(data.data);
                        break;
                    case 1:
                        BuildAndShowGroupProfile(data.data);
                        break;
                    default:
                        console.log("مشکلی پیش آمد");
                        break;
                }
            }
            else {

            }
        },
        error: function (request, status, error) {
            console.log('Failed To Get List!');
        }
    });
}

async function BuildAndShowPvProfile(data) {

    ModalContent.innerHTML = '';

    var divhead = document.createElement('div');
    divhead.classList.add('profile-image-container');

    var img = document.createElement('img');
    img.src = '/' + data.imageName;
    img.classList.add('profile-image');

    var h2name = document.createElement('h2');
    h2name.classList.add('profile-name');
    h2name.innerHTML = data.name;

    divhead.appendChild(img);
    divhead.appendChild(h2name);


    var divbody = document.createElement('div');
    divbody.classList.add('profile-body');

    var h3username = document.createElement('h3');
    h3username.classList.add('profile-username');
    h3username.innerHTML = 'نام کاربری :' + data.username;

    var pbio = document.createElement('p');
    pbio.classList.add('profile-bio');
    pbio.innerHTML = data.bio;

    divbody.appendChild(h3username);
    divbody.appendChild(document.createElement('hr'));
    divbody.appendChild(pbio);


    ModalContent.appendChild(divhead);
    ModalContent.appendChild(divbody);

    ShowModal();
}

async function BuildAndShowGroupProfile(data) {
    //Todo : Create Profile For Group And Create Ling For Join To Group
    debugger;
}