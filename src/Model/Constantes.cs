using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;

namespace GerenciadorFinanceiroSimples.Model
{
    public static class Constantes
    {
        public static string DatabasePath = Path.Combine(new[] { ApplicationData.Current.LocalFolder.Path, "database", "gerenciadorfinanceiro.sqlite" });
        public static string PAGINA_CONTA = "PaginaConta";
        public static string PAGINA_CONTA_DETALHES = "PaginaContaDetalhes";
        public static string PAGINA_LANCAMENTO = "PaginaLancamento";
        
    }
}
