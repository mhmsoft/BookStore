using mvc3.Areas.AdminPanel.Models.Interface;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace mvc3.Areas.AdminPanel.Models.Repository
{
    public class UserRepository : IIslem<user>
    {
        private kitapProjesiEntities _context;
        public UserRepository(kitapProjesiEntities Context)
        {
            this._context = Context;
        }
        public user Bul(int id)
        {
            return _context.user.AsNoTracking().FirstOrDefault(x => x.userId == id);
        }

        public void Guncelle(user entity)
        {
            if (entity != null)
            {
                user eski = Bul(entity.userId);
                _context.Entry(eski).State = EntityState.Detached;
                _context.user.Attach(entity);
                _context.Entry(entity).State = EntityState.Modified;
                _context.SaveChanges();
            }
        }

        public void Kaydet(user entity)
        {
            if (entity != null)
            {
                _context.user.Add(entity);
                _context.SaveChanges();

            }

        }

        public List<user> Listele()
        {
            return _context.user.AsNoTracking().ToList();
        }

        public void Sil(user entity)
        {
            if (entity != null)
            {
                _context.user.Remove(entity);
                _context.SaveChanges();

            }
        }
    }
}