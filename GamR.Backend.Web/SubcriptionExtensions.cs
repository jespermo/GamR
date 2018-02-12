using System;
using System.Linq;
using System.Threading.Tasks;
using GamR.Backend.Core.Framework;
using GamR.Backend.Web.Views.ViewManagers;

namespace GamR.Backend.Web
{
    public static class SubcriptionExtensions
    {
        public static async Task SubscribeAll(this IViewManager manager, IEventSubscriber eventSubscriber)
        {
            var events = typeof(IEvent)
                .Assembly
                .GetTypes()
                .Where(x => typeof(IEvent).IsAssignableFrom(x) && !x.IsInterface && !x.IsAbstract)
                .Select(t =>
                {
                    var makeGenericType = typeof(ISubscribeToEvent<>).MakeGenericType(t);
                    var tuple = Tuple.Create(makeGenericType, t);
                    return tuple;
                })
                .ToList();

            var interfaces = manager.GetType()
                .GetInterfaces()
                .Join(events, x => x, x => x.Item1, (inner, outer) => new { GenericType = inner, GenericArgument = outer.Item2 })
                .ToList();

            var methodHandle = typeof(IEventSubscriber).GetMethod(nameof(IEventSubscriber.Subscribe));

            foreach (var types in interfaces)
            {
                var genericMethodHandle = methodHandle.MakeGenericMethod(types.GenericArgument);
                await (Task)genericMethodHandle.Invoke(eventSubscriber, new[] { manager });
            }
        }
    }
}