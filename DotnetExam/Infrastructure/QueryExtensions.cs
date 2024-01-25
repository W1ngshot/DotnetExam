using System.Linq.Expressions;
using DotnetExam.Infrastructure.Exceptions;
using DotnetExam.Models.Enums;
using DotnetExam.Models.Main;
using Microsoft.EntityFrameworkCore;

namespace DotnetExam.Infrastructure;

public static class QueryExtensions
{
    public static async Task<T> FirstOrNotFoundAsync<T>(
        this IQueryable<T> queryable,
        CancellationToken cancellationToken = default)
    {
        return await queryable.FirstOrDefaultAsync(cancellationToken) 
               ?? throw new NotFoundException<T>();
    }
    
    public static async Task<T> FirstOrNotFoundAsync<T>(
        this IQueryable<T> queryable,
        Expression<Func<T, bool>> predicate,
        CancellationToken cancellationToken = default)
    {
        return await queryable.FirstOrDefaultAsync(predicate, cancellationToken) 
               ?? throw new NotFoundException<T>();
    }
    
    public static async Task<bool> AnyOrNotFoundAsync<T>(
        this IQueryable<T> queryable,
        Expression<Func<T, bool>> predicate,
        CancellationToken cancellationToken = default)
    {
        return await queryable.AnyAsync(predicate, cancellationToken) 
            ? true 
            : throw new NotFoundException<T>();
    }

    public static async Task<bool> IsPlayingAsync(this IQueryable<Game> queryable,
        Guid userId, CancellationToken cancellationToken = default)
    {
        return await queryable.AnyAsync(game =>
                (game.State == GameState.NotStarted || game.State == GameState.Started)
                && (game.Host.UserId == userId || game.Opponent != null && game.Opponent.UserId == userId),
            cancellationToken);
    }
}