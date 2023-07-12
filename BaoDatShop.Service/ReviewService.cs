using BaoDatShop.DTO.ProductType;
using BaoDatShop.DTO.Review;
using BaoDatShop.Model.Model;
using BaoDatShop.Responsitories;
using Eshop.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaoDatShop.Service
{
    public interface IReviewService
    {
        
        public bool CreateImageReview(IFormFile model);
        public Review Create(string AccountID,ReviewRequest model);
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
        private readonly IWebHostEnvironment _environment;
        private readonly IInvoiceDetailService IInvoiceDetailService;
        public ReviewService(IWebHostEnvironment _environment,
            IInvoiceDetailService IInvoiceDetailServicem,
            IReviewResponsitories reviewResponsitories)
        {
            this.reviewResponsitories = reviewResponsitories;
            this._environment = _environment;
            this.IInvoiceDetailService = IInvoiceDetailService;
        }

        public Review Create(string AccountID,ReviewRequest model)
        {
                 
            Review result = new();
            result.Image = null;
            result.Star = model.Star;
            result.ProductId = model.ProductId;
            result.AccountId = AccountID;
            result.Content = model.Content;
            result.DateTime =DateTime.Now;
            result.Status = true;
            var a= reviewResponsitories.Create(result);
            if (a == true)
            {
                return (result);
            }

            else return null;
        }

        public bool CreateImageReview(IFormFile model)
        {
            var a = reviewResponsitories.GetById(int.Parse(model.FileName));
            var fileName = model.FileName + ".jpg";
            var uploadFolder = Path.Combine(_environment.WebRootPath, "Image", "ReviewImage");
            var uploadPath = Path.Combine(uploadFolder, fileName);
            try
            {
                using (FileStream fs = System.IO.File.Create(uploadPath))
                {
                    model.CopyTo(fs);
                    fs.Flush();
                }
                var c = reviewResponsitories.GetById(int.Parse(model.FileName));
                c.Image = fileName;
                reviewResponsitories.Update(c);
               
            }
            catch (Exception e)
            {
                return false;
            }
            return true;
        }

        public bool Delete(int id)
        {
            return reviewResponsitories.Delete(id);
        }

        public List<Review> GetAll()
        {
           return reviewResponsitories.GetAll().OrderByDescending(a=>a.DateTime).ToList();
        }

        public List<Review> GetAllStatusFalse()
        {
            return reviewResponsitories.GetAll().Where(a=>a.Status==false).ToList().OrderByDescending(a => a.DateTime).ToList();
        }

        public List<Review> GetAllStatusTrue()
        {
            return reviewResponsitories.GetAll().Where(a => a.Status == true).ToList().OrderByDescending(a => a.DateTime).ToList();
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
            result.Image = model.Image;
            result.ProductId = model.ProductId;
            result.AccountId = AccountId;
            result.Content = model.Content;
            result.DateTime = DateTime.Now;
            return reviewResponsitories.Update(result);
        }
    }
}
