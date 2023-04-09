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
        public bool Create(CreateReview model);
        public bool Update(int id, CreateReview model);
        public bool Delete(int id);
        public Review GetById(int id);
        public List<Review> GetAll();
    }
    public class ReviewService : IReviewService
    {
        private readonly IReviewResponsitories reviewResponsitories;
        public ReviewService(IReviewResponsitories reviewResponsitories)
        {
            this.reviewResponsitories = reviewResponsitories;
        }

        public bool Create(CreateReview model)
        {
            Review result = new();
            result.Star = model.Star;
            result.ProductId = model.ProductId;
            result.AccountId = model.AccountId;
            result.Content = model.Content;
            result.DateTime = model.DateTime;
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

        public Review GetById(int id)
        {
            return reviewResponsitories.GetById(id);
        }

        public bool Update(int id, CreateReview model)
        {
            Review result = reviewResponsitories.GetById(id);
            result.Star = model.Star;
            result.ProductId = model.ProductId;
            result.AccountId = model.AccountId;
            result.Content = model.Content;
            result.DateTime = model.DateTime;
            return reviewResponsitories.Update(result);
        }
    }
}
