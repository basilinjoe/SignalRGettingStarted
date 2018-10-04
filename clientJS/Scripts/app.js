var connection = $.hubConnection("http://localhost:51030/signalr", { useDefaultPath: false });
var proxy = connection.createHubProxy('chatHub');
proxy.on("message", function (message) {
    addMessageToDom(message);
});

function addMessageToDom(message) {
    var msg = message.replace(/&/g, "&amp;").replace(/</g, "&lt;").replace(/>/g, "&gt;");
    var li = document.createElement("li");
    li.textContent = msg;
    document.getElementById("messagesList").appendChild(li);
}

connection.start()
    .done(function () { console.log('Now connected, connection ID=' + connection.id); })
    .fail(function () { console.log('Could not connect'); });

document.getElementById("sendButton").addEventListener("click", function (event) {
    var user = document.getElementById("userInput").value;
    var message = document.getElementById("messageInput").value;
    proxy.invoke("send", user, message)
        .done(function (res) {
            addMessageToDom(message);
        })
        .fail(function (err) {
        return console.error(err.toString());
    });
    event.preventDefault();
});

connection.starting(function () {
    console.log('Stasrting..');
});

connection.received(function (data) {
    console.log('received', data);
});

connection.connectionSlow(function () {
    console.log('We are currently experiencing difficulties with the connection.')
});

connection.reconnecting(function () {
    console.log('reconnecting');
});

connection.reconnected(function () {
    console.log('reconnected');
});

connection.stateChanged(function (data) {
    console.log('stateChanged', data);
});

connection.disconnected(function () {
    console.log('disconnected');
});