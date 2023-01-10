using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonoGameDrawingApp.Ui.TextInput.Filters.Base
{
    public class AlphabeticUpperTextInputFilter : ITextInputFilter
    {
        public ISet<char> AllowedCharacters => "ABCDEFGHIJKLMNOPQRSTUVWXYZ".ToHashSet();

        public ITextInputFilter[] SubFilters => null;
    }
}
