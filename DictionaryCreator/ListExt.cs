using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DictionaryCreator
{
    public static class ListExt
    {
        public static List<WordDefinition> ReplaceAllWords<T>(this List<WordDefinition> source, WordDefinition oldValue, WordDefinition newValue)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));

            List<WordDefinition> result = new List<WordDefinition>();
            foreach (WordDefinition item in source)
            {
                if (item.WordEquals(oldValue))
                    result.Add(newValue);
                else
                    result.Add(item);
            }
            return result;
        }
    }
}
