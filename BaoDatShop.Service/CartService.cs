using BaoDatShop.DTO.Cart;
using BaoDatShop.DTO.Product;
using BaoDatShop.Responsitories;
using Eshop.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaoDatShop.Service
{
    public interface ICartService
    {
        public bool Create(CreateCart model);
        public bool Update(int id, CreateCart model);
        public bool Delete(int id);
        public Cart GetById(int id);
        public List<Cart> GetAll();
        public bool Up(int id);
        public bool Down(int id);
        public bool DeleteAll(int id);
    }
    public class CartService : ICartService
    {
        private readonly ICartResponsitories cartResponsitories;
        public CartService(ICartResponsitories cartResponsitories)
        {
            this.cartResponsitories = cartResponsitories;
        }

        public bool Create(CreateCart model)
        {
           Cart result = new();
            result.ProductId = model.ProductId;
            result.AccountId = model.AccountId;
            result.Quantity = model.Quantity;
            return cartResponsitories.Create(result);
        }

        public bool Delete(int id)
        {
            return cartResponsitories.Delete(id);
        }

        public bool DeleteAll(int id)
        {
            return cartResponsitories.DeleteAll(id);
        }

        public bool Down(int id)
        {
            var result = cartResponsitories.GetById(id);
            result.Quantity--;
            return cartResponsitories.Update(result);
        }

        public List<Cart> GetAll()
        {
            return cartResponsitories.GetAll();
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

        public bool Update(int id, CreateCart model)
        {
            Cart result = cartResponsitories.GetById(id);
            result.ProductId = model.ProductId;
            result.AccountId = model.AccountId;
            result.Quantity = model.Quantity;
            return cartResponsitories.Update(result);
        }
    }
}
