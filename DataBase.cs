using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Npgsql;
using NpgsqlTypes;
using project.Model;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace project
{
    public class DataBase
    {
        NpgsqlConnection con = new NpgsqlConnection(Constants.Connect);

        public async Task InsertTitleAsync (Result result, string filmName)
        {
            var sql = "insert into public.\"FindByName\"(\"id\", \"filmName\", \"titleNameText\", \"titleReleaseText\", \"titleTypeText\", \"topCredits\")"
                + $"values (@id, @filmName, @titleNameText, @titleReleaseText, @titleTypeText, @topCredits)";

            NpgsqlCommand comm = new NpgsqlCommand(sql, con);
            comm.Parameters.AddWithValue("filmName", filmName);
            comm.Parameters.AddWithValue("id", result.id);
            comm.Parameters.AddWithValue("titleNameText", result.titleNameText);
            comm.Parameters.AddWithValue("titleReleaseText", result.titleReleaseText);
            comm.Parameters.AddWithValue("titleTypeText", result.titleTypeText);
            comm.Parameters.AddWithValue("topCredits", result.topCredits);
            //comm.Parameters.AddWithValue("imageType", result.imageType);
            //comm.Parameters.AddWithValue("url", result.titlePosterImageModel.url);
            //comm.Parameters.AddWithValue("maxHeight", result.titlePosterImageModel.maxHeight);
            //comm.Parameters.AddWithValue("maxWidth", result.titlePosterImageModel.maxWidth);
            //comm.Parameters.AddWithValue("caption", result.titlePosterImageModel.caption);
            await con.OpenAsync();
            await comm.ExecuteScalarAsync();
            await con.CloseAsync();
        }
        
        public async Task InsertCreditAsync (Result1 result1, string topCredit)
        {
            var sql = "insert into public.\"FindTopCredit\"(\"id\", \"displayNameText\", \"knownForJobCategory\", \"knownForTitleText\", \"knownForTitleYear\", \"topCredit\")"
                + $"values (@id, @displayNameText, @knownForJobCategory, @knownForTitleText, @knownForTitleYear, @topCredit)";
            NpgsqlCommand comm = new NpgsqlCommand(sql, con);
            comm.Parameters.AddWithValue("topCredit", topCredit);
            comm.Parameters.AddWithValue("id", result1.id);
            comm.Parameters.AddWithValue("displayNameText", result1.displayNameText);
            comm.Parameters.AddWithValue("knownForJobCategory", result1.knownForJobCategory);
            comm.Parameters.AddWithValue("knownForTitleText", result1.knownForTitleText);
            comm.Parameters.AddWithValue("knownForTitleYear", result1.knownForTitleYear);
            //comm.Parameters.AddWithValue("url", result1.avatarImageModel.url);
            //comm.Parameters.AddWithValue("maxHeight", result1.avatarImageModel.maxHeight);
            //comm.Parameters.AddWithValue("maxWidth", result1.avatarImageModel.maxWidth);
            //comm.Parameters.AddWithValue("caption", result1.avatarImageModel.caption);
            await con.OpenAsync();
            await comm.ExecuteScalarAsync();
            await con.CloseAsync();
        }

        public async Task InsertRandomAsync(Result result)
        {
            var sql = "INSERT INTO public.\"RandomMovie\"(\"id\", \"titleNameText\", \"titleReleaseText\", \"titleTypeText\", \"topCredits\") " +
                      "VALUES (@id, @titleNameText, @titleReleaseText, @titleTypeText, @topCredits)";

            
            await using (var comm = new NpgsqlCommand(sql, con))
            {
                comm.Parameters.AddWithValue("id", result.id);
                comm.Parameters.AddWithValue("titleNameText", result.titleNameText ?? (object)DBNull.Value);
                comm.Parameters.AddWithValue("titleReleaseText", result.titleReleaseText ?? (object)DBNull.Value);
                comm.Parameters.AddWithValue("titleTypeText", result.titleTypeText ?? (object)DBNull.Value);
                comm.Parameters.AddWithValue("topCredits", result.topCredits ?? (object)DBNull.Value);

                // Встановлення типу для параметра
                if (result.titleReleaseText == null)
                {
                    comm.Parameters["titleReleaseText"].NpgsqlDbType = NpgsqlDbType.Text;
                }

                await con.OpenAsync();
                await comm.ExecuteScalarAsync();
                await con.CloseAsync();
            }
        }

        public async Task InsertRatingAsync(string filmId, int score)
        {
            var sql = "insert into public.\"FilmRatings\"(\"Filmid\", \"Score\")"
                + $"values (@Filmid, @Score)";

            NpgsqlCommand comm = new NpgsqlCommand(sql, con);
            comm.Parameters.AddWithValue("Filmid", filmId);
            comm.Parameters.AddWithValue("Score", score);

            await con.OpenAsync();
            await comm.ExecuteScalarAsync();
            await con.CloseAsync();
        }

        public async Task<int?> GetRatingAsync(string filmId)
        {
            var sql = "SELECT \"Score\" FROM public.\"FilmRatings\" WHERE \"Filmid\" = @Filmid";

            using (var comm = new NpgsqlCommand(sql, con))
            {
                comm.Parameters.AddWithValue("Filmid", filmId);

                await con.OpenAsync();
                using (var reader = await comm.ExecuteReaderAsync())
                {
                    if (await reader.ReadAsync())
                    {
                        var score = reader.GetInt32(0);
                        return score;
                    }
                }
                await con.CloseAsync();
            }
            return null;
        }
    }
}
