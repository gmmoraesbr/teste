baseUrl = "http://localhost:54017/api/v1/";
var oTableEstacionamento;
var oTableManobrista;
$(document).ready(function () {
    oTableEstacionamento = $('#table-estacionamento').DataTable();
    oTableManobrista = $('#table-manobrista').DataTable({
        "order": [[0, "desc"]],
        "language": {
            "sEmptyTable": "Nenhum registro encontrado",
            "sInfo": "Mostrando de _START_ até _END_ de _TOTAL_ registros",
            "sInfoEmpty": "Mostrando 0 até 0 de 0 registros",
            "sInfoFiltered": "(Filtrados de _MAX_ registros)",
            "sInfoPostFix": "",
            "sInfoThousands": ".",
            "sLengthMenu": "_MENU_ resultados por página",
            "sLoadingRecords": "Carregando...",
            "sProcessing": "Processando...",
            "sZeroRecords": "Nenhum registro encontrado",
            "sSearch": "Pesquisar",
            "oPaginate": {
                "sNext": "Próximo",
                "sPrevious": "Anterior",
                "sFirst": "Primeiro",
                "sLast": "Último"
            },
            "oAria": {
                "sSortAscending": ": Ordenar colunas de forma ascendente",
                "sSortDescending": ": Ordenar colunas de forma descendente"
            }
        }
    });
    
});


manobristaList();
pessoasSelected();
estacionamentoList()


function estacionamentoList() {
    $.ajax({
        url: baseUrl + 'estacionamentos',
        type: 'GET',
        success: function (data) {
            $('.loading-kpi-js').hide();
            oTableEstacionamento.clear().draw();
            
            insertEstacionamentoInfo(data);
        },
        error: function (data) {
            // console.log(data);
        },
    });
}

function insertEstacionamentoInfo(data) {
    
    for (var i = 0; i < data.length; i++) {
        oTableEstacionamento.row.add([
            '<div data-td="3_' + i + '">' + data[i].marca + '</div>',
            '<div data-td="4_' + i + '">' + data[i].modelo + '</div>',
            '<div data-td="5_' + i + '">' + data[i].placa + '</div>',
            '<div data-td="6_' + i + '">' + data[i].pessoaId + '</div>',
            '<div data-position="' + i + '" data-id="' + data[i].id + '" class="btn-group btn-group-toggle" data-toggle="buttons">' +
            '<label class="btn btn-success btn-sm active edit-estacionamento" ><input type="radio" name="options" autocomplete="off" checked> Editar </label>' +
            '<label class="btn btn-danger btn-sm delete-estacionamento"><input type="radio" name="options"  autocomplete="off"> Excluir</label>' +
            '</div>',
        ]).draw(true);

        var td_6 = $('div[data-td=6_' + i + ']').parent().hide();
    }
    // DELETE
    $('.delete-estacionamento').click(function(event){
        event.preventDefault();
        
        var Id = $(this).parent().attr('data-id');
        $('.loading-kpi-js').show();
        $.ajax({
            url: baseUrl+ `estacionamentos/` + Id,
            type: 'DELETE',
            contentType: "application/json",
            success: function (data) {
                
                if(data.success == true){
                    $.notify("Entrada deletada com sucesso!", "success");

                    estacionamentoList();
                }else{
                    $.notify("Não foi possível deletar Entrada", "error");
                }
                
            },
            error: function (data) {
                $.notify("Não foi possível deletar Entrada", "error");
            },
        });
        
    });
   
    // UPDATE
    $('.edit-estacionamento').click(function(event){
       event.preventDefault();  
        
       var pos = $(this).parent().attr('data-position');
       var Id = $(this).parent().attr('data-id');
       
       var td_3 = $('div[data-td=3_' + pos + ']').text();
       var td_4 = $('div[data-td=4_' + pos + ']').text();
       var td_5 = $('div[data-td=5_' + pos + ']').text();
       var td_6 = $('div[data-td=6_' + pos + ']').text();
       
       $('#Marca').val(td_3);
       $('#Modelo').val(td_4);
       $('#Placa').val(td_5);
       $('#pessoasSelected').val(td_6);
       
       $('#table-estacionamento').css('opacity','0.3');
       
       $('#insertEstacionamento').parent().hide();
       $('#updateEstacionamento').parent().show();
       
       $('#updateEstacionamento').click(function(event){
            event.preventDefault();

            var marca = $('#Marca').val();
            var modelo = $('#Modelo').val();
            var placa = $('#Placa').val();
            var pessoaSelected = $("#pessoasSelected option:selected").val();

            if(marca && modelo && placa && pessoaSelected){
                
                $('.loading-kpi-js').show(); 
                $.ajax({
                    url: baseUrl + `estacionamentos/` + Id,
                    type: 'PUT',
                    contentType: "application/json",
                    data: JSON.stringify({ Id: Id, PessoaId: pessoaSelected, Marca: marca, Modelo: modelo, Placa: placa, Manobrado: 1 }),
                    success: function (data) {

                        if(data.success == true){
                            $.notify("Entrada alterada com sucesso!", "success");

                            $('#insertEstacionamento').parent().show();
                            $('#updateEstacionamento').parent().hide();

                            $('.loading-kpi-js').hide();
                            $('#table-estacionamento').css('opacity','10');

                            $('#Marca').val('');
                            $('#Modelo').val('');
                            $('#Placa').val('');

                            manobistaList();
                            
                        }else{
                            $.notify("Não foi possível alterar entrada", "error");
                        }

                    },
                    error: function (data) {
                        $.notify("Não foi possível alterar entrada", "error");
                    },
                });

            }
        });
    });
}

