using ET.ODataExamples.Infrastructures;
using ET.ODataExamples.Services;
using ET.ODataExamples.Storage.CustomAttributes;
using ET.ODataExamples.Storage.Entities;
using ET.ODataExamples.Storage.Models;
using Microsoft.AspNet.OData;
using Microsoft.AspNet.OData.Routing;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;

namespace ET.ODataExamples.Api.Controllers
{
    /// <summary></summary>
    public class ProductsController : ODataController
    {
        private readonly IProductService _productService;

        /// <summary>
        /// Kurallar için CRUD metodlarının yer aldığı kontrol dosyasıdır. 
        /// </summary>
        /// <param name="ruleService"></param>
        public ProductsController(IProductService productService)
        {
            _productService = productService;
        }


        /// <summary></summary>
        /// <returns></returns>
        [HttpGet]
        [Summary("Bu metod kural listesini elde etmek için kullanır.")]
        [ETEnableQuery]
        [ProducesResponseType(typeof(IQueryable<ProductDmo>), 200)] //OK
        [ProducesResponseType(typeof(ApiErrorModel), StatusCodes.Status400BadRequest)]
        public IActionResult Get()
        {
            var list = _productService.Get();
            return Ok(list);
        }

        /// <summary>
        /// Bu metod istenen kural modelini kaydetmek için kullanır.
        /// </summary>
        /// <param name="model">
        /// Kaydedilmek istenen kural modelidir.
        /// </param>
        /// <returns>
        /// Durum kodu ile servis sonuç nesnesi.
        /// </returns>
        [HttpPost]
        [ODataRoute("Products")]
        public IActionResult Post([FromBody] ProductPostModel model)
        {
            Guid result = _productService.Post(model);
            return Ok(result);
        }

        /// <summary>
        /// Bu metod istenen kural modelini güncellemek için kullanır.
        /// </summary>
        /// <param name="model">
        /// Güncellenmek istenen kural modelidir.
        /// </param>
        /// <returns>
        /// Durum kodu ile servis sonuç nesnesi.
        /// </returns>
        [HttpPut]
        [ODataRoute("Products")]
        public IActionResult Put([FromBody] ProductPutModel model)
        {
            Guid result = _productService.Put(model);
            return Ok(result);

        }

        /// <summary>
        /// Bu metod istenen kuralın silinmesi için kullanır.
        /// </summary>
        /// <param name="id">Kural kodu</param>
        /// <returns>
        /// Durum kodu ile servis sonuç nesnesi.
        /// </returns>
        [HttpDelete("api/Products/{id}")]
        public IActionResult Delete(Guid id)
        {
            Guid result = _productService.Delete(id);
            return Ok(result);
        }
    }
}
