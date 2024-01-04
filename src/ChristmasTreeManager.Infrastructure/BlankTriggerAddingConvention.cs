using Microsoft.EntityFrameworkCore;

namespace ChristmasTreeManager.Infrastructure;

public class BlankTriggerAddingConvention : Microsoft.EntityFrameworkCore.Metadata.Conventions.IModelFinalizingConvention
{
    public virtual void ProcessModelFinalizing(
        Microsoft.EntityFrameworkCore.Metadata.Builders.IConventionModelBuilder modelBuilder,
        Microsoft.EntityFrameworkCore.Metadata.Conventions.IConventionContext<Microsoft.EntityFrameworkCore.Metadata.Builders.IConventionModelBuilder> context)
    {
        foreach (var entityType in modelBuilder.Metadata.GetEntityTypes())
        {
            var table = Microsoft.EntityFrameworkCore.Metadata.StoreObjectIdentifier.Create(entityType, Microsoft.EntityFrameworkCore.Metadata.StoreObjectType.Table);
            if (table is not null
                && entityType.GetDeclaredTriggers().All(t => t.GetDatabaseName(table.Value) is null))
            {
                entityType.Builder.HasTrigger(table.Value.Name + "_Trigger");
            }

            foreach (var fragment in entityType.GetMappingFragments(Microsoft.EntityFrameworkCore.Metadata.StoreObjectType.Table))
            {
                if (entityType.GetDeclaredTriggers().All(t => t.GetDatabaseName(fragment.StoreObject) is null))
                {
                    entityType.Builder.HasTrigger(fragment.StoreObject.Name + "_Trigger");
                }
            }
        }
    }
}
