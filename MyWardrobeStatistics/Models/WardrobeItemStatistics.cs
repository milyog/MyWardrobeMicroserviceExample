namespace MyWardrobeStatistics.Models
{ 
    public class WardrobeItemStatistics      
    {
        public Guid Id { get; set; }
        public string Category { get; set; }
        public string Subcategory { get; set; } 
        public int WardrobeItemUsage { get; set; }
        public DateTime LastTimeUsed { get; set; }

        public WardrobeItemStatistics(Guid id, string category, string subcategory, int wardrobeItemUsage, DateTime lastTimeUsed)
        {
            Id = id;
            Category = category;
            Subcategory = subcategory;
            WardrobeItemUsage = wardrobeItemUsage;
            LastTimeUsed = lastTimeUsed;
        }

        public WardrobeItemStatistics() { }
    }
}
