using FI.AtividadeEntrevista.BLL;
using WebAtividadeEntrevista.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using FI.AtividadeEntrevista.DML;
using Newtonsoft.Json;

namespace WebAtividadeEntrevista.Controllers
{
    public class BeneficiarioController : Controller
    {
        public JsonResult IncluirOuAlterar(BeneficiarioModel model, long idCliente)
        {
            BoBeneficiario boBeneficiario = new BoBeneficiario();

            if (!this.ModelState.IsValid)
            {
                List<string> erros = (from item in ModelState.Values
                                      from error in item.Errors
                                      select error.ErrorMessage).ToList();

                Response.StatusCode = 400;
                return Json(string.Join(Environment.NewLine, erros));
            }
            else
            {

                Beneficiario beneficiario = new Beneficiario()
                {
                    IdCliente = idCliente,
                    CPF = model.CPF,
                    Nome = model.Nome,
                    Id = model.Novo ? 0 : model.Id
                };
                if (beneficiario.Id != 0)
                    boBeneficiario.Alterar(beneficiario);
                else
                    boBeneficiario.Incluir(beneficiario);


                return Json("Cadastro efetuado com sucesso");
            }
        }

        [HttpPost]
        public JsonResult BeneficiarioList(long idCliente)
        {
            try
            {
                List<Beneficiario> beneficiarios = new BoBeneficiario().ListarPorCliente(idCliente);

                return Json(new { Result = "OK", Records = beneficiarios.OrderBy(b => b.Nome), TotalRecordCount = beneficiarios.Count });
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = "Erro no servidor: " + ex.Message });
            }
        }

        public JsonResult ExcluirBeneficiario(long beneficiarioId)
        {
            BoBeneficiario boBeneficiario = new BoBeneficiario();
            var beneficiario = boBeneficiario.Consultar(beneficiarioId);
            if (beneficiario != null)
            {
                boBeneficiario.Excluir(beneficiario.Id);
            }
            return Json("Beneficiário excluído com sucesso.");
        }
    }
}