using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonoGameDrawingApp.Ui.TextInput.Filters.Base
{
    public class NumericTextInputFilter : ITextInputFilter
    {
        public ISet<char> AllowedCharacters => "0123456789".ToHashSet();

        public ITextInputFilter[] SubFilters => null;
    }
}
