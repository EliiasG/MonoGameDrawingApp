using MonoGameDrawingApp.Ui.Base.TextInput.Filters;
using MonoGameDrawingApp.Ui.Base.TextInput.Filters.Alphanumeric;
using System.Collections.Generic;
using System.Linq;

namespace MonoGameDrawingApp.Ui.FileSystemTrees
{
    internal class FileSystemTextInputFilter : ITextInputFilter
    {
        public ISet<char> AllowedCharacters { get; } = " _-.,;+¤&()[]".ToHashSet();

        public ITextInputFilter[] SubFilters { get; } = new ITextInputFilter[]
        {
            new AlphanumericTextInputFilter(),
        };
    }
}
