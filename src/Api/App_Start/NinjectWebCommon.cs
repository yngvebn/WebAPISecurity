using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics.Contracts;
using System.Reflection;
using System.Web.Http;
using System.Web.Http.Dependencies;
using Api.Api;
using Api.Infrastructure.Security;
using Ninject.Extensions.Factory.Factory;
using Ninject.Modules;
using Ninject.Syntax;
using RsaHelpers;

[assembly: WebActivator.PreApplicationStartMethod(typeof(Api.App_Start.NinjectWebCommon), "Start")]
[assembly: WebActivator.ApplicationShutdownMethodAttribute(typeof(Api.App_Start.NinjectWebCommon), "Stop")]

namespace Api.App_Start
{
    using System;
    using System.Web;

    using Microsoft.Web.Infrastructure.DynamicModuleHelper;

    using Ninject;
    using Ninject.Web.Common;
    using Ninject.Extensions.Factory;
    using Ninject.extensions.DictionaryAdapter;

    public static class NinjectWebCommon
    {
        private static readonly Bootstrapper bootstrapper = new Bootstrapper();

        /// <summary>
        /// Starts the application
        /// </summary>
        public static void Start()
        {
            DynamicModuleUtility.RegisterModule(typeof(OnePerRequestHttpModule));
            DynamicModuleUtility.RegisterModule(typeof(NinjectHttpModule));
            bootstrapper.Initialize(CreateKernel);
        }

        /// <summary>
        /// Stops the application.
        /// </summary>
        public static void Stop()
        {
            bootstrapper.ShutDown();
        }

        /// <summary>
        /// Creates the kernel that will manage your application.
        /// </summary>
        /// <returns>The created kernel.</returns>
        private static IKernel CreateKernel()
        {
            var kernel = new StandardKernel();
            kernel.Bind<Func<IKernel>>().ToMethod(ctx => () => new Bootstrapper().Kernel);
            GlobalConfiguration.Configuration.DependencyResolver = new NinjectDependencyResolver(kernel);
            kernel.Bind<IHttpModule>().To<HttpApplicationInitializationHttpModule>();

            RegisterServices(kernel);
            return kernel;
        }

        /// <summary>
        /// Load your modules or register your services here!
        /// </summary>
        /// <param name="kernel">The kernel.</param>
        private static void RegisterServices(IKernel kernel)
        {

            kernel.Bind<IRsaKeys>().To<HardCodedRsaKeys>();
            kernel.Bind<IRsa>().To<Rsa>();
            kernel.Bind<ISessionRepository>().To<FakeSessionRepository>();
            GlobalConfiguration.Configuration.Filters
                    .Add(new AuthorizationTokenFilter(kernel.Get<IRsa>()));

        }
    }

    internal class FakeSessionRepository: ISessionRepository
    {
    }

    internal class HardCodedRsaKeys : IRsaKeys
    {
        public string PublicKey
        {
            get
            {
                return
                    "<RSAKeyValue><Modulus>1TGTGmcJRlLCRXo9M38P9LDWIkBXEbKHefxQru13votIB0ODnlKPK2GOpEfJcr66pnjRdON/xcvEojKLjOFC1l1lCzrwTnCD8//aY8YmTgoxT6K4ytJrlUuEusnMzPJuZviYUSl2Takbe0vKilaztJi0nj8HlJFDC8Vu/JF/S0k=</Modulus><Exponent>AQAB</Exponent></RSAKeyValue>";
            }
        }
        public string PrivateKey
        {
            get
            {
                return
                    "<RSAKeyValue><Modulus>1TGTGmcJRlLCRXo9M38P9LDWIkBXEbKHefxQru13votIB0ODnlKPK2GOpEfJcr66pnjRdON/xcvEojKLjOFC1l1lCzrwTnCD8//aY8YmTgoxT6K4ytJrlUuEusnMzPJuZviYUSl2Takbe0vKilaztJi0nj8HlJFDC8Vu/JF/S0k=</Modulus><Exponent>AQAB</Exponent><P>7WcuuUMEOOB6AI9POJQCHTdqj0pvuMlI1BI/N7DSCCO0fJdevJrBfUtfbSxWQELsGL5y3EzcKoeop3YugbCg1Q==</P><Q>5eTnNMvFsPxqW+FrlB6vcDFZ9xPIkh2sPgJNZjiQKh6H7E/8Az2e/CMWdwigHA8qzNvaEPurhKEC2q6+ugoapQ==</Q><DP>kUurTvtzJBRO1vTeuXPsb1ExSI14HxIiHpkkU8NGaHDhz7cc5jWY4kQ1HS4bg6zxrpsw1R+9R9JLKGKuR/WAGQ==</DP><DQ>OsN4FhbAQa1DwpisVwBA9/ylcnKsIi1TicYs4qQytZF4TP9k+68UpH6Tj3m083ctCZBo/U5XWV+Oyzc/qW5LwQ==</DQ><InverseQ>rg/rBMmudXUIBOyN+vd26HFhuNXwMk0WxLsW/cDElufRcN+ieNiqwy3V5g5Kak4SoiHCFr4+pONyRc+AbYfJgw==</InverseQ><D>W4se/E5UCDNPIiA8GVmtE0e/mxN/j6TWUYYLayGisloCQsQ1xwzyVxFb+6Srlq7ZXNQyNHvfiKJXu8HydDrhxHvL/cDWcV6Ua72iiCEdCIv4XOt+5L52L4qaZekIjPUZwejTduuhGZDIPbLd4p23vsyCN3rfi3unEcCzE2Sjs5E=</D></RSAKeyValue>";
            }
        }
    }

    public class NinjectDependencyResolver : NinjectDependencyScope, IDependencyResolver
    {
        private IKernel kernel;

        public NinjectDependencyResolver(IKernel kernel)
            : base(kernel)
        {
            this.kernel = kernel;
        }

        public IDependencyScope BeginScope()
        {
            return new NinjectDependencyScope(kernel.BeginBlock());
        }
    }
    public class NinjectDependencyScope : IDependencyScope
    {
        private IResolutionRoot resolver;

        internal NinjectDependencyScope(IResolutionRoot resolver)
        {
            Contract.Assert(resolver != null);

            this.resolver = resolver;
        }

        public void Dispose()
        {
        }

        public object GetService(Type serviceType)
        {
            if (resolver == null)
                throw new ObjectDisposedException("this", "This scope has already been disposed");

            return resolver.TryGet(serviceType);
        }

        public IEnumerable<object> GetServices(Type serviceType)
        {
            if (resolver == null)
                throw new ObjectDisposedException("this", "This scope has already been disposed");

            return resolver.GetAll(serviceType);
        }
    }
}
