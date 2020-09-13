using Microsoft.AspNet.OData;
using Microsoft.AspNet.OData.Query;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Linq;

namespace ET.ODataExamples.Infrastructures
{
    public class ETEnableQueryAttribute : EnableQueryAttribute
    {
        //Project dosyasına AspNetCore.Mvc ve Http için eklenmesi gereken satır aşağıdaki gibidir.
        //  <ItemGroup>
        //      <FrameworkReference Include = "Microsoft.AspNetCore.App" />
        //  </ItemGroup >
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            //OData Contains de bulunan casesensitive bugından dolayı eklenmiştir.
            string newQueryString = ODataFilterHelper.ContainsQueryString(context.HttpContext.Request.QueryString.Value);

            context.HttpContext.Request.QueryString = new QueryString(newQueryString);
            base.OnActionExecuting(context);
        }

        public override IQueryable ApplyQuery(IQueryable query, ODataQueryOptions queryOptions)
        {
            return queryOptions.ApplyTo(query);
        }
        public override void ValidateQuery(HttpRequest request, ODataQueryOptions queryOptions)
        {
            base.ValidateQuery(request, queryOptions);
        }
    }
}
