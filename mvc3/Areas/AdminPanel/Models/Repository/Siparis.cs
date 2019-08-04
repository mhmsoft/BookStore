using mvc3.Areas.AdminPanel.Models.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace mvc3.Areas.AdminPanel.Models.Repository
{
    public class SiparisRepository : IIslem<siparis>
    {
        private kitapProjesiEntities _context;
        public SiparisRepository(kitapProjesiEntities Context)
        {
            _context = Context;
        }
        public siparis Bul(int id)
        {
            return _context.siparis.Find(id);
        }

        public void Guncelle(siparis entity)
        {
            if (entity != null)
            {
                siparis old = Bul(entity.siparisNo);
                _context.Entry(old).State = System.Data.Entity.EntityState.Detached;
                _context.Entry(entity).State= System.Data.Entity.EntityState.Modified;
                _context.SaveChanges();
            }
        }

        public void Kaydet(siparis entity)
        {
            if (entity != null)
            {
                _context.siparis.Add(entity);
                _context.SaveChanges();
            }
           
        }

        public List<siparis> Listele()
        {
            return _context.siparis.AsNoTracking().ToList();
        }

        public void Sil(siparis entity)
        {
            if (entity != null)
            {
                _context.siparis.Remove(entity);
                _context.SaveChanges();
            }
        }
    }
}