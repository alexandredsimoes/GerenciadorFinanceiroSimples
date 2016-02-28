using GalaSoft.MvvmLight.Ioc;
using GalaSoft.MvvmLight.Views;
using GerenciadorFinanceiroSimples.Model;
using GerenciadorFinanceiroSimples.Repositorio;
using GerenciadorFinanceiroSimples.Services;
using GerenciadorFinanceiroSimples.View;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GerenciadorFinanceiroSimples.ViewModel
{
    public class Localizador
    {
        public Localizador()
        {
            var navigationService = new NavigationService();
            navigationService.Configure(Constantes.PAGINA_CONTA, typeof(PaginaConta));
            navigationService.Configure(Constantes.PAGINA_CONTA_DETALHES, typeof(PaginaContaDetalhe));
            navigationService.Configure(Constantes.PAGINA_LANCAMENTO, typeof(PaginaLancamento));

            SimpleIoc.Default.Register<INavigationService>(() => navigationService);
            SimpleIoc.Default.Register<IServicoDados, ServicoDados>();
            SimpleIoc.Default.Register<ContaRepositorio>();
            SimpleIoc.Default.Register<PaginaPrincipalViewModel>();
            SimpleIoc.Default.Register<PaginaContaViewModel>();
            SimpleIoc.Default.Register<PaginaContaDetalheViewModel>();
            SimpleIoc.Default.Register<PaginaLancamentoViewModel>();
        }

        public PaginaPrincipalViewModel PaginaPrincipal
        {
            get
            {
                return SimpleIoc.Default.GetInstance<PaginaPrincipalViewModel>();
            }
        }
        public PaginaLancamentoViewModel PaginaLancamento
        {
            get
            {
                return SimpleIoc.Default.GetInstance<PaginaLancamentoViewModel>();
            }
        }

        public PaginaContaViewModel PaginaConta
        {
            get
            {
                return SimpleIoc.Default.GetInstance<PaginaContaViewModel>();
            }
        }

        public PaginaContaDetalheViewModel PaginaContaDetalhe
        {
            get
            {
                return SimpleIoc.Default.GetInstance<PaginaContaDetalheViewModel>();
            }
        }
    }
}
