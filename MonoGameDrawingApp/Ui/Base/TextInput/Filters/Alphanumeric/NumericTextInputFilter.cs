using MonoGameDrawingApp.Ui.Base.TextInput.Filters;
using System.Collections.Generic;
using System.Linq;

namespace MonoGameDrawingApp.Ui.Base.TextInput.Filters.Alphanumeric
{
    public class NumericTextInputFilter : ITextInputFilter
    {
        private readonly ISet<char> _allowedCharacters = "0123456789".ToHashSet();

        public ISet<char> AllowedCharacters => _allowedCharacters;

        public ITextInputFilter[] SubFilters => null;
    }
}
