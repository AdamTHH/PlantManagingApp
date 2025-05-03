using System.ComponentModel.DataAnnotations;

namespace NOVENYGONDOZASIRENDSZER_GKBS9Q_TOTH.Models
{
    public enum NovenyKategoria
    {
        Szobanoveny = 1,
        Kertinoveny = 2,
        Virag = 3,
        Szukkulens = 4,
        FuszerNoveny = 5
    }

    public class Noveny
    {
        [Key]
        public int Id { get; set; }
        public string Nev { get; set; }
        public NovenyKategoria Kategoria { get; set; }
        public double NapiVizigeny { get; set; }
        public int OntozesiGyakorisag { get; set; }

        public Noveny(string nev, NovenyKategoria kategoria, double napiVizigeny, int ontozesiGyakorisag)
        {
            Nev = nev;
            Kategoria = kategoria;
            NapiVizigeny = napiVizigeny;
            OntozesiGyakorisag = ontozesiGyakorisag;
        }
    }
}
