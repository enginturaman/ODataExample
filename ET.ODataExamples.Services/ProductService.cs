using ET.ODataExamples.Repositories;
using ET.ODataExamples.Storage.Entities;
using ET.ODataExamples.Storage.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ET.ODataExamples.Services
{
    public interface IProductService
    {
        IQueryable<ProductDmo> Get();
        Guid Post(ProductPostModel model);
        Guid Put(ProductPutModel model);
        Guid Delete(Guid id);
    }

    public class ProductService : IProductService
    {
        private readonly IProductRepository _productRepository;

        public ProductService(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        public IQueryable<ProductDmo> Get()
        {
            return _productRepository.Get();
        }

        public Guid Post(ProductPostModel model)
        {
            return _productRepository.Post(model);
        }

        public Guid Put(ProductPutModel model)
        {
            return _productRepository.Put(model);
        }

        public Guid Delete(Guid id)
        {
            return _productRepository.Delete(id);
        }
    }
}
