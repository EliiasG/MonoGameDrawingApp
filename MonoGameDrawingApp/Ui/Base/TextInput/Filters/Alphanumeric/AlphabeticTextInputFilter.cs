using MonoGameDrawingApp.Ui.Base.TextInput.Filters.Alphanumeric;
using System.Collections.Generic;

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
