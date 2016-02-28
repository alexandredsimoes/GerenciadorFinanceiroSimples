using GerenciadorFinanceiroSimples.Model;
using GerenciadorFinanceiroSimples.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GerenciadorFinanceiroSimples.Repositorio
{
    public class ContaRepositorio
    {
        private readonly IServicoDados _servicoDados;
        public ContaRepositorio(IServicoDados servicoDados)
        {
            _servicoDados = servicoDados;
        }

        public async Task<IReadOnlyList<ContaView>> ListarContas()
        {
            var sql = @"select ContaId, NomeConta, DataCriacao, 
                    ((select ifnull(sum(l.Valor),0) from Lancamento l where l.Tipo = '+' and l.ContaId = c.ContaId) -
                    (select ifnull(sum(l.Valor),0) from Lancamento l where l.Tipo = '-' and l.ContaId = c.ContaId )) Total                       
                     from Conta c";
            return await _servicoDados.Contexto.QueryAsync<ContaView>(sql, new object[] { });
        }

        public async Task<double> SomarContas()
        {
            var totalReceita = (await _servicoDados.Contexto.Table<Lancamento>()
                .Where(c => c.Tipo == "+")
                .ToListAsync())
                .Sum(c => c.Valor);

            var totalDespesa = (await _servicoDados.Contexto.Table<Lancamento>()
                .Where(c => c.Tipo == "-")
                .ToListAsync())
                .Sum(c => c.Valor);

            return totalReceita - totalDespesa;
        }

        internal async Task<bool> SalvarLancamento(Lancamento detalhe)
        {
            return await _servicoDados.Contexto.InsertOrReplaceAsync(detalhe) > 0;            
        }

        internal async Task<IReadOnlyCollection<Categoria>> ListarCategorias(string tipo)
        {
            return await _servicoDados.Contexto.Table<Categoria>().Where(c => c.Tipo == tipo).ToListAsync();             
        }

        public async Task<bool> CriarConta(Conta conta)
        {
            return await _servicoDados.Contexto.InsertAsync(conta) > 0;
        }
        public async Task<bool> AlteraConta(Conta conta)
        {
            return await _servicoDados.Contexto.UpdateAsync(conta) > 0;
        }

        public async Task<Conta> ObterDetalhes(int contaId)
        {
            return await _servicoDados.Contexto.Table<Conta>().Where(c => c.ContaId == contaId).FirstOrDefaultAsync();
        }

        public async Task<bool> Remover(Conta conta)
        {
            return await _servicoDados.Contexto.DeleteAsync(conta) > 0;
        }

        public async Task<bool> RemoverLancamento(Lancamento lancamento)
        {
            return await _servicoDados.Contexto.DeleteAsync(lancamento) > 0;
        }

        public async Task<IReadOnlyList<LancamentoView>> ListarLancamentos(int contaId)
        {
            var sql = @"
                    SELECT l.LancamentoId
                          ,l.ContaId
                          ,l.DataLancamento
                          ,l.DataVencimento
                          ,l.Valor
                          ,l.Tipo
                          ,l.Descricao
                          ,l.CategoriaId
                          ,c.Nome as NomeCategoria      
                      FROM Lancamento as l 
                    INNER JOIN Categoria as c on c.CategoriaId = l.CategoriaId 
                   WHERE l.ContaId = ?";

            var resultado = await _servicoDados.Contexto.QueryAsync<LancamentoView>(sql, new object[] { contaId });
            return resultado;
                        
        }

        public async Task<Categoria> ObterDetalhesCategoria(int categoriaId)
        {
            return await _servicoDados.Contexto.Table<Categoria>().Where(c => c.CategoriaId == categoriaId).FirstOrDefaultAsync();
        }

        public async Task<Lancamento> ObterDetalhesLancamento(int lancamentoId)
        {
            return await _servicoDados.Contexto.Table<Lancamento>().Where(c => c.LancamentoId == lancamentoId).FirstOrDefaultAsync();
        }
    }
}
