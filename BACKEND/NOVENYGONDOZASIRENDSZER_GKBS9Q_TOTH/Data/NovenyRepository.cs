using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using NOVENYGONDOZASIRENDSZER_GKBS9Q_TOTH.Models;

namespace NOVENYGONDOZASIRENDSZER_GKBS9Q_TOTH.Data
{
    public class NovenyRepository : INovenyRepository
    {
        private readonly NovenyDbContext db;

        public NovenyRepository(NovenyDbContext dbContext)
        {
            db = dbContext;
        }

        public void Create(Noveny noveny)
        {
            if (db.Novenyek.Any(n => n.Id == noveny.Id))
            {
                throw new Exception("A növény azonosítója már létezik.");
            }
            db.Novenyek.Add(noveny);
            db.SaveChanges();
        }

        public Noveny Read(int id)
        {
            return db.Novenyek.FirstOrDefault(n => n.Id == id);
        }

        public IEnumerable<Noveny> Read()
        {
            return db.Novenyek.ToList();
        }

        public void Update(int id, Noveny updatedNoveny)
        {
            var noveny = db.Novenyek.FirstOrDefault(n => n.Id == id);
            if (noveny != null)
            {
                noveny.Nev = updatedNoveny.Nev;
                noveny.Kategoria = updatedNoveny.Kategoria;
                noveny.NapiVizigeny = updatedNoveny.NapiVizigeny;
                noveny.OntozesiGyakorisag = updatedNoveny.OntozesiGyakorisag;

                db.Novenyek.Update(noveny);
                db.SaveChanges();
            }
        }

        public void Delete(int id)
        {
            var noveny = db.Novenyek.FirstOrDefault(n => n.Id == id);
            if (noveny != null)
            {
                db.Novenyek.Remove(noveny);
                db.SaveChanges();
            }
        }
    }
}
