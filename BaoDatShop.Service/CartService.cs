﻿using BaoDatShop.DTO.Cart;
using BaoDatShop.DTO.Product;
using BaoDatShop.Responsitories;
using Eshop.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace BaoDatShop.Service
{
    public interface ICartService
    {
       
        public bool Create(CreateCartRequest model);
        public bool Update(int id, CreateCartRequest model);
        public bool Delete(int id);
        public Cart GetById(int id);
        public List<GetAllCartResponse> GetAll(string id);
        public bool Up(int id);
        public bool Down(int id);
        public bool DeleteAll(string id);
    }
    public class CartService : ICartService
    {
        private readonly ICartResponsitories cartResponsitories;
        public CartService(ICartResponsitories cartResponsitories)
        {
            this.cartResponsitories = cartResponsitories;
        }

        public bool Create(CreateCartRequest model)
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

        public List<GetAllCartResponse> GetAll(string id)
        { 
            var tamp= cartResponsitories.GetAll(id); 
            List<GetAllCartResponse> result = new ();
            foreach (var item in tamp)
            {
                GetAllCartResponse createCart = new();
                createCart.CartId = item.Id;
                createCart.ProductId = item.ProductId;  
                createCart.AccountId = item.AccountId;
                createCart.Quantity = item.Quantity;
                result.Add(createCart);
            }
            return result;
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
            result.ProductId = model.ProductId;
            result.AccountId = model.AccountId;
            result.Quantity = model.Quantity;
            return cartResponsitories.Update(result);
        }
    }
}
