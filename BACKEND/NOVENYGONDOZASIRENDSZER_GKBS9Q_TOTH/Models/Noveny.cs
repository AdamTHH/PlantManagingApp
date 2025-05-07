using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace NOVENYGONDOZASIRENDSZER_GKBS9Q_TOTH.Models
{
    public class Noveny
    {
        public enum NovenyKategoria
        {
            [Display(Name = "Szobanövény")]
            Szobanoveny = 1,

            [Display(Name = "Kertinövény")]
            Kertinoveny = 2,

            [Display(Name = "Virág")]
            Virag = 3,

            [Display(Name = "Szukkulens")]
            Szukkulens = 4,

            [Display(Name = "Fűszernövény")]
            Fuszernoveny = 5
        }

        public static NovenyKategoria GetNovenyEnum(string displayName)
        {
            return Enum.GetValues(typeof(NovenyKategoria))
                       .Cast<NovenyKategoria>()
                       .FirstOrDefault(value =>
                           typeof(NovenyKategoria).GetField(value.ToString())
                           ?.GetCustomAttribute<DisplayAttribute>()
                           ?.Name == displayName);
        }

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
