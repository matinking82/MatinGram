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

async function AddChatroom(item, first = false) {

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

    if (!first) {
        ChtroomsList.appendChild(li);
    } else {
        ChtroomsList.appendChild(li);
    }
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

async function LoadChatroomByJoinGuid(guid) {
    //GetChatroomGuidByJoinGuid
    var Data = {
        joinLinkGuid: guid
    };
    $.ajax({
        contentType: 'application/x-www-form-urlencoded',
        type: "POST",
        url: "/Chatrooms/GetChatroomGuidByJoinGuid",
        data: Data,
        success: function (data) {
            if (data.status == 0) {
                loadChatroom(data.data);
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

async function loadChatroom(Guid) {
    $.ajax({
        contentType: 'application/x-www-form-urlencoded',
        type: "GET",
        url: "/OpenChat/" + Guid,
        success: function (data) {
            debugger;
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
    debugger;
    for (var i = 0; i < data.messages.length; i++) {
        var item = data.messages[i];
        if (item.messageType == 1) {
            AddInfoMessage(item);
        } else {
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
    messagesBox.scrollTo(0, 100000010000001000);

}

async function AddInfoMessage(item) {
    var outdiv = document.createElement('div');
    var span = document.createElement('span');


    outdiv.classList.add('col-message-info');
    span.classList.add('badge');
    span.innerHTML = item.text;

    outdiv.appendChild(span);

    ChatsBox.appendChild(outdiv);
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
    /////////////
    inP.innerHTML = MakeMessagesText(item.text);
    /////////////


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
                alert('کاربری یافت نشد');
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

            if (item.messageType == 1) {
                AddInfoMessage(item);
            } else {
                AddMessagePV(item);
            }
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
    //////////////
    inP.innerHTML = MakeMessagesText(item.text);
    /////////////

    indiv.appendChild(inP);
    outdiv.appendChild(indiv);

    ChatsBox.appendChild(outdiv);
}

function MakeMessagesText(Text) {

    Text = SetUserNames(Text);
    Text = SetJoinLinks(Text);

    return Text;
}

function SetUserNames(Text) {
    var rx = new RegExp('\\s{0,1}\@{1}(\\w{1}[\\d \\w]+)\\b');


    while (Text.match(rx) != null) {
        var found = Text.match(rx);

        Text = Text.replace(rx, ' <span class="username-link in-message-link" onclick="LoadPV(\'' + found[1] + '\')">' + found[1] + '</span>');
    }

    return Text;
}

function SetJoinLinks(Text) {

    //https://localhost:5001/JoinChat/4ad5c061-184e-44b2-93af-bdda78e90552
    var rx = new RegExp('\\s{0,1}(https://localhost:5001/JoinChat/([\\d \\w]{8}-[\\d \\w]{4}-[\\d \\w]{4}-[\\d \\w]{4}-[\\d \\w]{12}))', "g");

    var foundAll = Text.match(rx);

    if (foundAll != null) {
        var foundFixed = [];
        for (var i = 0; i < foundAll.length; i++) {
            var matched = foundAll[i];

            var matchedFixed = matched.replace(new RegExp('\\s+'), '');
            if (!foundFixed.includes(matchedFixed)) {
                foundFixed.push(matchedFixed);
            }

        }

        for (var i = 0; i < foundFixed.length; i++) {
            var matched = foundFixed[i];


            var rx2 = new RegExp('https://localhost:5001/JoinChat/([\\d \\w]{8}-[\\d \\w]{4}-[\\d \\w]{4}-[\\d \\w]{4}-[\\d \\w]{12})');
            var f1 = matched.match(rx2);

            var rx3 = new RegExp(f1[0], 'g');
            Text = Text.replace(rx3, '<span class="in-message-link" onclick="JoinChat(\'' + f1[1] + '\')">' + f1[0] + '</span>')
        }
    }


    //while (Text.match(rx) != null) {

    //    Text = Text.replace(rx, ' <span class="in-message-link" onclick="JoinChat(\'' + found[2] + '\')">' + found[1] + '</span>');
    //}

    return Text;
}

async function JoinChat(guid) {

    var Data = {
        joinLinkGuid: guid
    }

    $.ajax({
        contentType: 'application/x-www-form-urlencoded',
        type: "POST",
        url: "/JoinChat",
        data: Data,
        success: function (data) {
            if (data.status == 0) {
                AddChatroom(data.data, true);
            } else if (data.status == 4) {
                LoadChatroomByJoinGuid(guid);
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
                    alert("مشکلی در ارسال پیام پیش آمد");
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
    var Data = {
        'chatroomGuid': ChatroomGuid
    }
    ChatroomGuid = Data.chatroomGuid;

    $.ajax({
        contentType: 'application/x-www-form-urlencoded',
        type: "GET",
        data: Data,
        url: "/Chatrooms/GetProfile",
        success: function (data) {
            ChatroomGuid = data.guid;
            if (data.status == 0) {
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

    ModalContent.innerHTML = '';
    ShowModal();
    /////////////////////////////////
    var headdiv = document.createElement('div');
    headdiv.classList.add('profile-image-container');


    var groupImg = document.createElement('img');
    groupImg.classList.add('profile-image');
    groupImg.src = data.imageName;

    var h2name = document.createElement('h2');
    h2name.classList.add('profile-name');
    h2name.innerHTML = data.groupName;

    if (data.myLevel >= 1) {
        var settingIcon = document.createElement('i');
        settingIcon.classList.add('fa', 'fa-chain');
        headdiv.appendChild(settingIcon);
    }

    headdiv.appendChild(groupImg);
    headdiv.appendChild(h2name);
    /////////////////////////////

    var controlsdiv = document.createElement('div');
    controlsdiv.classList.add('profile-controls');

    var controlDiv = document.createElement('div');
    controlDiv.classList.add('profile-controls-item');
    controlDiv.innerHTML = 'لینک دعوت به گروه';
    controlDiv.addEventListener('click', OpenGroupLink);

    controlsdiv.appendChild(controlDiv);


    var membersContainerDiv = document.createElement('div');
    membersContainerDiv.classList.add('members-list-container');
    membersContainerDiv.innerHTML = '<h2 dir="rtl">اعضا : </h2>'

    for (var i = 0; i < data.members.length; i++) {
        var item = data.members[i];

        var memberdiv = document.createElement('div');
        memberdiv.classList.add('member');
        memberdiv.setAttribute('key', item.hashKey);

        var memberimg = document.createElement('img');
        memberimg.src = item.imageName;
        memberimg.classList.add('member-image');

        var spanName = document.createElement('span');
        spanName.classList.add('member-name');
        spanName.innerHTML = item.name;

        memberdiv.appendChild(memberimg);
        memberdiv.appendChild(spanName);

        switch (item.memberLevel) {

            case 1:
                var adminstar = document.createElement('i');
                adminstar.classList.add('fa', 'fa-star', 'admin');
                memberdiv.appendChild(adminstar);
                break;

            case 2:
                var adminstar = document.createElement('i');
                adminstar.classList.add('fa', 'fa-star', 'master');
                memberdiv.appendChild(adminstar);
                break;

            default:
                break;
        }

        membersContainerDiv.appendChild(memberdiv);
    }



    ModalContent.appendChild(headdiv);
    ModalContent.appendChild(controlsdiv);
    ModalContent.appendChild(membersContainerDiv);



}

function OpenGroupLink() {
    ModalContent.innerHTML = '';

    var outdiv = document.createElement('div');
    outdiv.classList.add('group-link-container');

    var indiv = document.createElement('div');
    indiv.classList.add('group-link');

    var spanlink = document.createElement('span');
    spanlink.classList.add('group-link-display');

    var copyIcon = document.createElement('i');
    copyIcon.classList.add('fa', 'fa-clipboard');
    copyIcon.addEventListener('click', CopyGroupLink);

    indiv.appendChild(spanlink);
    indiv.appendChild(copyIcon);

    var btnChage = document.createElement('button');
    btnChage.classList.add('btn', 'btn-primary');
    btnChage.innerHTML = 'تعویض لینک گروه';

    btnChage.addEventListener('click', btnChangeLink_Clicked);


    outdiv.appendChild(indiv);
    outdiv.appendChild(document.createElement('br'));
    outdiv.appendChild(btnChage);

    ModalContent.appendChild(outdiv);

    GetGroupLinkByGuid(ChatroomGuid);
}

async function CopyGroupLink() {
    var link = document.querySelector('span.group-link-display').innerHTML;

    copyTextToClipboard(link);

    alert('در کلیپ بورد کپی شد');
}

function GetGroupLinkByGuid(guid) {
    //ToDo
    ChatroomGuid = guid;
    $.ajax({
        contentType: 'application/x-www-form-urlencoded',
        type: "GET",
        url: "/GetGroupLink/" + guid,
        success: function (data) {
            if (data.status == 0) {
                document.querySelector('span.group-link-display').innerHTML = 'https://localhost:5001/JoinChat/' + data.data.chatroomJoinLinkGuid;
            }
            else {

            }
        },
        error: function (request, status, error) {
            console.log('Failed To Get List!');
        }
    });
}

function btnChangeLink_Clicked() {

    ChangeGroupLink(ChatroomGuid);

}

function ChangeGroupLink(guid) {
    ChatroomGuid = guid;
    $.ajax({
        contentType: 'application/x-www-form-urlencoded',
        type: "GET",
        url: "/ChangeGroupLink/" + guid,
        success: function (data) {
            if (data.status == 0) {
                document.querySelector('span.group-link-display').innerHTML = 'https://localhost:5001/JoinChat/' + data.data;
            }
            else {

            }
        },
        error: function (request, status, error) {
            console.log('Failed To Get List!');
        }
    });
}

function fallbackCopyTextToClipboard(text) {
    var textArea = document.createElement("textarea");
    textArea.value = text;

    // Avoid scrolling to bottom
    textArea.style.top = "0";
    textArea.style.left = "0";
    textArea.style.position = "fixed";

    document.body.appendChild(textArea);
    textArea.focus();
    textArea.select();

    try {
        var successful = document.execCommand('copy');
        var msg = successful ? 'successful' : 'unsuccessful';
        console.log('Fallback: Copying text command was ' + msg);
    } catch (err) {
        console.error('Fallback: Oops, unable to copy', err);
    }

    document.body.removeChild(textArea);
}

function copyTextToClipboard(text) {
    if (!navigator.clipboard) {
        fallbackCopyTextToClipboard(text);
        return;
    }
    navigator.clipboard.writeText(text).then(function () {
        console.log('Async: Copying to clipboard was successful!');
    }, function (err) {
        console.error('Async: Could not copy text: ', err);
    });
}