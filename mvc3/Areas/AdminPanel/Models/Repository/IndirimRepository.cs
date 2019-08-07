using mvc3.Areas.AdminPanel.Models.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace mvc3.Areas.AdminPanel.Models.Repository
{
    public class IndirimRepository : IIslem<indirim>
    {
        private kitapProjesiEntities _context;
        public IndirimRepository(kitapProjesiEntities Context)
        {
            _context = Context;
        }
        public indirim Bul(int id)
        {
            return _context.indirim.Find(id);
        }

        public void Guncelle(indirim entity)
        {
            if (entity != null)
            {
                indirim old = Bul(entity.indirimNo);
                _context.Entry(old).State = System.Data.Entity.EntityState.Detached;
                _context.Entry(entity).State = System.Data.Entity.EntityState.Modified;
                _context.SaveChanges();
            }
        }

        public void Kaydet(indirim entity)
        {
            if (entity != null)
            {
                _context.indirim.Add(entity);
                _context.SaveChanges();
            }
        }

        public List<indirim> Listele()
        {
            return _context.indirim.AsNoTracking().ToList();
        }

        public void Sil(indirim entity)
        {
            if (entity != null)
            {
                _context.indirim.Remove(entity);
                _context.SaveChanges();
            }
        }
    }
}