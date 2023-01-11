using System.Collections.Generic;

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
