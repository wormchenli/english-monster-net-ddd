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
        var entityTypesWithSoftDeletion = builder.Model.GetEntityTypes()
            .Where( e=> e.ClrType.IsAssignableTo(typeof(TInterface)));

        foreach (var entityType in entityTypesWithSoftDeletion)
        {
            var property = entityType.FindProperty(propertyName);
            var parameter = Expression.Parameter(entityType.ClrType, "p");
        }
    }
}
