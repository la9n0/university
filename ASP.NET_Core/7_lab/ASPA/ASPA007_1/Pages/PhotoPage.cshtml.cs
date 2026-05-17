using Microsoft.AspNetCore.Mvc.RazorPages;
using DAL_Celebrity_MSSQL;

namespace ASPA007_1.Pages
{
    public class PhotoPageModel : PageModel
    {
        public Celebrity Celebrity { get; set; }
        public List<LifeEvent> LifeEvents { get; set; } = new();

        public void OnGet(int id)
        {
            using (var db = new Repository(AppConfig.ConnectionString))
            {
                Celebrity = db.GetCelebrityById(id);
                LifeEvents = db.GetAllLifeEvents()
                    .Where(e => e.CelebrityId == id)
                    .ToList();
            }
        }
    }
}