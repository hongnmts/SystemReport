namespace SystemReport.WebAPI.Models
{
    public interface TEntity<T>
    {
        T Id { get; set; }
    }
}