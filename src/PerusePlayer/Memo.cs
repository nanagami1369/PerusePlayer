using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PerusePlayer
{
    internal class Memo : ViewBase
    {
        public TimeSpan Time { get; }

        private string _value;
        public string Value
        {
            get => _value;
            set => SetProperty(ref _value, value);
        }


        public Memo(TimeSpan time)
        {
            Time = time;
            _value = "";
        }

    }
}
