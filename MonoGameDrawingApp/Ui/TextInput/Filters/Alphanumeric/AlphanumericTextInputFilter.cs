using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonoGameDrawingApp.Ui.TextInput.Filters.Base
{
    public class AlphanumericTextInputFilter : ITextInputFilter
    {
        private ITextInputFilter[] _subFilters = new ITextInputFilter[]
        {
            new AlphabeticTextInputFilter(),
            new NumericTextInputFilter(),
        };

        public ISet<char> AllowedCharacters => null;

        public ITextInputFilter[] SubFilters => _subFilters;
    }
}
