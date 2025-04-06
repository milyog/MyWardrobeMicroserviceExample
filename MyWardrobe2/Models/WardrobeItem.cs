namespace MyWardrobe.Models
{
    public class WardrobeItem(
        Guid id,
        string category,
        string subcategory,
        int wardrobeItemUsage,
        DateTime lastTimeUsed
        )
    {
        public Guid Id { get; set; } = id;
        public string Category { get; set; } = category;
        public string Subcategory { get; set; } = subcategory;
        public int WardrobeItemUsage { get; set; } = wardrobeItemUsage;
        public DateTime LastTimeUsed { get; set; } = lastTimeUsed;

    }
}
