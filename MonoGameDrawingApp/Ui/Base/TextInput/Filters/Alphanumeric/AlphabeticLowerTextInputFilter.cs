using System.Collections.Generic;
using System.Linq;

namespace MonoGameDrawingApp.Ui.Base.TextInput.Filters.Alphanumeric
{
    public class AlphabeticLowerTextInputFilter : ITextInputFilter
    {
        public ISet<char> AllowedCharacters { get; } = "abcdefghijklmnopqrstuvwxyz".ToHashSet();

        public ITextInputFilter[] SubFilters => null;
    }
}
