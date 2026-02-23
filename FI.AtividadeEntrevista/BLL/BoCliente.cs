// BoCliente.cs

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FI.AtividadeEntrevista.BLL
{
    public class BoCliente
    {
        /// <summary>
        /// Inclui um novo cliente
        /// </summary>
        /// <param name="cliente">Objeto de cliente</param>
        public long Incluir(DML.Cliente cliente)
        {
            if (cliente == null)
                throw new Exception("Cliente inválido.");

            cliente.CPF = NormalizarCpf(cliente.CPF);

            ValidarCpfObrigatorio(cliente.CPF);

            if (!ValidarCpf(cliente.CPF))
                throw new Exception("CPF inválido.");

            if (VerificarExistencia(cliente.CPF))
                throw new Exception("CPF já cadastrado.");

            DAL.DaoCliente cli = new DAL.DaoCliente();
            return cli.Incluir(cliente);
        }

        /// <summary>
        /// Altera um cliente
        /// </summary>
        /// <param name="cliente">Objeto de cliente</param>
        public void Alterar(DML.Cliente cliente)
        {
            if (cliente == null)
                throw new Exception("Cliente inválido.");

            cliente.CPF = NormalizarCpf(cliente.CPF);

            ValidarCpfObrigatorio(cliente.CPF);

            if (!ValidarCpf(cliente.CPF))
                throw new Exception("CPF inválido.");

            DAL.DaoCliente cli = new DAL.DaoCliente();

            DML.Cliente atual = cli.Consultar(cliente.Id);
            if (atual == null)
                throw new Exception("Cliente não encontrado.");

            string cpfAtual = NormalizarCpf(atual.CPF);

            if (!string.Equals(cpfAtual, cliente.CPF, StringComparison.Ordinal))
            {
                if (cli.VerificarExistencia(cliente.CPF))
                    throw new Exception("CPF já cadastrado.");
            }

            cli.Alterar(cliente);
        }

        /// <summary>
        /// Consulta o cliente pelo id
        /// </summary>
        /// <param name="id">id do cliente</param>
        /// <returns></returns>
        public DML.Cliente Consultar(long id)
        {
            DAL.DaoCliente cli = new DAL.DaoCliente();
            return cli.Consultar(id);
        }

        /// <summary>
        /// Excluir o cliente pelo id
        /// </summary>
        /// <param name="id">id do cliente</param>
        /// <returns></returns>
        public void Excluir(long id)
        {
            DAL.DaoCliente cli = new DAL.DaoCliente();
            cli.Excluir(id);
        }

        /// <summary>
        /// Lista os clientes
        /// </summary>
        public List<DML.Cliente> Listar()
        {
            DAL.DaoCliente cli = new DAL.DaoCliente();
            return cli.Listar();
        }

        /// <summary>
        /// Lista os clientes
        /// </summary>
        public List<DML.Cliente> Pesquisa(int iniciarEm, int quantidade, string campoOrdenacao, bool crescente, out int qtd)
        {
            DAL.DaoCliente cli = new DAL.DaoCliente();
            return cli.Pesquisa(iniciarEm, quantidade, campoOrdenacao, crescente, out qtd);
        }

        /// <summary>
        /// VerificaExistencia
        /// </summary>
        /// <param name="CPF"></param>
        /// <returns></returns>
        public bool VerificarExistencia(string CPF)
        {
            DAL.DaoCliente cli = new DAL.DaoCliente();
            return cli.VerificarExistencia(NormalizarCpf(CPF));
        }

        private void ValidarCpfObrigatorio(string cpf)
        {
            if (string.IsNullOrWhiteSpace(cpf))
                throw new Exception("CPF é obrigatório.");
        }

        private string NormalizarCpf(string cpf)
        {
            if (string.IsNullOrWhiteSpace(cpf))
                return string.Empty;

            return new string(cpf.Where(char.IsDigit).ToArray());
        }

        private bool ValidarCpf(string cpf)
        {
            if (cpf == null)
                return false;

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
    }
}