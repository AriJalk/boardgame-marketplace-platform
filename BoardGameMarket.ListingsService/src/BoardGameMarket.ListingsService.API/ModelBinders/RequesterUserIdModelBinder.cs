using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace BoardGameMarket.ListingsService.API.ModelBinders
{
    public class RequesterUserIdModelBinder : IModelBinder
    {
        public Task BindModelAsync(ModelBindingContext bindingContext)
        {
            var headerName = "X-Requester-UserId";
            if (!bindingContext.HttpContext.Request.Headers.TryGetValue(headerName, out var headerValue))
            {
                bindingContext.Result = ModelBindingResult.Failed();
                return Task.CompletedTask;
            }

            if (Guid.TryParse(headerValue, out var guid))
            {
                bindingContext.Result = ModelBindingResult.Success(guid);
            }
            else
            {
                bindingContext.Result = ModelBindingResult.Failed();
            }

            return Task.CompletedTask;
        }
    }
}
