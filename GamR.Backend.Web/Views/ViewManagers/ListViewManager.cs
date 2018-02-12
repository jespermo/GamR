using System;
using System.Collections.Immutable;

namespace GamR.Backend.Web.Views.ViewManagers
{
    public abstract class ListViewManager<TView>
    {
        public abstract IImmutableList<TView> All();
        public abstract IImmutableList<TView> Where(Func<TView, bool> predicate);
    }
}