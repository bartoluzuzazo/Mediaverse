using System.Linq.Expressions;
using Ardalis.Specification;
using MediaVerse.Domain.Entities;
using MediaVerse.Domain.ValueObjects.Enums;

namespace MediaVerse.Client.Application.Extensions.Specification;

public static class SpecificationExtensions
{
    
    public static ISpecificationBuilder<Entry> OrderEntry(
        this ISpecificationBuilder<Entry> specificationBuilder,
        EntryOrder order, OrderDirection direction)
    {
        switch (order)
        {
            case EntryOrder.Id:
                specificationBuilder.OrderByDirection(e => e.Id, direction);
                break;
            case EntryOrder.Name:
                specificationBuilder.OrderByDirection(e => e.Name, direction);
                break;
            case EntryOrder.Rating:
                specificationBuilder.Where(e => e.Ratings.Count != 0).OrderByDirection(e => e.Ratings.Average(r => r.Grade), direction);
                break;
            case EntryOrder.Release:
                specificationBuilder.OrderByDirection(e => e.Release.Year, direction).ThenByDirection(e => e.Release.Month, direction).ThenByDirection(e => e.Release.Day, direction);
                break;
            default:
                specificationBuilder.Where(e => e.Ratings.Count != 0).OrderByDirection(e => e.Ratings.Average(r => r.Grade), direction);
                break;
        }

        return specificationBuilder;
    }
    
    
    public static ISpecificationBuilder<Comment> OrderComments(
        this ISpecificationBuilder<Comment> specificationBuilder,
        CommentOrder order, OrderDirection direction)
    {
        switch (order)
        {
            case CommentOrder.Votes:
                specificationBuilder.OrderByDirection(c =>c.Votes.Select(c=>c.IsPositive ? 1 : -1).Sum(), direction);
                break;
            case CommentOrder.VoteCount:
                specificationBuilder.OrderByDirection(c => c.Votes.Count(), direction);
                break;
        }

        return specificationBuilder;
    }
    
    public static ISpecificationBuilder<T> Paginate<T>(
        this ISpecificationBuilder<T> specificationBuilder,
        int page, int size)
    {
        specificationBuilder.Skip((page - 1) * size).Take(size);

        return specificationBuilder;
    }
    
    public static IOrderedSpecificationBuilder<T> OrderByDirection<T>(
        this ISpecificationBuilder<T> specificationBuilder,
        Expression<Func<T, object?>> orderExpression, OrderDirection direction)
        => direction == OrderDirection.Ascending
            ? OrderBy(specificationBuilder, orderExpression, true)
            : OrderByDescending(specificationBuilder, orderExpression, true);

    private static IOrderedSpecificationBuilder<T> OrderBy<T>(
        this ISpecificationBuilder<T> specificationBuilder,
        Expression<Func<T, object?>> orderExpression,
        bool condition)
    {
        if (condition)
        {
            ((List<OrderExpressionInfo<T>>)specificationBuilder.Specification.OrderExpressions).Add(
                new OrderExpressionInfo<T>(orderExpression, OrderTypeEnum.OrderBy));
        }

        var orderedSpecificationBuilder =
            new OrderedSpecificationBuilder<T>(specificationBuilder.Specification, !condition);

        return orderedSpecificationBuilder;
    }

    private static IOrderedSpecificationBuilder<T> OrderByDescending<T>(
        this ISpecificationBuilder<T> specificationBuilder,
        Expression<Func<T, object?>> orderExpression,
        bool condition)
    {
        if (condition)
        {
            ((List<OrderExpressionInfo<T>>)specificationBuilder.Specification.OrderExpressions).Add(
                new OrderExpressionInfo<T>(orderExpression, OrderTypeEnum.OrderByDescending));
        }

        var orderedSpecificationBuilder =
            new OrderedSpecificationBuilder<T>(specificationBuilder.Specification, !condition);

        return orderedSpecificationBuilder;
    }

    public static IOrderedSpecificationBuilder<T> ThenByDirection<T>(
        this IOrderedSpecificationBuilder<T> orderedBuilder,
        Expression<Func<T, object?>> orderExpression, OrderDirection direction)
    {
        return direction == OrderDirection.Ascending
            ? orderedBuilder.ThenBy<T>(orderExpression, true)
            : orderedBuilder.ThenByDescending<T>(orderExpression, true);
    }

    private static IOrderedSpecificationBuilder<T> ThenBy<T>(
        this IOrderedSpecificationBuilder<T> orderedBuilder,
        Expression<Func<T, object?>> orderExpression,
        bool condition)
    {
        if (condition && !orderedBuilder.IsChainDiscarded)
            ((List<OrderExpressionInfo<T>>)orderedBuilder.Specification.OrderExpressions).Add(
                new OrderExpressionInfo<T>(orderExpression, OrderTypeEnum.ThenBy));
        else
            orderedBuilder.IsChainDiscarded = true;
        return orderedBuilder;
    }

    private static IOrderedSpecificationBuilder<T> ThenByDescending<T>(
        this IOrderedSpecificationBuilder<T> orderedBuilder,
        Expression<Func<T, object?>> orderExpression,
        bool condition)
    {
        if (condition && !orderedBuilder.IsChainDiscarded)
            ((List<OrderExpressionInfo<T>>)orderedBuilder.Specification.OrderExpressions).Add(
                new OrderExpressionInfo<T>(orderExpression, OrderTypeEnum.ThenByDescending));
        else
            orderedBuilder.IsChainDiscarded = true;
        return orderedBuilder;
    }
}