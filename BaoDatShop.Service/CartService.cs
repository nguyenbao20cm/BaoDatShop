using BaoDatShop.DTO.Cart;
using BaoDatShop.DTO.Product;
using BaoDatShop.Responsitories;
using Eshop.Models;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace BaoDatShop.Service
{
    public interface ICartService
    {
        public int GetAllTotal(string id);
        public bool Create(CreateCartRequest model);
        public bool Update(int id, CreateCartRequest model);
        public bool Delete(int id);
        public Cart GetById(int id);
        public List<Cart> GetAll(string id);
        public bool Up(int id);
        public bool Down(int id);
        public bool DeleteAll(string id);
    }
    public class CartService : ICartService
    {
        private readonly ICartResponsitories cartResponsitories;
        private readonly IProductResponsitories IProductResponsitories;
        private readonly IProductSizeService IProductSizeService;
        private readonly IKhoHangResposirity IKhoHangResposirity;
        public CartService(ICartResponsitories cartResponsitories, IKhoHangResposirity IKhoHangResposirity,
            IProductResponsitories IProductResponsitories, IProductSizeService IProductSizeService)
        {
            this.cartResponsitories = cartResponsitories;
            this.IKhoHangResposirity = IKhoHangResposirity;
            this.IProductResponsitories = IProductResponsitories;
            this.IProductSizeService = IProductSizeService;

        }

        public bool Create(CreateCartRequest model)
        {
            if (model.Quantity > IKhoHangResposirity.GetAll().Where(a=>a.ProductSizeId==model.ProductSizeId).FirstOrDefault().Stock) return false;
           Cart result = new();
            result.ProductSizeId = model.ProductSizeId;
            result.AccountId = model.AccountId;
            result.Quantity = model.Quantity;
            var tamp=cartResponsitories.GetAll(result.AccountId).ToList();
            if (tamp != null)
            {
                foreach (var a in tamp)
                {
                    if (result.ProductSizeId == a.ProductSizeId)
                    {
                        a.Quantity += result.Quantity;
                       return cartResponsitories.Update(a);
                    }
                }
            }
        
            return cartResponsitories.Create(result);
        }

        public bool Delete(int id)
        {
            return cartResponsitories.Delete(id);
        }

        public bool DeleteAll(string id)
        {
            return cartResponsitories.DeleteAll(id);
        }

        public bool Down(int id)
        {
            var result = cartResponsitories.GetById(id);
            if(result.Quantity==0) return cartResponsitories.Delete(id);
            result.Quantity--;
            return cartResponsitories.Update(result);
        }

        public List<Cart> GetAll(string id)
        { 
            var result = cartResponsitories.GetAll(id);
            //List<GetAllCartResponse> result = new ();
            //foreach (var item in tamp)
            //{
            //    GetAllCartResponse createCart = new();
            //    createCart.CartId = item.Id;
            //    createCart.ProductSizeId = item.ProductSizeId;  
            //    createCart.AccountId = item.AccountId;
            //    createCart.Quantity = item.Quantity;

            //    result.Add(createCart);
            //}
        
            return result;
        }

        public int GetAllTotal(string id)
        {
            
                int Total = 0;
                var tamp = cartResponsitories.GetAll(id);

                foreach (var item in tamp)
                {
                    var productId = IProductSizeService.GetById(item.ProductSizeId).ProductId;
                    Total += item.Quantity * (IProductResponsitories.GetById(productId).PriceSales);
                }
                return Total;
        }

        public Cart GetById(int id)
        {
            return cartResponsitories.GetById(id);
        }

        public bool Up(int id)
        {
            var result = cartResponsitories.GetById(id);
            result.Quantity++;
            return cartResponsitories.Update(result);
        }

        public bool Update(int id, CreateCartRequest model)
        {
            Cart result = cartResponsitories.GetById(id);
            result.ProductSizeId = model.ProductSizeId;
            result.AccountId = model.AccountId;
            result.Quantity = model.Quantity;
            return cartResponsitories.Update(result);
        }
    }
}
