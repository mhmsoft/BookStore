using mvc3.Areas.AdminPanel.Models.Interface;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Configuration;

namespace mvc3.Areas.AdminPanel.Models.Repository
{
    public class KategoriRepository : IIslem<kategori>
    {
        kitapProjesiEntities _context;
        public KategoriRepository( kitapProjesiEntities Context)
        {
            _context = Context;
        }


        public kategori Bul(int id)
        {  
            return _context.kategori.Find(id);
        }

        public void Guncelle(kategori entity)
        {
            if (entity != null)
            {
                kategori eski = Bul(entity.kategoriNo);
                _context.Entry(eski).State = EntityState.Detached;
                _context.Entry(entity).State = EntityState.Modified;
                _context.SaveChanges();
            }
        }

        public void Kaydet(kategori entity)
        {
            if (entity!=null)
            {
                _context.kategori.Add(entity);
                _context.SaveChanges();
            }
        }

        public List<kategori> Listele()
        {
            return _context.kategori.ToList();
        }

        public void Sil(kategori entity)
        {
            if (entity != null)
            {
                _context.kategori.Remove(entity);
                _context.SaveChanges();
            }
        }
    }
}