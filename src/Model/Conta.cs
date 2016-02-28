using SQLite.Net.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GerenciadorFinanceiroSimples.Model
{
    [Table("Conta")]
    public class Conta
    {
        [PrimaryKey]
        [AutoIncrement]
        public int ContaId { get; set; }
        public string NomeConta { get; set; }
        public DateTime DataCriacao { get; set; }
    }
}
