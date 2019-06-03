using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.ModelBinding.Binders;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace DetegoServer.Helpers
{
    public class CustomBinderProvider : IModelBinderProvider
    {
        public IModelBinder GetBinder(ModelBinderProviderContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }
            if (context.Metadata.ModelType == typeof(int[]) || context.Metadata.ModelType == typeof(List<int>))
            {
                return new BinderTypeModelBinder(typeof(IntCommaDelimitedArrayParameterBinder));
            }
            if (context.Metadata.ModelType == typeof(double[]) || context.Metadata.ModelType == typeof(List<double>))
            {
                return new BinderTypeModelBinder(typeof(DoubleCommaDelimitedArrayParameterBinder));
            }
            return null;
        }
    }

    public class IntCommaDelimitedArrayParameterBinder : IModelBinder
    {
        public Task BindModelAsync(ModelBindingContext bindingContext)
        {
            var value = string.Join(',', bindingContext.ValueProvider.GetValue(bindingContext.FieldName));
            // Check if the argument value is null or empty
            if (string.IsNullOrEmpty(value))
            {
                return Task.CompletedTask;
            }
            var items = value?.Split(',').Select(int.Parse).ToArray();
            bindingContext.Result = ModelBindingResult.Success(items);
            if (bindingContext.ModelType == typeof(List<int>))
            {
                bindingContext.Result = ModelBindingResult.Success(items.ToList());
            }
            return Task.CompletedTask;
        }
    }

    public class DoubleCommaDelimitedArrayParameterBinder : IModelBinder
    {
        public Task BindModelAsync(ModelBindingContext bindingContext)
        {
            var value = string.Join(',', bindingContext.ValueProvider.GetValue(bindingContext.FieldName));
            // Check if the argument value is null or empty
            if (string.IsNullOrEmpty(value))
            {
                return Task.CompletedTask;
            }
            CultureInfo culture = new CultureInfo("en-US");
            var items = value?.Split(',').Select(s => double.Parse(s, culture)).ToArray();
            bindingContext.Result = ModelBindingResult.Success(items);
            if (bindingContext.ModelType == typeof(List<double>))
            {
                bindingContext.Result = ModelBindingResult.Success(items.ToList());
            }
            return Task.CompletedTask;
        }
    }
}
