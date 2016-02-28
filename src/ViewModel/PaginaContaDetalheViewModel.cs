using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Views;
using GerenciadorFinanceiroSimples.Model;
using GerenciadorFinanceiroSimples.Repositorio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GerenciadorFinanceiroSimples.ViewModel
{
    public class PaginaContaDetalheViewModel : BaseViewModel
    {
        private readonly ContaRepositorio _contaRepositorio;
        private readonly INavigationService _navigationService;
        public PaginaContaDetalheViewModel(ContaRepositorio contaRepositorio, INavigationService navigationService)
        {
            

            _contaRepositorio = contaRepositorio;
            _navigationService = navigationService;

            PageLoad = new RelayCommand(PageLoadExecute);
            AlterarConta = new RelayCommand(AlterarContaExecute);
            NovoLancamento = new RelayCommand(() => { _navigationService.NavigateTo(Constantes.PAGINA_LANCAMENTO, null); });
            SelecionarItem = new RelayCommand<object>(SelecionarItemExecute);
        }

        private void SelecionarItemExecute(object obj)
        {
            var item = obj as Lancamento;

            if (item == null) return;

            _navigationService.NavigateTo(Constantes.PAGINA_LANCAMENTO, item.LancamentoId);
        }

        private void AlterarContaExecute()
        {
            _navigationService.NavigateTo(Constantes.PAGINA_CONTA, Parametro);
        }

        private async void PageLoadExecute()
        {
            var id = int.Parse(Parametro.ToString());
            Lancamentos = await _contaRepositorio.ListarLancamentos(id);
        }

        public RelayCommand AlterarConta { get; set; }
        public RelayCommand PageLoad { get; set; }
        public RelayCommand NovoLancamento { get; set; }
        public RelayCommand<object> SelecionarItem { get; set; }


        private IReadOnlyList<Lancamento> _Lancamentos;

        public IReadOnlyList<Lancamento> Lancamentos
        {
            get { return _Lancamentos; }
            set { Set(() => Lancamentos, ref _Lancamentos, value); }
        }


    }
}