function pessoasSelected() {
    $.ajax({
        url: baseUrl + 'pessoas',
        type: 'GET',
        success: function (data) {
            insertPessoasSelected(data);
        },
        error: function (data) {
            // console.log(data);
        },
    });
}

function insertPessoasSelected(data){

    $.each(data, function (i, item) {
        $('#pessoasSelected').append($('<option>', { 
            value: item.id,
            text : item.nome 
        }));
    });
}

$('#insertEstacionamento').click(function(event){
    event.preventDefault();
    
    var marca = $('#Marca').val();
    var modelo = $('#Modelo').val();
    var placa = $('#Placa').val();
    var pessoaSelected = $("#pessoasSelected option:selected").val();
    
    if(marca && modelo && placa && pessoaSelected){
 
         $('.loading-kpi-js').show();
         $.ajax({
             url: baseUrl + `estacionamentos`,
             type: 'POST',
             contentType: "application/json",
             data: JSON.stringify({ PessoaId: pessoaSelected, Marca: marca, Modelo: modelo, Placa: placa, Manobrado: 1 }),
             success: function (data) {

                if(data.success == true){
                    $.notify("Entrada inserida com sucesso!", "success");

                    estacionamentoList();
                 
                    $('.loading-kpi-js').hide();
                    $('#Marca').val('');
                    $('#Modelo').val('');
                    $('#Placa').val('');
                    $('#pessoasSelected').val('');
                }else{
                    $.notify("Não foi possível adicionar entrada", "error");   
                }

             },
             error: function (data) {
                 console.log(url);
                 $.notify("Não foi possível adicionar entrada", "error");
             },
         });
     }
 
 });

/***********Manobrista************/

function manobristaList() {
    $.ajax({
        url: baseUrl + 'pessoas',
        type: 'GET',
        success: function (data) {
            $('.loading-kpi-js').hide();
            oTableManobrista.clear().draw();
            
            insertInfo(data);
        },
        error: function (data) {
            // console.log(data);
        },
    });
}

