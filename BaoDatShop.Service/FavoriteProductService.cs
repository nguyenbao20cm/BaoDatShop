using BaoDatShop.DTO.CreateFavoriteProduct;
using BaoDatShop.DTO.CreateImageProduct;
using BaoDatShop.Model.Model;
using BaoDatShop.Responsitories;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaoDatShop.Service
{
    public interface IFavoriteProductService
    {
        public bool Create(string id,CreateFavoriteProduct model);
        public bool Delete(string idacc,int id);

        public List<FavoriteProduct> GetAll(string id);
      

    }
    public class FavoriteProductService : IFavoriteProductService
    {
        private readonly IFavoriteProductRespositories IFavoriteProductRespositories;

        public FavoriteProductService(IFavoriteProductRespositories IFavoriteProductRespositories)
        {
            this.IFavoriteProductRespositories = IFavoriteProductRespositories;
        }
        public bool Create(string id,CreateFavoriteProduct model)
        {
            FavoriteProduct a = new();
            a.ProductId=model.ProductId;
            a.AccountId = id;
            a.Status = true;
            return IFavoriteProductRespositories.Create(a);
        }

        public bool Delete(string idacc,int id)
        {
          
            return IFavoriteProductRespositories.Delete(idacc, id);
        }

        public List<FavoriteProduct> GetAll(string id)
        {
            return IFavoriteProductRespositories.GetAll().Where(a=>a.AccountId==id).ToList();
        }
    }
}
