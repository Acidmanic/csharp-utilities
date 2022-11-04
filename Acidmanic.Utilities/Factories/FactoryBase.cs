using System;
using System.Collections.Generic;
using System.Reflection;
using Acidmanic.Utilities.Results;

namespace Acidmanic.Utilities.Factories
{
    public abstract class FactoryBase<TProduct,TArgument>
    {

        private readonly Func<Type, object> _resolver;
        private readonly FactoryMatching _matching;
        private readonly List<Type> _implementationTypes;

        public FactoryBase(Func<Type, object> resolver, FactoryMatching matching)
        {
            _resolver = resolver;
            _matching = matching;
            _implementationTypes = new List<Type>();
            
            ScanAssembly(Assembly.GetCallingAssembly());
        }

        public FactoryBase(FactoryMatching matching):this(type => null, matching) { }

        public FactoryBase():this(type => null,FactoryMatching.MatchByInstance) { }
        

        public void ScanAssembly(Assembly assembly)
        {
            try
            {
                var types = assembly.GetTypes();

                var productType = typeof(TProduct);
                
                foreach (var type in types)
                {
                    if (type.IsAssignableFrom(productType))
                    {
                        if (!type.IsAbstract && !type.IsInterface)
                        {
                            _implementationTypes.Add(type);
                        }
                    }
                }
            }
            catch (Exception e)
            {
                
            }
        }
        
        public TProduct Make(TArgument value)
        {
            foreach (var type in _implementationTypes)
            {
                var matched = Matches(type, value);

                if (matched)
                {
                    return matched;
                }
            }

            return DefaultValue();
        }


        protected Result<TProduct> Matches(Type type, TArgument argument)
        {
            bool matched = false;
            
            if (_matching == FactoryMatching.MatchByType)
            {
                matched = MatchesByType(type, argument);

                if (!matched)
                {
                    return new Result<TProduct>().FailAndDefaultValue();
                }
            }

            var instance = Instantiate(type);

            if (instance == null)
            {
                return new Result<TProduct>().FailAndDefaultValue();
            }
            
            if (_matching == FactoryMatching.MatchByInstance)
            {
                matched = MatchesByInstance(instance, argument);
            }

            return new Result<TProduct>(matched, instance);
        }

        protected abstract bool MatchesByType(Type productType, TArgument value);

        protected abstract bool MatchesByInstance(TProduct product, TArgument value);


        protected virtual TProduct DefaultValue()
        {
            return default;
        }
        protected virtual TProduct Instantiate(Type type)
        {
            var instance = TryResolve(type) ?? TryInstantiateByConstructor(type);

            if (instance is TProduct product)
            {
                return product;
            }

            return default;
        }

        private object TryInstantiateByConstructor(Type type)
        {
            var constructor = type.GetConstructor(new Type[] { });

            if (constructor != null)
            {
                try
                {
                    return constructor.Invoke(new object[] { });
                }
                catch (Exception e)
                {
                    
                }
            }

            return null;
        }

        private object TryResolve(Type type)
        {
            try
            {
                return _resolver(type);
            }
            catch (Exception e)
            { }

            return null;
        }
    }
}