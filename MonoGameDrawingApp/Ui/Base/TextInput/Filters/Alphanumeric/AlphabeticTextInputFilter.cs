using MonoGameDrawingApp.Ui.Base.TextInput.Filters;
using System.Collections.Generic;

namespace MonoGameDrawingApp.Ui.TextInput.Filters.Alphanumeric
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
