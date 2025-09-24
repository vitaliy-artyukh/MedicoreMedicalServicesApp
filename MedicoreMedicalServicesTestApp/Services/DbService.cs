using System.Linq.Expressions;
using MedicoreMedicalServicesTestApp.Data;
using Microsoft.Extensions.Logging;
using SQLite;

namespace MedicoreMedicalServicesTestApp.Services;

public interface IDbService
{
    Task<int> Insert<T>(T item) where T : class;
    Task<int> InsertAll<T>(IEnumerable<T> items) where T : class;
    Task<List<T>> GetAll<T>() where T : class, new();
    Task<List<T>> GetBy<T>(Expression<Func<T, bool>> predicate) where T : class, new();
    Task DeleteAll<T>() where T : class;
    Task Update<T>(T item) where T : class;

    Task RunInTransactionAsync(Func<Task> action);
}

public class DbService : IDbService
{
    private readonly ILogger<IDbService> _logger;

    private ISQLiteAsyncConnection? _db;

    public DbService(ILogger<IDbService> logger)
    {
        _logger = logger;
    }

    private async Task InitializeAsync()
    {
        if (_db != null)
            return;

        var databasePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "medicore_medical.db");
        _db = new SQLiteAsyncConnection(databasePath);

        await _db.CreateTableAsync<ResponseDto>();
        await _db.CreateTableAsync<QuestionnaireResponsesDto>();
    }

    public async Task<int> Insert<T>(T item) where T : class
    {
        await InitializeAsync();
        return await _db!.InsertAsync(item);
    }
    public async Task<int> InsertAll<T>(IEnumerable<T> items) where T : class
    {
        await InitializeAsync();
        return await _db!.InsertAllAsync(items);
    }

    public async Task<List<T>> GetAll<T>() where T : class, new()
    {
        await InitializeAsync();
        return await _db!.Table<T>().ToListAsync();
    }

    public async Task<List<T>> GetBy<T>(Expression<Func<T, bool>> predicate) where T : class, new()
    {
        await InitializeAsync();
        return await _db!.Table<T>().Where(predicate).ToListAsync();
    }

    public async Task Update<T>(T item) where T : class
    {
        await InitializeAsync();
        await _db!.UpdateAsync(item);
    }

    public async Task DeleteAll<T>() where T : class
    {
        await InitializeAsync();
        await _db!.DeleteAllAsync<T>();
    }

    public async Task RunInTransactionAsync(Func<Task> action)
    {
        await InitializeAsync();

        var connection = _db!.GetConnection();
        
        using var _ = connection.Lock();

        connection.BeginTransaction();

        try
        {
            await action();
            connection.Commit();
        }
        catch
        {
            connection.Rollback();
            throw;
        }
    }
}