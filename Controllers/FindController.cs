using Microsoft.AspNetCore.Mvc;
using project.Clients;
using project.Model;

namespace project.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class FindController : ControllerBase
    {
        private readonly ILogger<FindController> _logger;

        public FindController(ILogger<FindController> logger)
        {
            _logger = logger;
        }

        [HttpGet("find-by-title")]
        public async Task<Find> GetFilmDetails(string filmName)
        {
            DataBase db = new DataBase();
            FilmClients clients = new FilmClients();
            Find find = await clients.GetFilmDetails(filmName);

            // Перевіряємо, чи є результати
            if (find.titleResults.results != null && find.titleResults.results.Length > 0)
            {
                // Беремо перший результат
                Result firstResult = find.titleResults.results[0];

                // Виконуємо вставку в базу даних
                await db.InsertTitleAsync(firstResult, filmName);
            }

            return find;
        }

        [HttpGet("find-by-date")]
        public async Task<Find> GetFilmDetailsByDate( string titleReleaseText)
        {
            DataBase db = new DataBase();
            FilmClients clients = new FilmClients();
            Find find = await clients.GetFilmDetailsByDate(titleReleaseText);
            if (find.titleResults.results != null && find.titleResults.results.Length > 0)
            {
                // Беремо перший результат
                Result firstResult = find.titleResults.results[0];

                // Виконуємо вставку в базу даних
                await db.InsertTitleAsync(firstResult, titleReleaseText);
            }
            return find;
        }

        [HttpGet("find-by-credits")]
        public async Task<Find> GetFilmDetailsByCredits( string topCredit)
        {
            DataBase db = new DataBase();
            FilmClients clients = new FilmClients();
            Find find = await clients.GetFilmDetailsByCredits(topCredit);


            foreach (var result in find.nameResults.results)
            {
                await db.InsertCreditAsync(result, topCredit);
            }

            return find;
        }

        [HttpGet("random-film")]
        public async Task<Result> GetRandomFilm()
        {
            FilmClients clients = new FilmClients();
            DataBase db = new DataBase();
            Result randomFilm = await clients.GetRandomFilm();

            // Перевірка значень перед вставкою в базу даних
            if (randomFilm == null)
            {
                throw new Exception("Random film not found");
            }

            await db.InsertRandomAsync(randomFilm);
            return randomFilm;
        }

        [HttpPost("rate-film")]
        public async Task<ActionResult> RateFilm( string filmId, int score)
        {
            if (score < 1 || score > 10)
            {
                return BadRequest("Score must be between 1 and 10.");
            }

            var db = new DataBase();
            await db.InsertRatingAsync(filmId, score);

            return Ok("Rating added successfully.");
        }

        [HttpGet("get-rating")]
        public async Task<ActionResult> GetRating( string filmId)
        {
            var db = new DataBase();
            var score = await db.GetRatingAsync(filmId);

            if (score == null)
            {
                return NotFound("Film not found.");
            }

            return Ok(new { FilmId = filmId, Score = score });
        }
    }
}

    
