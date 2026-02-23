
$(document).ready(function () {

    console.log("beneficiarios:", window.beneficiarios);
    console.log("len:", (window.beneficiarios || []).length);
    function onlyDigits(s) { return (s || '').replace(/\D/g, ''); }

    function maskCpf(value) {
        var v = onlyDigits(value);
        if (v.length > 11) v = v.substring(0, 11);
        v = v.replace(/(\d{3})(\d)/, '$1.$2');
        v = v.replace(/(\d{3})(\d)/, '$1.$2');
        v = v.replace(/(\d{3})(\d{1,2})$/, '$1-$2');
        return v;
    }

    if (document.getElementById("gridClientes"))
        $('#gridClientes').jtable({
            title: 'Clientes',
            paging: true, //Enable paging
            pageSize: 5, //Set page size (default: 10)
            sorting: true, //Enable sorting
            defaultSorting: 'Nome ASC', //Set default sorting
            actions: {
                listAction: urlClienteList,
            },
            fields: {
                Nome: {
                    title: 'Nome',
                    width: '40%'
                },
                Email: {
                    title: 'Email',
                    width: '35%'
                },
                CPF: {
                    title: 'CPF',
                    width: '15%',
                    display: function (data) {
                        return maskCpf(data.record.CPF);
                    }
                },
                Alterar: {
                    title: '',
                    display: function (data) {
                        return '<button onclick="window.location.href=\'' + urlAlteracao + '/' + data.record.Id + '\'" class="btn btn-primary btn-sm">Alterar</button>';
                    }
                }
            }
        });

    //Load student list from server
    if (document.getElementById("gridClientes"))
        $('#gridClientes').jtable('load');
})