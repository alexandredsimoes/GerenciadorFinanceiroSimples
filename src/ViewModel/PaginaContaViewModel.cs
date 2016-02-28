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
    public class PaginaContaViewModel : BaseViewModel
    {
        private readonly ContaRepositorio _contaRepositorio;
        private readonly INavigationService _navigationService;
        public PaginaContaViewModel(ContaRepositorio contaRepositorio, INavigationService navigationService)
        {
            Detalhe = new Conta() { DataCriacao = DateTime.Now };

            _contaRepositorio = contaRepositorio;
            _navigationService = navigationService;

            PageLoad = new RelayCommand(PageLoadExecute);
            Salvar = new RelayCommand(SalvarExecute);
            Remover = new RelayCommand(RemoverExecute);
        }

        private async void RemoverExecute()
        {
            if (Detalhe == null) return;

            if (await _contaRepositorio.Remover(Detalhe))
            {
                _navigationService.GoBack();
            }
        }

        private async void SalvarExecute()
        {
            if (Detalhe == null) return;

            if (Detalhe.ContaId > 0)
                await _contaRepositorio.AlteraConta(Detalhe);
            else
                await _contaRepositorio.CriarConta(Detalhe);

            _navigationService.GoBack();
        }

        private async void PageLoadExecute()
        {
            var parametro = Parametro;

            if (parametro != null)
                Detalhe = await _contaRepositorio.ObterDetalhes((int)parametro);
        }



        public RelayCommand PageLoad { get; private set; }
        public RelayCommand Salvar { get; private set; }
        public RelayCommand Remover { get; private set; }

        private Conta _Detalhe;
        public Conta Detalhe
        {
            get { return _Detalhe; }
            set { Set(() => Detalhe, ref _Detalhe, value); }
        }

    }
}
