using MonoGameDrawingApp.Ui.TextInput.Filters.Base;
using System.Collections.Generic;
using System.Linq;

namespace MonoGameDrawingApp.Ui.TextInput.Filters
{
    internal class FileSystemTextInputFilter : ITextInputFilter
    {
        private ISet<char> _allowedCharacters = " _-.,;+¤&()[]".ToHashSet();

        private readonly ITextInputFilter[] _subFilters = new ITextInputFilter[]
        {
            new AlphanumericTextInputFilter(),
        };

        public ISet<char> AllowedCharacters => _allowedCharacters;

        public ITextInputFilter[] SubFilters => _subFilters;
    }
}
