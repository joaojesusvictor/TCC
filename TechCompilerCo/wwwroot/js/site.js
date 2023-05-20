// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

$(document).ready(function () {
    $('#MinhaIndex').DataTable({ 
        dom: 'lBfrtip',
        language: {
            processing: "Processando...",
            search: "Pesquisar:",
            lengthMenu: "_MENU_ resultados por página",
            info: "Mostrando de _START_ até _END_ <br/> _TOTAL_ registros no total",
            infoEmpty: "Mostrando de 0 até 0 <br/> 0 registros no total",
            infoFiltered: "(filtrados de _MAX_ registros no total)",
            infoPostFix: "",
            loadingRecords: "Carregando...",
            zeroRecords: "Nenhum registro encontrado",
            emptyTable: "Nenhum registro encontrado",
            paginate: {
                first: "Primeiro",
                previous: "Anterior",
                next: "Próximo",
                last: "Último"
            },
            aria: {
                sortAscending: "Ordenar colunas de forma ascendente",
                sortDescending: "Ordenar colunas de forma descendente"
            }
        },        
        buttons: [
            'spacer',
            {
                extend: 'excelHtml5',
                text: '<i class="fa fa-table"></i> Excel',                
                titleAttr: 'Excel'
            }
        ],
        responsive: true
    });
});

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
                $('#Pais').val('Brasil');

            },
            error: function (jqXHR, textStatus, errorThrown) {
                console.log("Erro ao consultar o CEP. Mensagem:" + errorThrown);
                alert("Cep não encontrado!");
            }
        });
    }
}

function abrirModalRemoto(url) {
    $.ajax({
        url: url,
        success: function (returnedModalPartial) {
            $('body').append(returnedModalPartial);
        }
    });
}

setTimeout(function () {
    $('.alert').hide();
}, 3000);

$('#btnImprimir').click(function () {
    const conteudo = $('#divImpressao').html();
    const win = window.open('', '', 'height=700,width=700');

    win.document.write('<html>');
    win.document.write('<head>');
    win.document.write('<title>' + $('#tituloImpressao').val() + '</title>');
    win.document.write('</head>');
    win.document.write('<body>');
    win.document.write(conteudo);
    win.document.write('</body>');
    win.document.write('</html>');

    win.print();
    win.close();
});