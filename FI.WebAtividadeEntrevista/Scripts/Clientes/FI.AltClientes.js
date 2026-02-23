(function () {
    "use strict";

    if (window.__FI_CLIENTE_ALTERAR_JS_LOADED__) return;
    window.__FI_CLIENTE_ALTERAR_JS_LOADED__ = true;

    $(document).ready(function () {

        function onlyDigits(s) { return (s || "").replace(/\D/g, ""); }

        function maskCpf(value) {
            var v = onlyDigits(value);
            if (v.length > 11) v = v.substring(0, 11);
            v = v.replace(/(\d{3})(\d)/, "$1.$2");
            v = v.replace(/(\d{3})(\d)/, "$1.$2");
            v = v.replace(/(\d{3})(\d{1,2})$/, "$1-$2");
            return v;
        }

        function cpfValido(cpf) {
            var v = onlyDigits(cpf);

            if (v.length !== 11) return false;
            if (/^(\d)\1{10}$/.test(v)) return false;

            var soma = 0;
            for (var i = 0; i < 9; i++) soma += parseInt(v.charAt(i), 10) * (10 - i);

            var resto = soma % 11;
            var dv1 = (resto < 2) ? 0 : (11 - resto);
            if (parseInt(v.charAt(9), 10) !== dv1) return false;

            soma = 0;
            for (i = 0; i < 10; i++) soma += parseInt(v.charAt(i), 10) * (11 - i);

            resto = soma % 11;
            var dv2 = (resto < 2) ? 0 : (11 - resto);

            return parseInt(v.charAt(10), 10) === dv2;
        }

        var beneficiariosList = Array.isArray(window.beneficiarios) ? window.beneficiarios : [];
        var indiceEdicaoBeneficiario = -1;

        function atualizarHiddenBeneficiarios() {
            var $hidden = $("#BeneficiariosJson");
            if ($hidden.length) {
                $hidden.val(JSON.stringify(beneficiariosList));
            }
        }

        function atualizarGridBeneficiarios() {
            var $grid = $("#gridBeneficiarios");
            if (!$grid.length) return;

            var $tbody = $grid.find("tbody");
            $tbody.empty();

            for (var i = 0; i < beneficiariosList.length; i++) {
                var b = beneficiariosList[i];
                var cpfFormatado = maskCpf(b.CPF);

                var linha = ''
                    + '<tr>'
                    + '  <td>' + cpfFormatado + '</td>'
                    + '  <td>' + (b.Nome || "") + '</td>'
                    + '  <td>'
                    + '    <button type="button" class="btn btn-primary btn-sm btn-alterar-beneficiario" data-index="' + i + '">Alterar</button> '
                    + '    <button type="button" class="btn btn-danger btn-sm btn-excluir-beneficiario" data-index="' + i + '">Excluir</button>'
                    + '  </td>'
                    + '</tr>';

                $tbody.append(linha);
            }
        }

        $(document).on("input", "#CPF", function () {
            $(this).val(maskCpf($(this).val()));
        });

        $(document).on("input", "#CPFBeneficiario", function () {
            $(this).val(maskCpf($(this).val()));
        });

        var obj = window.obj;
        if (obj) {
            $("#formCadastro #Nome").val(obj.Nome);
            $("#formCadastro #CEP").val(obj.CEP);
            $("#formCadastro #Email").val(obj.Email);
            $("#formCadastro #Sobrenome").val(obj.Sobrenome);
            $("#formCadastro #Nacionalidade").val(obj.Nacionalidade);
            $("#formCadastro #Estado").val(obj.Estado);
            $("#formCadastro #Cidade").val(obj.Cidade);
            $("#formCadastro #Logradouro").val(obj.Logradouro);
            $("#formCadastro #Telefone").val(obj.Telefone);
            $("#formCadastro #CPF").val(maskCpf(obj.CPF));
        }

        $(document).on("click", "#btnIncluirBeneficiario", function () {
            var cpf = $("#CPFBeneficiario").val();
            var nome = $("#NomeBeneficiario").val();

            if (!cpf || !nome) {
                ModalDialog("Atenção", "Informe CPF e Nome do beneficiário.");
                return;
            }

            var cpfDigits = onlyDigits(cpf);

            if (!cpfValido(cpfDigits)) {
                ModalDialog("Atenção", "CPF de beneficiário inválido.");
                return;
            }

            for (var i = 0; i < beneficiariosList.length; i++) {
                if (i === indiceEdicaoBeneficiario) continue;
                if (beneficiariosList[i].CPF === cpfDigits) {
                    ModalDialog("Atenção", "Já existe beneficiário com este CPF para este cliente.");
                    return;
                }
            }

            if (indiceEdicaoBeneficiario >= 0) {
                beneficiariosList[indiceEdicaoBeneficiario].CPF = cpfDigits;
                beneficiariosList[indiceEdicaoBeneficiario].Nome = nome;
            } else {
                beneficiariosList.push({ Id: 0, CPF: cpfDigits, Nome: nome });
            }

            indiceEdicaoBeneficiario = -1;
            $("#CPFBeneficiario").val("");
            $("#NomeBeneficiario").val("");

            atualizarGridBeneficiarios();
            atualizarHiddenBeneficiarios();
        });

        $(document).on("click", ".btn-alterar-beneficiario", function () {
            var index = parseInt($(this).data("index"), 10);
            if (isNaN(index) || index < 0 || index >= beneficiariosList.length) return;

            var b = beneficiariosList[index];
            indiceEdicaoBeneficiario = index;

            $("#CPFBeneficiario").val(maskCpf(b.CPF));
            $("#NomeBeneficiario").val(b.Nome);
        });

        $(document).on("click", ".btn-excluir-beneficiario", function () {
            var index = parseInt($(this).data("index"), 10);
            if (isNaN(index) || index < 0 || index >= beneficiariosList.length) return;

            beneficiariosList.splice(index, 1);
            indiceEdicaoBeneficiario = -1;

            $("#CPFBeneficiario").val("");
            $("#NomeBeneficiario").val("");

            atualizarGridBeneficiarios();
            atualizarHiddenBeneficiarios();
        });

        atualizarGridBeneficiarios();
        atualizarHiddenBeneficiarios();

        $(document).on("submit", "#formCadastro", function (e) {
            e.preventDefault();

            var formEl = this;
            if (typeof formEl.reportValidity === "function") {
                if (!formEl.reportValidity()) return;
            }

            if (typeof window.urlPost === "undefined" || !window.urlPost) {
                ModalDialog("Erro", "urlPost não foi definida na View.");
                return;
            }
            if (typeof window.urlRetorno === "undefined" || !window.urlRetorno) {
                ModalDialog("Erro", "urlRetorno não foi definida na View.");
                return;
            }

            atualizarHiddenBeneficiarios();

            var $form = $(this);

            $.ajax({
                url: window.urlPost,
                method: "POST",
                data: {
                    "Id": $form.find("#Id").val(),
                    "Nome": $form.find("#Nome").val(),
                    "Sobrenome": $form.find("#Sobrenome").val(),
                    "Nacionalidade": $form.find("#Nacionalidade").val(),
                    "CEP": $form.find("#CEP").val(),
                    "Estado": $form.find("#Estado").val(),
                    "Cidade": $form.find("#Cidade").val(),
                    "Logradouro": $form.find("#Logradouro").val(),
                    "Email": $form.find("#Email").val(),
                    "Telefone": $form.find("#Telefone").val(),
                    "CPF": onlyDigits($form.find("#CPF").val()),
                    "BeneficiariosJson": $("#BeneficiariosJson").val()
                },
                error: function (r) {
                    if (r && r.status === 400) {
                        ModalDialog("Ocorreu um erro", r.responseJSON || r.responseText || "Requisição inválida.");
                    } else if (r && r.status === 500) {
                        ModalDialog("Ocorreu um erro", "Ocorreu um erro interno no servidor.");
                    } else {
                        ModalDialog("Ocorreu um erro", "Falha ao salvar. Verifique sua conexão e tente novamente.");
                    }
                },
                success: function (r) {
                    ModalDialog("Sucesso!", r, function () {
                        window.location.href = window.urlRetorno;
                    });
                }
            });
        });

    });
})();

function ModalDialog(titulo, texto, onClose) {
    var random = Math.random().toString().replace(".", "");
    var html = ''
        + '<div id="' + random + '" class="modal fade" tabindex="-1" role="dialog">'
        + '  <div class="modal-dialog" role="document">'
        + '    <div class="modal-content">'
        + '      <div class="modal-header">'
        + '        <button type="button" class="close" data-dismiss="modal" aria-hidden="true">×</button>'
        + '        <h4 class="modal-title">' + titulo + '</h4>'
        + '      </div>'
        + '      <div class="modal-body">'
        + '        <p>' + texto + '</p>'
        + '      </div>'
        + '      <div class="modal-footer">'
        + '        <button type="button" class="btn btn-default" data-dismiss="modal">Fechar</button>'
        + '      </div>'
        + '    </div>'
        + '  </div>'
        + '</div>';

    $("body").append(html);

    var $modal = $("#" + random);

    $modal.on("hidden.bs.modal", function () {
        $modal.remove();
        if (typeof onClose === "function") onClose();
    });

    $modal.modal("show");
}