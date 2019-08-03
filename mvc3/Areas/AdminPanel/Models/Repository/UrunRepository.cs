using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using mvc3.Areas.AdminPanel.Models.Interface;
using System.Data.Entity;
namespace mvc3.Areas.AdminPanel.Models.Repository
{
    public class UrunRepository:IIslem<urun>
    {
        //kitapEntity context;
         kitapProjesiEntities _context;
        public UrunRepository(kitapProjesiEntities Context)
        {
            //this.context = Context;
            _context = Context;
        }

        public List<kategori> KategoriListesi()
        {
            return _context.kategori.AsNoTracking().ToList();
        }
        public void Kaydet(urun entity)
        {
            if (entity != null)
            {
                _context.urun.Add(entity);
                _context.SaveChanges();
            }
        }
        public void Guncelle(urun entity)
        {
            if (entity != null)
            {
                urun eski = Bul(entity.urunNo);
               _context.Entry(eski).State = EntityState.Detached;
                _context.urun.Attach(entity);
                _context.Entry(entity).State = EntityState.Modified;
                _context.SaveChanges();
            }
        }
        public void ResimKaydet(resim resim)
        {
            if (resim != null)
            {
                _context.resim.Add(resim);
                _context.SaveChanges();
            }
        }
        public void Sil(urun entity)
        {
            if (entity != null)
            {
                _context.urun.Remove(entity);
                _context.SaveChanges();
            }
        }

        public List<urun> Listele()
        {
            return _context.urun.AsNoTracking().ToList();
        }

        public urun Bul(int id)
        {
            return _context.urun.Find(id);
        }

        public string resimsil(int id)
        {
            resim silinecekResim= _context.resim.Find(id);
            if (silinecekResim!=null)
            {
                _context.resim.Remove(silinecekResim);
                return " seçtiğiniz resim silindi";
            }
            else
                return " silinecek resim bulunamadı!";
           
        }
        public List<urun> Listele(Func<urun, bool> where)
        {
            return _context.urun.AsNoTracking().Where(where).ToList();
        }

        


        
    }
}