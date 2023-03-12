using MonoGameDrawingApp.Ui.Base.TextInput.Filters.Alphanumeric;
using System.Collections.Generic;
using System.Linq;

namespace MonoGameDrawingApp.Ui.Base.TextInput.Filters.Types
{
    internal class FloatTextInputFilter : ITextInputFilter
    {
        public ISet<char> AllowedCharacters { get; } = ".-/".ToHashSet();

        public ITextInputFilter[] SubFilters { get; } = new ITextInputFilter[] { new NumericTextInputFilter() };
    }
}
