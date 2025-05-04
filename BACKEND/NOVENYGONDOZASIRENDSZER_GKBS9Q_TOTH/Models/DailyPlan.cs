namespace NOVENYGONDOZASIRENDSZER_GKBS9Q_TOTH.Models
{
    public class DailyPlan
    {
        public IEnumerable<Noveny> napiOntozendoNovenyek { get; set; }
        public double napiVizigenySzum => napiOntozendoNovenyek.Sum(x => x.NapiVizigeny);
    }
}
