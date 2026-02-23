using FI.AtividadeEntrevista.BLL;
using FI.AtividadeEntrevista.DML;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Transactions;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using WebAtividadeEntrevista.Models;

namespace WebAtividadeEntrevista.Controllers
{
    public class ClienteController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public ActionResult Incluir()
        {
            var model = new ClienteModel
            {
                Id = 0,
                Beneficiarios = new List<BeneficiarioModel>()
            };

            return View(model);
        }

        [HttpPost]
        public JsonResult Incluir(ClienteModel model)
        {
            BoCliente bo = new BoCliente();

            if (!ModelState.IsValid)
            {
                var erros = (from item in ModelState.Values
                             from error in item.Errors
                             select error.ErrorMessage).ToList();

                Response.StatusCode = 400;
                return Json(string.Join(Environment.NewLine, erros));
            }

            model.CPF = OnlyDigits(model.CPF);

            if (bo.VerificarExistencia(model.CPF))
            {
                Response.StatusCode = 400;
                return Json("CPF já cadastrado.");
            }

            var beneficiarios = ParseBeneficiarios(model.BeneficiariosJson);

            var erroBenef = ValidarBeneficiarios(beneficiarios);
            if (!string.IsNullOrEmpty(erroBenef))
            {
                Response.StatusCode = 400;
                return Json(erroBenef);
            }

            using (var scope = new TransactionScope())
            {
                model.Id = bo.Incluir(new Cliente()
                {
                    CEP = model.CEP,
                    Cidade = model.Cidade,
                    Email = model.Email,
                    Estado = model.Estado,
                    Logradouro = model.Logradouro,
                    Nacionalidade = model.Nacionalidade,
                    Nome = model.Nome,
                    CPF = model.CPF,
                    Sobrenome = model.Sobrenome,
                    Telefone = model.Telefone
                });

                if (beneficiarios != null && beneficiarios.Count > 0)
                {
                    var boBen = new BoBeneficiario();

                    foreach (var b in beneficiarios)
                    {
                        boBen.Incluir(new Beneficiario()
                        {
                            Nome = b.Nome,
                            CPF = OnlyDigits(b.CPF),
                            IdCliente = model.Id
                        });
                    }
                }

                scope.Complete();
            }

            return Json("Cadastro efetuado com sucesso");
        }

        [HttpGet]
        public ActionResult Alterar(long id)
        {
            var bo = new BoCliente();
            var cliente = bo.Consultar(id);

            if (cliente == null)
                return HttpNotFound("Cliente não encontrado.");

            var boBen = new BoBeneficiario();
            var beneficiariosDb = boBen.ListarPorCliente(id) ?? new List<FI.AtividadeEntrevista.DML.Beneficiario>();

            var model = new ClienteModel()
            {
                Id = cliente.Id,
                CEP = cliente.CEP,
                Cidade = cliente.Cidade,
                Email = cliente.Email,
                Estado = cliente.Estado,
                Logradouro = cliente.Logradouro,
                Nacionalidade = cliente.Nacionalidade,
                Nome = cliente.Nome,
                CPF = cliente.CPF,
                Sobrenome = cliente.Sobrenome,
                Telefone = cliente.Telefone,

                Beneficiarios = beneficiariosDb.Select(b => new BeneficiarioModel
                {
                    Id = b.Id,
                    CPF = b.CPF,
                    Nome = b.Nome
                }).ToList()
            };

            return View(model);
        }

