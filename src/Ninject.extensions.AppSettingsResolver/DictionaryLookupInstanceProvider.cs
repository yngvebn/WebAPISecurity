using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Moq;
using Ninject.Extensions.Factory;
using Ninject.Extensions.Factory.Factory;

namespace Ninject.extensions.DictionaryAdapter
{
    internal class DictionaryLookupInstanceProvider<T> : StandardInstanceProvider where T : class
    {
        private readonly NameValueCollection _dictionary;
        private T internalProxy;
        private Dictionary<string, string> _values;  

        public DictionaryLookupInstanceProvider(NameValueCollection dictionary)
        {
            _dictionary = dictionary;
            _values = new Dictionary<string, string>();
            SetupProxy();
        }

        public override object GetInstance(IInstanceResolver instanceResolver, MethodInfo methodInfo, object[] arguments)
        {
            return _values[methodInfo.Name];
        }

        private void SetupProxy()
        {
            var mock = new Mock<T>();
            mock.SetupAllProperties();
            var type = typeof (T);
            KeyPrefix prefixSetting = type.GetCustomAttribute<KeyPrefix>();
            
            foreach (var property in typeof(T).GetProperties())
            {
                _values.Add("get_"+property.Name, _dictionary[prefixSetting.Prefix+property.Name]);
            }

            internalProxy = mock.Object;
        }
    }
}
