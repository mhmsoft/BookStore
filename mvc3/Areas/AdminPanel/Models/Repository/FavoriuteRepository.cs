using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using mvc3.Areas.AdminPanel.Models.Interface;

namespace mvc3.Areas.AdminPanel.Models.Repository
{
    public class FavoriuteRepository : IIslem<favorim>
    {
        private kitapProjesiEntities _context;
        public FavoriuteRepository(kitapProjesiEntities Context)
        {
            _context = Context;
        }
        public favorim Bul(int id)
        {
            return _context.favorim.Find(id);
        }

        public void Guncelle(favorim entity)
        {
            if (entity != null)
            {
                favorim eski = Bul(entity.id);
                _context.Entry(eski).State = EntityState.Detached;
                _context.Entry(entity).State = EntityState.Modified;
                _context.SaveChanges();
            }
        }

        public void Kaydet(favorim entity)
        {
            if (entity != null)
            {
                _context.favorim.Add(entity);
                _context.SaveChanges();
            }
        }

        public List<favorim> Listele()
        {
            return _context.favorim.ToList();
        }

        public void Sil(favorim entity)
        {
            if (entity != null)
            {
                _context.favorim.Remove(entity);
                _context.SaveChanges();
            }
        }

       
    }
}