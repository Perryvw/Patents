using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Patents
{
    class ComboboxItem
    {
        public string Text { get; set; }
        public Func<List<String>> Value { get; set; }

        public ComboboxItem(string text, Func<List<String>> value)
        {
            this.Text = text;
            this.Value = value;
        }

        public override string ToString()
        {
            return Text;
        }
    }
}
