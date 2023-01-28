using System.Collections.Generic;

namespace MonoGameDrawingApp.Ui.Base.TextInput.Filters.Alphanumeric
{
    public interface ITextInputFilter
    {
        ISet<char> AllowedCharacters { get; }

        ITextInputFilter[] SubFilters { get; }
    }
}
