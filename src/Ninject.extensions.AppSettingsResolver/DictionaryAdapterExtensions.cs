using System;
using System.Collections.Specialized;
using Ninject.Extensions.Factory;

namespace Ninject.extensions.DictionaryAdapter
{
    public static class DictionaryAdapterExtensions
    {
        public static Ninject.Syntax.IBindingWhenInNamedWithOrOnSyntax<TInterface> ToDictionaryAdapter<TInterface>(
            this Ninject.Syntax.IBindingToSyntax<TInterface> bind, Func<NameValueCollection> dictionary) where TInterface: class
        {
            return bind.ToFactory(() => new DictionaryLookupInstanceProvider<TInterface>(dictionary()));
        }
    }
}