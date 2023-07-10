
using BaoDatShop.DTO.Voucher;
using BaoDatShop.Model.Model;
using BaoDatShop.Responsitories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaoDatShop.Service
{
    public interface IVoucherService
    {
        public bool Create(CreateVoucher model);
        public bool Update(int id, CreateVoucher model);
        public bool Delete(int id);
        public List<Voucher> GetAll();
        public List<Voucher> GetAllStatusFalse();
        public List<Voucher> GetAllStatusTrue();
        
        public Voucher ValidationVoucher(string Name);
    }
    public class VoucherService: IVoucherService
    {
        private readonly IVoucherRespositories IVoucherRespositories;

        public VoucherService(IVoucherRespositories IVoucherRespositories)
        {
            this.IVoucherRespositories = IVoucherRespositories;
        }
        public bool Create(CreateVoucher model)
        {
            Voucher a = new();
            a.Disscount=model.Disscount;
            a.Title = model.Title;
            a.Name = model.Name;
            return IVoucherRespositories.Create(a);
        }

        public bool Delete(int id)
        {
         
            return IVoucherRespositories.Delete(id);
        }

        public List<Voucher> GetAll()
        {
            return IVoucherRespositories.GetAll();
        }

        public List<Voucher> GetAllStatusFalse()
        {
            return IVoucherRespositories.GetAll().ToList();
        }

        public List<Voucher> GetAllStatusTrue()
        {
            return IVoucherRespositories.GetAll().ToList();
        }

        public bool Update(int id, CreateVoucher model)
        {
            var a = IVoucherRespositories.GetById(id);
            if(model.Name!=a.Name)
            {
                foreach(var item in IVoucherRespositories.GetAll())
                {
                    if (model.Name == item.Name)
                        return false;
                }    
            }
            a.Name = model.Name;
            //a.Status = model.Status;
            a.Disscount = model.Disscount;
            a.Title = model.Title;
          return  IVoucherRespositories.Update(a);
        }

        public Voucher ValidationVoucher(string Name)
        {
            var a = IVoucherRespositories.GetByName(Name);
            if (a == null)
                return null;
            else
                return a;
        }
    }
}
