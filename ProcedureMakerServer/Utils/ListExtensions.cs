﻿using DocumentFormat.OpenXml.InkML;
using DocumentFormat.OpenXml.Wordprocessing;
using EFCoreBase.Entities;
using EFCoreBase.Interfaces;
using Microsoft.EntityFrameworkCore;
using Org.BouncyCastle.Tls;

namespace ProcedureMakerServer.Utils;

public static class ListExtensions
{
    // type that has a collectino of other entities
    // generate the copyTo 
    /// <summary>
    /// must flow from current to updated or else it will remove teh added elements
    /// </summary>
    public static EnumerableDifferenceResult<T> GetDifferences<T>(this IEnumerable<T> current, IEnumerable<T> updated)
        where T : EntityBase
    {
        // COntainsEntity?
        IEnumerable<T> common = current.Where(x => updated.Any(y => x.Id == y.Id));
        IEnumerable<T> added = updated.Where(x => !current.Any(y => y.Id == x.Id));
        IEnumerable<T> removed = current.Where(x => !updated.Any(y => y.Id == x.Id));

        EnumerableDifferenceResult<T> differences = new()
        {
            Common = common,
            Added = added,
            Removed = removed,
            Updated = updated,
        };
        return differences;
    }

    // defeind by user
    public static async Task HandleUpdate<TDto, TEntity, TContext>(this IEnumerable<TDto> current,
        IEnumerable<TDto> updated,
        Func<TDto, TDto, Task> updateCommon,
        Func<TDto, Task> handleAdded,
        Func<TDto, TEntity, Task> handleRemoved,
        TContext context)
        where TDto : EntityBase
        where TEntity : EntityBase
        where TContext : DbContext
    {
        var diff = current.GetDifferences(updated);
        await diff.HandleUpdate(updateCommon, handleAdded, handleRemoved, context);
    }

    public static async Task HandleUpdate<TDto, TEntity, TContext>(this IEnumerable<TDto> current,
        IEnumerable<TDto> updated,
        Func<TDto, Task<TEntity>> handleAdded,
        TContext context)
        where TContext : DbContext
        where TEntity : EntityBase, ICopyFromAbleDto<TDto>
        where TDto : EntityBase
    {
        var diff = current.GetDifferences(updated);
        await diff.MapEnumerableToEntities(handleAdded, context);
    }


    public class EnumerableDifferenceResult<T> where T : EntityBase
    {
        public IEnumerable<T> Common { get; set; } = new List<T>();
        public IEnumerable<T> Added { get; set; } = new List<T>();
        public IEnumerable<T> Removed { get; set; } = new List<T>();

        public IEnumerable<T> Updated { get; set; } = new List<T>();

        // Deconstructor method
        public void Deconstruct(out IEnumerable<T> common, out IEnumerable<T> added, out IEnumerable<T> removed)
        {
            common = Common;
            added = Added;
            removed = Removed;
        }

        /// <summary>
        /// Everything is maanged by user code
        /// </summary>
        public async Task HandleUpdate1(Func<T, T, Task> updateCommon, Func<T, Task> handleAdded,
            Func<T, Task> handleRemoved)
        {
            foreach (var current in Common)
            {
                var updated = Updated.FirstOrDefault(x => x.Id == current.Id);
                // I am sending back always the old one.
                // therefore the information never gets passed.
                // I will inject both then. 
                await updateCommon(current, updated);
            }

            foreach (var item in Added)
            {
                await handleAdded(item);
            }

            foreach (var item in Removed)
            {
                await handleRemoved(item);
            }
        }


        /// <summary>
        /// <para>Common: defined by user </para>
        /// <para>Added: defined by user </para>
        /// <para>Removed: defined by user </para>
        /// </summary>
        public async Task HandleUpdate<TEntity, TContext>(Func<T, T, Task> updateCommon, Func<T, Task> handleAdded,
            Func<T, TEntity, Task> handleRemoved, TContext context)
            where TContext : DbContext
            where TEntity : EntityBase
        {
            var set = context.Set<TEntity>();
            foreach (var current in Common)
            {
                var updated = Updated.FirstOrDefault(x => x.Id == current.Id);
                await updateCommon(current, updated);
            }

            foreach (var item in Added)
            {
                await handleAdded(item);
            }

            foreach (var item in Removed)
            {
                var entity = await set.FirstAsync(x => x.Id == item.Id);
                await handleRemoved(item, entity);
            }

            await context.SaveChangesAsync();
        }

        /// <summary>
        /// <para>Common: Copied from Dto through ICOpyAbleDto </para>
        /// <para>Added: Entity returned by User </para>
        /// <para>Removed: inferred from dto </para>
        /// </summary>
        public async Task MapEnumerableToEntities<TEntity, TContext>(
            Func<T, Task<TEntity>> handleAdded,
            TContext context)
            where TContext : DbContext
            where TEntity : EntityBase, ICopyFromAbleDto<T>
        {
            // gets the associated enttiy automatically and saves it
            var set = context.Set<TEntity>();
            foreach (var current in Common) // i think the problem is that I need to get the old one. 
            {
                var entity = await set.FirstAsync(x => x.Id == current.Id);
                var updated = Updated.FirstOrDefault(x => x.Id == current.Id);
                entity.CopyFromDto(updated);
            }

            //
            foreach (var item in Added)
            {
                var entity = await handleAdded(item);
                set.Add(entity);
            }

            foreach (var item in Removed)
            {
                var entity = await set.FirstAsync(x => x.Id == item.Id);
                //  await onRemoved(item);
                set.Remove(entity);
            }

            await context.SaveChangesAsync();
        }
        // should take an overload that creates the entity from the dto
    }

    public interface ICopyFromAbleDto<T> where T : EntityBase
    {
        public void CopyFromDto(T dto);
    }
}