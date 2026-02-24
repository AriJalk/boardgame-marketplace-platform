using BoardGameMarket.ListingsService.API.ModelBinders;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace BoardGameMarket.ListingsService.API.CustomAttributes
{
    [AttributeUsage(AttributeTargets.Parameter)]
    public class FromRequesterHeaderAttribute : ModelBinderAttribute
    {
        public FromRequesterHeaderAttribute() : base(typeof(RequesterUserIdModelBinder))
        {

        }
    }
}
