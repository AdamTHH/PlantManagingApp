using Microsoft.AspNetCore.Mvc;
using NOVENYGONDOZASIRENDSZER_GKBS9Q_TOTH.Data;
using NOVENYGONDOZASIRENDSZER_GKBS9Q_TOTH.Models;

namespace NOVENYGONDOZASIRENDSZER_GKBS9Q_TOTH.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class NovenyController : ControllerBase
    {
        private readonly INovenyRepository repo;
        public NovenyController(INovenyRepository repository)
        {
            repo = repository;
        }
        [HttpGet]
        public ActionResult<IEnumerable<Noveny>> ReadAll()
        {
            var novenyek = repo.Read();
            return Ok(novenyek);
        }
        [HttpGet("{id}")]
        public ActionResult<Noveny> Read(int id)
        {
            var noveny = repo.Read(id);
            if (noveny == null)
            {
                return NotFound();
            }
            return Ok(noveny);
        }
        [HttpPost]
        public ActionResult Create([FromBody] Noveny noveny)
        {
            try
            {
                repo.Create(noveny);
                return CreatedAtAction(nameof(Read), new { nev = noveny.Nev }, noveny);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.InnerException);
            }
        }
        [HttpPut("{id}")]
        public ActionResult Update(int id, [FromBody] Noveny updatedNoveny)
        {
            try
            {
                repo.Update(id, updatedNoveny);
                return NoContent();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpDelete("{id}")]
        public ActionResult Delete(int id)
        {
            try
            {
                repo.Delete(id);
                return NoContent();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("getweeklyplan")]
        public ActionResult<IEnumerable<DailyPlan>> GetWeeklyPlan()
        {
            try
            {
                var weeklyPlan = repo.GetWeeklyPlan();
                return Ok(weeklyPlan);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpPost("uploadmatrix")]
        public ActionResult UploadMatrix([FromBody] string novenyMatrix)
        {
            try
            {
                var rows = novenyMatrix.Split(new[] { '\n' }, StringSplitOptions.RemoveEmptyEntries);
                foreach (var row in rows)
                {
                    var asd = row.Split(',');
                    if (asd.Length != 4)
                    {
                        return BadRequest("Invalid matrix format.");
                    }

                    var nev = asd[0].Trim();
                    if (!Enum.TryParse<NovenyKategoria>(asd[1].Trim(), out var kategoria))
                    {
                        return BadRequest($"Invalid category: {asd[1]}");
                    }

                    if (!double.TryParse(asd[2].Trim(), out var napiVizigeny))
                    {
                        return BadRequest($"Invalid daily water requirement: {asd[2]}");
                    }

                    if (!int.TryParse(asd[3].Trim(), out var ontozesiGyakorisag))
                    {
                        return BadRequest($"Invalid watering frequency: {asd[3]}");
                    }

                    var noveny = new Noveny(nev, kategoria, napiVizigeny, ontozesiGyakorisag);
                    repo.Create(noveny);
                }

                return Ok("Matrix processed successfully.");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
