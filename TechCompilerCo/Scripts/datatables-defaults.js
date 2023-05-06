$('#MinhaIndex').DataTable({
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
    }
});