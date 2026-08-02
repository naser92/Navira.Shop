using DevExtreme.AspNet.Data;
using DevExtreme.AspNet.Data.Helpers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Navira.Shop.Core.Extensions;

namespace Navira.Shop.Core.ViewModels
{
    [ModelBinder(BinderType = typeof(GridParametersBinder))]
    public class GridParameters : DataSourceLoadOptionsBase
    {
        // public new int Take { get; } = 15;
    }
    public class GridParametersBinder : IModelBinder
    {



        public Task BindModelAsync(ModelBindingContext bindingContext)
        {
            var parameters = new GridParameters();
            DataSourceLoadOptionsParser.Parse(parameters, key => bindingContext.ValueProvider.GetValue(key).FirstOrDefault().ApplyCorrectYeKe());
            parameters.Take = parameters.Take > 100 ? 100 : parameters.Take.IsNegativeOrZero() ? 10 : parameters.Take;
            bindingContext.Result = ModelBindingResult.Success(parameters);
            return Task.CompletedTask;
        }

    }

    [ModelBinder(BinderType = typeof(SortedGridParametersBinder))]
    public class SortedGridParameters : GridParameters
    {
        // public new int Take { get; } = 15;
    }
    public class SortedGridParametersBinder : IModelBinder
    {



        public Task BindModelAsync(ModelBindingContext bindingContext)
        {
            var parameters = new SortedGridParameters();
            DataSourceLoadOptionsParser.Parse(parameters, key => bindingContext.ValueProvider.GetValue(key).FirstOrDefault().ApplyCorrectYeKe());
            parameters.Take = parameters.Take > 100 ? 100 : parameters.Take.IsNegativeOrZero() ? 10 : parameters.Take;
            if (parameters.Sort.IsNull() || parameters.Sort.Length == 0)
                parameters.Sort = new SortingInfo[] { new SortingInfo() { Desc = true, Selector = "CreatedDate" } };
            bindingContext.Result = ModelBindingResult.Success(parameters);
            return Task.CompletedTask;
        }

    }
}