function insertInfo(data) {
    
    for (var i = 0; i < data.length; i++) {

        var date = new Date(data[i].dataNascimento);
        
        oTableManobrista.row.add([
            '<div data-td="0_' + i + '">' + data[i].nome + '</div>',
            '<div data-td="1_' + i + '">' + data[i].documento + '</div>',
            '<div data-td="2_' + i + '">' + date.getDate() + '/' + date.getMonth() + '/' + date.getFullYear() + '</div>',
            '<div data-position="' + i + '" data-id="' + data[i].id + '" class="btn-group btn-group-toggle" data-toggle="buttons">' +
            '<label class="btn btn-success btn-sm active edit-manobrista" ><input type="radio" name="options" autocomplete="off" checked> Editar </label>' +
            '<label class="btn btn-danger btn-sm delete-manobrista"><input type="radio" name="options"  autocomplete="off"> Excluir</label>' +
            '</div>',
        ]).draw(true);
    }

    // DELETE
    $('.delete-manobrista').click(function(event){
        event.preventDefault();
        
        var Id = $(this).parent().attr('data-id');
        $('.loading-kpi-js').show();
        $.ajax({
            url: baseUrl+ `pessoas/` + Id,
            type: 'DELETE',
            contentType: "application/json",
            success: function (data) {
                
                if(data.success == true){
                    $.notify("Manobrista deletada com sucesso!", "success");
                    manobristaList();
                }else{
                    $.notify("Não foi possível deletar manobrista", "error");
                }
                
            },
            error: function (data) {
                $.notify("Não foi possível deletar manobrista", "error");
            },
        });
        
    });
   
    // UPDATE
    $('.edit-manobrista').click(function(event){
       event.preventDefault();  
        
       var pos = $(this).parent().attr('data-position');
       var Id = $(this).parent().attr('data-id');
       
       var td_0 = $('div[data-td=0_' + pos + ']').text();
       var td_1 = $('div[data-td=1_' + pos + ']').text();
       var td_2 = $('div[data-td=2_' + pos + ']').text();
       
       $('#Nome').val(td_0);
       $('#Documento').val(td_1);
       $('#DataNascimento').val(td_2);
       
       $('#table-manobrista').css('opacity','0.3');
       
       $('#insertManobrista').parent().hide();
       $('#updateManobrista').parent().show();
       
       $('#updateManobrista').click(function(event){
            event.preventDefault();

            var nome = $('#Nome').val();
            var documento = $('#Documento').val();
            
            var parts = $('#DataNascimento').val().split('/');
            var nascimento = new Date(parts[2], parts[1] - 1, parts[0]);
            
            if(nome && documento && nascimento){
                
                $('.loading-kpi-js').show(); 
                $.ajax({
                    url: baseUrl + `pessoas/` + Id,
                    type: 'PUT',
                    contentType: "application/json",
                    data: JSON.stringify({ Id: Id, Nome: nome, Documento: documento, DataNascimento: nascimento, TipoPessoa: 1 }),
                    success: function (data) {
                        
                        if(data.success == true){
                            $.notify("Manobrista alterada com sucesso!", "success");
                        
                            $('#insertManobrista').parent().show();
                            $('#updateManobrista').parent().hide();

                            $('.loading-kpi-js').hide();
                            $('#table-manobrista').css('opacity','10');

                            $('#Nome').val('');
                            $('#Documento').val('');
                            $('#DataNascimento').val('');

                            manobistaList();
                        }else{
                            $.notify("Não foi possível alterar manobrista", "error");
                        }
                        

                    },
                    error: function (data) {
                        $.notify("Não foi possível alterar manobrista", "error");
                    },
                });

            }
        });
    });
}

$('#scheduled-select').datepicker({
            uiLibrary: 'bootstrap4',
            format: 'dd/mm/yyyy',
            locale: 'pt-br',
        });

$('#insertManobrista').click(function(event){
   event.preventDefault();
   
   var nome = $('#Nome').val();
   var documento = $('#Documento').val();
   var nascimento = $('#DataNascimento').val();
   
   
   if(nome && documento && nascimento){

        var arr = nascimento.split('/');
        nascimento = arr[2]+'-'+arr[1]+'-'+arr[0];

        $('.loading-kpi-js').show();
        $.ajax({
            url: baseUrl + `pessoas`,
            type: 'POST',
            contentType: "application/json",
            data: JSON.stringify({ Nome: nome, Documento: documento, DataNascimento: nascimento, TipoPessoa: 1 }),
            success: function (data) {

                if(data.success == true){
                    $.notify("Manobrista inserida com sucesso!", "success");
                    manobristaList();
                    
                    $('.loading-kpi-js').hide();
                    $('#Nome').val('');
                    $('#Documento').val('');
                    $('#DataNascimento').val('');
                }else{
                    $.notify("Não foi possível adicionar manobrista", "error");
                }

            },
            error: function (data) {
                $.notify("Não foi possível adicionar manobrista", "error");
            },
        });
    }

});
