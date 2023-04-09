using BaoDatShop.DTO.News;
using BaoDatShop.DTO.Product;
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
    public interface INewsService
    {
        public bool Create(CreateNews model);
        public bool Update(int id, CreateNews model);
        public bool Delete(int id);
        public News GetById(int id);
        public List<News> GetAll();
    }
    public class NewsService: INewsService
    {
        private readonly INewsResponsitories INewsResponsitories;
        public NewsService(INewsResponsitories INewsResponsitories)
        {
            this.INewsResponsitories = INewsResponsitories;
        }

        public bool Create(CreateNews model)
        {
            News result = new();
            result.NewsName = model.NewsName;
            result.DateTime = model.DateTime;
            result.Content = model.Content;
            result.Status = true;
            return INewsResponsitories.Create(result);
        }

        public bool Delete(int id)
        {
            News result = INewsResponsitories.GetById(id);
            result.Status = false;
            return INewsResponsitories.Update(result);
        }

        public List<News> GetAll()
        {
            return INewsResponsitories.GetAll();
        }

        public News GetById(int id)
        {
            return INewsResponsitories.GetById(id);
        }

        public bool Update(int id, CreateNews model)
        {
            News result = INewsResponsitories.GetById(id);
            result.NewsName = model.NewsName;
            result.DateTime = model.DateTime;
            result.Content = model.Content;
            return INewsResponsitories.Update(result);
        }
    }
}
