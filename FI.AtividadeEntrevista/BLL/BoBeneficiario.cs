using System;
using System.Collections.Generic;
using System.Linq;

namespace FI.AtividadeEntrevista.BLL
{
    public class BoBeneficiario
    {
        private string NormalizarCpf(string cpf)
        {
            if (string.IsNullOrWhiteSpace(cpf))
                return string.Empty;

            return new string(cpf.Where(char.IsDigit).ToArray());
        }

        private bool ValidarCpf(string cpf)
        {
            cpf = NormalizarCpf(cpf);

            if (cpf.Length != 11)
                return false;

            if (cpf.Distinct().Count() == 1)
                return false;

            int[] nums = cpf.Select(c => c - '0').ToArray();

            int soma = 0;
            for (int i = 0; i < 9; i++)
                soma += nums[i] * (10 - i);

            int resto = soma % 11;
            int dv1 = (resto < 2) ? 0 : (11 - resto);

            if (nums[9] != dv1)
                return false;

            soma = 0;
            for (int i = 0; i < 10; i++)
                soma += nums[i] * (11 - i);

            resto = soma % 11;
            int dv2 = (resto < 2) ? 0 : (11 - resto);

            return nums[10] == dv2;
        }

        public List<DML.Beneficiario> ListarPorCliente(long idCliente)
        {
            DAL.DaoBeneficiario dao = new DAL.DaoBeneficiario();
            return dao.ListarPorCliente(idCliente);
        }

        public long Incluir(DML.Beneficiario beneficiario)
        {
            DAL.DaoBeneficiario dao = new DAL.DaoBeneficiario();
            beneficiario.CPF = NormalizarCpf(beneficiario.CPF);
            return dao.Incluir(beneficiario);
        }

        public void ExcluirPorCliente(long idCliente)
        {
            DAL.DaoBeneficiario dao = new DAL.DaoBeneficiario();
            dao.ExcluirPorCliente(idCliente);
        }

        public void SalvarBeneficiarios(long idCliente, List<DML.Beneficiario> beneficiarios)
        {
            if (idCliente <= 0)
                throw new Exception("Cliente inválido.");

            if (beneficiarios == null)
                beneficiarios = new List<DML.Beneficiario>();

            foreach (var b in beneficiarios)
            {
                b.IdCliente = idCliente;
                b.CPF = NormalizarCpf(b.CPF);

                if (string.IsNullOrWhiteSpace(b.CPF))
                    throw new Exception("CPF do beneficiário é obrigatório.");

                if (!ValidarCpf(b.CPF))
                    throw new Exception("CPF do beneficiário inválido.");

                if (string.IsNullOrWhiteSpace(b.Nome))
                    throw new Exception("Nome do beneficiário é obrigatório.");
            }

            if (beneficiarios.Select(x => x.CPF).Distinct().Count() != beneficiarios.Count)
                throw new Exception("Não é permitido incluir beneficiários com CPF duplicado para o mesmo cliente.");

            DAL.DaoBeneficiario dao = new DAL.DaoBeneficiario();

            dao.ExcluirPorCliente(idCliente);

            foreach (var b in beneficiarios)
                dao.Incluir(b);
        }
    }
}