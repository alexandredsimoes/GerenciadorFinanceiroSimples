using SQLite.Net.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GerenciadorFinanceiroSimples.Model
{
    public class LancamentoView : Lancamento
    {        
        public string NomeCategoria { get; set; }
    }
}
