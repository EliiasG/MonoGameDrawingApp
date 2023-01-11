using System.Collections.Generic;

namespace MonoGameDrawingApp.Ui.TextInput.Filters.Base
{
    public interface ITextInputFilter
    {
        ISet<char> AllowedCharacters { get; }

        ITextInputFilter[] SubFilters { get; }
    }
}
