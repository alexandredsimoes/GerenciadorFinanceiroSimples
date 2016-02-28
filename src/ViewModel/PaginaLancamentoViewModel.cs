using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Views;
using GerenciadorFinanceiroSimples.Model;
using GerenciadorFinanceiroSimples.Repositorio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Popups;

namespace GerenciadorFinanceiroSimples.ViewModel
{
    public class PaginaLancamentoViewModel : BaseViewModel
    {
        private readonly ContaRepositorio _contaRepositorio;
        private readonly INavigationService _navigationService;
        public PaginaLancamentoViewModel(ContaRepositorio contaRepositorio, INavigationService navigationService)
        {            
            _contaRepositorio = contaRepositorio;
            _navigationService = navigationService;

            PageLoad = new RelayCommand(PageLoadExecute);
            Salvar = new RelayCommand(SalvarExecute);
            Remover = new RelayCommand(RemoverExecute);
        }

        private async void RemoverExecute()
        {
            var dialog = new MessageDialog("Deseja remover esse lançamento?", "Remover lançamento");

            var cmdSim = new UICommand("Sim", async(o) =>
            {
                if (await _contaRepositorio.RemoverLancamento(Detalhe))
                    _navigationService.GoBack();
            });
            var cmdNao = new UICommand("Não", (o) => { });
            dialog.Commands.Add(cmdSim);
            dialog.Commands.Add(cmdNao);
            await dialog.ShowAsync();
        }

        private async  void SalvarExecute()
        {
            if (!(await Validar())) return;

            Detalhe.Tipo = TipoSelecionado.Id == 1 ? "+" : "-";
            Detalhe.CategoriaId = CategoriaSelecionada.CategoriaId;
            Detalhe.ContaId = ContaSelecionada.ContaId;
            await _contaRepositorio.SalvarLancamento(Detalhe);

            _navigationService.GoBack();
        }

        private async Task<bool> Validar()
        {
            if (ContaSelecionada == null)
            {
                await new MessageDialog("Informe a conta", "Gerenciador financeiro").ShowAsync();
                return false;
            }

            if (TipoSelecionado == null)
            {
                await new MessageDialog("Informe o tipo de lançamento", "Gerenciador financeiro").ShowAsync();
                return false;
            }

            if (CategoriaSelecionada == null)
            {
                await new MessageDialog("Informe a categoria de lançamento", "Gerenciador financeiro").ShowAsync();
                return false;
            }

            if (Detalhe.Valor <= 0)
            {
                await new MessageDialog("Informe o valor do lançamento", "Gerenciador financeiro").ShowAsync();
                return false;
            }

            return true;
        }

        private async void PageLoadExecute()
        {
            var lista = new List<ListaSelecao>();
            lista.Add(new ListaSelecao() { Id = 1, Nome = "Receita" });
            lista.Add(new ListaSelecao() { Id = 2, Nome = "Despesa" });
            Tipos = lista;

            await CarregarDadosAuxiliares();

            if(Parametro != null)
            {
                var id = int.Parse(Parametro.ToString());

                Detalhe = await _contaRepositorio.ObterDetalhesLancamento(id);
                ContaSelecionada = Contas.FirstOrDefault(c => c.ContaId == Detalhe.ContaId);
                CategoriaSelecionada = Categorias.FirstOrDefault(c => c.CategoriaId == Detalhe.CategoriaId);
                TipoSelecionado = Tipos.FirstOrDefault(c => c.Id == (Detalhe.Tipo == "+" ? 1 : 2));
            }
            else
            {
                Detalhe = new Lancamento() { DataLancamento = DateTime.Now, DataVencimento = DateTime.Now };
            }
        }

        private async Task CarregarDadosAuxiliares()
        {
            Categorias = await _contaRepositorio.ListarCategorias("-");
            Contas = await _contaRepositorio.ListarContas();
        }

        private IReadOnlyCollection<Categoria> _Categorias;
        public IReadOnlyCollection<Categoria> Categorias {
            get
            {
                return _Categorias;
            }
            set
            {
                Set(()=>Categorias, ref _Categorias,value);
            }
        }

        private Categoria _CategoriaSelecionada;
        public Categoria CategoriaSelecionada
        {
            get { return _CategoriaSelecionada; }
            set { Set(() => CategoriaSelecionada, ref _CategoriaSelecionada, value); }
        }

        private IReadOnlyCollection<ListaSelecao> _Tipos;
        public IReadOnlyCollection<ListaSelecao> Tipos
        {
            get
            {
                return _Tipos;
            }
            set
            {
                Set(() => Tipos, ref _Tipos, value);
            }
        }

        private ListaSelecao _TipoSelecionado;
        public ListaSelecao TipoSelecionado
        {
            get { return _TipoSelecionado; }
            set { Set(() => TipoSelecionado, ref _TipoSelecionado, value); }
        }

        private Lancamento _Detalhe;
        public Lancamento Detalhe
        {
            get { return _Detalhe; }
            set { Set(() => Detalhe, ref _Detalhe, value); }
        }

        private IReadOnlyCollection<Conta> _Contas;
        public IReadOnlyCollection<Conta> Contas
        {
            get
            {
                return _Contas;
            }
            set
            {
                Set(() => Contas, ref _Contas, value);
            }
        }

        private Conta _ContaSelecionada;
        public Conta ContaSelecionada
        {
            get { return _ContaSelecionada; }
            set { Set(() => ContaSelecionada, ref _ContaSelecionada, value); }
        }

        public RelayCommand PageLoad { get; private set; }
        public RelayCommand Salvar { get; private set; }
        public RelayCommand Remover { get; private set; }
    }
}
