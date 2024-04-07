namespace Application.Interfaces;

public interface IRepository<TEntity>
{
    //TODO: пагинация почитатать
    public TEntity GetById(Guid id);
    
    public List<TEntity> Get();
    
    public TEntity Create(TEntity person);
    
    public TEntity Update(TEntity person);
    
    public bool Delete(Guid id);
}