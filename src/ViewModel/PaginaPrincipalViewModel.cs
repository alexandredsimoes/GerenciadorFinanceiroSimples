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
    public class PaginaPrincipalViewModel : BaseViewModel
    {
        private readonly ContaRepositorio _contaRepositorio;
        private readonly INavigationService _navigationService;

        public PaginaPrincipalViewModel(ContaRepositorio contaRepositorio, INavigationService navigationService)
        {
            _contaRepositorio = contaRepositorio;
            _navigationService = navigationService;

            PageLoad = new RelayCommand(PageLoadExecute);
            SelecionarConta = new RelayCommand<object>(SelecionarContaExecute);
            CriarConta = new RelayCommand(CriarContaExecute);
        }

        private async void PageLoadExecute()
        {
            Contas = await _contaRepositorio.ListarContas();
            TotalGeral = await _contaRepositorio.SomarContas();
        }

        private void SelecionarContaExecute(object obj)
        {
            var conta = obj as Conta;

            if (conta != null)
            {
                _navigationService.NavigateTo(Constantes.PAGINA_CONTA_DETALHES, conta.ContaId);
            }
        }
        private void CriarContaExecute()
        {
            _navigationService.NavigateTo(Constantes.PAGINA_CONTA);
        }

        public RelayCommand PageLoad { get; private set; }
        public RelayCommand CriarConta { get; private set; }
        public RelayCommand<object> SelecionarConta { get; private set; }

        private IReadOnlyList<Conta> _Contas;

        public IReadOnlyList<Conta> Contas
        {
            get { return _Contas; }
            set { Set(() => Contas, ref _Contas, value); }
        }                          

        private double _TotalGeral;
        public double TotalGeral
        {
            get { return _TotalGeral; }
            set { Set(() => TotalGeral, ref _TotalGeral, value); }
        }

    }
}
