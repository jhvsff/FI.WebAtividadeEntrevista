$(document).ready(function () {

    if (obj != null)
        var idCliente = obj.Id;

    function carregarBeneficiariosDoServidor() {
        return $.ajax({
            url: urlBeneficiarioList,
            type: 'POST',
            dataType: 'json',
            data: {
                idCliente: idCliente
            },
            success: function (data) {
                if (data.Result === "OK") {
                    beneficiariosLocal = data.Records;
                    atualizarGridBeneficiarios();
                } else {
                    console.error("Erro ao carregar beneficiários do servidor:", data.Message);
                }
            },
            error: function (jqXHR, textStatus, errorThrown) {
                console.error("Erro na chamada BeneficiarioList:", textStatus, errorThrown);
            }
        });
    }

    if ($("#gridBeneficiarios").length > 0) {
        $('#gridBeneficiarios').jtable({
            title: 'Beneficiários',
            paging: false,
            sorting: false,
            actions: {
                listAction: function () {
                    return {
                        Result: "OK",
                        Records: beneficiariosLocal
                    };
                }
            },
            fields: {
                CPF: {
                    title: 'CPF',
                    width: '30%'
                },
                Nome: {
                    title: 'Nome',
                    width: '35%'
                },
                Alterar: {
                    title: 'Ações',
                    width: '25%',
                    display: function (data) {
                        return '<button onclick="carregarBeneficiario(' + data.record.Id + ')" class="btn btn-primary btn-sm">Alterar</button>' +
                            '<button onclick="excluirBeneficiario(' + data.record.Id + ')" class="btn btn-danger btn-sm">Excluir</button>';
                    }
                }
            }
        });

        carregarBeneficiariosDoServidor();
    }

    window.excluirBeneficiario = function (idBeneficiario) {
        beneficiario = beneficiariosLocal.find(b => b.Id == idBeneficiario);
        if (!beneficiario.novo)
            beneficiariosRemovidos.push(idBeneficiario);
        beneficiariosLocal = beneficiariosLocal.filter(b => b.Id !== idBeneficiario);
        atualizarGridBeneficiarios();
    };


    function atualizarGridBeneficiarios() {
        $('#gridBeneficiarios').jtable('load');
    }

    window.carregarBeneficiario = function (idBeneficiario) {
        var beneficiario = beneficiariosLocal.find(b => b.Id === idBeneficiario);
        if (beneficiario) {
            $('#beneficiarioId').val(beneficiario.Id);
            $('#beneficiarioCPF').val(beneficiario.CPF);
            $('#beneficiarioNome').val(beneficiario.Nome);
            $('#btnIncluirBeneficiario').text('Salvar');
            $('#beneficiariosModal').modal('show');
        } else {
            alert("Beneficiário não encontrado.");
        }
    };

    function gerarNovoIdTemporario() {
        if (beneficiariosLocal.length === 0) return 1;
        var maxId = Math.max(...beneficiariosLocal.map(b => b.Id));
        return maxId + 1;
    }

    $('#btnIncluirBeneficiario').click(function () {

        if (ValidaCpf() && ValidaNome()) {
            var id = $('#beneficiarioId').val();
            var idDoCliente = idCliente;
            var cpf = $('#beneficiarioCPF').val();
            var nome = $('#beneficiarioNome').val();

            if (id) {
                var beneficiario = beneficiariosLocal.find(b => b.Id === parseInt(id));
                if (beneficiario) {
                    beneficiario.CPF = cpf;
                    beneficiario.Nome = nome;
                }
            } else {
                var newBeneficiario = {
                    Id: gerarNovoIdTemporario(),
                    CPF: cpf,
                    Nome: nome,
                    idCliente: idDoCliente,
                    novo: true
                };
                beneficiariosLocal.push(newBeneficiario);
            }

            atualizarGridBeneficiarios();
            $('#formBeneficiario')[0].reset();
            $('#beneficiarioId').val('');
            $('#btnIncluirBeneficiario').text('Incluir');
        };
    });

    function ValidaNome() {
        var nome = $('#beneficiarioNome').val();
        if (nome == "") {
            alert("O campo Nome é de preenchimento obrigatório!")
            return false;
        }
        return true;
    }

    function ValidaCpf() {
        var cpf = $('#beneficiarioCPF').val();

        if (cpf === "") {
            alert("O campo CPF é de preenchimento obrigatório!");
            return false;
        }

        var cpfNumerico = cpf.replace(/[^\d]/g, '');

        var regexCpf = /^\d{3}\.\d{3}\.\d{3}-\d{2}$/;
        if (!regexCpf.test(cpf)) {
            alert('O CPF está em um formato inválido. Formato esperado: 999.999.999-99');
            return false;
        }

        function validarCpfNumerico(cpfNumerico) {
            var soma = 0;
            var resto;
            var i;

            if (/^(\d)\1+$/.test(cpfNumerico)) {
                return false;
            }

            for (i = 1; i <= 9; i++) soma += parseInt(cpfNumerico.substring(i - 1, i)) * (11 - i);
            resto = (soma * 10) % 11;
            if ((resto === 10) || (resto === 11)) resto = 0;
            if (resto !== parseInt(cpfNumerico.substring(9, 10))) return false;

            soma = 0;
            for (i = 1; i <= 10; i++) soma += parseInt(cpfNumerico.substring(i - 1, i)) * (12 - i);
            resto = (soma * 10) % 11;
            if ((resto === 10) || (resto === 11)) resto = 0;
            if (resto !== parseInt(cpfNumerico.substring(10, 11))) return false;

            return true;
        }

        if (!validarCpfNumerico(cpfNumerico)) {
            alert('Digite um CPF válido!');
            return false;
        }

        function validarCpfUnicoLocal(cpf) {
            var cpfNumerico = cpf.replace(/[^\d]/g, '');
            var id = $('#beneficiarioId').val() || 0;
            var beneficiarioExistente = beneficiariosLocal.some(function (beneficiario) {
                var beneficiarioCpfNumerico = beneficiario.CPF.replace(/[^\d]/g, '');
                return (beneficiarioCpfNumerico === cpfNumerico) && beneficiario.Id != id;
            });

            if (beneficiarioExistente) {
                return false;
            }

            return true;
        }

        if (!validarCpfUnicoLocal(cpf)) {
            alert('Já existe um beneficiário cadastrado com esse CPF!');
            return false;
        }

        return true;
    }
});
