using SQLite.Net.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GerenciadorFinanceiroSimples.Model
{
    [Table("Configuracao")]
    public class Configuracao
    {

        public Configuracao()
        {
        }

        [PrimaryKey]
        public string Chave { get; set; }
        public string Valor { get; set; }
    }
}
