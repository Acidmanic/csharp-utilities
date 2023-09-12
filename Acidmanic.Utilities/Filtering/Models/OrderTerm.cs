namespace Acidmanic.Utilities.Filtering.Models
{
    public class OrderTerm
    {
        public string Key { get; set; } = "Id";

        public OrderSort Sort { get; set; } = OrderSort.Ascending;
    }
}