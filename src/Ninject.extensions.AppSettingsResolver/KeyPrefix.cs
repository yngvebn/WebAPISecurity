using System;

namespace Ninject.extensions.DictionaryAdapter
{
    public class KeyPrefix : Attribute
    {
        public string Prefix { get; set; }
    }
}