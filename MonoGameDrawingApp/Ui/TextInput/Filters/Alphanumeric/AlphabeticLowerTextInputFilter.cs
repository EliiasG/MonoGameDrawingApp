using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonoGameDrawingApp.Ui.TextInput.Filters.Base
{
    public class AlphabeticLowerTextInputFilter : ITextInputFilter
    {
        public ISet<char> AllowedCharacters => "abcdefghijklmnopqrstuvwxyz".ToHashSet();

        public ITextInputFilter[] SubFilters => null;
    }
}
