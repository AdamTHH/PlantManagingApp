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

        public IEnumerable<DailyPlan> GetWeeklyPlan()
        {
            var haviLista = new List<DailyPlan>();
            for (int i = 1; i < 32; i++)
            {
                var napiTerv = new DailyPlan();
                napiTerv.napiOntozendoNovenyek = db.Novenyek.Where(x => i % x.OntozesiGyakorisag == 0).ToList();
                haviLista.Add(napiTerv);
            }
            return haviLista;
        }
        public void UploadMatrix(string novenyMatrix)
        {
            foreach (var row in novenyMatrix.Split('\n', StringSplitOptions.RemoveEmptyEntries))
            {
                var values = row.Split(',');

                var kategoria = Noveny.GetNovenyEnum(values[1].Trim());
                var napiVizigeny = double.Parse(values[2].Trim());
                var ontozesiGyakorisag = int.Parse(values[3].Trim());

                Create(new Noveny(values[0].Trim(), kategoria, napiVizigeny, ontozesiGyakorisag));
            }
        }

        public string GetFunFact()
        {
            Random rand = new Random();

            string ranomNoveny = db.Novenyek.ToList()[rand.Next(db.Novenyek.Count())].Nev;

            string[] allapotok = {
    "szereti",
    "várja",
    "rühelli",
    "keresi",
    "megveti",
    "óvja",
    "gyűlöli",
    "tiszteli",
    "csodálja",
    "nem érti"
};

            string allapot = allapotok[rand.Next(allapotok.Length)];

            string[] fonevek = {
    "rózsát",
    "levendulát",
    "kertészkedést",
    "locsolást",
    "napraforgót",
    "ültetést",
    "gyomlálást",
    "komposztálást",
    "metszést",
    "fűnyírást"
};

            string fonev = fonevek[rand.Next(fonevek.Length)];

            return $"A {ranomNoveny} {allapot} a {fonev}!";
        }
    }
}
