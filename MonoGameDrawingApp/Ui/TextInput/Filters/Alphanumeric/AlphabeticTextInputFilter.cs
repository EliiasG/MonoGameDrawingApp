using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonoGameDrawingApp.Ui.TextInput.Filters.Base
{
    public class AlphabeticTextInputFilter : ITextInputFilter
    {
        private readonly ITextInputFilter[] _subFilters = new ITextInputFilter[]
        {
            new AlphabeticLowerTextInputFilter(),
            new AlphabeticUpperTextInputFilter(),
        };
        public ISet<char> AllowedCharacters => null;

        public ITextInputFilter[] SubFilters => _subFilters;
    }
}
