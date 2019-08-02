using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using mvc3.Areas.AdminPanel.Models.Interface;

namespace mvc3.Areas.AdminPanel.Models.Repository
{
    public class FavoriuteRepository : IIslem<Favorim>
    {
        private kitapProjesiEntities _context;
        public FavoriuteRepository(kitapProjesiEntities Context)
        {
            _context = Context;
        }
        public Favorim Bul(int id)
        {
            return _context.Favorim.Find(id);
        }

        public void Guncelle(Favorim entity)
        {
            if (entity != null)
            {
                Favorim eski = Bul(entity.id);
                _context.Entry(eski).State = EntityState.Detached;
                _context.Entry(entity).State = EntityState.Modified;
                _context.SaveChanges();
            }
        }

        public void Kaydet(Favorim entity)
        {
            if (entity != null)
            {
                _context.Favorim.Add(entity);
                _context.SaveChanges();
            }
        }

        public List<Favorim> Listele()
        {
            return _context.Favorim.ToList();
        }

        public void Sil(Favorim entity)
        {
            if (entity != null)
            {
                _context.Favorim.Remove(entity);
                _context.SaveChanges();
            }
        }
    }
}