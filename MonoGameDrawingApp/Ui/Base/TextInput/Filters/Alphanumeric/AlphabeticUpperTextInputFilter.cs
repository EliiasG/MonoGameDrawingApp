using System.Collections.Generic;
using System.Linq;

namespace MonoGameDrawingApp.Ui.Base.TextInput.Filters.Alphanumeric
{
    public class AlphabeticUpperTextInputFilter : ITextInputFilter
    {
        public ISet<char> AllowedCharacters { get; } = "ABCDEFGHIJKLMNOPQRSTUVWXYZ".ToHashSet();

        public ITextInputFilter[] SubFilters => null;
    }
}
