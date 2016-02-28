using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SQLite.Net.Async;
using SQLite.Net;
using GerenciadorFinanceiroSimples.Model;
using SQLite.Net.Platform.WinRT;
using Windows.Storage;
using System.IO;

namespace GerenciadorFinanceiroSimples.Services
{
    public class ServicoDados : IServicoDados
    {
        private SQLiteAsyncConnection _conexao;

        public ServicoDados()
        {
            string databasePath = Constantes.DatabasePath;
            var connectionFactory = new Func<SQLiteConnectionWithLock>(() => new SQLiteConnectionWithLock(new SQLitePlatformWinRT(), new SQLiteConnectionString(databasePath, storeDateTimeAsTicks: false)));
            _conexao = new SQLiteAsyncConnection(connectionFactory);
        }

        public SQLiteAsyncConnection Contexto
        {
            get
            {
                return _conexao;
            }
        }

        public async Task InicializarBaseDeDados(int versao)
        {
            bool existe = false;
            try
            {
                var dbFile = await StorageFile.GetFileFromPathAsync(Constantes.DatabasePath);
                existe = true;
            }
            catch (FileNotFoundException)
            {
                existe = false;
            }

            if (!existe)
            {
                await ApplicationData.Current.LocalFolder.CreateFolderAsync("database", CreationCollisionOption.ReplaceExisting);                

                await _conexao.CreateTableAsync<Configuracao>();
                await _conexao.CreateTableAsync<Categoria>();
                await _conexao.CreateTableAsync<Conta>();
                await _conexao.CreateTableAsync<Lancamento>();


                //Insere algumas categorias de despesas
                await _conexao.InsertAsync(new Categoria() { Nome = "alimentacao", Tipo = "-" });
                await _conexao.InsertAsync(new Categoria() { Nome = "pagamentos", Tipo = "-" });
                await _conexao.InsertAsync(new Categoria() { Nome = "diversão", Tipo = "-" });
                await _conexao.InsertAsync(new Categoria() { Nome = "carro", Tipo = "-" });
                await _conexao.InsertAsync(new Categoria() { Nome = "saúde", Tipo = "-" });

                //Insere algumas categorias de receitas
                await _conexao.InsertAsync(new Categoria() { Nome = "salário", Tipo = "-" });
                await _conexao.InsertAsync(new Categoria() { Nome = "adiantamento", Tipo = "-" });
                await _conexao.InsertAsync(new Categoria() { Nome = "outros", Tipo = "-" });

                //Cria duas contas
                await _conexao.InsertAsync(new Conta() { DataCriacao = DateTime.Now, NomeConta = "Conta corrente" });
                await _conexao.InsertAsync(new Conta() { DataCriacao = DateTime.Now, NomeConta = "Cartão de crédito" });

                //Lanca algumas despesas
                await _conexao.InsertAsync(new Lancamento() { ContaId = 1, DataLancamento = DateTime.Now, Descricao = "Balada", Tipo = "-", Valor = 150, CategoriaId = 3 });
                await _conexao.InsertAsync(new Lancamento() { ContaId = 1, DataLancamento = DateTime.Now.AddDays(-3), Descricao = "Internet", Tipo = "-", Valor = 50, CategoriaId = 2 });

                //Lanca uma receita
                await _conexao.InsertAsync(new Lancamento() { ContaId = 1, DataLancamento = DateTime.Now, Descricao = "Salário do mês", Tipo = "+", Valor = 10000, CategoriaId = 6 });

                await _conexao.InsertAsync(new Lancamento() { ContaId = 2, DataLancamento = DateTime.Now, Descricao = "Compras", Tipo = "-", Valor = 300, CategoriaId = 2 });

                //Cria o valor da versão
                await _conexao.InsertAsync(new Configuracao() { Chave = "versao", Valor = "1" });
            }
            else
            {
                //Aqui é feito a atualização do banco de dados (alteração de estrutura)
                //SQLiteAsyncConnection db = new SQLiteAsyncConnection(Constantes.DatabasePath, SQLiteOpenFlags.Create | SQLiteOpenFlags.ReadWrite, false);

                //Prepara para o processo de atualização
                //var versaoDB = 0;
                //var config = await _conexao.Table<Configuracao>().Where(c => c.Nome == "versao").FirstOrDefaultAsync();
                //if (config == null)
                //{
                //    await _conexao.InsertAsync(new Configuracao() { Nome = "versao", Valor = "1", Sincronizado = false });
                //}
                //else
                //    versaoDB = int.Parse(config.Valor);

                //var updateCommand = new SQLiteConnection(Constantes.DatabasePath, SQLiteOpenFlags.ReadWrite).CreateCommand("", new object[] { });

                //if(versaoDB == 0) //Não existia versão
                //{
                //    updateCommand.CommandText = @"alter table Lancamento Add `NumeroDocumento`	TEXT";
                //    updateCommand.ExecuteNonQuery();
                //}
            }
        }
    }
}
