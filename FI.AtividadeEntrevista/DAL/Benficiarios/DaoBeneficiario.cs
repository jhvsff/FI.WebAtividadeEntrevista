using FI.AtividadeEntrevista.DML;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

namespace FI.AtividadeEntrevista.DAL.Benficiarios
{
    /// <summary>
    /// Classe de acesso a dados do Beneficiario
    /// </summary>
    internal class DaoBeneficiario : AcessoDados
    {
        /// <summary>
        /// Inclui um novo Beneficiario
        /// </summary>
        /// <param name="beneficiario">Objeto de beneficiario</param>
        internal long Incluir(Beneficiario beneficiario)
        {

            List<SqlParameter> parametros = new List<SqlParameter>
            {
                new SqlParameter("Nome", beneficiario.Nome),
                new SqlParameter("CPF", beneficiario.CPF),
                new SqlParameter("IDCLIENTE", beneficiario.IdCliente)
            };

            DataSet ds = base.Consultar("FI_SP_IncBeneficiario", parametros);
            long ret = 0;
            if (ds.Tables[0].Rows.Count > 0)
                long.TryParse(ds.Tables[0].Rows[0][0].ToString(), out ret);
            return ret;
        }

        /// <summary>
        /// Consulta Beneficiários pelo Id
        /// </summary>
        /// <param name="id">Id do Beneficiário</param>
        internal Beneficiario Consultar(long id)
        {
            List<SqlParameter> parametros = new List<SqlParameter>
            {
                new SqlParameter("ID", id)
            };

            DataSet ds = base.Consultar("FI_SP_ConsBeneficiario", parametros);
            List<Beneficiario> beneficiario = Converter(ds);

            return beneficiario.FirstOrDefault();
        }

        internal bool VerificarExistencia(string CPF, long ID)
        {
            List<SqlParameter> parametros = new List<SqlParameter>
            {
                new SqlParameter("CPF", CPF),
                new SqlParameter("ID", ID)
            };

            DataSet ds = base.Consultar("FI_SP_VerificaBeneficiario", parametros);

            return ds.Tables[0].Rows.Count > 0;
        }

        /// <summary>
        /// Lista todos os beneficiarios
        /// </summary>
        internal List<Beneficiario> Listar()
        {
            List<SqlParameter> parametros = new List<SqlParameter>
            {
                new SqlParameter("ID", 0)
            };

            DataSet ds = base.Consultar("FI_SP_ConsBeneficiario", parametros);
            List<Beneficiario> listaBeneficiarios = Converter(ds);

            return listaBeneficiarios;
        }

        /// <summary>
        /// Lista todos os beneficiarios de um cliente
        /// </summary>
        /// /// <param name="idCliente">Id do Cliente</param>
        internal List<Beneficiario> ListarPorCliente(long idCliente)
        {
            List<SqlParameter> parametros = new List<SqlParameter>
            {
                new SqlParameter("Id_Cliente", idCliente)
            };

            DataSet ds = base.Consultar("FI_SP_ConsBeneficiarioPorCliente", parametros);
            List<Beneficiario> listaBeneficiarios = Converter(ds);

            return listaBeneficiarios;
        }

        /// <summary>
        /// Altera um Beneficiário
        /// </summary>
        /// <param name="beneficiario">Objeto de beneficiário</param>
        internal void Alterar(Beneficiario beneficiario)
        {

            List<SqlParameter> parametros = new List<SqlParameter>
            {
                new SqlParameter("Nome", beneficiario.Nome),
                new SqlParameter("ID", beneficiario.Id),
                new SqlParameter("CPF", beneficiario.CPF)
            };

            base.Executar("FI_SP_AltBenef", parametros);
        }


        /// <summary>
        /// Excluir Beneficiario
        /// </summary>
        /// <param name="id">Id do Beneficiario</param>
        internal void Excluir(long id)
        {
            List<SqlParameter> parametros = new List<SqlParameter>
            {
                new SqlParameter("Id", id)
            };

            base.Executar("FI_SP_DelBeneficiario", parametros);
        }

        private List<Beneficiario> Converter(DataSet ds)
        {
            List<Beneficiario> lista = new List<Beneficiario>();

            if (ds != null && ds.Tables != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow row in ds.Tables[0].Rows)
                {
                    Beneficiario beneficiario = new Beneficiario
                    {
                        Id = row.Field<long>("Id"),
                        CPF = row.Field<string>("CPF"),
                        Nome = row.Field<string>("Nome"),
                        IdCliente = row.Field<long>("IdCliente")
                    };
                    lista.Add(beneficiario);
                }
            }

            return lista;
        }
    }
}
