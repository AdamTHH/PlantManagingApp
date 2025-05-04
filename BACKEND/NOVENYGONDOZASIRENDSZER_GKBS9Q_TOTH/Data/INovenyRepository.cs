using System.Collections.Generic;
using NOVENYGONDOZASIRENDSZER_GKBS9Q_TOTH.Models;

namespace NOVENYGONDOZASIRENDSZER_GKBS9Q_TOTH.Data
{
    public interface INovenyRepository
    {
        void Create(Noveny noveny);
        Noveny Read(int id);
        IEnumerable<Noveny> Read();
        void Update(int id, Noveny updatedNoveny);
        void Delete(int id);
        IEnumerable<DailyPlan> GetWeeklyPlan();
    }
}
