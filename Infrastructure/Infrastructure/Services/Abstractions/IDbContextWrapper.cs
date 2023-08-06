namespace Infrastructure.Services.Abstractions
{
    public interface IDbContextWrapper<T>
        where T : DbContext
    {
        T DbContext { get; }
        Task<IDbContextTransaction> BeginTransactionAsync(CancellationToken cancellationToken);
    }
}
