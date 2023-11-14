using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DictionaryCreator
{
    public class WordDefinition
    {
        public string Word { get; set; }
        public string Description { get; set; }
        public string IPA { get; set; }
        public string Example { get; set; }

        public bool WordEquals(WordDefinition other)
        {
            if (Word == other.Word &&
                Description == other.Description &&
                IPA == other.IPA &&
                Example == other.Example)
                return true;
            return false;
        }
    }
}
