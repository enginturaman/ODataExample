using ET.ODataExamples.Storage.CustomAttributes;
using Microsoft.AspNet.OData;
using Microsoft.AspNetCore.Mvc;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Text;

namespace ET.ODataExamples.Infrastructures
{

    public class SwaggerRemoveVerbsFilter : IDocumentFilter
    {
        /// <summary>
        /// https://stackoverflow.com/a/31159468
        /// </summary>
        /// <param name="swaggerDoc"></param>
        /// <param name="context"></param>
        public void Apply(OpenApiDocument swaggerDoc, DocumentFilterContext context)
        {
            var methodss = swaggerDoc.Paths.Select(i => i.Value);
            Assembly assembly = typeof(Controller).Assembly;
            var thisAssemblyTypes = Assembly.GetExecutingAssembly().GetTypes().ToList();
            var odatacontrollers = thisAssemblyTypes.Where(t => t.BaseType == typeof(ODataController)).ToList();

            foreach (var odataContoller in odatacontrollers)  // this the OData controllers in the API
            {
                var methods = odataContoller.GetMethods().Where(x => x.ReturnType == typeof(IActionResult)).ToList();
                if (!methods.Any())
                    continue; // next controller 

                foreach (var method in methods)
                {
                    StringBuilder sb = new StringBuilder();
                    List<String> listParams = new List<String>();
                    var parameterInfo = method.GetParameters();
                    foreach (ParameterInfo pi in parameterInfo)
                    {
                        listParams.Add(string.Format("{{{0}}}", pi.Name));
                    }
                    sb.Append(String.Join(", ", listParams.ToArray()));

                    var odataPathItem = new OpenApiPathItem();
                    var op = new OpenApiOperation();

                    var path = "/" + "api" + "/" + odataContoller.Name.Replace("Controller", "");

                    var apiStandartMethod = new[] { "Get", "Post", "Put", "Delete" };
                    if (!string.IsNullOrEmpty(method.Name) && !apiStandartMethod.Contains(method.Name))
                    {
                        path = $"{path}/{method.Name}";
                    }
                    if (listParams.Any())
                    {
                        if (listParams.Contains("{guid}"))
                            path = path + "/{guid}";
                        if (listParams.Contains("{id}"))
                            path = path + "/{id}";
                        if (listParams.Contains("{bankEftCode}"))
                            path = path + "/{bankEftCode}";
                    }

                    if (method.IsDefined(typeof(ETEnableQueryAttribute)))
                    {
                        AddOdataQuery(op);
                    }

                    op.Tags = new List<OpenApiTag>
                    {
                        new OpenApiTag { Name = odataContoller.Name.Replace("Controller","")}
                    };

                    var isPost = method.GetCustomAttribute<HttpPostAttribute>();
                    var isGet = method.GetCustomAttribute<HttpGetAttribute>();
                    var isDelete = method.GetCustomAttribute<HttpDeleteAttribute>();
                    var isPut = method.GetCustomAttribute<HttpPutAttribute>();

                    OperationType operationType = OperationType.Get;
                    if (isPost != null)
                        operationType = OperationType.Post;
                    if (isPut != null)
                        operationType = OperationType.Put;
                    if (isDelete != null)
                        operationType = OperationType.Delete;

                    op.OperationId = operationType.ToString();
                    var summary = method.GetCustomAttribute(typeof(Summary), false);
                    op.Summary = summary == null ? "" : (summary as Summary).Name;
                    op.Description = "";

                    op.Deprecated = false;

                    var ResponseAttributes = method.GetCustomAttributes(typeof(ProducesResponseTypeAttribute), false);

                    op.Responses = new OpenApiResponses();
                    foreach (var ResponseAttribute in ResponseAttributes)
                    {
                        var response = new OpenApiResponse();

                        var type = ((ProducesResponseTypeAttribute)ResponseAttribute).Type;

                        var statusCode = ((ProducesResponseTypeAttribute)ResponseAttribute).StatusCode;
                        response.Description = Enum.IsDefined(typeof(HttpStatusCode), statusCode).ToString();
                        op.Responses.Add(statusCode.ToString(), response);
                    }

                    odataPathItem.Operations[operationType] = op;
                    //odataPathItem.Operations[OperationType.Options] = op;
                    try
                    {

                        if (swaggerDoc.Paths.ContainsKey(path))
                        {
                            if (swaggerDoc.Paths[path].Operations.ContainsKey(operationType))
                            {
                                if (swaggerDoc.Paths[path].Operations[operationType].Parameters.Any())
                                {
                                    foreach (var prm in op.Parameters)
                                    {
                                        swaggerDoc.Paths[path].Operations[operationType].Parameters.Add(prm);
                                    }
                                }
                                else
                                {
                                    swaggerDoc.Paths[path].Operations[operationType].Parameters = op.Parameters;
                                }
                            }
                            else
                                swaggerDoc.Paths[path].Operations.Add(operationType, op);
                        }
                        else
                        {
                            swaggerDoc.Paths.Add(path, odataPathItem);
                        }

                    }


                    catch (Exception ex)
                    {

                        throw new Exception(ex.Message);
                    }
                }
            }

        }

        private void AddOdataQuery(OpenApiOperation op)
        {
            op.Parameters.Add(new OpenApiParameter
            {
                Name = "$filter",
                In = ParameterLocation.Query,
                Required = false,
                Schema = new OpenApiSchema
                {
                    Type = "string"
                }
            });
            op.Parameters.Add(new OpenApiParameter
            {
                Name = "$expand",
                In = ParameterLocation.Query,
                Required = false,
                Schema = new OpenApiSchema
                {
                    Type = "string"
                }
            });
            op.Parameters.Add(new OpenApiParameter
            {
                Name = "$top",
                In = ParameterLocation.Query,
                Required = false,
                Schema = new OpenApiSchema
                {
                    Type = "string"
                }
            });
            op.Parameters.Add(new OpenApiParameter
            {
                Name = "$skip",
                In = ParameterLocation.Query,
                Required = false,
                Schema = new OpenApiSchema
                {
                    Type = "string"
                }
            });
            op.Parameters.Add(new OpenApiParameter
            {
                Name = "$select",
                In = ParameterLocation.Query,
                Required = false,
                Schema = new OpenApiSchema
                {
                    Type = "string"
                }
            });
            op.Parameters.Add(new OpenApiParameter
            {
                Name = "$orderby",
                In = ParameterLocation.Query,
                Required = false,
                Schema = new OpenApiSchema
                {
                    Type = "string"
                }
            });
        }


    }
}
