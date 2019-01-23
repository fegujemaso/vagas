function ObterNovoGuid() {
    function s4() {
        return Math.floor((1 + Math.random()) * 0x10000)
            .toString(16)
            .substring(1);
    }

    var guid = s4() + s4() + '-' + s4() + '-' + s4() + '-' + s4() + '-' + s4() + s4() + s4();

    return guid.toUpperCase();
}

function OnAjaxRequestSuccess(data) {

}

function OnAjaxRequestError(XMLHttpRequest, textStatus, errorThrown) {
    if (XMLHttpRequest.readyState == 4) {
        //ExibirMensagemErro('Erro Local', '<strong>Código: ' + XMLHttpRequest.status + '</strong><br /><strong>Descrição: </strong>' + XMLHttpRequest.statusText);
    }
    else if (XMLHttpRequest.readyState == 0) {
        ExibirMensagemErro('Erro Local', 'Não foi possível efetuar a conexão com nossos servidores! Tente novamente ou entre em contato com o nosso suporte.');
    }
    else {

    }
}

function AjaxPostRequestAsync(url, data, callback) {
    var contentType = "application/x-www-form-urlencoded; charset=utf-8";

    $.support.cors = true

    return $.ajax({
        url: url, //"/Api/Cliente/Inserir",
        type: 'POST',
        data: '=' + encodeURIComponent(data),
        crossDomain: true,
        contentType: contentType,
        cache: false,
        async: true,
        success: OnAjaxRequestSuccess,
        error: OnAjaxRequestError,
        complete: function (xhr, textStatus) {
            var statusCode = xhr.status;

            if (statusCode > 0 && statusCode != 200 && statusCode != 401 && statusCode != 400 && statusCode != 409) {
                alert("Erro:" + statusCode + "\n\nCode: " + xhr.status + " (" + xhr.statusText + ')\n\nUrl: ' + url + '\n\nResponse: ' + xhr.responseText);

                return;
            }

            if (callback != null)
                callback(JSON.stringify({ "HttpStatusCode": xhr.status, "HttpStatusText": xhr.statusText, "HttpContent": xhr.responseText }));
        }
    });
}

function AjaxGetRequestAsync(url, callback) {
    var contentType = "application/x-www-form-urlencoded; charset=utf-8";

    $.support.cors = true

    return $.ajax({
        url: url, //"/Api/Cliente/Inserir",
        type: 'GET',
        //data: '=' + encodeURIComponent(data),
        crossDomain: true,
        //contentType: contentType,
        cache: false,
        async: true,
        success: OnAjaxRequestSuccess,
        error: OnAjaxRequestError,
        complete: function (xhr, textStatus) {
            var statusCode = xhr.status;

            if (statusCode > 0 && statusCode != 200 && statusCode != 401 && statusCode != 400 && statusCode != 409) {
                alert("Erro:" + statusCode + "\n\nCode: " + xhr.status + " (" + xhr.statusText + ')\n\nUrl: ' + url + '\n\nResponse: ' + xhr.responseText);

                return;
            }

            if (callback != null)
                callback(JSON.stringify({ "HttpStatusCode": xhr.status, "HttpStatusText": xhr.statusText, "HttpContent": xhr.responseText }));
        }
    });
}

function ExibirMensagemErro(titulo, mensagem) {
    toastr['error'](mensagem, titulo);
}

function ExibirMensagemErroCallback(titulo, mensagem, callback) {
    bootbox.dialog({
        animate: true,
        message: mensagem,
        title: titulo,
        buttons: {
            main: {
                label: "Entendi!",
                className: "btn-danger",
                callback: callback
            }
        }
    });
}

function ProcessarResponseJson(data) {
    var httpResponseMessage = JSON.parse(data);

    if (httpResponseMessage === null || httpResponseMessage === undefined)
        return null;

    var jsonObject = JSON.parse(httpResponseMessage.HttpContent);

    var mensagem = '';

    if (httpResponseMessage.HttpStatusCode === 400) {
        mensagem = 'Um erro ocorreu, por favor, contate o administrador do sistema e tente novamente!<br /><br />';

        mensagem += '<strong>Mensagem</strong>: ' + jsonObject['Message'] + '<br />';

        mensagem += '<strong>Classe/Módulo</strong>: ' + jsonObject['TargetSite'] + '<br />';

        mensagem += '<strong>Linha</strong>: ' + jsonObject['LineNumber'] + '<br />';

        ExibirMensagemErro('Erro de Requisição', mensagem);

        return null;
    } else if (httpResponseMessage.HttpStatusCode == 409) {
        mensagem = '<p class="text-center">Usuário não encontrado ou senha inválida!<br /><br />Caso você não tenha acesso ou tenha esquecido sua senha, entre em contato com o suporte.</p>';

        ExibirMensagemErro('Erro de Acesso', mensagem);

        return null;
    }

    return jsonObject;
}

/*****************************************************************************************************/
/* Funções de conversão */
/*****************************************************************************************************/

(function ($) {
    $.fn.toJsonObject = function () {
        var result = {};

        var inputDisabled = this.find(':input:disabled').removeAttr('disabled');

        var inputDefault = [];

        var inputLista = this.find('input');

        for (var i = 0; i < inputLista.length; i++)
            if ($(inputLista[i]).val() === '' && $(inputLista[i]).attr('default') !== '' && $(inputLista[i]).attr('default') !== undefined && $(inputLista[i]).attr('default') !== null) {
                $(inputLista[i]).val($(inputLista[i]).attr('default'));

                inputDefault.push(inputLista[i]);
            }

        var a = this.serializeArray();

        for (var i = 0; i < inputDefault.length; i++)
            $(inputDefault[i]).val('');

        inputDisabled.attr('disabled', 'disabled');

        $.each(a, function () {
            if (result[this.name] !== undefined) {
                if (!result[this.name].push) {
                    result[this.name] = [result[this.name]];
                }
                result[this.name].push(this.value || '');
            } else {
                result[this.name] = this.value || '';
            }
        });

        return result;
    }

    $.fn.toJsonList = function () {
        var result = [];

        var a = this.toJsonObject();

        $.each(a, function (key, value) {
            if (Array.isArray(value))
                $.each(value, function (keyIndex, keyItem) {
                    var item = result[keyIndex];

                    if (item !== undefined) { // existe já
                        if (item[key] === undefined) {// não existe o nome da propriedade
                            item[key] = keyItem;
                        }
                    } else {
                        item = {};

                        item[key] = keyItem;

                        result.push(item);
                    }
                });
        });

        if (result.length === 0 && !jQuery.isEmptyObject(a))
            result.push(a);

        return result;
    }
})(jQuery);

/*****************************************************************************************************/
/* Funções de pressionamento do ENTER */
/*****************************************************************************************************/
function AoPressionarEnter(sender, e, funcao, parametro) {
    if (e.keyCode == 13) {
        funcao(parametro);

        e.preventDefault();

        return false;
    }
}
