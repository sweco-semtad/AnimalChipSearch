using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnimalChipSearch.ViewModels
{
    public class EmptyControlViewModel : ViewModelBase
    {
        private String _text;
        public String Text
        {
            get { return _text; }
            set
            {
                _text = value;
                RaisePropertyChanged("Text");
            }
        }
    }
}
