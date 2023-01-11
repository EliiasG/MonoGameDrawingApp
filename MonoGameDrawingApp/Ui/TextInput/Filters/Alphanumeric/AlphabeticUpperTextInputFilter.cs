﻿using System.Collections.Generic;
using System.Linq;

namespace MonoGameDrawingApp.Ui.TextInput.Filters.Base
{
    public class AlphabeticUpperTextInputFilter : ITextInputFilter
    {
        private ISet<char> _allowedCharacters = "ABCDEFGHIJKLMNOPQRSTUVWXYZ".ToHashSet();

        public ISet<char> AllowedCharacters => _allowedCharacters;

        public ITextInputFilter[] SubFilters => null;
    }
}