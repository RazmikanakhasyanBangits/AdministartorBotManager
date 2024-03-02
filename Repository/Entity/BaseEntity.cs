namespace Repository.Entity;

public class BaseEntity<T>
{
    public T Id { get; set; }
    public DateTime CreationDate { get; set; } = DateTime.Now;
    public DateTime LastUpdatedDate { get; set; } = DateTime.UtcNow;
    public DateTime DeletionDate { get; set; } = DateTime.UtcNow;
}