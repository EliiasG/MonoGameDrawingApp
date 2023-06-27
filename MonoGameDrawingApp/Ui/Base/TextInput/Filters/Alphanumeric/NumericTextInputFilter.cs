using System.Collections.Generic;
using System.Linq;

namespace MonoGameDrawingApp.Ui.Base.TextInput.Filters.Alphanumeric
{
    public class NumericTextInputFilter : ITextInputFilter
    {
        public ISet<char> AllowedCharacters { get; } = "0123456789".ToHashSet();

        public ITextInputFilter[] SubFilters => null;
    }
}
