using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonoGameDrawingApp.Ui.TextInput.Filters.Base
{
    public interface ITextInputFilter
    {
        ISet<char> AllowedCharacters { get; }

        ITextInputFilter[] SubFilters { get; }
    }
}
