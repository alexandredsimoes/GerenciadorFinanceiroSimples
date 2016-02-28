using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

namespace GerenciadorFinanceiroSimples.ViewModel
{
    public class BaseViewModel : ViewModelBase
    {
        public BaseViewModel()
        {
            Voltar = new RelayCommand(VoltarExecute);
        }

        private void VoltarExecute()
        {
            var frame = Window.Current.Content as Frame;
            if (frame != null)
            {
                if (frame.CanGoBack)
                    frame.GoBack();
            }
        }

        private object _Parametro;

        public object Parametro
        {
            get { return _Parametro; }
            set { Set(() => Parametro, ref _Parametro, value); }
        }


        private NavigationMode _ModoNavegacao;

        public NavigationMode ModoNavegacao
        {
            get { return _ModoNavegacao; }
            set { Set(() => ModoNavegacao, ref _ModoNavegacao, value); }
        }

        public RelayCommand Voltar { get; private set; }
    }
}
