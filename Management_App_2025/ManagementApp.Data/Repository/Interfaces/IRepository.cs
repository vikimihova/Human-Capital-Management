using System.Linq.Expressions;

namespace ManagementApp.Data.Repository.Interfaces
{
    public interface IRepository<TType, TId> // for CRUD operations between Services and Database
    {
        // Get by id
        TType GetById(TId id); // synchronous
        Task<TType> GetByIdAsync(TId id); // asynchronous


        // Get all
        IEnumerable<TType> GetAll(); // synchronous
        Task<IEnumerable<TType>> GetAllAsync(); // asynchronous ...
        IQueryable<TType> GetAllAttached(); // only synchronous


        // Get first or default
        TType FirstOrDefault(Func<TType, bool> predicate);
        Task<TType> FirstOrDefaultAsync(Expression<Func<TType, bool>> predicate);


        // Add
        void Add(TType item);
        Task AddAsync(TType item);


        // Add range
        void AddRange(TType[] items);
        Task AddRangeAsync(TType[] items);


        // Delete
        bool Delete(TType item);
        Task<bool> DeleteAsync(TType item);


        // Update
        bool Update(TType item);
        Task<bool> UpdateAsync(TType item);
    }
}
