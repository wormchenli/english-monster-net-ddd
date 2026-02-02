// Author: Li Chen
// Date: 2024-06-11

using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;

namespace CommonUtils;

public static class EFCoreExtension
{
    /// <summary>
    /// set global query filter for soft delete : IsDeleted = false
    /// entities must implement TInterface, e.g. ISoftDelete
    /// </summary>
    /// <param name="builder"></param>
    /// <param name="propertyName">Soft-delete flag property name. Defaults to <c>IsDeleted</c>.</param>
    public static void EnableSoftDeleteFilter<TInterface>(this ModelBuilder builder, string propertyName = "IsDeleted")
    {
        // 1. find entities that implement TInterface
        var entityTypesWithSoftDeletion = builder.Model.GetEntityTypes()
            .Where( e=> e.ClrType.IsAssignableTo(typeof(TInterface)));
        // 2. iterate, construct filter expression, and apply
        foreach (var entityType in entityTypesWithSoftDeletion)
        {
            var property = entityType.FindProperty(propertyName);
            // Expression start: p =>
            
            var parameter = Expression.Parameter(entityType.ClrType, "p");
            // p.IsDeleted
            
            var memberExpression = Expression.Property(parameter, property!.PropertyInfo!);
            // !p.IsDeleted
            
            var notExpression = Expression.Not(memberExpression);
            // Final lambda: p => !p.IsDeleted  (equivalent to p.IsDeleted == false)
            
            var lambda = Expression.Lambda(notExpression, parameter);
            builder.Entity(entityType.ClrType).HasQueryFilter(lambda);
            // or entityType.SetQueryFilter(lambda);
        }
    }
}
