var messagesBox = document.querySelector(".chatroom-main .col-content");
messagesBox.scrollTo(0, 100000010000001000);

var chatroom = document.querySelector('.chatroom-main');
var chatHead = document.querySelector('.chatroom-main .col-head');
var chatList = document.querySelector('.main .col-left');
var messages = document.querySelectorAll('div.messages li');
var BackButton = document.querySelector('.chatroom-main .col-head .back');


for (let i = 0; i < messages.length; i++) {
    const element = messages[i];

    element.addEventListener('click', function () {
        var Guid = element.getAttribute('Guid');
        openChat(Guid);
    });
}

function openChat(Guid) {

    if (window.window.innerWidth <= 768) {

        chatList.style = 'display:none;';

        chatroom.style = 'display: block;overflow-y:scroll;';
        chatHead.style = 'position:sticky;top:0px;z-index:10;';
        BackButton.addEventListener('click', closeChat);

        BackButton.removeAttribute('hidden');
    }


}


function closeChat() {
    chatList.style = '';
    chatroom.style = '';
    chatHead.style = '';
}