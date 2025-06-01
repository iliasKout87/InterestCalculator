namespace InterestCalculator.Application.Persistence;

public interface IReadOnlyRepository<T>
{
    Task<T> GetAsync();
}

