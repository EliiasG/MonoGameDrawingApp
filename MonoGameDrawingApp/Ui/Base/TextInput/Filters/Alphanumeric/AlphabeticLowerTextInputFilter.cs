using MonoGameDrawingApp.Ui.Base.TextInput.Filters;
using System.Collections.Generic;
using System.Linq;

namespace MonoGameDrawingApp.Ui.TextInput.Filters.Alphanumeric
{
    public class AlphabeticLowerTextInputFilter : ITextInputFilter
    {
        private ISet<char> _allowedCharacters = "abcdefghijklmnopqrstuvwxyz".ToHashSet();

        public ISet<char> AllowedCharacters => _allowedCharacters;

        public ITextInputFilter[] SubFilters => null;
    }
}