        [HttpPost]
        public JsonResult Alterar(ClienteModel model)
        {
            var bo = new BoCliente();

            if (!ModelState.IsValid)
            {
                var erros = (from item in ModelState.Values
                             from error in item.Errors
                             select error.ErrorMessage).ToList();

                Response.StatusCode = 400;
                return Json(string.Join(Environment.NewLine, erros));
            }

            if (model.Id <= 0)
            {
                Response.StatusCode = 400;
                return Json("Id do cliente inválido.");
            }

            model.CPF = OnlyDigits(model.CPF);

            var clienteAtual = bo.Consultar(model.Id);
            if (clienteAtual == null)
            {
                Response.StatusCode = 400;
                return Json("Cliente não encontrado.");
            }

            if (!string.Equals(clienteAtual.CPF, model.CPF, StringComparison.InvariantCultureIgnoreCase))
            {
                if (bo.VerificarExistencia(model.CPF))
                {
                    Response.StatusCode = 400;
                    return Json("CPF já cadastrado.");
                }
            }

            var beneficiarios = ParseBeneficiarios(model.BeneficiariosJson);

            var erroBenef = ValidarBeneficiarios(beneficiarios);
            if (!string.IsNullOrEmpty(erroBenef))
            {
                Response.StatusCode = 400;
                return Json(erroBenef);
            }

            using (var scope = new TransactionScope())
            {
                bo.Alterar(new Cliente()
                {
                    Id = model.Id,
                    CEP = model.CEP,
                    Cidade = model.Cidade,
                    Email = model.Email,
                    Estado = model.Estado,
                    Logradouro = model.Logradouro,
                    Nacionalidade = model.Nacionalidade,
                    Nome = model.Nome,
                    CPF = model.CPF,
                    Sobrenome = model.Sobrenome,
                    Telefone = model.Telefone
                });

                var boBen = new BoBeneficiario();

                boBen.ExcluirPorCliente(model.Id);

                if (beneficiarios != null && beneficiarios.Count > 0)
                {
                    foreach (var b in beneficiarios)
                    {
                        boBen.Incluir(new Beneficiario()
                        {
                            Nome = b.Nome,
                            CPF = OnlyDigits(b.CPF),
                            IdCliente = model.Id
                        });
                    }
                }

                scope.Complete();
            }

            return Json("Cadastro alterado com sucesso");
        }

        [HttpPost]
        public JsonResult ClienteList(int jtStartIndex = 0, int jtPageSize = 0, string jtSorting = null)
        {
            try
            {
                int qtd = 0;
                string campo = string.Empty;
                string crescente = string.Empty;
                string[] array = (jtSorting ?? "").Split(' ');

                if (array.Length > 0) campo = array[0];
                if (array.Length > 1) crescente = array[1];

                var clientes = new BoCliente().Pesquisa(
                    jtStartIndex,
                    jtPageSize,
                    campo,
                    crescente.Equals("ASC", StringComparison.InvariantCultureIgnoreCase),
                    out qtd
                );

                return Json(new { Result = "OK", Records = clientes, TotalRecordCount = qtd });
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }
        }

        private static string OnlyDigits(string value)
        {
            if (string.IsNullOrEmpty(value))
                return value;

            return new string(value.Where(char.IsDigit).ToArray());
        }

        private static List<BeneficiarioModel> ParseBeneficiarios(string beneficiariosJson)
        {
            if (string.IsNullOrWhiteSpace(beneficiariosJson))
                return new List<BeneficiarioModel>();

            var serializer = new JavaScriptSerializer();
            return serializer.Deserialize<List<BeneficiarioModel>>(beneficiariosJson) ?? new List<BeneficiarioModel>();
        }

        private static string ValidarBeneficiarios(List<BeneficiarioModel> beneficiarios)
        {
            if (beneficiarios == null || beneficiarios.Count == 0)
                return null;

            foreach (var b in beneficiarios)
                b.CPF = OnlyDigits(b.CPF);

            if (beneficiarios.Any(b => string.IsNullOrWhiteSpace(b.Nome)))
                return "Nome do beneficiário é obrigatório.";

            if (beneficiarios.Any(b => string.IsNullOrWhiteSpace(b.CPF) || b.CPF.Length != 11))
                return "CPF do beneficiário inválido.";

            var duplicados = beneficiarios
                .GroupBy(b => b.CPF)
                .Where(g => g.Count() > 1)
                .Select(g => g.Key)
                .ToList();

            if (duplicados.Count > 0)
                return "Não é permitido cadastrar mais de um beneficiário com o mesmo CPF para o mesmo cliente.";

            return null;
        }
    }
}