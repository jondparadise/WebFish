using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace WebFish.DesktopWPF.ViewModels
{
    public class BoardViewModel : INotifyPropertyChanged
    {
        
        
        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string callerName = null)
        {
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(callerName));
        }
    }
}
