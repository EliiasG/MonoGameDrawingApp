using System.Collections.Generic;

namespace MonoGameDrawingApp.Ui.Base.TextInput.Filters.Alphanumeric
{
    public class AlphabeticTextInputFilter : ITextInputFilter
    {
        public ISet<char> AllowedCharacters => null;

        public ITextInputFilter[] SubFilters { get; } = new ITextInputFilter[]
        {
            new AlphabeticLowerTextInputFilter(),
            new AlphabeticUpperTextInputFilter(),
        };
    }
}
