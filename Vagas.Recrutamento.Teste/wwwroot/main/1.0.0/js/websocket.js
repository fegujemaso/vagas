var webSocketMonitor = undefined;

$(document).ready(function () {
    webSocketMonitor = new WebSocket((location.protocol === "https:" ? "wss:" : "ws:") + "//" + window.location.host + "/Monitor");

    webSocketMonitor.onopen = WebSocket_OnOpen;

    //webSocketMonitor.onclose = WebSocket_OnClose;

    webSocketMonitor.onerror = WebSocket_OnError;

    webSocketMonitor.onmessage = function (e) {
        $('#msgs').prepend(e.data + '</br>');
    };
});

function WebSocket_OnOpen(e) {
    console.log("Socket conectado", e);

    $('#msgs').append('Novo socket conectado');
}
function WebSocket_OnClose(e) {
    console.log("Socket fechado", e);
}
function WebSocket_OnError(e) {
    console.log("Socket com erro", e);
}
