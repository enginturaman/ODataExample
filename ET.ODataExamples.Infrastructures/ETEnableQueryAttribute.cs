using Microsoft.AspNet.OData;
using Microsoft.AspNet.OData.Query;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Linq;

namespace ET.ODataExamples.Infrastructures
{
    public class ETEnableQueryAttribute : EnableQueryAttribute, IExceptionFilter
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
            var res = queryOptions.ApplyTo(query);
            return res;
        }
        public override void ValidateQuery(HttpRequest request, ODataQueryOptions queryOptions)
        {
            base.ValidateQuery(request, queryOptions);
        }
        private bool AuthorizationIsOK()
        {
            return false;
        }

        public void OnException(ExceptionContext context)
        {

            var result = new AuthApiErrorModel
            {
                Errors = new System.Collections.Generic.List<Errors>() { new Errors { Code = "InternalServerError", Message = context.Exception.Message } }
            };
            //context.Result = new JsonResult(result);

            Microsoft.AspNetCore.Mvc.Formatters.MediaTypeCollection mediaTypeCollection = new Microsoft.AspNetCore.Mvc.Formatters.MediaTypeCollection();
            mediaTypeCollection.Add("application/json");

            context.Result = new ObjectResult(result)
            {
                ContentTypes = mediaTypeCollection,
                StatusCode = (int)StatusCodes.Status500InternalServerError,
            };
        }
    }
}
