using BaoDatShop.DTO.ProductType;
using BaoDatShop.DTO.Review;
using BaoDatShop.Model.Model;
using BaoDatShop.Responsitories;
using Eshop.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaoDatShop.Service
{
    public interface IReviewService
    {
        public bool Create(string AccountID,ReviewRequest model);
        public bool Update(string AccountId,int IdReview, ReviewRequest model);
        public bool Delete(int id);
        public List<Review> GetByIdProduct(int IdProduct);
        public Review GetById(int id);
        public List<Review> GetAll();
        public List<Review> GetAllStatusTrue();
        public List<Review> GetAllStatusFalse();
        
    }
    public class ReviewService : IReviewService
    {
        private readonly IReviewResponsitories reviewResponsitories;
        public ReviewService(IReviewResponsitories reviewResponsitories)
        {
            this.reviewResponsitories = reviewResponsitories;
        }

        public bool Create(string AccountID,ReviewRequest model)
        {
            Review result = new();
            result.Star = model.Star;
            result.ProductId = model.ProductId;
            result.AccountId = AccountID;
            result.Content = model.Content;
            result.DateTime =DateTime.Now;
            result.Status = true;
            return reviewResponsitories.Create(result);
        }

        public bool Delete(int id)
        {
            Review result = reviewResponsitories.GetById(id);
            result.Status = false;
            return reviewResponsitories.Update(result);
        }

        public List<Review> GetAll()
        {
           return reviewResponsitories.GetAll();
        }

        public List<Review> GetAllStatusFalse()
        {
            return reviewResponsitories.GetAll().Where(a=>a.Status==false).ToList();
        }

        public List<Review> GetAllStatusTrue()
        {
            return reviewResponsitories.GetAll().Where(a => a.Status == true).ToList();
        }

        public Review GetById(int id)
        {
            return reviewResponsitories.GetById(id);
        }

        public List<Review> GetByIdProduct(int IdProduct)
        {
            return   reviewResponsitories.GetAll().Where(a=>a.ProductId==IdProduct).ToList();

        }

        public bool Update(string AccountId,int IdReview, ReviewRequest model)
        {
            Review result = reviewResponsitories.GetById(IdReview);
            result.Star = model.Star;
            result.ProductId = model.ProductId;
            result.AccountId = AccountId;
            result.Content = model.Content;
            result.DateTime = DateTime.Now;
            return reviewResponsitories.Update(result);
        }
    }
}
