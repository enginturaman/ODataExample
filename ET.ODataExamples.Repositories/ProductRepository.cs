using ET.ODataExamples.Storage.Entities;
using ET.ODataExamples.Storage.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ET.ODataExamples.Repositories
{

    public interface IProductRepository
    {
        public IQueryable<ProductDmo> Get();

        public Guid Post(ProductPostModel model);
        public Guid Put(ProductPutModel model);
        public Guid Delete(Guid id);
    }

    public class ProductRepository : IProductRepository
    {
        ApiContext _context;

        public ProductRepository(
            ApiContext context)
        {
            _context = context;
        }

        public Guid Delete(Guid id)
        {
            throw new NotImplementedException();
        }

        public IQueryable<ProductDmo> Get()
        {
            return _context.Products.AsQueryable();
        }

        public Guid Post(ProductPostModel model)
        {
            throw new NotImplementedException();
        }

        public Guid Put(ProductPutModel model)
        {
            throw new NotImplementedException();
        }
    }
}
