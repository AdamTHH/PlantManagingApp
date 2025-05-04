namespace NOVENYGONDOZASIRENDSZER_GKBS9Q_TOTH.Models
{
    public class DailyPlan
    {
        public List<Noveny> napiOntozendoNovenyek = new List<Noveny> ();
        public double napiVizigenySzum => napiOntozendoNovenyek.Sum(x => x.NapiVizigeny);
    }
}
