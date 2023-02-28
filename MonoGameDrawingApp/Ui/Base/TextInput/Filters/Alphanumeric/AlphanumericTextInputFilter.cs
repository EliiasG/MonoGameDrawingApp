using System.Collections.Generic;

namespace MonoGameDrawingApp.Ui.Base.TextInput.Filters.Alphanumeric
{
    public class AlphanumericTextInputFilter : ITextInputFilter
    {
        private readonly ITextInputFilter[] _subFilters = new ITextInputFilter[]
        {
            new AlphabeticTextInputFilter(),
            new NumericTextInputFilter(),
        };

        public ISet<char> AllowedCharacters => null;

        public ITextInputFilter[] SubFilters => _subFilters;
    }
}
