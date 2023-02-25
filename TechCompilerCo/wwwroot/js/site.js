﻿// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

function buscarCep() {

    let valorCep = $('#Cep').val();

    let cepFormat = valorCep.replaceAll(".", "").replaceAll("-", "");

    if (cepFormat == "" || cepFormat.length > 8) {

        alert("Cep inválido !!!");
        $('#Cep').val();
        return null;

    } else {

        $.ajax({
            type: 'GET',
            url: "https://viacep.com.br/ws/" + cepFormat + "/json/",
            cache: false,
            dataType: 'json',
            success: function (data) {
                if (data.erro) {
                    alert("Cep não encontrado!");
                    return null;
                }

                $('#Endereco').val(data.logradouro);
                $('#Bairro').val(data.bairro);
                $('#Cidade').val(data.localidade);
                $('#Uf').val(data.uf);

            },
            error: function (jqXHR, textStatus, errorThrown) {
                console.log("Erro ao consultar o CEP. Mensagem:" + errorThrown);
                alert("Cep não encontrado!");
            }
        });
    }
}

setTimeout(function () {
    $('.alert').hide();
}, 3000);