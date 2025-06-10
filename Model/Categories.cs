
using System.Text.Json.Serialization;

namespace project_API.Model
{
    public class Categories
    {
        [Key]
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }

    }
}
